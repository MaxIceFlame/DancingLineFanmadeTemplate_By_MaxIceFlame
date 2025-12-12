using DG.Tweening;
using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class SetMaterialColor : MonoBehaviour
    {
        [SerializeField, TableList] private List<SingleColor> colors = new();
        [SerializeField] private float duration = 2f;
        [SerializeField, DrawWithUnity] private Ease ease = Ease.Linear;
        [SerializeField] private AnimationCurve curve = LevelManager.linearCurve;
        [SerializeField] private bool useCurve;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            foreach (var s in colors)
            {
                if (useCurve)
                    s.SetColor(duration, curve);
                else s.SetColor(duration, ease);
            }
        }
    }
}