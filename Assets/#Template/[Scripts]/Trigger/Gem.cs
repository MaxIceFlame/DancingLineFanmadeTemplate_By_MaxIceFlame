using DancingLineFanmade.Level;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    public class Gem : MonoBehaviour
    {
        [SerializeField] private Mesh gemMesh;
        [SerializeField] private string effectPath = "Prefabs/GetGem";
        [SerializeField] private bool fake;

        private Player player;
        private GameObject effectPrefab;
        private GameObject effect;
        private bool got;
        private int index;

        private MeshRenderer meshRenderer;

        private MeshRenderer MeshRenderer
        {
            get
            {
                if (!meshRenderer)
                    meshRenderer = GetComponent<MeshRenderer>();
                return meshRenderer;
            }
        }

        private MeshFilter meshFilter;

        private MeshFilter MeshFilter
        {
            get
            {
                if (!meshFilter)
                    meshFilter = GetComponent<MeshFilter>();
                return meshFilter;
            }
        }

        private void Start()
        {
            MeshRenderer.enabled = true;
            MeshFilter.mesh = gemMesh;
            effectPrefab = Resources.Load<GameObject>(effectPath);
            player = Player.Instance;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 60f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || got || fake)
                return;
            got = true;
            player.Events?.Invoke(6);
            MeshRenderer.enabled = false;
            index = player.Checkpoints.Count;
            if (QualitySettings.GetQualityLevel() > 0)
                effect = Instantiate(effectPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
            if (player.BlockCount < player.BlockLimit)
                player.BlockCount++;
            LevelManager.revivePlayer += ResetData;
        }

        private void ResetData()
        {
            LevelManager.revivePlayer -= ResetData;
            LevelManager.CompareCheckpointIndex(index, () =>
            {
                got = false;
                MeshRenderer.enabled = true;
            });
            if (effect)
                Destroy(effect);
        }

        private void OnDestroy()
        {
            LevelManager.revivePlayer -= ResetData;
        }
    }
}