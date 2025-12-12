using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class TimeScale : MonoBehaviour
    {
        [SerializeField, DrawWithUnity] private KeyCode key = KeyCode.T;
        [SerializeField, Range(0f, 3f)] private float value = 1.5f;
        private bool on;

#if UNITY_EDITOR
        private void Update()
        {
            if (LevelManager.GameState != GameStatus.Playing)
                return;
            if (!on)
            {
                if (!Input.GetKeyDown(key))
                    return;
                AudioManager.Pitch = value;
                Time.timeScale = value;
                on = true;
            }
            else
            {
                if (!Input.GetKeyDown(key))
                    return;
                AudioManager.Pitch = 1f;
                Time.timeScale = Player.Instance.levelData.timeScale;
                on = false;
            }
        }
#endif
    }
}