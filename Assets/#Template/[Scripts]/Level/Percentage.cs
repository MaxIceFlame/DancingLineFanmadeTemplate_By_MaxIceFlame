using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent, RequireComponent(typeof(SpriteRenderer))]
    public class Percentage : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        [SerializeField, DrawWithUnity] private LevelPercentage percentage;
        [SerializeField] private Color color = Color.black;

        [HideInInspector] public Sprite p10;
        [HideInInspector] public Sprite p20;
        [HideInInspector] public Sprite p30;
        [HideInInspector] public Sprite p40;
        [HideInInspector] public Sprite p50;
        [HideInInspector] public Sprite p60;
        [HideInInspector] public Sprite p70;
        [HideInInspector] public Sprite p80;
        [HideInInspector] public Sprite p90;

        private void Start()
        {
            SetPercentage();
        }
        
        private void OnEnable()
        {
            SetPercentage();
        }

        private void OnValidate()
        {
            SetPercentage();
        }

        private void SetPercentage()
        {
            spriteRenderer = GetComponent<SpriteRenderer>()
                ? GetComponent<SpriteRenderer>()
                : gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.color = color;

            switch (percentage)
            {
                case LevelPercentage.p10:
                    spriteRenderer.sprite = p10;
                    gameObject.name = "10%";
                    break;
                case LevelPercentage.p20:
                    spriteRenderer.sprite = p20;
                    gameObject.name = "20%";
                    break;
                case LevelPercentage.p30:
                    spriteRenderer.sprite = p30;
                    gameObject.name = "30%";
                    break;
                case LevelPercentage.p40:
                    spriteRenderer.sprite = p40;
                    gameObject.name = "40%";
                    break;
                case LevelPercentage.p50:
                    spriteRenderer.sprite = p50;
                    gameObject.name = "50%";
                    break;
                case LevelPercentage.p60:
                    spriteRenderer.sprite = p60;
                    gameObject.name = "60%";
                    break;
                case LevelPercentage.p70:
                    spriteRenderer.sprite = p70;
                    gameObject.name = "70%";
                    break;
                case LevelPercentage.p80:
                    spriteRenderer.sprite = p80;
                    gameObject.name = "80%";
                    break;
                case LevelPercentage.p90:
                    spriteRenderer.sprite = p90;
                    gameObject.name = "90%";
                    break;
            }
        }
    }
    
    public enum LevelPercentage
    {
        [InspectorName("10%")] p10,
        [InspectorName("20%")] p20,
        [InspectorName("30%")] p30,
        [InspectorName("40%")] p40,
        [InspectorName("50%")] p50,
        [InspectorName("60%")] p60,
        [InspectorName("70%")] p70,
        [InspectorName("80%")] p80,
        [InspectorName("90%")] p90
    }
}