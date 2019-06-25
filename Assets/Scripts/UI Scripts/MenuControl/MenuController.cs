using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class MenuController : MonoBehaviour {

	//the menus the controller handles
	public GameObject mainMenu;
	public GameObject levelSelect;
	public GameObject CreditMenu;

	//if each menu is active
	public bool mainMenuActive;
	public bool levelSelectActive;
	public bool CreditMenuActive;

	//the button input listener
	private ButtonListener buttonListen;

	// Use this for initialization
	void Start () {  

        if (!Advertisement.isInitialized) {
            Advertisement.Initialize("1758989");
        }

		buttonListen = GetComponent<ButtonListener> ();
		mainMenuActive = true;

		buttonListen.UserUiObjects[0].GetComponent<ButtonPress> ().isActive = true;
		buttonListen.UserUiObjects[5].GetComponent<ButtonPress> ().isActive = true;

		buttonListen.UserUiObjects[1].GetComponent<ButtonPress> ().isActive = false;
		buttonListen.UserUiObjects[2].GetComponent<ButtonPress> ().isActive = false;
		buttonListen.UserUiObjects[3].GetComponent<ButtonPress> ().isActive = false;
		buttonListen.UserUiObjects[4].GetComponent<ButtonPress> ().isActive = false;
		buttonListen.UserUiObjects[6].GetComponent<ButtonPress> ().isActive = false;
        buttonListen.UserUiObjects[7].GetComponent<ButtonPress> ().isActive = false;
		buttonListen.UserUiObjects[8].GetComponent<ButtonPress> ().isActive = false;

		mainMenu.SetActive (true);
		levelSelect.SetActive (false);
		CreditMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		//old stuff
		//List<ButtonPress> rawButtonObjects = buttonListen.getPressed ();

		//if (rawButtonObjects == null)
		//	return;

		//List<ButtonType> buttonObjects = new List<ButtonType> ();

		//foreach (ButtonPress press in rawButtonObjects)
		//	buttonObjects.Add (press.type);
		

		//ButtonType button = buttonObjects[0].GetComponent<ButtonPress> ().type;

		//tests if the mainMenu is active
		if (mainMenuActive) {
			//menu Menu

			//tests if the startbutton is pressed
			if (/*button == ButtonType.ARROW_UP*/buttonListen.isPressed(ButtonType.ARROW_UP)) {
				//sets the bool for the levels
				mainMenuActive = false;
				levelSelectActive = true;

				//sets the gameobject
				mainMenu.SetActive (false);
				levelSelect.SetActive (true);

				//disables own buttons
				buttonListen.UserUiObjects[0].GetComponent<ButtonPress> ().isActive = false;
				buttonListen.UserUiObjects[5].GetComponent<ButtonPress> ().isActive = false;

				//enables next buttons
				buttonListen.UserUiObjects[1].GetComponent<ButtonPress> ().isActive = true;
				buttonListen.UserUiObjects[2].GetComponent<ButtonPress> ().isActive = true;
				buttonListen.UserUiObjects[3].GetComponent<ButtonPress> ().isActive = true;
                buttonListen.UserUiObjects[4].GetComponent<ButtonPress> ().isActive = true;

                ////temp////
                buttonListen.UserUiObjects[7].GetComponent<ButtonPress> ().isActive = true;
				buttonListen.UserUiObjects[8].GetComponent<ButtonPress>().isActive = true;
				////////////

				return;
			}

			//tests if the credits button is pressed
			if (buttonListen.isPressed(ButtonType.ATTACK_LIGHT)) {
				//sets the bool for the levels
				mainMenuActive = false;
				CreditMenuActive = true;

				//sets the gameobject
				mainMenu.SetActive (false);
				CreditMenu.SetActive (true);

				//disables own buttons
				buttonListen.UserUiObjects[0].GetComponent<ButtonPress> ().isActive = false;
				buttonListen.UserUiObjects[5].GetComponent<ButtonPress> ().isActive = false;

				//enables next buttons
				buttonListen.UserUiObjects[6].GetComponent<ButtonPress> ().isActive = true;

				return;
			}

		} else if (levelSelectActive) {
            //levelSelect

            //tests if the Level1 is pressed
            if (buttonListen.isPressed(ButtonType.ARROW_DOWN)) {
                //Advertisement.Show("video");
                //loads scene
                //SceneManager.LoadScene ("INSERT SCENE HERE", LoadSceneMode.Single);
                return;
			}

			//tests if the Level2 is pressed
			if (buttonListen.isPressed(ButtonType.ARROW_LEFT)) {
                //Advertisement.Show("video");
                //loads scene
                //SceneManager.LoadScene ("INSERT SCENE HERE", LoadSceneMode.Single);
                return;
			}

			//tests if the Level3 is pressed
			if (buttonListen.isPressed(ButtonType.ARROW_RIGHT)) {
                //Advertisement.Show("video");
                //loads scene
                //SceneManager.LoadScene ("INSERT SCENE HERE", LoadSceneMode.Single);
                return;
			}

			//tests if the tutorial is pressed
			if (buttonListen.isPressed(ButtonType.ARROW_UP)) {
                Advertisement.Show("video");
                //loads scene
                SceneManager.LoadScene ("Tutorial", LoadSceneMode.Single);
                return;
			}

			//////////////temp//////////////////
			/// //tests if the beta is pressed
			if (buttonListen.isPressed(ButtonType.DEATH)) {
                Advertisement.Show("video");
                //loads scene
                SceneManager.LoadScene ("BetaLevel", LoadSceneMode.Single);
				return;
			}
			////////////////////////////////////

		} else if (CreditMenuActive) {
			//credits
			//tests if the back button is pressed
			if (buttonListen.isPressed(ButtonType.PAUSE_RESUME)) {

				//sets the bool for the levels
				CreditMenuActive = false;
				mainMenuActive = true;

				//sets the gameobject
				CreditMenu.SetActive (false);
				mainMenu.SetActive (true);

				//disables own buttons
				buttonListen.UserUiObjects[6].GetComponent<ButtonPress> ().isActive = false;

				//enables next buttons
				buttonListen.UserUiObjects[0].GetComponent<ButtonPress> ().isActive = true;
				buttonListen.UserUiObjects[5].GetComponent<ButtonPress> ().isActive = true;

				return;
			}
		}

	}
}
