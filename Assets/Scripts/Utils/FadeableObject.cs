using UnityEngine;

namespace EdwinGameDev.Utils
{
    public class FadeableObject : MonoBehaviour
    {
        private string transparencyProperty = "Transparency";
    
        private Material material;
        private float currentAlpha = 1f;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
            material.SetFloat(transparencyProperty, currentAlpha);
        }

        public void FadeTo(float targetAlpha, float fadeSpeed)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
            material.SetFloat(transparencyProperty, currentAlpha);
        }

        public float GetCurrentAlpha()
        {
            return currentAlpha;
        }
    }
}