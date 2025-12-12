using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class PlayerCubes : MonoBehaviour
    {
        private Transform[] cubes;

        internal void Play(Collision collision)
        {
            var componentsInChildren = GetComponentsInChildren<Transform>(true);
            cubes = new Transform[componentsInChildren.Length - 1];
            for (var i = 1; i < componentsInChildren.Length; i++)
            {
                cubes[i - 1] = componentsInChildren[i];
            }

            if (!(collision?.contacts.Length > 0))
                return;
            foreach (var t in cubes)
            {
                t.gameObject.SetActive(true);
                var num2 = Random.Range(0.6f, 1f);
                t.transform.localScale = new Vector3(num2, num2, num2);
                t.transform.rotation = Random.rotation;
                var normalized = t.transform.rotation.eulerAngles.normalized;
                t.gameObject.GetComponent<Rigidbody>().AddForce(normalized, ForceMode.Impulse);
            }
        }
    }
}