using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioClip selectedAudioClip;

	//clips in beginning
	public AudioClip ClipOnAwake;
	public bool StartClipOnAwake;
	private AudioSource source;

	//special clips
	public AudioClip death;
	public AudioClip win;
	public AudioClip menu;

	//for fading in or out
	private AudioClip fadeTo;
	private bool isFading = false;
	public float fadeTime;
	public float fastFadeTime;
	public bool isFast;
	
	// Update is called once per frame
	void Update () {
		//checks if fading
		if (isFading) {
			//checks if fading in or out
			//fading out if clips or different
			if (fadeTo == selectedAudioClip)
			{
				//if the same (fading in)
				//checks if volume is max
				if (source.volume + (1 / fadeTime) >= 1)
				{
					source.volume = 1;
					isFading = false;
					flipTimes(false);
					
				}
				//if volume is not max
				source.volume += (1 / fadeTime);
			}
			else {
				//if fading out
				//if the same (fading in)
				//checks if volume is min
				if (source.volume - (1 / fadeTime) <= 0)
				{
					//changes clip
					source.volume = 0;
					selectedAudioClip = fadeTo;
					source.clip = selectedAudioClip;
					source.Play();
				}
				//if volume is not min
				source.volume -= (1 / fadeTime);
			}
		}

		//if death or win clips completed
		//return to general song
		if (!source.loop) {
			if (!source.isPlaying) {
				source.loop = true;
				fadeToClip(menu);
			}
		}

	}

	private void Awake()
	{
		//setups source in script
		source = gameObject.GetComponent<AudioSource>();
		source.loop = true;

		//starts clip when awaked
		if (ClipOnAwake) {
			source.clip = ClipOnAwake;
			selectedAudioClip = ClipOnAwake;
			source.Play();
		}
	}

	//starts fading clip
	public void fadeToClip(AudioClip clip) {
		fadeTo = clip;
		isFading = true;
		Debug.Log("caught change");
	}

	public void playDeath() {
		flipTimes(true);
		fadeToClip(death);
		source.loop = false;
	}

	public void playWin() {
		flipTimes(true);
		fadeToClip(win);
		source.loop = false;
	}

	private void flipTimes(bool fast) {
		if (fast) {
			if (!isFast) {
				float temp = fastFadeTime;
				fastFadeTime = fadeTime;
				fadeTime = temp;
				fast = true;
			}
		}
		else {
			if (isFast) {
				float temp = fastFadeTime;
				fastFadeTime = fadeTime;
				fadeTime = temp;
				fast = false;
			}
		}
		
	}

}
