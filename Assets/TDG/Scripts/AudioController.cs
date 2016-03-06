using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour 
{
	public AudioSource audioSourcePrefab;
	public AudioSource music;

	private bool _isMuted;
	public bool isMuted 
	{
		get { return _isMuted; }
		set { music.mute = value; _isMuted = value; }
	}

	public static AudioController Instance { get; private set; }

	void Awake() 
	{
		if (Instance != null) {
			DestroyImmediate(gameObject);
			return;
		}
		Instance = this;
		_isMuted = false;
	}

	public void PlayAudio (AudioClip clip)
	{
		if (isMuted) return;
		
		var audioSource = Instantiate(audioSourcePrefab);
		audioSource.transform.SetParent(transform, false);
		audioSource.clip = clip;

		StartCoroutine(PlayAudioRoutine(audioSource));
	}

	IEnumerator PlayAudioRoutine (AudioSource audioSource)
	{
		audioSource.Play();

		while (audioSource.isPlaying) {
			yield return null;
		}

		Destroy(audioSource.gameObject);
	}
}
