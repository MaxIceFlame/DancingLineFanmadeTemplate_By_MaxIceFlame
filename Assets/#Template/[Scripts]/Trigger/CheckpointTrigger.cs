using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class CheckpointTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            transform.GetComponentInParent<Checkpoint>().EnterTrigger();
            gameObject.SetActive(false);
        }
    }
}