using System.Collections.Generic;
using System.Linq;
using DancingLineFanmade.Level;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Timer
{
    public class TimerMaterialColor : TimerBase
    {
        [SerializeField, TableList] private List<SingleColor> colors = new();

        protected override void TriggerAnimator()
        {
            base.TriggerAnimator();
            var tweens = colors.Select(VARIABLE =>
                useCurve ? VARIABLE.DoColor(duration, curve) : VARIABLE.DoColor(duration, ease)).ToList();
            tweens[0].OnComplete(onAnimatorFinished.Invoke);
        }
    }
}