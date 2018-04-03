using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TagSelector]
    public List<string> teleportables;
    public PlayerController player;

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
            player.inputEnabled = !player.inputEnabled;

            // Release the cursor and exit the game
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }
	}
}
