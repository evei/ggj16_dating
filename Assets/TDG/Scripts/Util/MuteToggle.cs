using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Toggle))]
public class MuteToggle : MonoBehaviour {

	void Start () {
		var toggle = GetComponent<Toggle> ();
		toggle.isOn = AudioController.Instance.isMuted;
		toggle.onValueChanged.AddListener((bool muted) => AudioController.Instance.isMuted = muted);
	}
}
