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
	public GameObject waitingPanel;

	public GameObject quitButton;

	GameManager GameManager { get { return GameManager.Instance; } }

	Player Player { get { return GameManager.Player; } }

	Coroutine decisionRoutine;
	const int TIME_TO_DECIDE = 30;
	float elapsedTime;

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
		quitButton.SetActive(false);
		waitingPanel.SetActive(false);
	}

	void HandleDateFlees ()
	{
		dateFleesPanel.SetActive(true);
		quitButton.SetActive(true);
	}

	void HandleDatePassesOut ()
	{
		datePassesOutPanel.SetActive(true);
		quitButton.SetActive(true);
	}

	void HandlePlayerFlees ()
	{
		playerFleesPanel.SetActive(true);
		quitButton.SetActive(true);
	}

	void HandlePlayerPassesOut ()
	{
		playerPassesOutPanel.SetActive(true);
		quitButton.SetActive(true);
	}

	void HandleDecisionTime ()
	{		
		decisionPanel.SetActive(true);
		noButton.onClick.AddListener(HandleNoButton);
		yesButton.onClick.AddListener(HandleYesButton);
		for (int i = 0; i < hearts.Length; i++) {
			var color = hearts[i].color;
			color.a = Player.ratesGiven[i].positive ? 1f : 0.5f;
			hearts[i].color = color;
		}
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
		HandleNoButton();
	}

	void HandleYesButton ()
	{
		StopCoroutine(decisionRoutine);
		StartCoroutine(SendAndWaitForDecisionOutcome(true));
	}

	void HandleNoButton ()
	{
		StopAllCoroutines();
		StartCoroutine(SendAndWaitForDecisionOutcome(false));
	}

	IEnumerator SendAndWaitForDecisionOutcome (bool playerLove)
	{
		decisionPanel.SetActive(false);
		waitingPanel.SetActive(true);

		GameManager.SendDecision(playerLove);

		while (Player.dateInLove == null) {
			yield return null;
		}
		DisplayDecisionEnding();
	}

	void StopTimer ()
	{
		StopCoroutine(decisionRoutine);
		decisionTimerText.text = string.Empty;
	}

	void DisplayDecisionEnding ()
	{
		waitingPanel.SetActive(false);
		quitButton.SetActive(true);
		// Display appropriate animation based on the player decision
		if (Player.playerInLove.positive && Player.dateInLove.positive) {
			inLovePanel.SetActive(true);
		} else if (Player.playerInLove.positive && !Player.dateInLove.positive){
			notInLovePanel.SetActive(true);
			// TODO Set appropriate message
		} else if (!Player.playerInLove.positive && Player.dateInLove.positive) {
			notInLovePanel.SetActive(true);
			// TODO Set appropriate message
		} else {
			notInLovePanel.SetActive(true);
			// TODO Set appropriate message
		}
	}
}
