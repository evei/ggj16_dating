using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour 
{
	public AudioSource audioSourcePrefab;

	public static AudioController Instance { get; private set; }

	void Awake() {
		if (Instance != null) {
			DestroyImmediate(gameObject);
			return;
		}
		Instance = this;
	}

	public void PlayAudio (AudioClip clip)
	{
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
