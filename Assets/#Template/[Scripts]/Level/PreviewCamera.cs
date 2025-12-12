using UnityEngine;

namespace DancingLineFanmade.Level
{
    public class PreviewCamera : MonoBehaviour
    {
        public Transform rotationHolder;
        public Transform scaleHolder;
        public Camera previewCamera;

        private void Start()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        public void SetTransform(Vector3 offset, Vector3 rotation, Vector3 scale, float fov)
        {
            rotationHolder.localPosition = offset;
            rotationHolder.localEulerAngles = rotation - new Vector3(60f, 0f, 0f);
            scaleHolder.localScale = scale;
            previewCamera.fieldOfView = fov;
        }
#endif
    }
}