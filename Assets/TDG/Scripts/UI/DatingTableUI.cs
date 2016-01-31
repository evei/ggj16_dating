using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

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

	public CharacterAnimationController animController;

	public ParticleSystem drinkBoozeParticles;

	GameManager GameManager { get { return GameManager.Instance; } }

	Player Player { get { return GameManager.Player; } }

	void Awake ()
	{
		quitButton.onClick.AddListener(HandleFleeButton);
		booseButton.onClick.AddListener(DrinkBooze);
		dateSpeechBuble.bubbleClosed += PlayerMoveStarts;
		speechBuble.bubbleClosed += PlayerMoveOver;
	}

	void OnDestroy ()
	{
		GameManager.OnDatePlaysCard -= HandleDatePlaysCard;
		GameManager.OnDateDrinks -= HandleDateDrinks;
		GameManager.OnDateFlees -= HandleDateFlees;
	}

	void Start ()
	{
		PopulateMainDeck();
		if (!Player.startsPhase) {
			LockPlayerCards();	
		}

		GameManager.PauseWebsocketListener(false);
		GameManager.OnDatePlaysCard += HandleDatePlaysCard;
		GameManager.OnDateDrinks += HandleDateDrinks;
		GameManager.OnDateFlees += HandleDateFlees;
	}

	void PopulateMainDeck ()
	{		
		for (int i = 0; i < playerDeckContentPanel.childCount; i++) {
			Destroy(playerDeckContentPanel.GetChild(i).gameObject);
		}
		foreach (var card in Player.cards.Where(c => !c.used)) {
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
		card.used = true;;
	}

	void PlayCard (Card card)
	{
		LockPlayerCards();
		var cardText = GameManager.ChooseTextForCard(card);
		GameManager.SendPlayCard(card, cardText);
		DisplayText(cardText, card.positive, speechBuble);
	}

	void DisplayText (CardText cardText, bool positive, SpeechBubble bubble)
	{
		bubble.DisplayText(positive ? cardText.good : cardText.bad);
	}

	void PlayerMoveStarts ()
	{
		UnlockPlayerCards();
		if (Player.startsPhase && Player.cards.TrueForAll(c => c.used)) {
			ratingPanel.Show();
		}
	}

	void PlayerMoveOver ()
	{
		if (!Player.startsPhase && Player.cards.TrueForAll(c => c.used)) {
			ratingPanel.Show();
		}
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

	#endregion

	#region Booze

	void DrinkBooze ()
	{
		GameManager.PlayerDrinks();
		PopulateMainDeck();
		StartCoroutine(DrinkBoozeRoutine());
	}

	IEnumerator DrinkBoozeRoutine ()
	{
		LockPlayerCards();

		drinkBoozeParticles.gameObject.SetActive(true);
		drinkBoozeParticles.Play();

		yield return new WaitForSeconds(4f);

		drinkBoozeParticles.gameObject.SetActive(false);
		drinkBoozeParticles.Stop();

		if (Player.boozeLevel >= GameManager.maxBoozeLevel) {
			GameManager.CurrentState = GameManager.GameState.PlayerPassesOut;
			SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
		}

		UnlockPlayerCards();
	}

	#endregion

	void HandleFleeButton ()
	{
		GameManager.CurrentState = GameManager.GameState.PlayerFlees;
		// TODO Send event so opponent is left alone and game ends
		// TODO Quit from server room
		SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
	}		

	#region other player

	void HandleDatePlaysCard (PlayCardPayload playCardPayload)
	{
		//TODO display card action/effect besides speech bubble?
		var card = GameManager.GetCard(playCardPayload.card);
		var cardText = GameManager.GetCardText(playCardPayload.text);
		DisplayText(cardText ?? GameManager.ChooseTextForCard(card), playCardPayload.positive, dateSpeechBuble);

		animController.PlayCard(card, AnimationEndsCallback);
	}

	void HandleDateDrinks (DrinkBoozePayload drinkBoozePayload)
	{		
		if (drinkBoozePayload.boozeLevel > GameManager.maxBoozeLevel) {
			//TODO
			Debug.Log("Date passed out");
			GameManager.CurrentState = GameManager.GameState.DatePassesOut;

			animController.PlayPassoutAnimation(LoadDecision);
		}
	}

	void HandleDateFlees ()
	{
		//TODO 
		Debug.Log("Date fled the dating scene");
		GameManager.CurrentState = GameManager.GameState.DateFlees;

		animController.PlayFleeAnimation(LoadDecision);
	}		

	#endregion

	void AnimationEndsCallback ()
	{
		Debug.LogWarning(">>>> Animation finished"); 
	}

	void LoadDecision ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
	}
}
