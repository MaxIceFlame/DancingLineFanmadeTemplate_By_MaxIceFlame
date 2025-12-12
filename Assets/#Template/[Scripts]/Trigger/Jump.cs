using DancingLineFanmade.Level;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class Jump : MonoBehaviour
    {
        [SerializeField, Min(0f)] internal float impulse = 500f;
        [SerializeField] private bool changeDirection;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (changeDirection)
                Player.Instance.Turn();
            Player.Instance.characterRigidbody.AddForce(0, impulse, 0, ForceMode.Impulse);
            Player.Instance.Events?.Invoke(7);
        }
    }
}