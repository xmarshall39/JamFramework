using UnityEngine;

namespace JamFramework
{
    public class GameplayTimer : MonoBehaviour
    {
        public float ElapsedTime { get; private set; } = 0f;
        public bool IsTimerRunning { get; private set; }

        public void RestartTimer()
        {
            IsTimerRunning = true;
            ElapsedTime = 0f;
        }

        public void StartTimer()
        {
            IsTimerRunning = true;
        }

        public void PauseTimer()
        {
            IsTimerRunning = false;
        }

        public int GetMinutes() => (int)(ElapsedTime / 60f);
        public int GetSeconds() => (int)ElapsedTime;
        public int GetMilliseconds() => (int)(ElapsedTime * 100);

        public int GetRollingSeconds() => (int)(ElapsedTime % 60);
        public int GetRollingMilliseconds() => (int)(GetMilliseconds() % 100);

        void Update()
        {
            if (IsTimerRunning)
            {
                ElapsedTime += Time.deltaTime;
            }
        }
    }
}
