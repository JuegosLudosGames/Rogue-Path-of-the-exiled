using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour {

	//if the button is active
	public bool isActive;

	//the type of the button
	public ButtonType type;

	//position in world coords of the button
	private Vector2 position;
	private float Width;
	private float Height;

	//the transform of the button
	private RectTransform rec;

	//used for intialisation
	public void Start() {
		//get position of the rectangle
		position = new Vector2 ();
		position.Set (transform.position.x, transform.position.y);

		//temperaryly get the rectangle transform component of the button
		rec = GetComponent<RectTransform>();
		//Vector3[] corners = rec.GetLocalCorners (new Vector3[4]);
		Vector3[] corners = new Vector3[4];
		rec.GetLocalCorners (corners);

		//calculate the height and width of the button
		Width = corners [2].x - corners [1].x;
		Height = corners [2].y - corners [0].y;
	}

	//checks if the button is currently being pressed
	public bool isPressed(Vector2 vector) {
		//tests if vector is inside the rectangle
		//checks if active
		if (!isActive)
			return false;
		//checks x coord
		if (!(vector.x < (position.x + (Width / 2)) && vector.x > (position.x - (Width / 2))))
			return false;
		//checks y coord
		if (!(vector.y < (position.y + (Height / 2)) && vector.y > (position.y - (Height / 2))))
			return false;

		return true;
	}

}
