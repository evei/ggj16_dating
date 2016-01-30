using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickDeckUI : MonoBehaviour 
{
	public Text phaseText;
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
			cardUI.cardButton.onClick.AddListener(() => HandleMainDeckCardClicked(cardUI, playerDeckContentPanel));
		}
	}

	void HandleMainDeckCardClicked (CardUI cardUI, Transform contentPanel)
	{
		GameManager.Player.cards.Add(cardUI.Card);

		cardUI.transform.SetParent(contentPanel, false);
		cardUI.cardButton.onClick.RemoveAllListeners();
		cardUI.cardButton.onClick.AddListener(() => HandlePlayerDeckCardClicked(cardUI, mainDeckContentPanel));

		if (GameManager.Player.cards.Count >= GameManager.maxCardsPerPhase) {
			mainDeckCanvasGroup.alpha = .5f;
			mainDeckCanvasGroup.interactable = false;
			readyButton.interactable = true;
		}
	}

	void HandlePlayerDeckCardClicked (CardUI cardUI, Transform contentPanel)
	{
		GameManager.Player.cards.Remove(cardUI.Card);

		cardUI.transform.SetParent(contentPanel, false);
		cardUI.cardButton.onClick.RemoveAllListeners();
		cardUI.cardButton.onClick.AddListener(() => HandleMainDeckCardClicked(cardUI, playerDeckContentPanel));

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
