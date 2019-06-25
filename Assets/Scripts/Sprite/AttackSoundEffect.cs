using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackSoundEffect : MonoBehaviour {

	//attack sounds
	public AudioClip[] attackSounds;

	//damage sounds
	public AudioClip[] damageSounds;

	private AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	public void playAttackSound() {
		//int in array that was choosen
		int choosen;
		choosen = new System.Random().Next(attackSounds.Length);

		//plays sound
		source.clip = attackSounds[choosen];
		source.Play();
	}

	public void playDamageSound()
	{
		//int in array that was choosen
		int choosen;
		choosen = new System.Random().Next(damageSounds.Length);

		//plays sound
		source.clip = damageSounds[choosen];
		source.Play();
	}

}
