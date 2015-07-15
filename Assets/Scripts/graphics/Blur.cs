using UnityEngine;
using System.Collections;

namespace graphics
{
    public enum BlurType {
        SOFT, HARD, FINAL
    }

    public class Blur : PostProcessBase {

        [SerializeField] private float amount = 0;
        [SerializeField] private AnimationCurve _soft = null;
        [SerializeField] private float _softTime = 0.3f;
        [SerializeField] private AnimationCurve _hard = null;
        [SerializeField] private float _hardTime = 0.6f;
        [SerializeField] private AnimationCurve _final = null;
        [SerializeField] private float _finalTime = 2f;

        private float _currentTime;
        private float _currentMaxTime;
        private AnimationCurve _currentCurve;
        private bool _playing;

        public void play(BlurType t)
        {
            if (_playing && t != BlurType.FINAL) return;

            if (t == BlurType.SOFT) {
                _currentMaxTime = _softTime;
                _currentCurve = _soft;
            } else if (t == BlurType.HARD) {
                _currentMaxTime = _hardTime;
                _currentCurve = _hard;
            } else if (t == BlurType.FINAL) {
                _currentMaxTime = _finalTime;
                _currentCurve = _final;
            }
            _playing = true;
            _currentTime = 0;
            amount = 0;
        }

        void Update()
        {
            if (!_playing) return;

            _currentTime += Time.deltaTime;
            amount = _currentCurve.Evaluate(_currentTime / _currentMaxTime);

            if (_currentTime > _currentMaxTime) {
                _playing = false;
                amount = 0;
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetFloat("_BlurAmount", amount / 10);
            Graphics.Blit(source, destination, material);
        }
    }
}

