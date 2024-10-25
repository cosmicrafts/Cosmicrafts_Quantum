using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToTop : MonoBehaviour
{
    private ScrollRect scrollRect;

    // Define the bounce animation curve in the Inspector. 
    // For example, start at 1 (top), overshoot to 1.05 (above the top), and then back to 1.
    public AnimationCurve bounceCurve = new AnimationCurve(
    new Keyframe(0f, 1f), // Start at the top
    new Keyframe(0.5f, 1.0075f), // Slight overshoot above the top
    new Keyframe(1f, 1f) // Settle back at the top
);



    void OnEnable()
    {
        if (!scrollRect) scrollRect = GetComponent<ScrollRect>();
        StartCoroutine(ResetScrollSmoothly());
    }

    IEnumerator ResetScrollSmoothly()
    {
        // Wait for content to be ready.
        yield return new WaitForSeconds(0.1f);

        float duration = 0.6f; // Duration of the bounce effect.
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate the elapsed proportion of the duration.
            float proportion = elapsed / duration;

            // Use the animation curve to get the position value.
            float curvePosition = bounceCurve.Evaluate(proportion);

            // Apply the calculated position from the curve.
            scrollRect.verticalNormalizedPosition = curvePosition;

            // Increment the elapsed time.
            elapsed += Time.deltaTime;

            // Wait until next frame.
            yield return null;
        }

        // Optional: Explicitly set to top to ensure ending position after bounce.
        scrollRect.verticalNormalizedPosition = 1;
    }
}
