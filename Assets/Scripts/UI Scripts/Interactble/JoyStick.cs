using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler , IPointerDownHandler {

	//the background image
	private Image background;

	//the stick image
	private Image stick;

	//the vector of input to be taken
	private Vector2 inputVec;

	//used at intialisation
	private void Start() {
		//takes images
		background = GetComponent<Image> ();
		stick = transform.GetChild (0).GetComponent<Image> ();
	}

	//when event data shows drag
	public virtual void OnDrag(PointerEventData ped) {
		Vector2 pos;

		//checks if the touched area is in the rectangle
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (background.rectTransform
																	, ped.position
																	, ped.pressEventCamera
																	,out pos)) 
		{

			//get joystick position
			pos.x = (pos.x / background.rectTransform.sizeDelta.x);
			pos.y = (pos.y / background.rectTransform.sizeDelta.y);

			//calculates and normalizes the vector
			inputVec = new Vector2 (pos.x*2 - 1,pos.y*2 - 1);
			inputVec = (inputVec.magnitude > 1.0f) ? inputVec.normalized : inputVec;

			//move Image
			stick.rectTransform.anchoredPosition = new Vector3 (inputVec.x * (background.rectTransform.sizeDelta.x / 2)
																,inputVec.y * (background.rectTransform.sizeDelta.y / 2));
		}

	}

	//when the joystick is pressed
	public virtual void OnPointerDown(PointerEventData ped) {
		OnDrag (ped);
	}

	//when removed from stick
	public virtual void OnPointerUp(PointerEventData ped) {
		inputVec = Vector2.zero;
		stick.rectTransform.anchoredPosition = Vector3.zero;
	}

	//grabs the horizontal position of joystick
	public float Horizontal() {
		return inputVec.x;
	}

	//grabs the vertical position of joystick
	public float Vertical() {
		return inputVec.y;
	}

}
