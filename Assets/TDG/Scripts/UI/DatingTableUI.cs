using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DatingTableUI : MonoBehaviour 
{
	public Button quitButton;
	public Button booseButton;

	public Transform playerDeckContentPanel;
	public GameObject cardPrefab;

	GameManager GameManager { get { return GameManager.Instance; } }

	void Awake ()
	{
		quitButton.onClick.AddListener(HandleQuitButton);
		booseButton.onClick.AddListener(DrinkBoose);
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
		} 
	}

	void PlayCard (Card card)
	{
		// TODO Do something.
		Debug.Log("Play Card");
	}

	void DrinkBoose ()
	{
		// TODO Do something
		Debug.Log("Drink Boose");
	}
}
