using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChanger : MonoBehaviour {

	public AudioClip clip;

	private void OnTriggerEnter2D(Collider2D other)
	{
		//tests if camera entered
		if (other.gameObject.tag.Equals("Player")) {
			GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>().fadeToClip(clip);
			Debug.Log("changing music");
		}
	}
}
