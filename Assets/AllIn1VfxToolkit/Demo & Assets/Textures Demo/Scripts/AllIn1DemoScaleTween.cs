using System;
using UnityEngine;

namespace AllIn1VfxToolkit.DemoAssets.TexturesDemo.Scripts
{
    public class AllIn1DemoScaleTween : MonoBehaviour
    {
        [SerializeField] private float maxTweenScale = 2.0f;
        [SerializeField] private float minTweenScale = 0.8f;
        [SerializeField] private float tweenSpeed = 15f;
        
        private bool isTweening = false;
        private float currentScale = 1f, iniScale;
        private Vector3 scaleToApply = Vector3.one;

        private void Start()
        {
            iniScale = transform.localScale.x;
        }

        private void Update()
        {
            if(!isTweening) return;
            currentScale = Mathf.Lerp(currentScale, iniScale, Time.unscaledDeltaTime * tweenSpeed);
            UpdateScaleToApply();
            ApplyScale();
            if(Mathf.Abs(currentScale - 1f) < 0.02f) isTweening = false;
        }

        private void UpdateScaleToApply()
        {
            scaleToApply.x = currentScale;
            scaleToApply.y = currentScale;
        }
        
        private void ApplyScale()
        {
            transform.localScale = scaleToApply;
        }

        public void ScaleUpTween()
        {
            isTweening = true;
            currentScale = iniScale * maxTweenScale;
            UpdateScaleToApply();
        }
        
        public void ScaleDownTween()
        {
            isTweening = true;
            currentScale = iniScale * minTweenScale;
            UpdateScaleToApply();
        }
    }
}