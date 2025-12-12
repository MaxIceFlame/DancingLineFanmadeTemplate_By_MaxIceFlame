using DG.Tweening;
using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class SetFog : MonoBehaviour
    {
        [SerializeField] private FogSettings fog;
        [SerializeField] private float duration = 2f;
        [SerializeField, DrawWithUnity] private Ease ease = Ease.Linear;
        [SerializeField] private AnimationCurve curve = LevelManager.linearCurve;
        [SerializeField] private bool useCurve;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (useCurve)
                fog.SetFog(Player.Instance.sceneCamera, duration, curve);
            else fog.SetFog(Player.Instance.sceneCamera, duration, ease);
        }
    }
}