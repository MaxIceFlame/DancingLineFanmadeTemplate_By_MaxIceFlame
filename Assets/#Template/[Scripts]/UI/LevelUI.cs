using DancingLineFanmade.Level;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DancingLineFanmade.UI
{
    public class LevelUI : MonoBehaviour
    {
        public static LevelUI Instance { get; private set; }

        [Title("Normal")] [SerializeField] private Text title;
        [SerializeField] private Text percentage;
        [SerializeField] private Text block;
        [SerializeField] private Image background;
        [SerializeField] private RectTransform barFill;
        [SerializeField] private RectTransform moveUpPart;
        [SerializeField] private RectTransform moveDownPart;
        [SerializeField] private List<CanvasGroup> normalAlpha;
        [SerializeField] private List<Button> buttons;

        [Title("Revive")] [SerializeField] private Text percentageRevive;
        [SerializeField] private RectTransform barFillRevive;
        [SerializeField] private RectTransform moveUpRevive;
        [SerializeField] private RectTransform moveDownRevive;
        [SerializeField] private Image hideScreenImage;
        [SerializeField] private List<CanvasGroup> reviveAlpha;
        [SerializeField] private List<Button> buttonsRevive;

        private Player player;
        private float progress;

        private void Awake()
        {
            Instance = this;
            player = Player.Instance;

            moveUpPart.anchoredPosition = new Vector2(0f, -250f);
            moveDownPart.anchoredPosition = new Vector2(0f, 430f);
            moveUpRevive.anchoredPosition = new Vector2(0f, -250f);
            moveDownRevive.anchoredPosition = new Vector2(0f, 260f);

            foreach (var group in normalAlpha)
            {
                group.alpha = 0f;
            }

            foreach (var group in reviveAlpha)
            {
                group.alpha = 0f;
            }

            background.color = Color.clear;

            foreach (var b in buttons)
            {
                b.interactable = false;
            }

            foreach (var b in buttonsRevive)
            {
                b.interactable = false;
            }
        }

        internal void NormalPage(float percent, int blockCount)
        {
            progress = percent;
            ShowPage(true, percent, blockCount);
        }

        internal void RevivePage(float percent)
        {
            progress = percent;
            ShowPage(false, percent);
        }

        internal void ShowPage(bool normal, float percent, int blockCount = 0)
        {
            if (normal)
            {
                moveUpPart.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutSine);
                moveDownPart.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutSine);
                background.DOFade(0.64f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    foreach (var b in buttons)
                    {
                        b.interactable = true;
                    }
                });
                foreach (var c in normalAlpha)
                {
                    c.DOFade(1f, 0.4f).SetEase(Ease.Linear);
                }

                barFill.sizeDelta = new Vector2(10f, 18f) + new Vector2(480f * percent, 0f);
                percentage.text = $"{(int)(percent * 100f)}%";
                block.text = $"{blockCount}/{player.BlockLimit}";
                title.text = player.levelData.levelTitle;
            }
            else
            {
                moveUpRevive.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutSine);
                moveDownRevive.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutSine);
                background.DOFade(0.64f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    foreach (var b in buttonsRevive)
                    {
                        b.interactable = true;
                    }
                });
                foreach (var c in reviveAlpha)
                {
                    c.DOFade(1f, 0.4f).SetEase(Ease.Linear);
                }

                barFillRevive.sizeDelta = new Vector2(10f, 18f) + new Vector2(480f * percent, 0f);
                percentageRevive.text = $"{(int)(percent * 100f)}%";
            }
        }

        public void ReloadScene()
        {
            foreach (var b in buttons)
            {
                b.interactable = false;
            }

            if (LoadingUI.Instance) LoadingUI.Instance.Load(SceneManager.GetActiveScene().name);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RevivePlayer()
        {
            foreach (var b in buttonsRevive)
            {
                b.interactable = false;
            }

            Player.RevivePlayer(player.Checkpoints[^1]);
        }

        public void CancelRevive()
        {
            foreach (var b in buttonsRevive)
            {
                b.interactable = false;
            }

            NormalPage(progress, player.BlockCount);

            moveUpRevive.DOAnchorPos(new Vector2(0f, -250f), 0.4f).SetEase(Ease.OutSine);
            moveDownRevive.DOAnchorPos(new Vector2(0f, 260f), 0.4f).SetEase(Ease.OutSine);
            foreach (var c in reviveAlpha)
            {
                c.DOFade(0f, 0.4f).SetEase(Ease.Linear);
            }

            foreach (var c in normalAlpha)
            {
                c.DOFade(1f, 0.4f).SetEase(Ease.Linear);
            }
        }

        internal void HideScreen(Color color, float duration, UnityAction fadeIn, UnityAction fadeOut)
        {
            foreach (var b in buttons)
            {
                b.interactable = false;
            }

            foreach (var b in buttonsRevive)
            {
                b.interactable = false;
            }

            hideScreenImage.color = new Color(color.r, color.g, color.b, 0f);
            hideScreenImage.DOFade(1f, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                ResetUI();
                fadeIn.Invoke();
                hideScreenImage.DOFade(0f, duration).SetEase(Ease.Linear).OnComplete(fadeOut.Invoke);
            });
        }

        private void ResetUI()
        {
            moveUpPart.anchoredPosition = new Vector2(0f, -250f);
            moveDownPart.anchoredPosition = new Vector2(0f, 430f);
            moveUpRevive.anchoredPosition = new Vector2(0f, -250f);
            moveDownRevive.anchoredPosition = new Vector2(0f, 260f);

            foreach (var group in normalAlpha)
            {
                group.alpha = 0f;
            }

            foreach (var group in reviveAlpha)
            {
                group.alpha = 0f;
            }

            background.color = Color.clear;

            foreach (var b in buttons)
            {
                b.interactable = false;
            }

            foreach (var b in buttonsRevive)
            {
                b.interactable = false;
            }
        }
    }
}