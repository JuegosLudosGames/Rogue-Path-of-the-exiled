using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuControl : MonoBehaviour {

	//weather the game is in a paused state
	public bool isPaused;

	//the menu
	public GameObject pauseMenu;

	//the buttons in the menu
	public GameObject resumeButton;
	public GameObject quitButton;

	//the button input listener
	private ButtonListener buttonListen;

	//the current control being used
	private static PauseMenuControl control;

	// Use this for initialization
	void Start () {
		//takes the listener
		buttonListen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonListener> ();

		//sets state
		control = this;
		resume ();
	}
	
	// Update is called once per frame
	void Update () {

		//checks if currently paused
		if (isPaused) {

			//old

			////takes all the pressed buttons
			//List<ButtonPress> presses = buttonListen.getPressed ();

			////verifies that there is something
			//if (presses == null)
			//	return;

			////takes all of the types
			//List<ButtonType> buttons = new List<ButtonType> ();

			//foreach (ButtonPress press in presses)
			//	buttons.Add (press.type);

			//checks if resume is pressed
			if(buttonListen.isPressed(ButtonType.PAUSE_RESUME))
				resume ();

			//check if quit was pressed
			if(buttonListen.isPressed(ButtonType.PAUSE_QUIT))
				quit ();
		}
	}

	//used to resume the game
	void resume() {
		//disables menu
		pauseMenu.SetActive (false);

		//sets game speed back
		Time.timeScale = 1f;

		//sets buttons to disable
		resumeButton.GetComponent<ButtonPress> ().isActive = false;
		quitButton.GetComponent<ButtonPress> ().isActive = false;

		//turns off paused state
		isPaused = false;
	}

	//quits the game
	void quit() {
		//loads main menu scene
		SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
	}

	//pauses the game
	public static void pause() {
		//sets pause menu to be active
		control.pauseMenu.SetActive (true);
		//freezes game
		Time.timeScale = 0f;
		//sets state
		control.isPaused = true;
		//sets buttons states
		control.resumeButton.GetComponent<ButtonPress> ().isActive = true;
		control.quitButton.GetComponent<ButtonPress> ().isActive = true;
	}

}
