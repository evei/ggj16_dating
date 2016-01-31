using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SpeechBubble : MonoBehaviour 
{
	public Action bubbleClosed = delegate {};

	public float displayTime = 5f;
	public Text cardText;
	public Button closeButton;

	WaitForSeconds waitTime;

	void Start ()
	{
		closeButton.onClick.AddListener(DeactivateBubble);
		waitTime = new WaitForSeconds(displayTime);
		gameObject.SetActive(false);
	}

	public void DisplayText (string text)
	{
		StopAllCoroutines();
		cardText.text = text;
		gameObject.SetActive(true);
		StartCoroutine(WaitAndClose());
	}

	IEnumerator WaitAndClose ()
	{
		yield return waitTime;
		DeactivateBubble();
	}

	void DeactivateBubble ()
	{
		gameObject.SetActive(false);
		bubbleClosed();
	}
}
