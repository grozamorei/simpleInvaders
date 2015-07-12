using UnityEngine;
using System.Collections;

namespace graphics
{
    [ExecuteInEditMode]
    public class PostProcessBase : MonoBehaviour
    {
        [SerializeField] private Shader _shader = Shader.Find("Custom/GrayScale");
    
        private Material _mat;
        protected Material material
        {
            get {
                if (_mat == null) {
                    _mat = new Material(_shader);
                    _mat.hideFlags = HideFlags.HideAndDontSave;
                }
                return _mat;
            }
        }
        
        void Start()
        {
            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
                return;
            }
            
            if (_shader == null || !_shader.isSupported) {
                enabled = false;
            }
        }
        
        void OnDisable()
        {
            if (_mat)
                DestroyImmediate(_mat);
        }
    }
}