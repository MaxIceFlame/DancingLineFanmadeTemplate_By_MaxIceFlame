using DancingLineFanmade.Level;
using DG.Tweening;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    public enum TriggerType
    {
        Open,
        Final,
        Waiting,
        Stop
    }

    [DisallowMultipleComponent]
    public class Pyramid : MonoBehaviour
    {
        [SerializeField] private float waitingTime = 5f;

        private Transform left;
        private Transform right;
        private const float width = 1.8f;
        private const float duration = 2f;

        private void Start()
        {
            left = transform.Find("Left");
            right = transform.Find("Right");
        }

        internal void Trigger(TriggerType type)
        {
            if (LevelManager.GameState == GameStatus.Died)
                return;
            switch (type)
            {
                case TriggerType.Open:
                    left.DOLocalMoveZ(width, duration).SetEase(Ease.Linear);
                    right.DOLocalMoveZ(-width, duration).SetEase(Ease.Linear);
                    LevelManager.revivePlayer += ResetDoor;
                    break;
                case TriggerType.Final:
                    if (CameraFollower.Instance) CameraFollower.Instance.follow = false;
                    LevelManager.GameState = GameStatus.Moving;
                    break;
                case TriggerType.Waiting: Invoke(nameof(Complete), waitingTime); break;
                case TriggerType.Stop: LevelManager.GameState = GameStatus.Completed; break;
            }
        }

        private void Complete()
        {
            LevelManager.GameOverNormal(true);
        }

        private void ResetDoor()
        {
            LevelManager.revivePlayer -= ResetDoor;
            left.localPosition = Vector3.zero;
            right.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetDoor;
        }
    }
}