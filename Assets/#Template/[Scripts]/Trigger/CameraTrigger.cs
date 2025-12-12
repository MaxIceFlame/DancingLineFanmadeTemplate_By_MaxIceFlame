using DancingLineFanmade.Level;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DancingLineFanmade.Trigger
{
    public class CameraTrigger : MonoBehaviour
    {
        private CameraFollower follower;

        [SerializeField] private UnityEvent onFinished;
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private Vector3 rotation = new(60f, 45f, 0f);
        [SerializeField] private Vector3 scale = Vector3.one;
        [SerializeField, Range(0f, 179f)] private float fieldOfView = 80f;
        [SerializeField] private bool follow = true;
        [SerializeField, Min(0f)] private float duration = 2f;
        [SerializeField, DrawWithUnity] private Ease ease = Ease.InOutSine;
        [SerializeField] private bool useCurve;
        [SerializeField] private AnimationCurve curve = LevelManager.linearCurve;
        [SerializeField, DrawWithUnity] private RotateMode mode = RotateMode.FastBeyond360;
        [SerializeField] private bool canBeTriggered = true;

        private PreviewCamera previewCamera;

#if UNITY_EDITOR
        [HorizontalGroup, Button("Create Preview", ButtonSizes.Large)]
        private void CreatePreviewCamera()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/PreviewCameraHolder");
            if (!previewCamera)
            {
                previewCamera = Instantiate(prefab, transform.position, Quaternion.Euler(Vector3.zero))
                    .GetComponent<PreviewCamera>();
                previewCamera.SetTransform(offset, rotation, scale, fieldOfView);
            }
            else Debug.LogError("相机预览已存在！");
        }

        [HorizontalGroup, Button("Destroy Preview", ButtonSizes.Large)]
        private void DestroyPreviewCamera()
        {
            if (previewCamera)
                DestroyImmediate(previewCamera.gameObject);
            else Debug.LogError("相机预览不存在！");
        }

        private void OnValidate()
        {
            if (previewCamera)
                previewCamera.SetTransform(offset, rotation, scale, fieldOfView);
        }
#endif

        private void Start()
        {
            follower = CameraFollower.Instance;
            if (previewCamera)
                Destroy(previewCamera.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !canBeTriggered)
                return;
            follower.follow = follow;
            var normalizedRotation = rotation - new Vector3(60f, 0f, 0f);
            follower.Trigger(offset, normalizedRotation, scale, fieldOfView, duration, ease, mode, onFinished, useCurve,
                curve);
        }

        public void Trigger()
        {
            if (canBeTriggered)
                return;
            follower.follow = follow;
            var normalizedRotation = rotation - new Vector3(60f, 0f, 0f);
            follower.Trigger(offset, normalizedRotation, scale, fieldOfView, duration, ease, mode, onFinished, useCurve,
                curve);
        }
    }
}