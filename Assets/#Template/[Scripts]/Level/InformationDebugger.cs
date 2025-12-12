using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class InformationDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum DebugInformation
        {
            GameState,
            Progress,
            AudioTime,
            AudioTimeFormatted,
            PlayerPosition,
            PlayerLocalPosition,
            PlayerRotation,
            PlayerLocalRotation,
            PlayerSpeed,
            BlockCount,
            CheckpointCount,
            Gravity,
            CameraFollowerOffset,
            CameraFollowerRotation,
            CameraFollowerRealRotation,
            CameraFollowerScale,
            CameraPosition,
            CameraLocalPosition,
            CameraRotation,
            CameraLocalRotation,
            CameraFov,
            FogMode,
            FogStart,
            FogEnd,
            FogDensity,
            LightRotation,
            LightLocalRotation,
            LightIntensity,
            LightShadowStrength,
            AmbientLightingType,
            AmbientIntensity,
            VideoQuality,
            AntiAliasing,
            Shadow
        }

        [SerializeField] private bool display = true;
        [SerializeField, DrawWithUnity] private KeyCode key = KeyCode.D;
        [SerializeField] private int size = 25;
        [SerializeField] private float space = 5;
        [SerializeField] private Font font;
        [SerializeField] private Rect rect = new(10, 10, 640, 30);

        [SerializeField, DrawWithUnity] private List<DebugInformation> information = new()
        {
            DebugInformation.Progress,
            DebugInformation.GameState,
            DebugInformation.PlayerPosition,
            DebugInformation.PlayerRotation,
            DebugInformation.BlockCount,
            DebugInformation.CheckpointCount,
            DebugInformation.CameraFollowerOffset,
            DebugInformation.CameraFollowerRotation,
            DebugInformation.CameraFollowerScale,
            DebugInformation.CameraFov
        };

        private Player player { get; set; }
        private Camera sceneCamera { get; set; }
        private Light sceneLight { get; set; }
        private bool show { get; set; }

        private static string antiAliasing
        {
            get
            {
                return QualitySettings.antiAliasing switch
                {
                    0 => "Disabled",
                    2 => "2x MSAA",
                    4 => "4x MSAA",
                    8 => "8x MSAA",
                    _ => "Disabled"
                };
            }
        }

        private static string shadow
        {
            get
            {
                return QualitySettings.shadows switch
                {
                    ShadowQuality.Disable => "Disabled",
                    ShadowQuality.All or ShadowQuality.HardOnly => "Enabled",
                    _ => "Disabled"
                };
            }
        }

        private void Start()
        {
            player = Player.Instance;
            sceneCamera = Player.Instance.sceneCamera;
            sceneLight = Player.Instance.sceneLight;
            show = display;
        }

        private void Update()
        {
            if (Input.GetKeyDown(key))
                show = !show;
        }

        private string GetInformation(DebugInformation type)
        {
            return type switch
            {
                DebugInformation.GameState => $"游戏状态：{LevelManager.GameState}",
                DebugInformation.Progress => $"关卡进度：{player.SoundTrackProgress}%",
                DebugInformation.AudioTime => $"音乐秒数：{AudioManager.Time:f2}s / {AudioManager.Length:f2}s",
                DebugInformation.AudioTimeFormatted =>
                    $"音乐时间：{FormatTime(AudioManager.Time)} / {FormatTime(AudioManager.Length)}",
                DebugInformation.PlayerPosition => $"线的坐标：{player.transform.position}",
                DebugInformation.PlayerLocalPosition => $"线的相对坐标：{player.transform.localPosition}",
                DebugInformation.PlayerRotation => $"线的朝向：{player.transform.eulerAngles}",
                DebugInformation.PlayerLocalRotation => $"线的相对朝向：{player.transform.localEulerAngles}",
                DebugInformation.PlayerSpeed => $"线的速度：{player.Speed}",
                DebugInformation.BlockCount => $"获取方块数量：{player.BlockCount}/{player.BlockLimit}",
                DebugInformation.CheckpointCount => $"激活检查点数量：{player.Checkpoints.Count}",
                DebugInformation.Gravity => $"场景重力：{Physics.gravity}",
                DebugInformation.CameraFollowerOffset => $"跟随相机偏移：{CameraFollower.Instance.rotator.localPosition}",
                DebugInformation.CameraFollowerRotation =>
                    $"跟随相机角度：{CameraFollower.Instance.rotator.localEulerAngles + new Vector3(60f, 0f, 0f)}",
                DebugInformation.CameraFollowerRealRotation =>
                    $"跟随相机实际角度：{CameraFollower.Instance.rotator.localEulerAngles}",
                DebugInformation.CameraFollowerScale => $"跟随相机缩放：{CameraFollower.Instance.scale.localScale}",
                DebugInformation.CameraPosition => $"相机坐标：{sceneCamera.transform.position}",
                DebugInformation.CameraLocalPosition => $"相机相对坐标：{sceneCamera.transform.localPosition}",
                DebugInformation.CameraRotation => $"相机角度：{sceneCamera.transform.eulerAngles}",
                DebugInformation.CameraLocalRotation => $"相机相对角度：{sceneCamera.transform.localEulerAngles}",
                DebugInformation.CameraFov => $"相机视场：{sceneCamera.fieldOfView}",
                DebugInformation.FogMode => $"雾气类型：{RenderSettings.fogMode}",
                DebugInformation.FogStart => $"雾气开始距离：{RenderSettings.fogStartDistance}",
                DebugInformation.FogEnd => $"雾气最远距离：{RenderSettings.fogEndDistance}",
                DebugInformation.FogDensity => $"雾气密度：{RenderSettings.fogDensity}",
                DebugInformation.LightRotation => $"光源角度：{sceneLight.transform.eulerAngles}",
                DebugInformation.LightLocalRotation => $"光源相对角度：{sceneLight.transform.localEulerAngles}",
                DebugInformation.LightIntensity => $"光源强度：{sceneLight.intensity}",
                DebugInformation.LightShadowStrength => $"阴影强度：{sceneLight.shadowStrength}",
                DebugInformation.AmbientLightingType => $"环境光类型：{RenderSettings.ambientMode}",
                DebugInformation.AmbientIntensity => $"环境光强度：{RenderSettings.ambientIntensity}",
                DebugInformation.VideoQuality => $"画质：{QualitySettings.names[QualitySettings.GetQualityLevel()]}",
                DebugInformation.AntiAliasing => $"抗锯齿：{antiAliasing}",
                DebugInformation.Shadow => $"阴影：{shadow}",
                _ => string.Empty
            };
        }

        private static string FormatTime(float seconds)
        {
            var minute = Mathf.Floor(seconds / 60);
            var second = (seconds % 60).ToString("f2");
            return $"{minute}min {second}s";
        }

        private void OnGUI()
        {
            var style = new GUIStyle
            {
                font = font,
                fontSize = size,
                normal =
                {
                    textColor = LevelManager.GetColorByContent(sceneCamera.backgroundColor)
                }
            };
            if (!show)
                return;
            for (var i = 0; i < information.Count; i++)
            {
                GUI.Label(new Rect(rect.x, rect.y + i * (size + space), rect.width, rect.height),
                    GetInformation(information[i]), style);
            }
        }
#endif
    }
}