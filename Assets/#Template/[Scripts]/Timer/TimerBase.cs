using DancingLineFanmade.Level;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DancingLineFanmade.Timer
{
    public abstract class TimerBase : MonoBehaviour
    {
        [SerializeField] internal UnityEvent onAnimatorStart;
        [SerializeField] internal UnityEvent onAnimatorFinished;
        [SerializeField, Min(0f)] protected float triggerTime;
        [SerializeField, Min(0f)] protected float duration = 2f;
        [SerializeField] private bool offsetTime;
        [SerializeField] private bool dontRevive;
        [SerializeField, DrawWithUnity] protected Ease ease = Ease.InOutSine;
        [SerializeField] protected AnimationCurve curve = LevelManager.linearCurve;
        [SerializeField] protected bool useCurve;

        private bool finished;
        private int index;

        private void Start()
        {
            triggerTime = offsetTime ? triggerTime - duration : triggerTime;
        }

        protected virtual void TriggerAnimator()
        {
            index = Player.Instance.Checkpoints.Count;
            finished = true;
            onAnimatorStart.Invoke();
            if (!dontRevive)
                LevelManager.revivePlayer += ResetState;
        }

        private void Update()
        {
            if (!finished && LevelManager.GameState == GameStatus.Playing && AudioManager.Time > triggerTime)
                TriggerAnimator();
        }

        private void ResetState()
        {
            LevelManager.revivePlayer -= ResetState;
            LevelManager.CompareCheckpointIndex(index, () =>
            {
                finished = false;
            });
        }
        
        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetState;
        }
    }
}