using DancingLineFanmade.Trigger;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DancingLineFanmade.Level
{
    public enum FakePlayerState
    {
        Moving,
        Stopped
    }

    [Serializable]
    public class ResetFakePlayer
    {
        public bool played;
        public int speed;
        public Vector3 position;
        public Vector3 rotation;

        public void GetData(FakePlayer player)
        {
            played = player.playing;
            speed = player.speed;
            position = player.transform.position;
            rotation = player.transform.eulerAngles;
        }

        public void SetData(FakePlayer player)
        {
            player.playing = played;
            player.speed = speed;
            player.transform.position = position;
            player.transform.eulerAngles = rotation;
            player.ClearPool();
            player.CreateTail();
        }
    }

    [DisallowMultipleComponent, RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class FakePlayer : MonoBehaviour
    {
        public FakePlayerState state { get; set; }
        public bool playing { get; set; }

        [Min(0)] public int speed = 12;
        public Material characterMaterial;
        public Vector3 startPosition = Vector3.zero;
        public Vector3 firstDirection = new(0, 90, 0);
        public Vector3 secondDirection = Vector3.zero;
        [Min(1)] public int poolSize = 100;
        public bool isWall;
        public bool drawDirection;

        [SerializeField] private bool createTurnTrigger = true;

        [SerializeField, ShowIf("@createTurnTrigger")]
        private bool synchronismWithPlayer;

        [SerializeField, ShowIf("@createTurnTrigger && !synchronismWithPlayer"), DrawWithUnity]
        private KeyCode createKey = KeyCode.P;

        [SerializeField, ShowIf("@createTurnTrigger")]
        private Vector3 triggerRotation = Vector3.zero;

        [SerializeField, ShowIf("@createTurnTrigger")]
        private Vector3 triggerScale = Vector3.one;

        private Transform triggerHolder;
        private int id;

        private Transform selfTransform;
        private GameObject tailPrefab;
        private GameObject dustParticle;

        private BoxCollider characterCollider;
        private Vector3 tailPosition;
        private Transform tail;
        private Transform tailHolder;
        private readonly ObjectPool<Transform> tailPool = new();
        private readonly ResetFakePlayer reset = new();

        private float TailDistance =>
            new Vector2(tailPosition.x - selfTransform.position.x, tailPosition.z - selfTransform.position.z).magnitude;

        private bool previousFrameIsGrounded;
        private const float groundedRayDistance = 0.05f;
        private ValueTuple<Vector3, Ray>[] groundedTestRays;
        private readonly RaycastHit[] groundedTestResults = new RaycastHit[1];

        public bool Falling
        {
            get
            {
                for (var i = 0; i < groundedTestRays.Length; i++)
                {
                    groundedTestRays[i].Item2.origin = selfTransform.position +
                                                       selfTransform.localRotation * groundedTestRays[i].Item1;
                    if (Physics.RaycastNonAlloc(groundedTestRays[i].Item2, groundedTestResults,
                            groundedRayDistance + 0.1f, -257, QueryTriggerInteraction.Ignore) > 0)
                        return false;
                }

                return true;
            }
        }

        private void Awake()
        {
            selfTransform = transform;
            GetComponent<Rigidbody>();
            playing = false;
            tailHolder = new GameObject(gameObject.name + "-TailHolder").transform;

            characterCollider = GetComponent<BoxCollider>();
            groundedTestRays = new ValueTuple<Vector3, Ray>[]
            {
                new(
                    characterCollider.center - new Vector3(characterCollider.size.x * 0.5f,
                        characterCollider.size.y * 0.5f - 0.1f, characterCollider.size.z * 0.5f),
                    new Ray(Vector3.zero, selfTransform.localRotation * Vector3.down)),
                new(
                    characterCollider.center - new Vector3(characterCollider.size.x * -0.5f,
                        characterCollider.size.y * 0.5f - 0.1f, characterCollider.size.z * 0.5f),
                    new Ray(Vector3.zero, selfTransform.localRotation * Vector3.down)),
                new(
                    characterCollider.center - new Vector3(characterCollider.size.x * 0.5f,
                        characterCollider.size.y * 0.5f - 0.1f, characterCollider.size.z * -0.5f),
                    new Ray(Vector3.zero, selfTransform.localRotation * Vector3.down)),
                new(
                    characterCollider.center - new Vector3(characterCollider.size.x * -0.5f,
                        characterCollider.size.y * 0.5f - 0.1f, characterCollider.size.z * -0.5f),
                    new Ray(Vector3.zero, selfTransform.localRotation * Vector3.down))
            };
            previousFrameIsGrounded = Falling;

            if (createTurnTrigger) 
                triggerHolder = new GameObject("FakePlayerTriggerHolder").transform;
        }

        private void Start()
        {
            tailPool.Size = poolSize;
            firstDirection = firstDirection.Convert();
            secondDirection = secondDirection.Convert();
            selfTransform.position = startPosition;
            selfTransform.eulerAngles = firstDirection;
            tailPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/FakeTail"), selfTransform);
            dustParticle = Resources.Load<GameObject>("Prefabs/Dust");

            selfTransform.GetComponent<MeshRenderer>().material = characterMaterial;
            tailPrefab.GetComponent<MeshRenderer>().material = characterMaterial;
            if (isWall)
            {
                gameObject.tag = "Obstacle";
                tailPrefab.tag = "Obstacle";
            }
            else
            {
                gameObject.tag = "FakePlayer";
                tailPrefab.tag = "FakePlayer";
            }

            state = FakePlayerState.Stopped;
            if (!createTurnTrigger)
                synchronismWithPlayer = false;

            CreateTail();
        }

        private void Update()
        {
            switch (state)
            {
                case FakePlayerState.Moving:
                    selfTransform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.Self);
                    if (tail && !Falling)
                    {
                        tail.position = (tailPosition + selfTransform.position) * 0.5f;
                        tail.localScale = new Vector3(tail.localScale.x, tail.localScale.y, TailDistance);
                        tail.position = new Vector3(tail.position.x, selfTransform.position.y, tail.position.z);
                        tail.LookAt(selfTransform);
                    }

                    if (previousFrameIsGrounded != Falling)
                    {
                        previousFrameIsGrounded = Falling;
                        if (Falling) tail = null;
                        else
                        {
                            CreateTail();
                            Destroy(
                                Instantiate(dustParticle,
                                    new Vector3(selfTransform.localPosition.x,
                                        selfTransform.localPosition.y - selfTransform.lossyScale.y * 0.5f + 0.2f,
                                        selfTransform.localPosition.z), Quaternion.Euler(90f, 0f, 0f)), 2f);
                        }
                    }

                    if (LevelManager.GameState == GameStatus.Died || LevelManager.GameState == GameStatus.Moving)
                        state = FakePlayerState.Stopped;
#if UNITY_EDITOR
                    if (!synchronismWithPlayer)
                    {
                        if (Input.GetKeyDown(createKey)) 
                            CreateTriggers();
                    }
                    else
                    {
                        if (LevelManager.Clicked) 
                            CreateTriggers();
                    }
#endif
                    break;
            }
        }

        internal void Turn()
        {
            selfTransform.eulerAngles = selfTransform.eulerAngles == firstDirection ? secondDirection : firstDirection;
            CreateTail();
        }

        internal void CreateTail()
        {
            var now = Quaternion.Euler(selfTransform.localEulerAngles);
            var offset = tailPrefab.transform.localScale.z * 0.5f;

            if (tail)
            {
                var last = Quaternion.Euler(tail.transform.localEulerAngles);
                var angle = Quaternion.Angle(last, now);
                if (angle is >= 0f and <= 90f) offset = 0.5f * Mathf.Tan(Mathf.PI / 180f * angle * 0.5f);
                else offset = -0.5f * Mathf.Tan(Mathf.PI / 180f * ((180f - angle) * 0.5f));
                var end = tailPosition + last * Vector3.forward * (TailDistance + offset);
                tail.position = (tailPosition + end) * 0.5f;
                tail.position = new Vector3(tail.position.x, selfTransform.position.y, tail.position.z);
                tail.localScale =
                    new Vector3(tail.localScale.x, tail.localScale.y, Vector3.Distance(tailPosition, end));
                tail.LookAt(selfTransform.position);
            }

            tailPosition = selfTransform.position + now * Vector3.back * Mathf.Abs(offset);
            if (!tailPool.Full)
            {
                tail = Instantiate(tailPrefab, selfTransform.position, selfTransform.rotation).transform;
                tail.parent = tailHolder;
                tailPool.Add(tail);
            }
            else
            {
                tail = tailPool.First();
                tailPool.Add(tail);
            }
        }

        internal void GetData()
        {
            reset.GetData(this);
        }

        internal void ResetState()
        {
            state = FakePlayerState.Stopped;
            reset.SetData(this);
        }

        internal void ClearPool()
        {
            tailPool.ClearAll();
            tail = null;
        }

        private void CreateTriggers()
        {
            var g = LevelManager.CreateTrigger(selfTransform.position, triggerRotation, triggerScale, false,
                "FakePlayerTurnTrigger " + id);
            id++;
            var f = g.AddComponent<FakePlayerTrigger>();
            g.transform.parent = triggerHolder;
            f.targetPlayer = this;
            f.type = SetType.Turn;
        }

        private void OnDrawGizmos()
        {
            if (drawDirection) LevelManager.DrawDirection(transform, 4f, 3f);
        }

        [Button("Get Start Position", ButtonSizes.Large)]
        private void GetStartPosition()
        {
            startPosition = transform.position;
        }
    }
}