using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Wooga.Coroutines;
using System.Collections;

public class GameManager
{
	public enum GameState 
	{
		Dating = 0,
		PlayerPassesOut = 1,
		DatePassesOut = 2,
		DateFlees = 3,
		PlayerFlees = 4,
		Decision = 5
	}

	public List<Card> allCards;

	public List<CardText> allTexts;

	public int maxPhasesNumber = 3;
	public int maxBoozeLevel = 5;
	public int maxCardsPerPhase = 5;

	public int phase;
	public bool FirstPhase {
		get {
			return phase == 0;
		}
	}

	public bool localPlayerFirst;

	public GameState CurrentState {
		get;
		set;
	}

	const int CARDS_FOR_PHASE = 20;

	public Player Player {
		get;
		private set;
	}

	static GameManager _instance;
	public static GameManager Instance {
		get {
			return _instance ?? (_instance = new GameManager());
		}	
	}

	GameManager ()
	{
//		RandomHelper.r = new System.Random(10000);

	}

	public void Init(Player.Gender gender)
	{
		phase = 0;
		CreatePlayer(gender);
		allCards = GetCards();
		allTexts = GetTexts();
	}

	public List<Card> GetSelectableCardsForPhase(int boozeLevel)
	{
		var cardsForPhase = GetAllCardsForPhaseAndBoozeLevel(boozeLevel);
		var selectedCards = TakeRandomCards(cardsForPhase, CARDS_FOR_PHASE);
		Debug.Log(string.Join("|", selectedCards.Select(c => c.ToString()).ToArray()));
		return selectedCards;
	}

	public List<Card> GetAllCardsForPhaseAndBoozeLevel(int boozeLevel) 
	{
		//TODO predefined sets?/
		return allCards.Where(c => (c.phase == -1 || c.phase == phase) && c.boozeLevel >= boozeLevel).ToList();
	}

	public void PlayerDrinks(Player player)
	{
		var cards = GetAllCardsForPhaseAndBoozeLevel(player.boozeLevel).Except(player.cards).ToList();
		var unusedCards = player.cards.Where(c => !c.used).ToList();
		var newCards = TakeRandomCards(cards, Math.Min(player.boozeLevel, unusedCards.Count));

		while (unusedCards.Count > 0 && newCards.Count > 0) {
			var unusedCard = unusedCards[RandomHelper.Next(unusedCards.Count)];
			player.cards[player.cards.FindIndex(c => c == unusedCard)] = newCards[0];
			unusedCards.Remove(unusedCard);
			newCards.RemoveAt(0);
		}
	}

	List<Card> GetCards ()
	{
		//return JsonUtility.FromJson<List<Card>>();

		var list = new List<Card>();
		int i = 0;
		for (i = 0; i < 30; i++) {
			list.Add(CreateCard(i, CardCategory.Talk, typeof(TalkCategory)));
		}

		for (; i < 50; i++) {
			list.Add(CreateCard(i, CardCategory.Emotion, typeof(EmotionCategory)));
		}

		for (; i < 70; i++) {
			list.Add(CreateCard(i, CardCategory.Action, typeof(ActionCategory)));
		}

		return list;
	}

	List<CardText> GetTexts ()
	{
		//return JsonUtility.FromJson<List<Text>>();

		return new List<CardText>();
	}

	Card CreateCard (int i, CardCategory cardCategory, System.Type enumType)
	{
		var subcategories = System.Enum.GetValues(enumType);
		int subCategory = (int)subcategories.GetValue(RandomHelper.Next(subcategories.Length));
		return new Card(i, cardCategory, subCategory, RandomHelper.TrueFalse(), -1, RandomHelper.Next(maxBoozeLevel + 1));
	}

	public List<Card> TakeRandomCards (List<Card> cards, int amount)
	{
		var result = new List<Card>();
		while (result.Count < amount && cards.Count > 0) {
			var index = RandomHelper.Next(cards.Count);
			var card = cards[index];
			result.Add(card);
			cards.RemoveAt(index);
		}

		return result;
	}

	public CardText GetTextForCard(Card card, Player player)
	{
		var possibleTexts = allTexts.Where(t => t.category == card.category && t.subCategory == card.subCategory).ToList();
		if (possibleTexts.Count > 0) {
			return possibleTexts[RandomHelper.Next(possibleTexts.Count)];
		}

		Debug.LogWarningFormat("No text found for card {0}. {1}", card.category, card.SubCategoryName);
		return new CardText(-1, card.category, card.subCategory, null, null);
	}

	void CreatePlayer (Player.Gender gender)
	{
		Player = new Player(gender);
	}

	public string GetPhaseName ()
	{
		// TODO Get the second and third phase names.

		switch (phase) {
		case 0:
			return "Introduction";
		case 1:
			return "Impress her";
		case 2:
			return "Go for it";
		}

		return string.Empty;
	}

	#region webservice
	const string ROOM = "tdg";

	public Action<WebserviceMessage> OnMessageFromOtherPlayer = msg => {};

	public Coroutine StartDate()
	{
		receivedMessages.Clear();
		listenRoutine = WooroutineRunner.StartRoutine(ListenToWebsocketRoutine());
		return WooroutineRunner.StartRoutine(StartDateRoutine());
	}

	IEnumerator StartDateRoutine()
	{
		var index = receivedMessages.Count;
		bool otherPlayerJoined = false;

		while (!otherPlayerJoined) {			
			while (!WebSocketWoo.Completed) {
				yield return WebSocketWoo.Await();
			}
			
			while (index >= receivedMessages.Count) {
				yield return null;
			}
			
			var msg = receivedMessages[index];
			if (msg.type == "joined" && msg.user_id != Player.id) {
				otherPlayerJoined = true;
			}
			index++;
		}
	}

	Wooroutine<WebSocket> _webSocketWoo;
	Wooroutine<WebSocket> WebSocketWoo {
		get {
			if (_webSocketWoo == null || _webSocketWoo.HasError || (_webSocketWoo.Completed && _webSocketWoo.ReturnValue.error != null)) {
				_webSocketWoo = WooroutineRunner.StartRoutine<WebSocket>(ConnectRoutine());
			}
			return _webSocketWoo;
		}
	}
		
	IEnumerator ConnectRoutine ()
	{
		var joinMsg = JsonUtility.ToJson(GetJoinMessage());
		Debug.LogWarning("(re)connecting: " + joinMsg);
		WebSocket w = new WebSocket(new Uri("ws://dating-room-ggj2016.herokuapp.com/websocket"));
		yield return WooroutineRunner.StartRoutine(w.Connect());
		w.SendString(joinMsg);
		yield return w;
	}
		
	List<WebserviceMessage> receivedMessages = new List<WebserviceMessage>();

	public void CloseWebsocket()
	{
		if (listenRoutine != null) {
			WooroutineRunner.StopRoutine(listenRoutine);
		}
		if (_webSocketWoo.Completed) {			
			_webSocketWoo.ReturnValue.Close();
		}
		else {
			_webSocketWoo.Stop();
			_webSocketWoo = null;
		}
	}

	Coroutine listenRoutine;
	IEnumerator ListenToWebsocketRoutine ()
	{
		while (true) {
			while (!WebSocketWoo.Completed) {
				yield return WebSocketWoo.Await();
			}
			var json = WebSocketWoo.ReturnValue.RecvString();
			if (json != null && WebSocketWoo.ReturnValue.error == null) {
				Debug.Log(json);
				var message = JsonUtility.FromJson<WebserviceMessage>(json);
				if (receivedMessages.All(m => m.id != message.id)) {
					receivedMessages.Add(message);
					if (message.user_id != Player.id) {
						OnMessageFromOtherPlayer(message);
					}
				}
			}
			if (WebSocketWoo.ReturnValue.error != null) {
				Debug.LogError(WebSocketWoo.ReturnValue.error);
			}
			yield return null;
		}
	}

	public void PlayCard (Card card, CardText cardText)
	{
		Send(new PlayCardPayload(card.id, cardText.id, card.positive));
	}

	public void Drink(int boozeLevel)
	{
		Send(new DrinkBoozePayload(boozeLevel));
	}

	public void RatePhase(bool positive)
	{
		Send(new RatePhasePayload(positive));
	}

	Coroutine Send (object payload)
	{
		sendRoutine = WooroutineRunner.StartRoutine(SendRoutine(JsonUtility.ToJson(new WebserviceMessage(Player.id, "send", ROOM, payload))));
		return sendRoutine;
	}

	Coroutine sendRoutine;
	IEnumerator SendRoutine (string message)
	{	
		if (sendRoutine != null) {
			var previousSendRoutine = sendRoutine;
			yield return previousSendRoutine;
		}

		while (!WebSocketWoo.Completed) {
			yield return WebSocketWoo.Await();			
		}

		WebSocketWoo.ReturnValue.SendString(message);
	}

	JoinMessage GetJoinMessage ()
	{
		var msg = new JoinMessage { user_id = Player.id};
		if (receivedMessages.Count > 0) {
			msg.last_id = receivedMessages[receivedMessages.Count - 1].id;
		}
		else {
			msg.last_id = -1;
		}
		return msg;
	}

	class JoinMessage 
	{
		public string user_id;
		public string type = "join";
		public string room = ROOM;
		public int last_id;
	}
	#endregion
}