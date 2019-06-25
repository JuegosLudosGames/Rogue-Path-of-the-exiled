using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : Triggerable {

	//menu
	public GameObject CompleteMenu;

	//buttons
	public GameObject RetryButton;
	public GameObject MenuButton;

	//control static
	public static LevelEnd control;

	//is it being shown
	public bool isShowing = false;

	//listener
	ButtonListener buttonListen;

	// Use this for initialization
	void Start()
	{
		control = this;
		buttonListen = GameObject.FindGameObjectWithTag("GameController").GetComponent<ButtonListener>();
		RetryButton.GetComponent<ButtonPress>().isActive = false;
		MenuButton.GetComponent<ButtonPress>().isActive = false;
		CompleteMenu.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (isShowing)
		{

			//old code

			////gets pressed
			//List<ButtonPress> buttons = buttonListen.getPressed();

			//if (buttons == null)
			//	return;

			////converts to types
			//List<ButtonType> types = new List<ButtonType>();
			//foreach (ButtonPress button in buttons)
			//	types.Add(button.type);

			////checks if retry pressed
			//if (types.Contains(ButtonType.PAUSE_RESUME))
			//{
			//	SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
			//}

			////checks if menu pressed
			//if (types.Contains(ButtonType.PAUSE_QUIT))
			//{
			//	SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
			//}

			if(buttonListen.isPressed(ButtonType.PAUSE_RESUME))
				SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
			
			if(buttonListen.isPressed(ButtonType.PAUSE_QUIT))
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
		}
	}

	public static void open()
	{
		//freeze game
		Time.timeScale = 0.0f;

		//sets object to active
		control.CompleteMenu.SetActive(true);

		//sets buttons to active
		control.RetryButton.GetComponent<ButtonPress>().isActive = true;
		control.MenuButton.GetComponent<ButtonPress>().isActive = true;

		//sets is showing
		control.isShowing = true;
	}

	public override void onTrigger()
	{
		open();
	}
}
