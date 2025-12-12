using DG.Tweening;
using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class SetLight : MonoBehaviour
    {
        [SerializeField] private new LightSettings light;
        [SerializeField] private float duration = 2f;
        [SerializeField, DrawWithUnity] private Ease ease = Ease.Linear;
        [SerializeField] private AnimationCurve curve = LevelManager.linearCurve;
        [SerializeField] private bool useCurve;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (useCurve)
                light.SetLight(Player.Instance.sceneLight, duration, curve);
            else light.SetLight(Player.Instance.sceneLight, duration, ease);
        }
    }
}