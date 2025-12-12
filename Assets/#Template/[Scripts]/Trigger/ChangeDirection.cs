using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    public enum ChangeType
    {
        Direction,
        Turn
    }

    [RequireComponent(typeof(Collider))]
    public class ChangeDirection : MonoBehaviour
    {
        [SerializeField, EnumToggleButtons] private ChangeType type = ChangeType.Direction;

        [SerializeField, ShowIf("@type == ChangeType.Direction")]
        private Vector3 firstDirection = new(0, 90, 0);

        [SerializeField, ShowIf("@type == ChangeType.Direction")]
        private Vector3 secondDirection = Vector3.zero;

        [SerializeField, ShowIf("@type == ChangeType.Direction")]
        private bool setCameraCoordinate;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            switch (type)
            {
                case ChangeType.Direction:
                    Player.Instance.firstDirection = firstDirection;
                    Player.Instance.secondDirection = secondDirection;
                    break;
                case ChangeType.Turn:
                    Player.Instance.Turn();
                    break;
            }

            if (type != ChangeType.Direction)
                return;
            Player.Instance.OnTurn.RemoveListener(SetCameraOrigin);
            if (!setCameraCoordinate)
                return;
            Player.Instance.OnTurn.AddListener(SetCameraOrigin);
            LevelManager.revivePlayer += RemovePlayerListener;
        }

        private void SetCameraOrigin()
        {
            Player.Instance.OnTurn.RemoveListener(SetCameraOrigin);
            CameraFollower.Instance.SetRotatingOrigin(firstDirection, secondDirection);
        }

        private void RemovePlayerListener()
        {
            Player.Instance.OnTurn.RemoveListener(SetCameraOrigin);
            LevelManager.revivePlayer -= RemovePlayerListener;
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= RemovePlayerListener;
        }

        private void OnDrawGizmos()
        {
            if (type != ChangeType.Direction)
                return;
            LevelManager.DrawDirection(transform, 3f, 3f);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}