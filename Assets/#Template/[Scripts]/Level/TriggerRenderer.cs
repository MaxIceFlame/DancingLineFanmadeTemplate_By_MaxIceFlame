//Thanks LAM for providing method.
using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent, RequireComponent(typeof(BoxCollider))]
    public class TriggerRenderer : MonoBehaviour
    {
        [SerializeField] private Color boxColor = new(0f, 1f, 0f, 0.5f);
        [SerializeField] private Color wireColor = Color.green;

        private BoxCollider box;
        
#if UNITY_EDITOR
        private static Vector3 MultiplyVector3(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        private void OnDrawGizmos()
        {
            if (!box) 
                box = GetComponent<BoxCollider>();
            
            var matrix = new Matrix4x4();
            matrix.SetTRS(transform.position + box.center, transform.rotation, MultiplyVector3(transform.lossyScale, box.size));
            Gizmos.matrix = matrix;
            
            Gizmos.color = wireColor;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = boxColor;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
#endif
    }
}