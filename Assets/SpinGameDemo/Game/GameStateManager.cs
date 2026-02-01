using System;
using DG.Tweening;
using SpinGameDemo.Context;
using SpinGameDemo.Game.Dialogs;
using SpinGameDemo.Game.Rewards;
using SpinGameDemo.User;
using UnityEngine.SceneManagement;

namespace SpinGameDemo.Game
{
    public enum GameState { Idle, Spin, Exit, BombExplode, GameEnd, GameWon, Restart }
    public class GameStateManager : IContextUnit
    {
        private GameState currentState;
        
        private DialogManager dialogManager;
        private RewardManager _rewardManager;
        
        public event Action<GameState> OnGameStateChanged; 
        public void Initialize()
        {
            dialogManager = GameContext.Get<DialogManager>();
            _rewardManager = GameContext.Get<RewardManager>();
        }

        public void Dispose()
        {
        }
        public GameState GetCurrentState() => currentState;
        public void SetState(GameState state)
        {
            if (currentState == state) return;
            currentState = state;
            OnGameStateChanged?.Invoke(state);
            switch (state)
            {
                case GameState.Idle:
                    break;
                case GameState.Spin:
                    break;
                case GameState.BombExplode:
                    ShowBombExploded();
                    break;
                case GameState.GameWon:
                    ShowWinDialog();
                    break;
                case GameState.GameEnd:
                    ShowGameOverDialog();
                    break;
                case GameState.Exit:
                    ShowExitDialog();
                    break;
                case GameState.Restart:
                    ReloadGame();
                    break;
            }
        }
        private void ShowWinDialog()
        {
            var gameEndDialog = dialogManager.ShowDialog<GameEndDialog>();
            gameEndDialog.SetAsWin();
            var rewards = _rewardManager.CollectRewards();
            gameEndDialog.SetRewards(rewards);
        }
        
        private void ShowBombExploded()
        {
            dialogManager.ShowDialog<BombDialog>();
        }
        
        private void ShowGameOverDialog()
        {
            var dialog = dialogManager.ShowDialog<GameEndDialog>();
            dialog.SetAsGameOver();
        }
        
        private void ShowExitDialog()
        {
            dialogManager.ShowDialog<ExitDialog>();
        }

        private void ReloadGame()
        {
            PersistentUserData.SetZone(0);
            DOTween.KillAll(true);
            SetState(GameState.Idle);
            SceneManager.LoadScene(0);
        }
    }
}