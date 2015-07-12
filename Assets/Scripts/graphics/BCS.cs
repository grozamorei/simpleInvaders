using UnityEngine;
using System.Collections;

namespace graphics
{
    public class BCS : PostProcessBase {
        
        [SerializeField] private float _brightness = 1.0f;
        [SerializeField] private float _saturation = 1.0f;
        [SerializeField] private float _contrast = 1.0f;
        
        void Update()
        {
            _brightness = Mathf.Clamp(_brightness, 0.0f, 2.0f);
            _saturation = Mathf.Clamp(_saturation, 0.0f, 2.0f);
            _contrast = Mathf.Clamp(_contrast, 0.0f, 3.0f);
        }
        
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetFloat("_Brightness", _brightness);
            material.SetFloat("_Saturation", _saturation);
            material.SetFloat("_Contrast", _contrast);
            
            Graphics.Blit(source, destination, material);
        }
    }
}