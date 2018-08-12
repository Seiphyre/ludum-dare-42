using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

	public AudioClip AmbientMusic;

	[Range(0, 1)]
	public float Volume;

	private AudioSource audioSource;
	private Dictionary<AudioClip, Coroutine> loopingSounds;

	#region -- Singleton ----------------------------

	private static SoundManager Instance;

	public static SoundManager GetInstance()
	{
		if (Instance == null)
		{
			Instance = FindObjectOfType<SoundManager>();

			if (Instance == null)
			{
				GameObject GridManager = new GameObject();
				Instance = GridManager.AddComponent<SoundManager>();
			}
		}

		return Instance;
	}

	#endregion


	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		audioSource = GetComponent<AudioSource>();
		loopingSounds = new Dictionary<AudioClip, Coroutine>();

		audioSource.clip = AmbientMusic;
		ChangeVolume(Volume);
	}

	// Use this for initialization
	void Start()
	{

	}

	public void PlayAmbiantMusic()
	{
		audioSource.Play();
	}

	public void StopAmbiantMusic()
	{
		audioSource.Stop();
	}

	public void ChangeVolume(float volume)
	{
		audioSource.volume = volume;
		Volume = volume;
	}

	public void ChangeVolume(Slider volume)
	{
		audioSource.volume = volume.value;
		Volume = volume.value;
	}

	public void PlaySound(AudioClip sound)
	{
		audioSource.PlayOneShot(sound);
	}

	public void PlaySoundLoop(AudioClip sound)
	{
		Coroutine soundCoroutine = StartCoroutine(SoundLoop(sound));

		if (loopingSounds.ContainsKey(sound))
		{
			StopCoroutine(loopingSounds[sound]);
			loopingSounds[sound] = soundCoroutine;
		}
		else
			loopingSounds.Add(sound, soundCoroutine);
	}

	public void StopSoundLoop(AudioClip sound)
	{
		if (loopingSounds.ContainsKey(sound))
		{
			if( loopingSounds[sound] != null)
			{
				StopCoroutine(loopingSounds[sound]);
				loopingSounds[sound] = null;
			}
		}
	}

	IEnumerator SoundLoop(AudioClip sound)
	{
		audioSource.PlayOneShot(sound);

		yield return new WaitForSeconds(sound.length);

		PlaySoundLoop(sound);
	}

}
