using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTexturePlayer : MonoBehaviour {

	public Sprite[] frames;
	public int framesPerSecond = 10;
	private SpriteRenderer render;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		int index = (int) ((Time.time * framesPerSecond) % frames.Length);
		render.sprite = frames [index];
	}
}
