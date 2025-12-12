using DG.Tweening;
using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Timer
{
    public class LocalRotAnimator : AnimatorBase
    {
        [SerializeField] private Vector3 rotation = Vector3.zero;
        [SerializeField, DrawWithUnity] private RotateMode rotateMode = RotateMode.Fast;

        private void Start()
        {
            finalTransform = transformType switch
            {
                TransformType.New => rotation,
                TransformType.Add => originalTransform + rotation,
                _ => finalTransform
            };
            InitTransform(AnimatorType.Rotation);
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
            TriggerAnimator(AnimatorType.Rotation, rotateMode);
            if (!dontRevive)
                LevelManager.revivePlayer += ResetData;
        }

        private void ResetData()
        {
            LevelManager.revivePlayer -= ResetData;
            LevelManager.CompareCheckpointIndex(index, () =>
            {
                InitTransform(AnimatorType.Rotation);
                finished = false;
            });
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetData;
        }

#if UNITY_EDITOR
        [Button("Get original rotation", ButtonSizes.Large), HorizontalGroup("0")]
        private void GetOriginalRot()
        {
            originalTransform = transform.localEulerAngles;
        }

        [Button("Set as original rotation", ButtonSizes.Large), HorizontalGroup("0")]
        private void SetOriginalRot()
        {
            transform.localEulerAngles = originalTransform;
        }
#endif
    }
}