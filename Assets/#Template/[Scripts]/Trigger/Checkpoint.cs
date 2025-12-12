using DancingLineFanmade.Level;
using DancingLineFanmade.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using DancingLineFanmade.Guideline;
using UnityEngine;
using UnityEngine.Events;

namespace DancingLineFanmade.Trigger
{
    [DisallowMultipleComponent]
    public class Checkpoint : MonoBehaviour
    {
        private Player player;

        private Transform rotator;
        private Transform frame;
        private Transform core;
        private Transform revivePosition;

        [SerializeField, Title("Player"), EnumToggleButtons]
        private Direction direction = Direction.First;

        [SerializeField, HorizontalGroup("Soundtrack")]
        private float soundtrackTime;

        [SerializeField, HorizontalGroup("Soundtrack"), HideLabel]
        private bool manualSoundtrackTime;

        [SerializeField, HorizontalGroup("Camera")]
        private new CameraSettings camera = new();

        [SerializeField, HorizontalGroup("Camera"), HideLabel]
        private bool manualCamera;

        [SerializeField, HorizontalGroup("Fog")]
        private FogSettings fog = new();

        [SerializeField, HorizontalGroup("Fog"), HideLabel]
        private bool manualFog;

        [SerializeField, HorizontalGroup("Light")]
        private new LightSettings light = new();

        [SerializeField, HorizontalGroup("Light"), HideLabel]
        private bool manualLight;

        [SerializeField, HorizontalGroup("Ambient")]
        private AmbientSettings ambient = new();

        [SerializeField, HorizontalGroup("Ambient"), HideLabel]
        private bool manualAmbient;

        [Title("Colors")] [SerializeField, TableList]
        private List<SingleColor> materialColorsAuto = new();

        [SerializeField, TableList] private List<SingleColor> materialColorsManual = new();

        [SerializeField, TableList] private List<SingleImage> imageColorsAuto = new();
        [SerializeField, TableList] private List<SingleImage> imageColorsManual = new();

        [Title("Event")] [SerializeField] private UnityEvent onRevive = new();

        private int trackProgress;
        private int playerSpeed;
        private Vector3 sceneGravity;
        private Vector3 playerFirstDirection;
        private Vector3 playerSecondDirection;

        private List<SetActive> actives = new();
        private List<PlayAnimator> animators = new();
        private List<FakePlayer> fakes = new();

        private void Start()
        {
            player = Player.Instance;

            rotator = transform.Find("Rotator");
            frame = rotator.Find("Frame");
            core = rotator.Find("Core");
            revivePosition = transform.Find("RevivePosition");
            rotator.localScale = Vector3.zero;

            actives = FindObjectsOfType<SetActive>(true).ToList();
            animators = FindObjectsOfType<PlayAnimator>(true).ToList();
            fakes = FindObjectsOfType<FakePlayer>(true).ToList();
        }

        private void Update()
        {
            frame.Rotate(Vector3.up, Time.deltaTime * -45f);
            core.Rotate(Vector3.up, Time.deltaTime * 45f);
        }

        internal void EnterTrigger()
        {
            player.Checkpoints.Add(this);
            rotator.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            if (!manualCamera && CameraFollower.Instance)
                camera = camera.GetCamera();
            if (!manualFog)
                fog = fog.GetFog();
            if (!manualLight)
                light = light.GetLight(player.sceneLight);
            if (!manualAmbient)
                ambient = ambient.GetAmbient();
            foreach (var s in materialColorsAuto)
            {
                s.GetColor();
            }

            foreach (var s in imageColorsAuto)
            {
                s.GetColor();
            }

            if (!manualSoundtrackTime)
            {
                soundtrackTime = AudioManager.Time;
                trackProgress = player.SoundTrackProgress;
            }

            playerSpeed = player.Speed;
            sceneGravity = Physics.gravity;
            playerFirstDirection = player.firstDirection;
            playerSecondDirection = player.secondDirection;

            foreach (var s in actives.Where(s => !s.activeOnAwake))
            {
                s.AddRevives();
            }

            foreach (var s in animators.SelectMany(a => a.animators.Where(s => !s.dontRevive)))
            {
                s.GetState();
            }

            foreach (var f in fakes)
            {
                f.GetData();
            }

            player.GetAnimatorProgresses();
            player.GetTimelineProgresses();
        }

        internal void Revival()
        {
            DOTween.Clear();
            LevelUI.Instance.HideScreen(fog.fogColor, 0.32f, () =>
                {
                    ResetScene();
                    LevelManager.revivePlayer.Invoke();
                    LevelManager.DestroyRemain();
                    core.gameObject.SetActive(false);
                    Player.Instance.characterRigidbody.isKinematic = true;

                    var manager = FindObjectOfType<GuidelineManager>();
                    if (manager.useGuideline)
                        manager.ResetAllTaps(soundtrackTime);
                },
                () =>
                {
                    Player.Instance.characterRigidbody.isKinematic = false;
                    player.allowTurn = true;
                });
        }

        private void ResetScene()
        {
            if (CameraFollower.Instance)
                camera.SetCamera(playerFirstDirection, playerSecondDirection);
            fog.SetFog(player.sceneCamera);
            light.SetLight(player.sceneLight);
            ambient.SetAmbient();
            foreach (var s in materialColorsAuto)
            {
                s.SetColor();
            }

            foreach (var s in materialColorsManual)
            {
                s.SetColor();
            }

            foreach (var s in imageColorsAuto)
            {
                s.SetColor();
            }

            foreach (var s in imageColorsManual)
            {
                s.SetColor();
            }

            AudioManager.Stop();
            AudioManager.Time = soundtrackTime;
            AudioManager.Volume = 1f;
            if (!manualSoundtrackTime)
                player.SoundTrackProgress = trackProgress;
            else player.SoundTrackProgress = (int)(AudioManager.GetProgress(soundtrackTime) * 100);
            player.ClearPool();
            player.BlockCount = 0;
            player.Speed = playerSpeed;
            Physics.gravity = sceneGravity;
            player.firstDirection = playerFirstDirection;
            player.secondDirection = playerSecondDirection;
            LevelManager.SetPlayerPosition(player, revivePosition.position, true, direction, true);
            foreach (var s in actives.Where(s => !s.activeOnAwake))
            {
                s.Revive();
            }

            foreach (var s in from a in animators from s in a.animators where !s.dontRevive && s.played select s)
            {
                s.SetState();
            }

            foreach (var f in fakes.Where(f => f.playing))
            {
                f.ResetState();
            }

            player.SetAnimatorProgresses();
            player.SetTimelineProgresses();
            onRevive.Invoke();
        }
    }
}