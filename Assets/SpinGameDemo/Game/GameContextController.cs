using SpinGameDemo.Context;
using UnityEngine;

namespace SpinGameDemo.Game
{
    public class GameContextController : MonoBehaviour
    {
        private void Awake()
        {
            ApplicationContext.Get<GameContext>().SetController(this);
        }
    }
}