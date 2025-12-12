using DancingLineFanmade.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class PyramidTrigger : MonoBehaviour
    {
        [SerializeField, EnumToggleButtons] private TriggerType type = TriggerType.Open;

        [SerializeField, ShowIf("@type == TriggerType.Final")]
        private bool changeDirection;

        [SerializeField, ShowIf("@type == TriggerType.Final && changeDirection")]
        private Vector3 finalDirection = Vector3.zero;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            transform.parent.GetComponent<Pyramid>().Trigger(type);
            if (type != TriggerType.Final || !changeDirection) 
                return;
            Player.Instance.firstDirection = finalDirection;
            Player.Instance.secondDirection = finalDirection;
            Player.Instance.Turn();
        }
    }
}