using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickDeckUI : MonoBehaviour 
{
	public Button backButton;
	public Button readyButton;

	public Transform mainDeckContentPanel;
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
	}

	void PopulateMainDeck ()
	{
		var player = GameManager.Player;
		var phaseCards = GameManager.GetSelectableCardsForPhase(player.boozeLevel);

		foreach (var card in phaseCards) {
			var cardUI = CreateCard(card, mainDeckContentPanel);
			cardUI.cardButton.onClick.AddListener(() => HandleCardClicked(card, playerDeckContentPanel));
		}
	}

	void HandleCardClicked (Card card, Transform contentPanel)
	{
		CreateCard(card, contentPanel);
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
