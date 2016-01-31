using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PickDeckUI : MonoBehaviour 
{
	public Text phaseText;
	public Text timerText;
	public Button backButton;
	public Button readyButton;

	public Transform mainDeckContentPanel;
	public CanvasGroup mainDeckCanvasGroup;

	public Transform playerDeckContentPanel;

	public GameObject cardPrefab;

	const string PHASE_TEXT_FORMAT = "Date Phase {0}: {1}";

	GameManager GameManager { get { return GameManager.Instance; } }

	const int TIME_TO_PICK = 30;
	float elapsedTime;

	List<Card> availableCards = new List<Card>();

	void Awake ()
	{
		backButton.onClick.AddListener(HandleBackButton);
		readyButton.onClick.AddListener(HandleReadyButton);
	}

	void Start ()
	{
		PopulateMainDeck();
		readyButton.interactable = false;

		timerText.text = TIME_TO_PICK.ToString();
		StartCoroutine(StartTimer());
		SetPhaseText();
	}

	void SetPhaseText ()
	{
		phaseText.text = string.Format(PHASE_TEXT_FORMAT, GameManager.phase + 1, GameManager.GetPhaseName());
	}

	void PopulateMainDeck ()
	{
		Debug.Log("CURRENT PHASE = " + GameManager.phase);

		var player = GameManager.Player;
		var phaseCards = GameManager.GetSelectableCardsForPhase();
		availableCards.AddRange(phaseCards);

		foreach (var card in phaseCards) {
			var cardUI = CreateCard(card, mainDeckContentPanel);
			cardUI.cardButton.onClick.AddListener(() => HandleMainDeckCardClicked(cardUI, playerDeckContentPanel));
		}
	}

	void HandleMainDeckCardClicked (CardUI cardUI, Transform contentPanel)
	{
		availableCards.Remove(cardUI.Card);
		GameManager.Player.cards.Add(cardUI.Card);

		cardUI.transform.SetParent(contentPanel, false);
		cardUI.cardButton.onClick.RemoveAllListeners();
		cardUI.cardButton.onClick.AddListener(() => HandlePlayerDeckCardClicked(cardUI, mainDeckContentPanel));

		if (GameManager.Player.cards.Count(c => !c.used) >= GameManager.maxCardsPerPhase) {
			mainDeckCanvasGroup.alpha = .5f;
			mainDeckCanvasGroup.interactable = false;
			readyButton.interactable = true;
		}
	}

	void HandlePlayerDeckCardClicked (CardUI cardUI, Transform contentPanel)
	{
		availableCards.Add(cardUI.Card);
		GameManager.Player.cards.Remove(cardUI.Card);

		cardUI.transform.SetParent(contentPanel, false);
		cardUI.cardButton.onClick.RemoveAllListeners();
		cardUI.cardButton.onClick.AddListener(() => HandleMainDeckCardClicked(cardUI, playerDeckContentPanel));

		if (GameManager.Player.cards.Count(c => !c.used) < GameManager.maxCardsPerPhase) {
			mainDeckCanvasGroup.alpha = 1f;
			mainDeckCanvasGroup.interactable = true;
			readyButton.interactable = false;
		}
	}

	CardUI CreateCard (Card card, Transform contentPanel)
	{
		var cardGo = Instantiate(cardPrefab);
		cardGo.transform.SetParent(contentPanel, false);
		var cardUI = cardGo.GetComponent<CardUI>();
		cardUI.Init(card);

		return cardUI;
	}

	void HandleBackButton ()
	{
		StopAllCoroutines();
		SceneManager.LoadScene(MainGameController.SCENE_GENDER_CHOICE);
	}

	void HandleReadyButton ()
	{
		StartPhase();
	}

	void StartPhase ()
	{
		StopAllCoroutines();

		// Check if Player has all cards, if not assign random cards to complete the deck
		if (GameManager.Player.cards.Count < GameManager.maxCardsPerPhase) {
			CompleteDeck();
		}

		SceneManager.LoadScene(MainGameController.SCENE_LOADING);
	}

	void CompleteDeck ()
	{
		var missingCardAmount = GameManager.maxCardsPerPhase - GameManager.Player.cards.Count;
		for (int i = 0; i < missingCardAmount; i++) {
			var cardIndex = RandomHelper.Next(availableCards.Count);
			GameManager.Player.cards.Add(availableCards[cardIndex]);
		}
	}

	IEnumerator StartTimer ()
	{
		while (elapsedTime <= TIME_TO_PICK) {
			timerText.text = ((int)(TIME_TO_PICK - elapsedTime)).ToString();
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		timerText.text = "0";

		StartPhase();
	}
}
