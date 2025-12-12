using System.Collections.Generic;
using DancingLineFanmade.Level;
using UnityEngine;
using UnityEngine.UI;

namespace DancingLineFanmade.UI
{
    [DisallowMultipleComponent]
    public class SetGameQuality : MonoBehaviour
    {
        [SerializeField] private Button qualityDecreaseButton;
        [SerializeField] private Button qualityIncreaseButton;
        [SerializeField] private Text qualityText;
        [SerializeField] private List<string> qualityNames = new();
        [SerializeField] private Button antiAliasingDecreaseButton;
        [SerializeField] private Button antiAliasingIncreaseButton;
        [SerializeField] private Text antiAliasingText;
        [SerializeField] private Toggle shadowToggle;

        private int LargestQualityLevel { get; set; }
        private int CurrentQualityLevel { get; set; }
        private int CurrentAntiAliasingLevel { get; set; }
        private int CurrentShadowLevel { get; set; }

        private void Awake()
        {
            LargestQualityLevel = QualitySettings.names.Length - 1;

#if UNITY_STANDALONE || UNITY_EDITOR
            QualitySettings.vSyncCount = 1;
#else
        QualitySettings.vSyncCount = 0;
#endif
            Application.targetFrameRate = int.MaxValue;
        }

        private void Start()
        {
            var player = Player.Instance;
            CurrentQualityLevel = player.videoQualityLevel <= LargestQualityLevel ? player.videoQualityLevel : 0;
            CurrentAntiAliasingLevel = player.antiAliasingLevel <= 3 ? player.antiAliasingLevel : 0;
            CurrentShadowLevel = player.shadow ? 1 : 0;
            QualitySettings.shadows = CurrentShadowLevel switch
            {
                0 => ShadowQuality.Disable,
                1 => ShadowQuality.HardOnly,
                _ => ShadowQuality.Disable
            };

            SetAntiAliasingButton();
            SetAntiAliasingText();
            SetQualityButton();
            SetQualityText();
            SetShadowToggle();
            SetEveryOption();
        }

        private static int ConvertAntiAliasingLevel(int level)
        {
            return level switch
            {
                0 => 0,
                1 => 2,
                2 => 4,
                3 => 8,
                _ => 0
            };
        }

        public void OnQualityChanged(bool increase)
        {
            if (increase)
                QualitySettings.IncreaseLevel(true);
            else QualitySettings.DecreaseLevel(true);
            CurrentQualityLevel = QualitySettings.GetQualityLevel();
            SetQualityButton();
            SetQualityText();
            SetEveryOption();
        }

        private void SetQualityButton()
        {
            qualityDecreaseButton.interactable = true;
            qualityIncreaseButton.interactable = true;
            if (CurrentQualityLevel <= 0)
                qualityDecreaseButton.interactable = false;
            else if (CurrentQualityLevel >= LargestQualityLevel)
                qualityIncreaseButton.interactable = false;
        }

        private void SetQualityText()
        {
            var difference = CurrentQualityLevel - qualityNames.Count;
            if (difference >= 0)
            {
                qualityText.text = $"未命名画质{difference + 1}";
                Debug.LogError("画质名称预设少于实际画质个数。请添加新的画质名称。");
            }
            else qualityText.text = qualityNames[CurrentQualityLevel];
        }

        public void OnAntiAliasingChanged(bool increase)
        {
            if (increase)
                CurrentAntiAliasingLevel++;
            else CurrentAntiAliasingLevel--;
            QualitySettings.antiAliasing = ConvertAntiAliasingLevel(CurrentAntiAliasingLevel);
            SetAntiAliasingButton();
            SetAntiAliasingText();
        }

        private void SetAntiAliasingButton()
        {
            antiAliasingDecreaseButton.interactable = true;
            antiAliasingIncreaseButton.interactable = true;
            switch (CurrentAntiAliasingLevel)
            {
                case <= 0:
                    antiAliasingDecreaseButton.interactable = false;
                    break;
                case >= 3:
                    antiAliasingIncreaseButton.interactable = false;
                    break;
                default:
                    return;
            }
        }

        private void SetAntiAliasingText()
        {
            antiAliasingText.text = CurrentAntiAliasingLevel switch
            {
                0 => "禁用",
                1 => "2x MSAA",
                2 => "4x MSAA",
                3 => "8x MSAA",
                _ => "禁用"
            };
        }

        public void OnShadowChanged()
        {
            CurrentShadowLevel = CurrentShadowLevel == 0 ? 1 : 0;
            QualitySettings.shadows = CurrentShadowLevel switch
            {
                0 => ShadowQuality.Disable,
                1 => ShadowQuality.HardOnly,
                _ => ShadowQuality.Disable
            };
        }

        private void SetShadowToggle()
        {
            var shadow = QualitySettings.shadows switch
            {
                ShadowQuality.Disable => false,
                ShadowQuality.All or ShadowQuality.HardOnly => true,
                _ => false
            };
            CurrentShadowLevel = shadow ? 1 : 0;
            shadowToggle.SetIsOnWithoutNotify(shadow);
        }

        private void SetEveryOption()
        {
            QualitySettings.antiAliasing = ConvertAntiAliasingLevel(CurrentAntiAliasingLevel);
            QualitySettings.shadows = CurrentShadowLevel switch
            {
                0 => ShadowQuality.Disable,
                1 => ShadowQuality.HardOnly,
                _ => ShadowQuality.Disable
            };
            QualitySettings.SetQualityLevel(CurrentQualityLevel, true);
        }
    }
}