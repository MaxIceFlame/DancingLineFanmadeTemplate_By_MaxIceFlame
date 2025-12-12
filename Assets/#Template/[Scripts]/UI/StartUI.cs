using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DancingLineFanmade.UI
{
    [DisallowMultipleComponent]
    public class StartUI : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> moveLeft;
        [SerializeField] private List<RectTransform> moveDown;

        public void Hide()
        {
            foreach (var l in moveLeft)
            {
                if (l.GetComponent<Button>())
                    l.GetComponent<Button>().interactable = false;
                l.DOAnchorPos(new Vector2(-120f, l.anchoredPosition.y), 0.4f).SetEase(Ease.InSine)
                    .OnComplete(() => { Destroy(gameObject); });
            }

            foreach (var d in moveDown)
            {
                if (d.GetComponent<Button>())
                    d.GetComponent<Button>().interactable = false;
                d.DOAnchorPos(new Vector2(d.anchoredPosition.x, -300f), 0.4f).SetEase(Ease.InSine);
            }
        }
    }
}