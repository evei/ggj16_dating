using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DatingTableUI : MonoBehaviour 
{
	public Button quitButton;
	public Button booseButton;

	public CanvasGroup playDeckCanvasGroup;
	public Transform playerDeckContentPanel;
	public GameObject cardPrefab;

	public SpeechBubble speechBuble;
	public SpeechBubble dateSpeechBuble;
	public RatingPanel ratingPanel;

	GameManager GameManager { get { return GameManager.Instance; } }

	void Awake ()
	{
		quitButton.onClick.AddListener(HandleFleeButton);
		booseButton.onClick.AddListener(DrinkBooze);
		speechBuble.bubbleClosed += UnlockPlayerCards;
		dateSpeechBuble.bubbleClosed += UnlockPlayerCards;
	}

	void Start ()
	{
		PopulateMainDeck();
	}

	void PopulateMainDeck ()
	{
		foreach (var card in GameManager.Player.cards) {
			var cardUI = CreateCard(card, playerDeckContentPanel);
			var cardToCreate = card;
			cardUI.cardButton.onClick.AddListener(() => HandlePlayerDeckCardClicked(cardToCreate));
			cardUI.cardButton.onClick.AddListener(() => Destroy(cardUI.gameObject));
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

	#region Gameplay

	void HandlePlayerDeckCardClicked (Card card)
	{
		PlayCard(card);
		GameManager.Player.cards.Remove(card);

		if (GameManager.Player.cards.Count <= 0) {
			// TODO Do something
			Debug.Log("Played Last card");
			ratingPanel.Show(); // TODO Move this line to the callback when both player finished playing their cards and the Bubbles are gone.
		} 
	}

	void PlayCard (Card card)
	{
		LockPlayerCards();
		DisplayText(card, speechBuble);
	}

	void DisplayText (Card card, SpeechBubble bubble)
	{
		var cardText = GameManager.GetTextForCard(card, GameManager.Player);
		bubble.DisplayText(card.positive ? cardText.Good : cardText.Bad);
	}

	void UnlockPlayerCards ()
	{
		playDeckCanvasGroup.alpha = 1;
		playDeckCanvasGroup.interactable = true;
	}

	void LockPlayerCards ()
	{
		playDeckCanvasGroup.alpha = .5f;
		playDeckCanvasGroup.interactable = false;
	}

	IEnumerator WaitForPlayerResponse ()
	{
		LockPlayerCards();
		// TODO Wait until player plays card
		yield return null;

		DisplayText(null, dateSpeechBuble); // TODO Pass the card
	}

	#endregion

	#region Booze

	void DrinkBooze ()
	{
		StartCoroutine(DrinkBoozeRoutine());
	}

	IEnumerator DrinkBoozeRoutine ()
	{
		GameManager.Player.boozeLevel++;

		// TODO Play Drink booze animation
		yield return null;

		if (GameManager.Player.boozeLevel >= GameManager.maxBoozeLevel) {
			GameManager.CurrentState = GameManager.GameState.PlayerPassesOut;
			// TODO Pass out animation
			SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
		}
	}

	void HandleDatePassesOut ()
	{
		GameManager.CurrentState = GameManager.GameState.DatePassesOut;
		// TODO Show Date passses out animation
		SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
	}

	#endregion

	void HandleFleeButton ()
	{
		GameManager.CurrentState = GameManager.GameState.PlayerFlees;
		// TODO Send event so opponent is left alone and game ends
		// TODO Quit from server room
		SceneManager.LoadScene(MainGameController.SCENE_LOBBY);
	}

	void HandleDateFlees ()
	{
		GameManager.CurrentState = GameManager.GameState.DateFlees;
		// TODO Show Date empty chair
		SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
	}
}
