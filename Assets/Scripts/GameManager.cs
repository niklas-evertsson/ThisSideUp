using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController[] players;

    void Start()
    {
        // Hide and lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Get all players
        players = FindObjectsOfType<PlayerController>();
    }

	void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            // Disable player input
            PlayerController.inputEnabled = !PlayerController.inputEnabled;

            // Release the cursor and exit the game
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }
	}
}
