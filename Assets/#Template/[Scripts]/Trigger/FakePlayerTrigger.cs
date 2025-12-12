using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    public enum SetType
    {
        Turn,
        ChangeDirection,
        SetState
    }

    [RequireComponent(typeof(Collider))]
    public class FakePlayerTrigger : MonoBehaviour
    {
        [SerializeField] internal FakePlayer targetPlayer;
        [SerializeField, EnumToggleButtons] internal SetType type = SetType.Turn;

        [SerializeField, ShowIf("type", SetType.ChangeDirection)]
        private Vector3 firstDirection = new(0, 90, 0);

        [SerializeField, ShowIf("type", SetType.ChangeDirection)]
        private Vector3 secondDirection = Vector3.zero;

        [SerializeField, ShowIf("type", SetType.SetState), EnumToggleButtons]
        private FakePlayerState state = FakePlayerState.Moving;

        private bool used;
        private int index;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                switch (type)
                {
                    case SetType.ChangeDirection:
                        targetPlayer.firstDirection = firstDirection;
                        targetPlayer.secondDirection = secondDirection;
                        break;
                    case SetType.SetState:
                        targetPlayer.state = state;
                        targetPlayer.playing = state switch
                        {
                            FakePlayerState.Moving => true,
                            FakePlayerState.Stopped => false,
                            _ => targetPlayer.playing
                        };
                        break;
                }
            }

            if (!other.CompareTag("FakePlayer") && !other.CompareTag("Obstacle"))
                return;
            switch (type)
            {
                case SetType.Turn:
                    if (!used)
                    {
                        index = Player.Instance.Checkpoints.Count;
                        LevelManager.revivePlayer += ResetData;
                        used = true;

                        var fakePlayerPos = targetPlayer.transform.position;
                        var position = transform.position;
                        var normalizedPlayerPos = new Vector3(fakePlayerPos.x, 0, fakePlayerPos.z);
                        var normalizedPos = new Vector3(position.x, 0, position.z);
                        var time = Mathf.Abs((normalizedPlayerPos - normalizedPos).magnitude) / targetPlayer.speed;
                        if (time > 0)
                            Invoke(nameof(TurnFakePlayer), time);
                        else TurnFakePlayer();
                    }

                    break;
            }
        }

        private void ResetData()
        {
            LevelManager.revivePlayer -= ResetData;
            LevelManager.CompareCheckpointIndex(index, () => used = false);
        }

        private void TurnFakePlayer()
        {
            targetPlayer.Turn();
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetData;
        }

        private void OnDrawGizmos()
        {
            if (type != SetType.ChangeDirection)
                return;
            LevelManager.DrawDirection(transform, 3f, 3f);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}