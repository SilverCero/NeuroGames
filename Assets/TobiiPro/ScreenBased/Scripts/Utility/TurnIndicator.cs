using System.Collections;
using UnityEngine;

public class TurnIndicator : MonoBehaviour
{
    public Light turnLight; // Assign the Light component in the Inspector

    private readonly Quaternion redRotation = Quaternion.Euler(0f, 45f, 0f); // 45° Y rotation for red
    private readonly Quaternion greenRotation = Quaternion.Euler(0f, 315f, 0f); // 315° Y rotation for green


    private readonly Color redColor = Color.red;
    private readonly Color greenColor = Color.green;

    private Coroutine currentIndicatorRoutine;

   
    public void ShowTurnIndicator(Color turnColor)
    {
        if (currentIndicatorRoutine != null)
            StopCoroutine(currentIndicatorRoutine);

        currentIndicatorRoutine = StartCoroutine(AnimateTurnIndicator(turnColor));
    }



    private IEnumerator AnimateTurnIndicator(Color targetColor)
    {
        if (turnLight == null)
        {
            Debug.LogError("Turn light is not assigned!");
            yield break;
        }

        // Update light rotation and color based on the target color
        Quaternion targetRotation = targetColor == redColor ? redRotation : greenRotation;
        Color initialColor = turnLight.color;
        Quaternion initialRotation = turnLight.transform.rotation;

        float duration = 1f; // Animation duration
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Lerp the color and rotation
            turnLight.color = Color.Lerp(initialColor, targetColor, t);
            turnLight.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            yield return null;
        }

        // Ensure final state is set
        turnLight.color = targetColor;
        turnLight.transform.rotation = targetRotation;
    }
}
//NetworkClient