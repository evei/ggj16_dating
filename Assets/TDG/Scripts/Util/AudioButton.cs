using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
[RequireComponent (typeof (AudioSource))]
public class AudioButton : MonoBehaviour 
{
	void Start ()
	{
		var audioSource = GetComponent<AudioSource>();
		GetComponent<Button>().onClick.AddListener(() => PlayAudio(audioSource));
	}

	void PlayAudio (AudioSource audioSource)
	{
		AudioController.Instance.PlayAudio(audioSource.clip);
	}
}
