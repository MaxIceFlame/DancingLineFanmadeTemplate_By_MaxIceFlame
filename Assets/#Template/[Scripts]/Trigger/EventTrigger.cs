using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent]
    public class EventTrigger : MonoBehaviour
    {
        [SerializeField] private bool invokeOnAwake;
        [SerializeField, HideIf("invokeOnAwake")] private bool invokeOnClick;
        [SerializeField] private UnityEvent onTriggerEnter = new();

        private Player player;
        private bool invoked;
        private int index;

        private void Start()
        {
            player = Player.Instance;
            if (!invokeOnAwake)
                return;
            Invoke();
            invoked = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || invokeOnAwake || invoked) 
                return;
            if (!invokeOnClick) 
                Invoke(); 
            else player.OnTurn.AddListener(Invoke);
            index = player.Checkpoints.Count;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !invokeOnAwake && invokeOnClick) 
                player.OnTurn.RemoveListener(Invoke);
        }

        private void Invoke()
        {
            if (invoked) 
                return;
            onTriggerEnter.Invoke();
            invoked = true;
            LevelManager.revivePlayer += ResetData;
        }

        private void ResetData()
        {
            LevelManager.revivePlayer -= ResetData;
            LevelManager.CompareCheckpointIndex(index, () =>
            {
                invoked = false;
                player.OnTurn.RemoveListener(Invoke);
            });
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetData;
        }
    }
}