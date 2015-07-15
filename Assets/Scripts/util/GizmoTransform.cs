using UnityEngine;
using System.Collections;

namespace util
{
    public class GizmoTransform : MonoBehaviour 
    {
        public Color color;
        public float radius;
        
        void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}