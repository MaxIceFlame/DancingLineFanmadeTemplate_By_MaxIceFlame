using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class TrailPredictor : MonoBehaviour
    {
        [Header("Settings")] [SerializeField, Min(0)]
        private int resolution = 100;

        [SerializeField, Min(0)] private float renderDistance = 50f;

        [SerializeField] private Color trailColor = Color.red;
        [SerializeField] private Color hitColor = Color.blue;

        [Header("Data")] [Min(0f)] public int horizontalSpeed = 12;
        [Min(0f)] public float verticalImpulse;

        private readonly LayerMask layerMask = Physics.DefaultRaycastLayers;
        private const QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

        private struct HitInfo
        {
            public Vector3 point;
            public Vector3 normal;
            public Vector3 start;
            public Vector3 end;
            public bool isFirstHit;
        }

        private HitInfo[] hitInfos;
        private HitInfo[] savedHitInfos; // 保存进入播放模式时的 hitInfos
        private Matrix4x4 localToWorldMatrix;
        private bool hasFirstHit;
        private bool isPlaying; // 标记是否进入播放模式

#if UNITY_EDITOR
        private void OnEnable()
        {
            // 监听播放模式改变事件
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            // 移除播放模式改变事件监听
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                isPlaying = true;
                savedHitInfos = hitInfos?.ToArray(); // 保存当前的 hitInfos
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                isPlaying = false;
                savedHitInfos = null; // 退出播放模式时清空保存的 hitInfos
            }
        }

        private void OnDrawGizmos()
        {
            if (!isPlaying)
            {
                CalculateTrail();
            }

            if (IsAnyTrailVisible())
            {
                DrawTrail();
            }
        }

        private bool IsAnyTrailVisible()
        {
            if (Application.isPlaying)
                return true;
            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null)
                return false;
            var viewCamera = sceneView.camera;
            if (viewCamera == null)
                return false;
            var planes = GeometryUtility.CalculateFrustumPlanes(viewCamera);
            var infosToCheck = isPlaying ? savedHitInfos : hitInfos;
            return infosToCheck != null && infosToCheck.Any(hit =>
                GeometryUtility.TestPlanesAABB(planes, new Bounds(hit.start, Vector3.one * 0.1f)) ||
                GeometryUtility.TestPlanesAABB(planes, new Bounds(hit.end, Vector3.one * 0.1f)));
        }

        private void CalculateTrail()
        {
            if (isPlaying) return; // 进入播放模式后停止计算

            hitInfos = new HitInfo[resolution - 1];
            localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            hasFirstHit = false;

            for (var i = 0; i < resolution - 1; i++)
            {
                var t1 = (float)i / (resolution - 1);
                var t2 = (float)(i + 1) / (resolution - 1);
                var x1 = Mathf.Lerp(0f, renderDistance, t1);
                var x2 = Mathf.Lerp(0f, renderDistance, t2);
                var y1 = GetValue(x1);
                var y2 = GetValue(x2);
                var startLocal = new Vector3(x1, y1, 0);
                var endLocal = new Vector3(x2, y2, 0);
                var startWorld = localToWorldMatrix.MultiplyPoint3x4(startLocal);
                var endWorld = localToWorldMatrix.MultiplyPoint3x4(endLocal);

                if (hasFirstHit)
                {
                    hitInfos[i] = new HitInfo
                    {
                        start = startWorld,
                        end = endWorld,
                        isFirstHit = false
                    };
                    continue;
                }

                if (Physics.Linecast(startWorld, endWorld, out var hit, layerMask, queryTriggerInteraction))
                {
                    hasFirstHit = true;

                    hitInfos[i] = new HitInfo
                    {
                        point = hit.point,
                        normal = hit.normal,
                        start = startWorld,
                        end = hit.point,
                        isFirstHit = true
                    };
                }
                else
                {
                    hitInfos[i] = new HitInfo
                    {
                        start = startWorld,
                        end = endWorld,
                        isFirstHit = false
                    };
                }
            }
        }

        private void DrawTrail()
        {
            var infosToDraw = isPlaying ? savedHitInfos : hitInfos;
            if (infosToDraw == null)
                return;
            var originalMatrix = Gizmos.matrix;
            Gizmos.matrix = localToWorldMatrix;

            for (var i = 0; i < infosToDraw.Length; i++)
            {
                var hit = infosToDraw[i];
                if (hasFirstHit && i > System.Array.FindIndex(infosToDraw, h => h.isFirstHit))
                    break;

                var localStart = localToWorldMatrix.inverse.MultiplyPoint3x4(hit.start);
                var localEnd = localToWorldMatrix.inverse.MultiplyPoint3x4(hit.end);
                var localPoint = localToWorldMatrix.inverse.MultiplyPoint3x4(hit.point);
                var localNormal = localToWorldMatrix.inverse.MultiplyVector(hit.normal);

                if (hit.isFirstHit)
                {
                    Gizmos.color = hitColor;
                    Gizmos.DrawSphere(localPoint, 0.1f);
                    Gizmos.DrawLine(localPoint, localPoint + localNormal * 2);
                    Gizmos.DrawWireCube(localPoint, new Vector3(1, 0, 1));
                    Gizmos.color = trailColor;
                    Gizmos.DrawLine(localStart, localPoint);
                }
                else
                {
                    Gizmos.color = trailColor;
                    Gizmos.DrawLine(localStart, localEnd);
                }
            }

            Gizmos.matrix = originalMatrix;
        }

        private float GetValue(float x)
        {
            float result;
            if (Player.Instance)
                result = verticalImpulse * x / (Player.Instance.characterRigidbody.mass * horizontalSpeed) -
                         x * x * Physics.gravity.magnitude / (2 * horizontalSpeed * horizontalSpeed);
            else result = -x * x * Physics.gravity.magnitude / (2 * horizontalSpeed * horizontalSpeed);
            return result;
        }
#endif
    }
}