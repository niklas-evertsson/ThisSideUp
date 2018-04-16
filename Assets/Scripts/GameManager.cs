using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Hide and lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
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
