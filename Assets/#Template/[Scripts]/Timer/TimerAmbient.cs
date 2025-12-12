using DancingLineFanmade.Level;
using DG.Tweening;
using UnityEngine;

namespace DancingLineFanmade.Timer
{
    public class TimerAmbient : TimerBase
    {
        [SerializeField] private AmbientSettings ambientSetting;

        protected override void TriggerAnimator()
        {
            base.TriggerAnimator();
            var tween = useCurve ? ambientSetting.DoAmbient(duration, curve) : ambientSetting.DoAmbient(duration, ease);
            tween.OnComplete(onAnimatorFinished.Invoke);
        }
    }
}