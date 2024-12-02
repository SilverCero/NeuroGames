using System.Collections;
using UnityEngine;

public class RandomTurnSelector : MonoBehaviour
{
    public TurnIndicator turnIndicator; // Reference to the TurnIndicator script
    public GameObject spinner; // The spinning object (e.g., the circle)
    public float spinSpeed = 720f; // Speed of spinning in degrees per second
    public float spinDuration = 3f; // Total duration of spinning
    private bool isRedTurn; // True if Player X (Red) starts, false for Player O (Green)

    private void Start()
    {
        // Randomize the initial rotation at the start of the game.
        RandomizeStartRotation();
    }

    // Method to start spinning and determine the turn
    public void SpinAndSelectTurn()
    {
        StartCoroutine(SpinAndSelectTurnCoroutine());
    }

    private IEnumerator SpinAndSelectTurnCoroutine()
    {
        if (spinner == null || turnIndicator == null)
        {
            Debug.LogError("Spinner or TurnIndicator is not assigned!");
            yield break;
        }

        RandomizeStartRotation(); // Reset rotation
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            spinner.transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }

        spinner.transform.Rotate(Vector3.forward, 0, Space.Self); // Stop spinning

        // Determine final rotation on Z-axis
        float finalZRotation = spinner.transform.eulerAngles.z;
        if (finalZRotation > 180f) finalZRotation -= 360f;

        // Decide turn based on rotation
        if (finalZRotation >= 0f && finalZRotation <= 180f)
        {
            Debug.Log("Red starts (Player X)!");
            turnIndicator.ShowTurnIndicator(Color.red);
            isRedTurn = true;
        }
        else
        {
            Debug.Log("Green starts (Player O)!");
            turnIndicator.ShowTurnIndicator(Color.green);
            isRedTurn = false;
        }
    }

    // Get the result of the spin
    public bool GetIsRedTurn()
    {
        return isRedTurn;
    }

    private void RandomizeStartRotation()
    {
        float randomStartRotation = Random.Range(0f, 360f);
        spinner.transform.rotation = Quaternion.Euler(0f, 0f, randomStartRotation);
    }
}
