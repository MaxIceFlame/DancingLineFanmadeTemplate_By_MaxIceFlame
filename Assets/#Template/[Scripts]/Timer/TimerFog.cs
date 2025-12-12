using DancingLineFanmade.Level;
using DG.Tweening;
using UnityEngine;

namespace DancingLineFanmade.Timer
{
    public class TimerFog : TimerBase
    {
        [SerializeField] private FogSettings fogSetting;

        protected override void TriggerAnimator()
        {
            base.TriggerAnimator();
            var sceneCamera = Player.Instance.sceneCamera;
            var tween = useCurve
                ? fogSetting.DoFog(sceneCamera, duration, curve)
                : fogSetting.DoFog(sceneCamera, duration, ease);
            tween.OnComplete(onAnimatorFinished.Invoke);
        }
    }
}