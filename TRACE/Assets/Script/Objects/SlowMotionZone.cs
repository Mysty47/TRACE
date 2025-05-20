using UnityEngine;

public class SlowMotionZone : MonoBehaviour
{
    [Range(0.01f, 1f)]
    public float slowMotionScale = 0.1f; // Super slow motion
    public float transitionSpeed = 2f;  // Speed of transition to slow motion
    public float slowMotionDuration = 2f; // Duration for slow-motion effect
    public GameObject SlowMotionEffect; // Assign a visual effect if needed

    private Coroutine timeScaleCoroutine;
    private Coroutine restoreTimeCoroutine;
    private bool isSlowMotionActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSlowMotionActive)
        {
            ApplySlowMotion();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isSlowMotionActive)
        {
            ApplySlowMotion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Restore time when leaving the zone
            RestoreTime();
        }
    }

    private void ApplySlowMotion()
    {
        isSlowMotionActive = true;

        if (timeScaleCoroutine != null)
            StopCoroutine(timeScaleCoroutine);

        if (restoreTimeCoroutine != null)
            StopCoroutine(restoreTimeCoroutine);

        timeScaleCoroutine = StartCoroutine(SmoothChangeTimeScale(slowMotionScale));
        EnableSlowMotionEffect(true);

        // Start restoring time after the set duration
        restoreTimeCoroutine = StartCoroutine(RestoreTimeScaleAfterDuration());
    }

    private void RestoreTime()
    {
        if (timeScaleCoroutine != null)
            StopCoroutine(timeScaleCoroutine);

        if (restoreTimeCoroutine != null)
            StopCoroutine(restoreTimeCoroutine);

        timeScaleCoroutine = StartCoroutine(SmoothChangeTimeScale(1f));
        EnableSlowMotionEffect(false);
        isSlowMotionActive = false;
    }

    private void EnableSlowMotionEffect(bool enabled)
    {
        if (SlowMotionEffect != null)
            SlowMotionEffect.SetActive(enabled);
    }

    private System.Collections.IEnumerator SmoothChangeTimeScale(float targetScale)
    {
        while (!Mathf.Approximately(Time.timeScale, targetScale))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, Time.fixedDeltaTime * transitionSpeed);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = targetScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private System.Collections.IEnumerator RestoreTimeScaleAfterDuration()
    {
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        RestoreTime();
    }
}
