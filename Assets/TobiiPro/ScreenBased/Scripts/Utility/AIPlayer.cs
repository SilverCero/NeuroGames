using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public CubeGameController gameController;

    // Method to trigger the AI to make its move
    public void MakeMove()
    {
        if (gameController == null || gameController.gameWon) return;

        // Try to find a winning move for 'O'
        if (TryMakeStrategicMove("O")) return;

        // Block 'X' from winning if possible
        if (TryMakeStrategicMove("X")) return;

        // Make a random move if no winning or blocking move is found
        MakeRandomMove();
    }

    // Tries to make a strategic move (either to win or block the opponent)
    private bool TryMakeStrategicMove(string tagToMatch)
    {
        for (int i = 0; i < gameController.gameBoard.Count; i += 3)
        {
            // Check each row
            if (CheckAndMakeMove(tagToMatch, i, i + 1, i + 2)) return true;
        }
        for (int i = 0; i < 3; i++)
        {
            // Check each column
            if (CheckAndMakeMove(tagToMatch, i, i + 3, i + 6)) return true;
        }
        // Check diagonals
        if (CheckAndMakeMove(tagToMatch, 0, 4, 8)) return true;
        if (CheckAndMakeMove(tagToMatch, 2, 4, 6)) return true;

        return false;
    }

    // Checks if two cubes in a line match the given tag and the third is empty; if so, makes the move
    private bool CheckAndMakeMove(string tagToMatch, int index1, int index2, int index3)
    {
        List<GameObject> gameBoard = gameController.gameBoard;

        if (gameBoard[index1].tag == tagToMatch && gameBoard[index2].tag == tagToMatch && gameBoard[index3].tag == "testo")
        {
            gameController.OnCubeSelected(gameBoard[index3]);
            return true;
        }
        if (gameBoard[index1].tag == tagToMatch && gameBoard[index3].tag == tagToMatch && gameBoard[index2].tag == "testo")
        {
            gameController.OnCubeSelected(gameBoard[index2]);
            return true;
        }
        if (gameBoard[index2].tag == tagToMatch && gameBoard[index3].tag == tagToMatch && gameBoard[index1].tag == "testo")
        {
            gameController.OnCubeSelected(gameBoard[index1]);
            return true;
        }
        return false;
    }

    // Makes a random move on an empty cube
    private void MakeRandomMove()
    {
        List<GameObject> emptyCubes = new List<GameObject>();

        foreach (var cube in gameController.gameBoard)
        {
            if (cube.tag == "testo") emptyCubes.Add(cube);
        }

        if (emptyCubes.Count > 0)
        {
            GameObject randomCube = emptyCubes[Random.Range(0, emptyCubes.Count)];
            gameController.OnCubeSelected(randomCube);
        }
    }
}
