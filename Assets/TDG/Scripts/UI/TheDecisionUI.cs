using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TheDecisionUI : MonoBehaviour 
{
	public GameObject decisionPanel;
	public Text decisionTimerText;
	public Button yesButton;
	public Button noButton;
	public Image[] hearts;

	public GameObject inLovePanel;
	public GameObject notInLovePanel;
	public GameObject PlayerPassesOutPanel;
	public GameObject DatePassesOutPanel;
	public GameObject PlayerFleesPanel;
	public GameObject DateFleesPanel;

	GameManager GameManager { get { return GameManager.Instance; } }

	Coroutine decisionRoutine;
	const int TIME_TO_DECIDE = 10;
	float elapsedTime;

	bool playerInLove;
	bool dateInLove;

	void Start ()
	{
		DeactivatePanels();

		// Decide which type of UI to display
		switch (GameManager.CurrentState) {
		case GameManager.GameState.DateFlees:
			break;
		case GameManager.GameState.DatePassesOut:
			break;
		case GameManager.GameState.Decision:
			decisionPanel.SetActive(true);
			decisionRoutine = StartCoroutine(StartDecisionTimer());
			break;
		case GameManager.GameState.PlayerFlees:
			break;
		case GameManager.GameState.PlayerPassesOut:
			break;
		}
	}

	void DeactivatePanels ()
	{
		decisionPanel.SetActive(false);
	}

	void HandleDateFlees ()
	{
		// TODO
	}

	void HandleDatePassesOut ()
	{
		// TODO
	}

	void HandlePlayerFlees ()
	{
		// TODO
	}

	void HandlePlayerPassesOut ()
	{
		// TODO
	}

	IEnumerator StartDecisionTimer ()
	{
		while (elapsedTime <= TIME_TO_DECIDE) {
			decisionTimerText.text = ((int)(TIME_TO_DECIDE - elapsedTime)).ToString();
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		decisionTimerText.text = "0";
		SendNoLove();
	}

	void HandleYesButton ()
	{
		StopCoroutine(decisionRoutine);
		SendLove();
	}

	void SendLove ()
	{
		playerInLove = true;
		// TODO Send love to other player
	}

	void HandleNoButton ()
	{
		StopAllCoroutines();
		SendNoLove();
	}

	void SendNoLove ()
	{
		// TODO Send NO love to the other player
	}

	void HandleDateDecision ()
	{
		// TODO Set the dateInLove coming from the other player
	}

	void StopTimer ()
	{
		StopCoroutine(decisionRoutine);
		decisionTimerText.text = string.Empty;
	}

	void DisplayDecisionEnding ()
	{
		// Display appropriate animation based on the player decision
		if (playerInLove && dateInLove) {
			// TODO
		} else if (playerInLove && !dateInLove){
			// TODO
		} else if (!playerInLove && dateInLove) {
			// TODO
		} else {
			// TODO
		}
	}
}
