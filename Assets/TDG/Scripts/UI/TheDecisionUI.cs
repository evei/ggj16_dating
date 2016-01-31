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
	public GameObject playerPassesOutPanel;
	public GameObject datePassesOutPanel;
	public GameObject playerFleesPanel;
	public GameObject dateFleesPanel;

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
			HandleDateFlees();
			break;
		case GameManager.GameState.DatePassesOut:
			HandleDatePassesOut();
			break;
		case GameManager.GameState.Decision:
			HandleDecisionTime();
			break;
		case GameManager.GameState.PlayerFlees:
			HandlePlayerFlees();
			break;
		case GameManager.GameState.PlayerPassesOut:
			HandlePlayerPassesOut();
			break;
		}
	}

	void DeactivatePanels ()
	{
		decisionPanel.SetActive(false);
		inLovePanel.SetActive(false);
		notInLovePanel.SetActive(false);
		playerPassesOutPanel.SetActive(false);
		datePassesOutPanel.SetActive(false);
		playerFleesPanel.SetActive(false);
		dateFleesPanel.SetActive(false);
	}

	void HandleDateFlees ()
	{
		dateFleesPanel.SetActive(true);
		// TODO
	}

	void HandleDatePassesOut ()
	{
		datePassesOutPanel.SetActive(true);
		// TODO
	}

	void HandlePlayerFlees ()
	{
		playerFleesPanel.SetActive(true);
		// TODO
	}

	void HandlePlayerPassesOut ()
	{
		playerPassesOutPanel.SetActive(true);
		// TODO
	}

	void HandleDecisionTime ()
	{
		decisionPanel.SetActive(true);
		decisionRoutine = StartCoroutine(StartDecisionTimer());
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
		// TODO Waiting Visual feedback
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
		// TODO Waiting Visual feedback
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
		decisionPanel.SetActive(false);
		// Display appropriate animation based on the player decision
		if (playerInLove && dateInLove) {
			inLovePanel.SetActive(true);
		} else if (playerInLove && !dateInLove){
			notInLovePanel.SetActive(true);
			// TODO Set appropriate message
		} else if (!playerInLove && dateInLove) {
			notInLovePanel.SetActive(true);
			// TODO Set appropriate message
		} else {
			notInLovePanel.SetActive(true);
			// TODO Set appropriate message
		}
	}
}
