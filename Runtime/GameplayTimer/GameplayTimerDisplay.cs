using UnityEngine;
using TMPro;

namespace JamFramework
{
    public class GameplayTimerDisplay : MonoBehaviour
    {
        public GameplayTimer timer;
        public TextMeshProUGUI text;

        public bool showMinutes, showSeconds, showMilliseconds;

        public void UpdateText()
        {
            if (showMinutes)
            {
                text.text = $"{timer.GetMinutes() : 00}:{Mathf.Min(timer.GetRollingSeconds(), 59) : 00}:{Mathf.Min(timer.GetRollingMilliseconds(), 99) : 00}";
            }
        }

        void Start()
        {
            UpdateText();
        }

        void LateUpdate()
        {
            if (timer.IsTimerRunning)
            {
                UpdateText();
            }
        }
    }
}
