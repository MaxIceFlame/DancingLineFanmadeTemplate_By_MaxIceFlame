#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Linq;
using DancingLineFanmade.Guideline;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class PlayerTrailRenderer : MonoBehaviour
    {
        [SerializeField] private GuidelineManager controller;
        [SerializeField] private int maxDistance = 36000;
        [SerializeField] private Color trailColor = Color.blue;
        [SerializeField] private Vector3 trailOffset = new(0f, 0.4f, 0f);
        [SerializeField] private bool renderTrail;
        [SerializeField] private bool renderTime;

        private List<Transform> trans = new();

#if UNITY_EDITOR
        [Button("Reload Trail Data", ButtonSizes.Large)]
        private void Reload()
        {
            trans.Clear();
            OnValidate();
        }

        private void OnValidate()
        {
            if (!controller.guidelineTapHolder)
                return;
            trans = controller.guidelineTapHolder.GetComponentsInChildren<Transform>().ToList();
            trans.RemoveRange(0, 1);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;
            if (!renderTrail && !renderTime)
                return;
            if (controller == null)
            {
                renderTrail = false;
                renderTime = false;
                Debug.LogError("引导线控制器未选择。");
                return;
            }

            if (controller.guidelineTapHolder == null)
            {
                renderTrail = false;
                renderTime = false;
                Debug.LogError("引导线父物体未选择。");
                return;
            }

            var rendererCamera = SceneView.lastActiveSceneView.camera;
            Gizmos.color = trailColor;
            Handles.color = trailColor;
            for (var i = 0; i < trans.Count; i++)
            {
                if (!((trans[i].position - rendererCamera.transform.position).sqrMagnitude <= maxDistance))
                    continue;
                if (renderTrail)
                {
                    if (i < trans.Count - 1)
                        Handles.DrawLine(trans[i].position + trailOffset, trans[i + 1].position + trailOffset,
                            3f);

                    Gizmos.DrawCube(trans[i].position + trailOffset, Vector3.one * 0.3f);
                }

                if (!renderTime)
                    continue;

                var style = LevelManager.GUIStyle(new Color(0f, 0f, 0f, 0.6f), Color.white, 15);
                if (i <= 0)
                    continue;
                var text = $"[{i}] {trans[i].GetComponent<GuidelineTap>().triggerTime}";
                Handles.Label(trans[i].position + trailOffset, text, style);
            }
        }
#endif
    }
}