using DancingLineFanmade.Level;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [RequireComponent(typeof(Collider))]
    public class SetImageColor : MonoBehaviour
    {
        [SerializeField, TableList] private List<SingleImage> images = new();
        [SerializeField] private float duration = 2f;
        [SerializeField, DrawWithUnity] private Ease ease = Ease.Linear;
        [SerializeField] private AnimationCurve curve = LevelManager.linearCurve;
        [SerializeField] private bool useCurve;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            foreach (var s in images)
            {
                if (useCurve)
                    s.SetColor(duration, curve);
                else s.SetColor(duration, ease);
            }
        }
    }
}