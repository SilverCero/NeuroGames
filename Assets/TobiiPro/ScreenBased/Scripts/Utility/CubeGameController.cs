using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class CubeGameController : MonoBehaviour
{
    public TurnIndicator turnIndicator;
    public RandomTurnSelector randomTurnSelector;
    private NetworkClient networkClient;

    private bool isPlayerXTurn; // True if it's X's turn, false for O's turn
    public bool gameWon = false;

    public GameObject backgroundCube; // Temporary cube to represent the background
    public List<GameObject> gameBoard; // Drag and drop the GameObjects in the Inspector

    void Start()
    {
        networkClient = FindObjectOfType<NetworkClient>();
        if (networkClient == null)
        {
            Debug.LogError("NetworkClient not found in the scene!");
        }

        randomTurnSelector.SpinAndSelectTurn();
        StartCoroutine(SetInitialTurnAfterSpin());
    }

    private IEnumerator SetInitialTurnAfterSpin()
    {
        yield return new WaitForSeconds(randomTurnSelector.spinDuration); // Wait for the spin to complete

        isPlayerXTurn = randomTurnSelector.GetIsRedTurn(); // Set turn based on spin result
        turnIndicator.ShowTurnIndicator(isPlayerXTurn ? Color.red : Color.green);
        Debug.Log($"Game starts with {(isPlayerXTurn ? "Player X (Red)" : "Player O (Green)")}");
    }

    public void OnCubeSelected(GameObject cube)
    {
        if (cube.CompareTag("reseto"))
        {
            Debug.Log("reset game");
            ResetGame();
            return;
        }
        if (cube.CompareTag("testo") && !gameWon)
        {
            int index = gameBoard.IndexOf(cube);
            if (index == -1) return;

            if (isPlayerXTurn)
            {
                cube.tag = "X";
                turnIndicator.ShowTurnIndicator(Color.green);
                StartCoroutine(TransitionColor(cube, Color.red));
            }
            else
            {
                cube.tag = "O";
                StartCoroutine(TransitionColor(cube, Color.green));
                turnIndicator.ShowTurnIndicator(Color.red);
            }

            if (networkClient != null)
            {
                networkClient.SendSelectionMessage(index + 1); // Send 1-based index to match cube numbers
            }

            isPlayerXTurn = !isPlayerXTurn; // Switch turns
            CheckForWin();
        }
    }

    private IEnumerator TransitionColor(GameObject cube, Color targetColor)
    {
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        Color initialColor = cubeRenderer.material.color;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * 0.5f;
            cubeRenderer.material.color = Color.Lerp(initialColor, targetColor, progress);
            yield return null;
        }

        cubeRenderer.material.color = targetColor;
    }

    private void CheckForWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[i * 3].tag == gameBoard[i * 3 + 1].tag &&
                gameBoard[i * 3 + 1].tag == gameBoard[i * 3 + 2].tag &&
                gameBoard[i * 3].tag != "testo")
            {
                Debug.Log($"{gameBoard[i * 3].tag} wins by row {i}!");
                gameWon = true;
                return;
            }

            if (gameBoard[i].tag == gameBoard[i + 3].tag &&
                gameBoard[i + 3].tag == gameBoard[i + 6].tag &&
                gameBoard[i].tag != "testo")
            {
                Debug.Log($"{gameBoard[i].tag} wins by column {i}!");
                gameWon = true;
                return;
            }
        }

        if (gameBoard[0].tag == gameBoard[4].tag && gameBoard[4].tag == gameBoard[8].tag && gameBoard[0].tag != "testo")
        {
            Debug.Log($"{gameBoard[0].tag} wins by the main diagonal!");
            gameWon = true;
            return;
        }
        if (gameBoard[2].tag == gameBoard[4].tag && gameBoard[4].tag == gameBoard[6].tag && gameBoard[2].tag != "testo")
        {
            Debug.Log($"{gameBoard[2].tag} wins by the anti-diagonal!");
            gameWon = true;
            return;
        }
    }

    public void ResetGame()
    {
        foreach (var cube in gameBoard)
        {
            cube.tag = "testo";
            cube.transform.localScale = new Vector3(2f, 2f, 1f);
            cube.GetComponent<Renderer>().material.color = Color.blue;
        }
        GameObject resetCube = gameBoard[9]; // Assuming the reset cube is at index 9
        resetCube.tag = "reseto"; // Ensure it keeps its reset tag
        resetCube.transform.localScale = new Vector3(1f, 1f, 1f);
        resetCube.GetComponent<Renderer>().material.color = Color.yellow;
        gameWon = false;
        randomTurnSelector.SpinAndSelectTurn(); // Re-randomize turn
        turnIndicator.transform.position = new Vector3(0, 10, -5);
        turnIndicator.transform.rotation = Quaternion.Euler(0, 0, 0);
        //turnIndicator.color = Color.magenta;      
        StartCoroutine(SetInitialTurnAfterSpin());
    }
}
