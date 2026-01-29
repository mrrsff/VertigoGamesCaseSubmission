using DG.Tweening;
using SpinGameDemo.Game;
using UnityEngine;

namespace SpinGameDemo.Context
{
    [DefaultExecutionOrder(-100)]
    public class ApplicationContextController : MonoBehaviour
    {
        private static ApplicationContextController instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            ApplicationContext.SetController(this);
            
            ApplicationContext.AddContextUnit(new GameContext());

            ApplicationContext.Initialize();
            
            Application.targetFrameRate = 144;

            DOTween.defaultEaseType = Ease.OutQuad;
            DOTween.Init(useSafeMode: false);
        }

        private void Update()
        {
            ApplicationContext.Update();
        }
    }
}