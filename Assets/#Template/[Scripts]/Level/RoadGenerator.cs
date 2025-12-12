using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class RoadGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private float roadWidth = 2f;
        [SerializeField] private float roadHeight = 1f;

        private GameObject road;
        private Vector3 roadPosition;
        private Transform currentRoad;
        private Transform roadHolder;

        private Vector3 roadObjectPosition =>
            Player.Instance.transform.position - new Vector3(0f, 0.5f * (roadHeight + 1f), 0f);

        private float RoadDistance => new Vector2(roadPosition.x - Player.Instance.transform.position.x,
            roadPosition.z - Player.Instance.transform.position.z).magnitude;

        private void Awake()
        {
            roadHolder = new GameObject("GeneratedRoadHolder").transform;
            road = Instantiate(roadPrefab, Vector3.one * 10000, Quaternion.Euler(Vector3.zero));
            road.transform.localScale = Vector3.one;
            roadPrefab.SetActive(false);
        }

        private void Start()
        {
            CreateRoad();
            currentRoad.position = roadObjectPosition;
            currentRoad.localScale = new Vector3(roadWidth, roadHeight, roadWidth);
            currentRoad.LookAt(roadObjectPosition);
            Player.Instance.OnTurn.AddListener(CreateRoad);
        }

        private void Update()
        {
            if (LevelManager.GameState != GameStatus.Playing || !currentRoad)
                return;
            currentRoad.position = (roadPosition + roadObjectPosition) * 0.5f;
            currentRoad.localScale = new Vector3(roadWidth, roadHeight, RoadDistance + roadWidth);
            currentRoad.LookAt(roadObjectPosition);
        }

        private void CreateRoad()
        {
            if (currentRoad)
            {
                var last = Quaternion.Euler(currentRoad.transform.localEulerAngles);
                var end = roadPosition + last * Vector3.forward * RoadDistance;
                currentRoad.position = (roadPosition + end) * 0.5f;
                currentRoad.position =
                    new Vector3(currentRoad.position.x, roadObjectPosition.y, currentRoad.position.z);
                currentRoad.localScale =
                    new Vector3(roadWidth, roadHeight, Vector3.Distance(roadPosition, end) + roadWidth);
                currentRoad.LookAt(roadObjectPosition);
            }

            roadPosition = roadObjectPosition;
            currentRoad = Instantiate(road, roadObjectPosition, Player.Instance.transform.rotation).transform;
            currentRoad.parent = roadHolder;
        }
#endif
    }
}