using UnityEngine;
using System.Collections;

namespace util
{
    public class CameraShake : MonoBehaviour
    {
        public Transform camTransform;
        
        public float shake = 0f;
        
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;
        
        Vector3 originalPos;
        
        void Awake()
        {
            if (camTransform != null) return;
            camTransform = GetComponent<Transform>();
        }
        
        void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }
        
        public void play(float amount, float time)
        {
            if (shakeAmount > 0) {
                shakeAmount += amount / 2;
                shake += time/4;
            } else {
                shakeAmount = amount;
                shake = time;
            }
        }
        
        void Update()
        {
            if (shake > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                
                shake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shake = 0f;
                shakeAmount = 0f;
                camTransform.localPosition = originalPos;
            }
        }
    }
}
