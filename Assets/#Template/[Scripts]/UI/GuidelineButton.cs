using UnityEngine;
using UnityEngine.UI;

namespace DancingLineFanmade.Guideline
{
    [DisallowMultipleComponent]
    public class GuidelineButton : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        [SerializeField] private Image image;
        [SerializeField] private Image background;
        [SerializeField] private Sprite on;
        [SerializeField] private Sprite off;

        private bool available;
        private GuidelineManager manager;

        private void Start()
        {
            manager = FindObjectOfType<GuidelineManager>();
            if (manager.autoplay)
            {
                available = true;
                button.SetActive(false);
                image.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
            }

            SetGuideline(available);
            if (manager.guidelineTapHolder)
                return;
            GetComponent<Button>().interactable = false;
            foreach (var i in GetComponentsInChildren<Image>())
            {
                i.enabled = false;
                i.raycastTarget = false;
            }

            background.enabled = false;
            background.raycastTarget = false;
        }

        public void OnClick()
        {
            available = !available;
            SetGuideline(available);
        }

        private void SetGuideline(bool use)
        {
            manager.useGuideline = use;
            manager.SetUseGuideline();
            image.sprite = use ? on : off;
        }
    }
}