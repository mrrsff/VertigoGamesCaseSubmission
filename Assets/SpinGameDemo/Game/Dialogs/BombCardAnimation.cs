using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public class BombCardAnimation : MonoBehaviour
    {
        [SerializeField] private Image shine;

        [SerializeField] private float rotationSpeed = 5f;
        private float startAlpha;

        private void Start()
        {
            startAlpha = shine.color.a;
        }

        private void Update()
        {
            if (shine != null)
            {
                shine.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                
                float alpha = (Mathf.Sin(Time.time * 2f) + 1f) / 2f * 0.5f + 0.5f; // pulsate between 0.5 and 1
                Color color = shine.color;
                color.a = alpha * startAlpha;
                shine.color = color; 
            }
        }
    }
}