using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

	public AudioClip AmbientMusic;

	[Space]

	public AudioClip AlarmSound;
	public AudioClip ReleaseObjectSound;
	public AudioClip TakeObjectSound;
	public AudioClip SelectUISound;
	public AudioClip DoorbellSound;
	public AudioClip TimerSound;

	[Range(0, 1)]
	public float VolumeMusic;

	[Range(0, 1)]
	public float VolumeSounds;

	private AudioSource audioSourceMusic = null;
	private AudioSource audioSourceSound = null;
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

		foreach (var source in GetComponents<AudioSource>())
		{
			if (audioSourceMusic == null)
				audioSourceMusic = source;
			else if (audioSourceSound == null)
				audioSourceSound = source;
		}

		loopingSounds = new Dictionary<AudioClip, Coroutine>();

		audioSourceMusic.clip = AmbientMusic;
		ChangeVolumeMusic(VolumeMusic);

		ChangeVolumeSound(VolumeSounds);
	}

	// Use this for initialization
	void Start()
	{

	}

	public void PlayAmbiantMusic()
	{
		audioSourceMusic.Play();
	}

	public void StopAmbiantMusic()
	{
		audioSourceMusic.Stop();
	}

	public void ChangeVolumeMusic(float volume)
	{
		audioSourceMusic.volume = volume;
		VolumeMusic = volume;
	}

	public void ChangeVolumeMusic(Slider volume)
	{
		audioSourceMusic.volume = volume.value;
		VolumeMusic = volume.value;
	}

	public void ChangeVolumeSound(float volume)
	{
		audioSourceSound.volume = volume;
		VolumeSounds = volume;
	}

	public void ChangeVolumeSound(Slider volume)
	{
		audioSourceSound.volume = volume.value;
		VolumeSounds = volume.value;
	}

	public void PlaySound(AudioClip sound)
	{
		audioSourceSound.PlayOneShot(sound, VolumeSounds);
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
		audioSourceSound.PlayOneShot(sound, VolumeSounds);

		yield return new WaitForSeconds(sound.length);

		PlaySoundLoop(sound);
	}

}
