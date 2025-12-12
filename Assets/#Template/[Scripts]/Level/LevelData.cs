using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace DancingLineFanmade.Level
{
    [CreateAssetMenu(menuName = "Dancing Line Fanmade/Level Data", fileName = "Level Data")]
    public class LevelData : ScriptableObject
    {
        public string levelTitle = "标题";
        public AudioClip soundTrack;
        [Min(0)] public int speed = 12;
        [Min(0)] public int blockLimit = 10;
        [Min(0f), Range(0f, 3f)] public float timeScale = 1f;
        public Vector3 gravity = LevelManager.defaultGravity;
        [TableList] public List<SingleColor> colors = new();

        internal void SetLevelData()
        {
            Player.Instance.Speed = speed;
            Player.Instance.BlockLimit = blockLimit;
            Time.timeScale = timeScale;
            Physics.gravity = gravity;
            foreach (var s in colors)
            {
                s.SetColor();
            }
        }

        [Button("Get Colors", ButtonSizes.Large), HorizontalGroup("Color")]
        internal void GetColors()
        {
            foreach (var s in colors)
            {
                s.GetColor();
            }
        }

        [Button("Set Colors", ButtonSizes.Large), HorizontalGroup("Color")]
        internal void SetColors()
        {
            foreach (var s in colors)
            {
                s.SetColor();
            }
        }
    }

    [Serializable]
    public class SingleColor
    {
        public Material material;
        public Color color = Color.white;
        public Color emissionColor = Color.clear;

        private static readonly int Emission = Shader.PropertyToID("_Emission");

        internal void GetColor()
        {
            color = material.color;
            emissionColor = material.GetColor(Emission);
        }

        internal void SetColor()
        {
            material.color = color;
            material.SetColor(Emission, emissionColor);
        }

        internal void SetColor(float duration, Ease ease)
        {
            material.DOColor(color, duration).SetEase(ease);
            material.DOColor(emissionColor, Emission, duration).SetEase(ease);
        }
        
        internal void SetColor(float duration, AnimationCurve curve)
        {
            material.DOColor(color, duration).SetEase(curve);
            material.DOColor(emissionColor, Emission, duration).SetEase(curve);
        }

        internal Tween DoColor(float duration, Ease ease)
        {
            var tween = material.DOColor(color, duration).SetEase(ease);
            material.DOColor(emissionColor, Emission, duration).SetEase(ease);
            return tween;
        }
        
        internal Tween DoColor(float duration, AnimationCurve curve)
        {
            var tween = material.DOColor(color, duration).SetEase(curve);
            material.DOColor(emissionColor, Emission, duration).SetEase(curve);
            return tween;
        }
    }

    [Serializable]
    public class SingleImage
    {
        public Image image;
        public Color color = Color.white;

        internal void GetColor()
        {
            color = image.color;
        }

        internal void SetColor()
        {
            image.color = color;
        }

        internal void SetColor(float duration, Ease ease)
        {
            image.DOColor(color, duration).SetEase(ease);
        }
        
        internal void SetColor(float duration, AnimationCurve curve)
        {
            image.DOColor(color, duration).SetEase(curve);
        }

        internal Tween DoColor(float duration, Ease ease)
        {
            return image.DOColor(color, duration).SetEase(ease);
        }
        
        internal Tween DoColor(float duration, AnimationCurve curve)
        {
            return image.DOColor(color, duration).SetEase(curve);
        }
    }

    [Serializable]
    public class FogSettings
    {
        public bool useFog = true;
        public Color fogColor = Color.white;
        public float start = 25f;
        public float end = 120f;

        public FogSettings()
        {
        }

        public FogSettings(Color fogColor, float start, float end)
        {
            useFog = true;
            this.fogColor = fogColor;
            this.start = start;
            this.end = end;
        }

        internal FogSettings GetFog()
        {
            var fog = new FogSettings
            {
                useFog = RenderSettings.fog,
                fogColor = RenderSettings.fogColor,
                start = RenderSettings.fogStartDistance,
                end = RenderSettings.fogEndDistance
            };
            return fog;
        }

        internal void SetFog(Camera camera)
        {
            RenderSettings.fog = useFog;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogStartDistance = start;
            RenderSettings.fogEndDistance = end;
            camera.backgroundColor = fogColor;
        }

        internal void SetFog(Camera camera, float duration, Ease ease)
        {
            RenderSettings.fog = useFog;
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, fogColor, duration)
                .SetEase(ease);
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x,
                start, duration).SetEase(ease);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, end,
                duration).SetEase(ease);
            DOTween.To(() => camera.backgroundColor, x => camera.backgroundColor = x, fogColor, duration)
                .SetEase(ease);
        }
        
        internal void SetFog(Camera camera, float duration, AnimationCurve curve)
        {
            RenderSettings.fog = useFog;
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, fogColor, duration)
                .SetEase(curve);
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x,
                start, duration).SetEase(curve);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, end,
                duration).SetEase(curve);
            DOTween.To(() => camera.backgroundColor, x => camera.backgroundColor = x, fogColor, duration)
                .SetEase(curve);
        }

        internal Tween DoFog(Camera camera, float duration, Ease ease)
        {
            RenderSettings.fog = useFog;
            Tween tween = DOTween
                .To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, fogColor, duration).SetEase(ease);
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x,
                start, duration).SetEase(ease);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, end,
                duration).SetEase(ease);
            DOTween.To(() => camera.backgroundColor, x => camera.backgroundColor = x, fogColor, duration)
                .SetEase(ease);
            return tween;
        }

        internal Tween DoFog(Camera camera, float duration, AnimationCurve curve)
        {
            RenderSettings.fog = useFog;
            Tween tween = DOTween
                .To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, fogColor, duration).SetEase(curve);
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x,
                start, duration).SetEase(curve);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, end,
                duration).SetEase(curve);
            DOTween.To(() => camera.backgroundColor, x => camera.backgroundColor = x, fogColor, duration)
                .SetEase(curve);
            return tween;
        }
    }

    [Serializable]
    public class LightSettings
    {
        public Vector3 rotation = Vector3.zero;
        public Color color = Color.white;
        public float intensity = 1f;
        [Range(0f, 1f)] public float shadowStrength = 0.8f;

        internal LightSettings GetLight(Light light)
        {
            var settings = new LightSettings
            {
                rotation = light.transform.eulerAngles,
                color = light.color,
                intensity = light.intensity,
                shadowStrength = shadowStrength
            };
            return settings;
        }

        internal void SetLight(Light light)
        {
            light.transform.eulerAngles = rotation;
            light.color = color;
            light.intensity = intensity;
            light.shadowStrength = shadowStrength;
        }

        internal void SetLight(Light light, float duration, Ease ease)
        {
            light.transform.DORotate(rotation, duration).SetEase(ease);
            light.DOColor(color, duration).SetEase(ease);
            light.DOIntensity(intensity, duration).SetEase(ease);
            light.DOShadowStrength(shadowStrength, duration).SetEase(ease);
        }
        
        internal void SetLight(Light light, float duration, AnimationCurve curve)
        {
            light.transform.DORotate(rotation, duration).SetEase(curve);
            light.DOColor(color, duration).SetEase(curve);
            light.DOIntensity(intensity, duration).SetEase(curve);
            light.DOShadowStrength(shadowStrength, duration).SetEase(curve);
        }

        internal Tween DoLight(Light light, float duration, Ease ease)
        {
            var tween = light.transform.DORotate(rotation, duration).SetEase(ease);
            light.DOColor(color, duration).SetEase(ease);
            light.DOIntensity(intensity, duration).SetEase(ease);
            light.DOShadowStrength(shadowStrength, duration).SetEase(ease);
            return tween;
        }

        internal Tween DoLight(Light light, float duration, AnimationCurve curve)
        {
            var tween = light.transform.DORotate(rotation, duration).SetEase(curve);
            light.DOColor(color, duration).SetEase(curve);
            light.DOIntensity(intensity, duration).SetEase(curve);
            light.DOShadowStrength(shadowStrength, duration).SetEase(curve);
            return tween;
        }
    }

    public enum EnvironmentLightingType
    {
        Skybox,
        Color,
        Gradient
    }

    [Serializable]
    public class AmbientSettings
    {
        [EnumToggleButtons] public EnvironmentLightingType lightingType = EnvironmentLightingType.Color;

        [Range(0f, 8f), ShowIf("@lightingType == EnvironmentLightingType.Skybox")]
        public float intensity = 1f;

        [ShowIf("@lightingType == EnvironmentLightingType.Color")]
        public Color ambientColor = new(0.67f, 0.67f, 0.67f, 1f);

        [ShowIf("@lightingType == EnvironmentLightingType.Gradient")]
        public Color skyColor = new(0.67f, 0.67f, 0.67f, 1f);

        [ShowIf("@lightingType == EnvironmentLightingType.Gradient")]
        public Color equatorColor = new(0.114f, 0.125f, 0.133f, 1f);

        [ShowIf("@lightingType == EnvironmentLightingType.Gradient")]
        public Color groundColor = new(0.047f, 0.043f, 0.035f, 1f);

        internal AmbientMode GetAmbientMode(EnvironmentLightingType type)
        {
            return type switch
            {
                EnvironmentLightingType.Skybox => AmbientMode.Skybox,
                EnvironmentLightingType.Color => AmbientMode.Flat,
                EnvironmentLightingType.Gradient => AmbientMode.Trilight,
                _ => AmbientMode.Flat
            };
        }

        internal EnvironmentLightingType GetEnvironmentLightingType(AmbientMode type)
        {
            return type switch
            {
                AmbientMode.Skybox => EnvironmentLightingType.Skybox,
                AmbientMode.Flat => EnvironmentLightingType.Color,
                AmbientMode.Trilight => EnvironmentLightingType.Gradient,
                _ => EnvironmentLightingType.Color
            };
        }

        internal AmbientSettings GetAmbient()
        {
            var ambient = new AmbientSettings
            {
                lightingType = GetEnvironmentLightingType(RenderSettings.ambientMode),
                intensity = RenderSettings.ambientIntensity,
                ambientColor = RenderSettings.ambientLight,
                skyColor = RenderSettings.ambientSkyColor,
                equatorColor = RenderSettings.ambientEquatorColor,
                groundColor = RenderSettings.ambientGroundColor
            };
            return ambient;
        }

        internal void SetAmbient()
        {
            RenderSettings.ambientMode = GetAmbientMode(lightingType);
            switch (lightingType)
            {
                case EnvironmentLightingType.Skybox:
                    RenderSettings.ambientIntensity = intensity;
                    break;
                case EnvironmentLightingType.Color:
                    RenderSettings.ambientLight = ambientColor;
                    break;
                case EnvironmentLightingType.Gradient:
                    RenderSettings.ambientSkyColor = skyColor;
                    RenderSettings.ambientEquatorColor = equatorColor;
                    RenderSettings.ambientGroundColor = groundColor;
                    break;
            }
        }

        internal void SetAmbient(float duration, Ease ease)
        {
            RenderSettings.ambientMode = GetAmbientMode(lightingType);
            switch (lightingType)
            {
                case EnvironmentLightingType.Skybox:
                    DOTween.To(() => RenderSettings.ambientIntensity,
                        x => RenderSettings.ambientIntensity = x, intensity, duration).SetEase(ease);
                    break;
                case EnvironmentLightingType.Color:
                    DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        ambientColor, duration).SetEase(ease);
                    break;
                case EnvironmentLightingType.Gradient:
                    DOTween.To(() => RenderSettings.ambientSkyColor, x => RenderSettings.ambientSkyColor = x,
                        skyColor, duration).SetEase(ease);
                    DOTween.To(() => RenderSettings.ambientEquatorColor,
                        x => RenderSettings.ambientEquatorColor = x, equatorColor, duration).SetEase(ease);
                    DOTween.To(() => RenderSettings.ambientGroundColor,
                        x => RenderSettings.ambientGroundColor = x, groundColor, duration).SetEase(ease);
                    break;
            }
        }
        
        internal void SetAmbient(float duration, AnimationCurve curve)
        {
            RenderSettings.ambientMode = GetAmbientMode(lightingType);
            switch (lightingType)
            {
                case EnvironmentLightingType.Skybox:
                    DOTween.To(() => RenderSettings.ambientIntensity,
                        x => RenderSettings.ambientIntensity = x, intensity, duration).SetEase(curve);
                    break;
                case EnvironmentLightingType.Color:
                    DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        ambientColor, duration).SetEase(curve);
                    break;
                case EnvironmentLightingType.Gradient:
                    DOTween.To(() => RenderSettings.ambientSkyColor, x => RenderSettings.ambientSkyColor = x,
                        skyColor, duration).SetEase(curve);
                    DOTween.To(() => RenderSettings.ambientEquatorColor,
                        x => RenderSettings.ambientEquatorColor = x, equatorColor, duration).SetEase(curve);
                    DOTween.To(() => RenderSettings.ambientGroundColor,
                        x => RenderSettings.ambientGroundColor = x, groundColor, duration).SetEase(curve);
                    break;
            }
        }

        internal Tween DoAmbient(float duration, Ease ease)
        {
            RenderSettings.ambientMode = GetAmbientMode(lightingType);
            switch (lightingType)
            {
                case EnvironmentLightingType.Skybox:
                    var tween1 = DOTween.To(() => RenderSettings.ambientIntensity,
                        x => RenderSettings.ambientIntensity = x, intensity, duration).SetEase(ease);
                    return tween1;
                case EnvironmentLightingType.Color:
                    var tween2 = DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        ambientColor, duration).SetEase(ease);
                    return tween2;
                case EnvironmentLightingType.Gradient:
                    var tween3 = DOTween.To(() => RenderSettings.ambientSkyColor,
                        x => RenderSettings.ambientSkyColor = x, skyColor, duration).SetEase(ease);
                    DOTween.To(() => RenderSettings.ambientEquatorColor,
                        x => RenderSettings.ambientEquatorColor = x, equatorColor, duration).SetEase(ease);
                    DOTween.To(() => RenderSettings.ambientGroundColor,
                        x => RenderSettings.ambientGroundColor = x, groundColor, duration).SetEase(ease);
                    return tween3;
                default:
                    return null;
            }
        }
        
        internal Tween DoAmbient(float duration, AnimationCurve curve)
        {
            RenderSettings.ambientMode = GetAmbientMode(lightingType);
            switch (lightingType)
            {
                case EnvironmentLightingType.Skybox:
                    var tween1 = DOTween.To(() => RenderSettings.ambientIntensity,
                        x => RenderSettings.ambientIntensity = x, intensity, duration).SetEase(curve);
                    return tween1;
                case EnvironmentLightingType.Color:
                    var tween2 = DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        ambientColor, duration).SetEase(curve);
                    return tween2;
                case EnvironmentLightingType.Gradient:
                    var tween3 = DOTween.To(() => RenderSettings.ambientSkyColor,
                        x => RenderSettings.ambientSkyColor = x, skyColor, duration).SetEase(curve);
                    DOTween.To(() => RenderSettings.ambientEquatorColor,
                        x => RenderSettings.ambientEquatorColor = x, equatorColor, duration).SetEase(curve);
                    DOTween.To(() => RenderSettings.ambientGroundColor,
                        x => RenderSettings.ambientGroundColor = x, groundColor, duration).SetEase(curve);
                    return tween3;
                default:
                    return null;
            }
        }
    }
}