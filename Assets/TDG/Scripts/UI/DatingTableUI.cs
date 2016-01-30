using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DatingTableUI : MonoBehaviour 
{
	public Button quitButton;
	public Button booseButton;

	public Transform playerDeckContentPanel;
	public GameObject cardPrefab;

	public SpeechBubble speechBuble;
	public RatingPanel ratingPanel;

	GameManager GameManager { get { return GameManager.Instance; } }

	void Awake ()
	{
		quitButton.onClick.AddListener(HandleQuitButton);
		booseButton.onClick.AddListener(DrinkBooze);
	}

	void HandleQuitButton ()
	{
		// TODO Quit from server room
		SceneManager.LoadScene(MainGameController.SCENE_LOBBY);
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

	void HandlePlayerDeckCardClicked (Card card)
	{
		PlayCard(card);
		GameManager.Player.cards.Remove(card);

		if (GameManager.Player.cards.Count <= 0) {
			// TODO Do something
			Debug.Log("Played Last card");
			ratingPanel.Show(); // TODO Move his line to the callback when both player finished playing their cards.
		} 
	}

	void PlayCard (Card card)
	{
		// TODO Do something.
		Debug.Log("Play Card");
		DisplayText(card);
	}

	void DrinkBooze ()
	{
		// TODO Do something
		Debug.Log("Drink Booze");
	}

	void DisplayText (Card card)
	{
		var cardText = GameManager.GetTextForCard(card, GameManager.Player);
		speechBuble.DisplayText(card.positive ? cardText.Good : cardText.Bad);
	}
}
