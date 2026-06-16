using System.Collections;
using UnityEngine;

namespace JamFramework
{
    public static class Helpers
    {
        #region Animation

        public static IEnumerator WaitForAnimationComplete(this Animator animator)
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }

        #endregion

        #region Audio
        public static float StandardPitchRange() => Random.Range(0.9f, 1.1f);

        public static IEnumerator ReleaseWhenComplete(AudioSource source)
        {
            yield return new WaitUntil( () => !source.isPlaying);
            AudioManager.Instance?.ReturnAudioSource(source);
        }
        #endregion
    }
}
