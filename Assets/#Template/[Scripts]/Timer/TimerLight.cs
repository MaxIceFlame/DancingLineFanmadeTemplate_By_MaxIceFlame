using DancingLineFanmade.Level;
using DG.Tweening;
using UnityEngine;

namespace DancingLineFanmade.Timer
{
    public class TimerLight : TimerBase
    {
        [SerializeField] private LightSettings lightSetting;

        protected override void TriggerAnimator()
        {
            base.TriggerAnimator();
            var sceneLight = Player.Instance.sceneLight;
            var tween = useCurve
                ? lightSetting.DoLight(sceneLight, duration, curve)
                : lightSetting.DoLight(sceneLight, duration, ease);
            tween.OnComplete(onAnimatorFinished.Invoke);
        }
    }
}