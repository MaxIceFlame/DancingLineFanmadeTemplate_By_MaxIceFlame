using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Timer
{
    public class LocalPosAnimator : AnimatorBase
    {
        [SerializeField] private Vector3 position = Vector3.zero;

        private void Start()
        {
            finalTransform = transformType switch
            {
                TransformType.New => position,
                TransformType.Add => originalTransform + position,
                _ => finalTransform
            };
            InitTransform(AnimatorType.Position);
            if (triggeredByTime)
                InitTime();
        }

        private void Update()
        {
            if (!finished && LevelManager.GameState == GameStatus.Playing && AudioManager.Time > triggerTime &&
                triggeredByTime)
                Trigger();
        }

        public void Trigger()
        {
            TriggerAnimator(AnimatorType.Position);
            if (!dontRevive)
                LevelManager.revivePlayer += ResetData;
        }

        private void ResetData()
        {
            LevelManager.revivePlayer -= ResetData;
            LevelManager.CompareCheckpointIndex(index, () =>
            {
                InitTransform(AnimatorType.Position);
                finished = false;
            });
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetData;
        }

#if UNITY_EDITOR
        [Button("Get original position", ButtonSizes.Large), HorizontalGroup("0")]
        private void GetOriginalPos()
        {
            originalTransform = transform.localPosition;
        }

        [Button("Set as original position", ButtonSizes.Large), HorizontalGroup("0")]
        private void SetOriginalPos()
        {
            transform.localPosition = originalTransform;
        }

        [Button("Get new position", ButtonSizes.Large), HorizontalGroup("1")]
        private void GetNewPos()
        {
            position = transformType switch
            {
                TransformType.New => transform.localPosition,
                TransformType.Add => transform.localPosition - originalTransform,
                _ => position
            };
        }

        [Button("Set as new position", ButtonSizes.Large), HorizontalGroup("1")]
        private void SetNewPos()
        {
            transform.localPosition = transformType switch
            {
                TransformType.New => position,
                TransformType.Add => originalTransform + position,
                _ => transform.localPosition
            };
        }
#endif
    }
}