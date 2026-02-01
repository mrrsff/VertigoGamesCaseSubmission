using System;
using SpinGameDemo.Context;
using SpinGameDemo.Game.Rewards;
using SpinGameDemo.Game.Spin;
using SpinGameDemo.Game.Wheel;
using SpinGameDemo.Game.Zones;
using UnityEngine;

namespace SpinGameDemo.Game
{
    public class SpinManager : IContextUnit
    {
        private RewardManager rewardManager;
        private ZoneManager zoneManager;
        private GameStateManager gameStateManager;

        private WheelAssets assets;
        private SpinPresetLibrary _presetLibrary;
        private SpinPreset currentPreset;
        private WheelPanelController _controller;
        private Action<Action<SpinPreset, Sprite, Sprite>> pendingSetup;
        
        private bool skipBomb;
        private bool readyToSpin;
        public bool CanSpin() => readyToSpin;
        public void Initialize()
        {
            gameStateManager = GameContext.Get<GameStateManager>();
            gameStateManager.OnGameStateChanged += HandleRestart;
            
            rewardManager = GameContext.Get<RewardManager>();
            zoneManager = GameContext.Get<ZoneManager>();
            zoneManager.OnZoneChanged += SetZone;
            
            _presetLibrary = Resources.Load<SpinPresetLibrary>("SpinPresetLibrary");
            _presetLibrary.Init();
            
            assets = Resources.Load<WheelAssets>("WheelAssets");
        }
        public void Dispose()
        {
        }
        
        public void SetController(WheelPanelController controller)
        {
            _controller = controller;
            pendingSetup?.Invoke(_controller.SetupPreset);
        }

        private void HandleRestart(GameState state)
        {
            if (state != GameState.Restart) return;
            readyToSpin = true;
            skipBomb = false;
        }

        public void ContinueAfterLose()
        {
            readyToSpin = true;
            skipBomb = true;
        }
        public int GetMaxZone() => _presetLibrary.GetZoneCount();
        public int GetSpinResultSliceIndex()
        {
            if (!skipBomb) return currentPreset.GetRandomOutcomeIndex();
            
            skipBomb = false;
            do
            {
                int index = currentPreset.GetRandomOutcomeIndex();
                var outcome = currentPreset.GetOutcome(index);
                if (!outcome.IsBomb) return index;
            } while (true);
        }

        public void StartSpin()
        {
            gameStateManager.SetState(GameState.Spin);
            readyToSpin = false;
        }

        private void SetZone(int newZone)
        {
            currentPreset = _presetLibrary.GetPreset(newZone);
            var wheelSprite = assets.GetWheelSprite(newZone);
            var pointerSprite = assets.GetPointerSprite(newZone);
            if (_controller) _controller.SetupPreset(currentPreset, wheelSprite, pointerSprite);
            else
            {
                pendingSetup = _controllerSetup =>
                {
                    _controllerSetup(currentPreset, wheelSprite, pointerSprite);
                    pendingSetup = null;
                };
            }
            readyToSpin = true;
        }
        public void ApplySpinResult(int sliceIndex)
        {
            gameStateManager.SetState(GameState.Idle);
            SpinOutcome outcome = currentPreset.GetOutcome(sliceIndex);
            var slot = _controller.GetSlotAt(sliceIndex);
            if (outcome.IsBomb)
            {
                HandleBomb();
                return;
            }
            
            outcome.Apply(rewardManager, slot);
        }

        private void HandleBomb()
        {
            gameStateManager.SetState(GameState.BombExplode);
        }
    }
}