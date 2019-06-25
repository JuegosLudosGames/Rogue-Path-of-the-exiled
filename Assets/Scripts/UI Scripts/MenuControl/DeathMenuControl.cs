using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class DeathMenuControl : Triggerable {

    public bool isDead = false;

    //the death menu
    public GameObject DeathMenu;

    //panel that will fade
    public GameObject panel;

    //button listener
    private ButtonListener buttonListen;

    //buttons
    public GameObject Restart;
    public GameObject Quit;
    public GameObject Revive;
    public GameObject FailLeave;

    //fail Menu
    public GameObject FailMenu;

    public bool wasPreviouslyRevived;

    public static DeathMenuControl control;

	//sound
	public SoundManager SManage;

	// Use this for initialization
	void Start () {
        DeathMenuControl.control = this;
        buttonListen = GameObject.FindGameObjectWithTag("GameController").GetComponent<ButtonListener>();
        DeathMenu.SetActive(false);
        Restart.GetComponent<ButtonPress>().isActive = false;
        Quit.GetComponent<ButtonPress>().isActive = false;
        Revive.GetComponent<ButtonPress>().isActive = false;
        panel.SetActive(false);
		SManage = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead) {

			//old code

			////gets pressed
			//List<ButtonPress> buttons = buttonListen.getPressed();

			//if (buttons == null)
			//    return;

			////converts to types
			//List<ButtonType> types = new List<ButtonType>();
			//foreach (ButtonPress button in buttons)
			//    types.Add(button.type);

			////checks fail menu
			//if (types.Contains(ButtonType.ATTACK_LIGHT)) {
			//    closeFailMenu();
			//    return;
			//}

			////checks restart
			//if (types.Contains(ButtonType.PAUSE_RESUME)) {
			//    restart();
			//    return;
			//}

			////checks quit
			//if (types.Contains(ButtonType.PAUSE_QUIT)) {
			//    quitToMain();
			//    return;
			//}

			////checks revive
			//if (types.Contains(ButtonType.DEATH)) {
			//    reviveAd();
			//    return;
			//}



			//checks fail menu
			if (buttonListen.isPressed(ButtonType.ATTACK_LIGHT))
			{
				closeFailMenu();
				return;
			}

			//checks restart
			if (buttonListen.isPressed(ButtonType.PAUSE_RESUME))
			{
				restart();
				return;
			}

			//checks quit
			if (buttonListen.isPressed(ButtonType.PAUSE_QUIT))
			{
				quitToMain();
				return;
			}

			//checks revive
			if (buttonListen.isPressed(ButtonType.DEATH))
			{
				reviveAd();
				return;
			}
		}
	}

    public static void died() {
        control.panel.SetActive(true);
        control.panel.GetComponent<FadePanel>().triggered = true;
		control.SManage.playDeath();
    }

    public static void open()
    {
        //sets death menu to be active
        control.DeathMenu.SetActive(true);
        control.FailMenu.SetActive(false);

        //tests if should enable ad button
        if (control.wasPreviouslyRevived)
        {
            control.Revive.SetActive(false);
        }

        //freezes game
        Time.timeScale = 0f;

        //sets state
        control.isDead = true;

        //sets buttons states
        control.Restart.GetComponent<ButtonPress>().isActive = true;
        control.Quit.GetComponent<ButtonPress>().isActive = true;
        if (!control.wasPreviouslyRevived)
        {
            control.Revive.GetComponent<ButtonPress>().isActive = true;
        }
    }

    void reviveAd() {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            ShowOptions options = new ShowOptions { resultCallback = verifyShowResult };
            Advertisement.Show("rewardedVideo", options);
        } else {
            openFailMenu();
        }
    }

    void revive() {

        close();

        //gets player
        ControllerPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllerPlayer>();

        //heals player
        player.heal(10f);

        //kills all enemies in a 3 unit radius
        //creates layer mask
        LayerMask mask = 1 << 9;

        //creates filter
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(mask);
        filter.ClearDepth();
        filter.useTriggers = true;

        //creates array
        Collider2D[] colliders = new Collider2D[30];

        //puts into array all the overlapping colliders
        Physics2D.OverlapCircle(player.transform.position, 3f, filter, colliders);

        //adds colliders into list
        foreach (Collider2D collide in colliders)
        {

            //tests if the collider is null
            if (collide == null)
                continue;

            //tests if the collider is a trigger
            if (collide.isTrigger)
                continue;

            //tests if the collider is a hitbox by checking if it is a box2d
            if (collide is BoxCollider2D)
            {
                EnemyMovement enemy = collide.gameObject.GetComponent<EnemyMovement>();
                if (enemy != null) {
                    enemy.damage(1f);
                }
               
            }
        }

        //revive Animation
        player.reviveAni();

        wasPreviouslyRevived = true;
    }

    void verifyShowResult(ShowResult result) {
        switch (result) {
            case ShowResult.Failed:
                openFailMenu();
                break;
            case ShowResult.Skipped:
                openFailMenu();
                break;
            case ShowResult.Finished:
                revive();
                break;
        }
    }

    void close() {

        //sets death menu
        DeathMenu.SetActive(false);

        //unfreezes game
        Time.timeScale = 1f;

        //sets state
        isDead = false;

        //resets control
        Restart.GetComponent<ButtonPress>().isActive = false;
        Quit.GetComponent<ButtonPress>().isActive = false;
        Revive.GetComponent<ButtonPress>().isActive = false;

        panel.GetComponent<FadePanel>().Reset();
        panel.SetActive(false);
    }

    void openFailMenu() {
        //enables menu
        FailMenu.SetActive(true);
        FailLeave.GetComponent<ButtonPress>().isActive = true;

        //disables prev
        Restart.GetComponent<ButtonPress>().isActive = false;
        Quit.GetComponent<ButtonPress>().isActive = false;
        Revive.GetComponent<ButtonPress>().isActive = false;
    }

    void closeFailMenu() {
        //disables menu
        FailMenu.SetActive(false);
        FailLeave.GetComponent<ButtonPress>().isActive = false;

        //enable death
        Restart.GetComponent<ButtonPress>().isActive = true;
        Quit.GetComponent<ButtonPress>().isActive = true;
        Revive.GetComponent<ButtonPress>().isActive = true;
    }

    void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    void quitToMain() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public override void onTrigger()
    {
        open();
    }
}
