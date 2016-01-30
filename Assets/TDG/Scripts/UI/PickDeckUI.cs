using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickDeckUI : MonoBehaviour 
{
	public Button backButton;
	public Button readyButton;

	public Transform mainDeckContentPanel;
	public CanvasGroup mainDeckCanvasGroup;

	public Transform playerDeckContentPanel;

	public GameObject cardPrefab;

	GameManager GameManager { get { return GameManager.Instance; } }

	void Awake ()
	{
		backButton.onClick.AddListener(HandleBackButton);
		readyButton.onClick.AddListener(HandleReadyButton);
	}

	void Start ()
	{
		PopulateMainDeck();
		readyButton.interactable = false;
	}

	void PopulateMainDeck ()
	{
		var player = GameManager.Player;
		var phaseCards = GameManager.GetSelectableCardsForPhase(player.boozeLevel);

		foreach (var card in phaseCards) {
			var cardUI = CreateCard(card, mainDeckContentPanel);
			var cardToCreate = card;
			cardUI.cardButton.onClick.AddListener(() => HandleMainDeckCardClicked(cardToCreate, playerDeckContentPanel));
			cardUI.cardButton.onClick.AddListener(() => Destroy(cardUI.gameObject));
		}
	}

	void HandleMainDeckCardClicked (Card card, Transform contentPanel)
	{
		GameManager.Player.cards.Add(card);
		var cardUI = CreateCard(card, contentPanel);
		cardUI.cardButton.onClick.AddListener(() => HandlePlayerDeckCardClicked(card, mainDeckContentPanel));
		cardUI.cardButton.onClick.AddListener(() => Destroy(cardUI.gameObject));

		if (GameManager.Player.cards.Count >= GameManager.maxCardsPerPhase) {
			mainDeckCanvasGroup.alpha = .5f;
			mainDeckCanvasGroup.interactable = false;
			readyButton.interactable = true;
		}
	}

	void HandlePlayerDeckCardClicked (Card card, Transform contentPanel)
	{
		GameManager.Player.cards.Remove(card);

		if (GameManager.Player.cards.Count < GameManager.maxCardsPerPhase) {
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
		SceneManager.LoadScene(MainGameController.SCENE_GENDER_CHOICE);
	}

	void HandleReadyButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_LOADING);
	}
}
