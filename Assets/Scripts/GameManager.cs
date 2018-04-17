using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float delayBetweenRounds = 3.0f;
    public GameObject loseText;
    public GameObject player1WinText;
    public GameObject player2WinText;

    private PlayerController[] players;
    private WaitForSeconds roundDelay;

    void Start()
    {
        // Hide and lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Get all players
        players = FindObjectsOfType<PlayerController>();
        roundDelay = new WaitForSeconds(delayBetweenRounds);
    }

	void Update()
    {
        if(OnePlayerLeft())
        {
            StartCoroutine(EndRound());
        }

        if(Input.GetKeyDown("escape"))
        {
            // Disable player input
            PlayerController.inputEnabled = !PlayerController.inputEnabled;

            // Release the cursor and exit the game
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }
	}

    bool OnePlayerLeft()
    {
        int playersLeft = 0;

        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].enabled)
            {
                playersLeft++;
            }
        }

        return playersLeft <= 1;
    }

    int GetWinnerNumber()
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].enabled)
            {
                return players[i].playerNumber;
            }
        }

        return 0;
    }

    IEnumerator EndRound()
    {
        switch (GetWinnerNumber())
        {
            case 1:
                player1WinText.SetActive(true);
                break;

            case 2:
                player2WinText.SetActive(true);
                break;

            default:
                loseText.SetActive(true);
                break;
        }

        yield return roundDelay;
        SceneManager.LoadScene("Main");
    }
}
