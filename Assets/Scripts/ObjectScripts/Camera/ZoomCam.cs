using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCam : Triggerable {

	public Camera cam;

	public float zoomTo;
	public float speed;

	public float stopRange = 0.05f;

	private bool isZooming;

	public override void onTrigger() {
		isZooming = true;
	}

	private void Start() {
		speed /= 10;
	}

	// Update is called once per frame
	void Update () {
		if (isZooming) {
			if (cam.orthographicSize > zoomTo) {
				cam.orthographicSize -= speed;
			} else {
				cam.orthographicSize += speed;
			}
			if ((zoomTo + stopRange) > cam.orthographicSize && cam.orthographicSize > (zoomTo + stopRange)) {
				cam.orthographicSize = zoomTo;
				isZooming = false;
			}
		}
	}
}
