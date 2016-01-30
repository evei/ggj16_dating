using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Wooga.Coroutines;
using System.Collections;

public class GameManager
{
	public List<Card> allCards;

	public List<CardText> allTexts;

	public int maxBoozeLevel = 5;
	public int maxCardsPerPhase = 5;

	int phase;
	public bool FirstPhase {
		get {
			return phase == 0;
		}
	}

	public bool localPlayerFirst;

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
		userId = null;
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

		Debug.LogWarningFormat("No text found for card {0}.{1}", card.category, card.SubCategoryName);
		return new CardText(-1, card.category, card.subCategory, null, null);
	}

	#region Player

	void CreatePlayer (Player.Gender gender)
	{
		Player = new Player(gender);
	}

	#endregion

	#region webservice
	Wooroutine<WebSocket> _webSocketWoo;
	Wooroutine<WebSocket> WebSocketWoo {
		get {
			while (_webSocketWoo == null || _webSocketWoo.HasError || (_webSocketWoo.Completed && !_webSocketWoo.ReturnValue.error.IsNullOrEmpty())) {
				_webSocketWoo = WooroutineRunner.StartRoutine<WebSocket>(WebsocketRoutine());
			}
			return _webSocketWoo;
		}
	}
		
	IEnumerator WebsocketRoutine ()
	{
		WebSocket w = new WebSocket(new Uri("ws://dating-room-ggj2016.herokuapp.com/websocket"));
		yield return WooroutineRunner.StartRoutine(w.Connect());
		w.SendString(GetJoinMessage());

		yield return w;
	}

	string userId;
	string room;
	List<WebserviceMessage> receivedMessages = new List<WebserviceMessage>();

	void ResetWebservice()
	{
		userId = null;
		room = null;
		receivedMessages.Clear();
		if (_webSocketWoo != null && _webSocketWoo.Completed) {
			_webSocketWoo.ReturnValue.Close();			
		}
	}

		
	IEnumerator ListenToWebsocketRoutine (WebSocket w)
	{
		while (w.error == null) {
			var json = w.RecvString();
			if (json != null) {
				var message = JsonUtility.FromJson<WebserviceMessage>(json);
				if (receivedMessages.All(m => m.id != message.id)) {
					receivedMessages.Add(message);
				}
			}
			yield return null;
		}
	}

	public void PlayCard (Card card, CardText cardText)
	{
		Send(new PlayCardPayload(card.id, cardText.id, card.positive));
	}

	public Wooroutine<WebserviceMessage> PlayCard (Card card, CardText cardText)
	{
		yield return Send(new PlayCardPayload(card.id, cardText.id, card.positive));

		//own message
		yield return WooroutineRunner.StartRoutine<WebserviceMessage>(WaitForMessageRoutine()).Await();

		var nextMessage = WooroutineRunner.StartRoutine<WebserviceMessage>(WaitForMessageRoutine());
		yield return nextMessage;
	}

	public void Drink(int boozeLevel)
	{
		Send(new DrinkBoozePayload(boozeLevel));
	}

	public Coroutine StartDate()
	{
		return WooroutineRunner.StartRoutine(StartDateRoutine());
	}

	IEnumerator StartDateRoutine ()
	{
		while (true) {
			var message = WooroutineRunner.StartRoutine<WebserviceMessage>(WaitForMessageRoutine());
			yield return message.Await();
			if (message.ReturnValue != null) {
				Debug.Log(message.ReturnValue);
				if (message.ReturnValue.users == null || message.ReturnValue.users.Length == 0) {
					localPlayerFirst = true;
					message = WooroutineRunner.StartRoutine<WebserviceMessage>(WaitForMessageRoutine());
//					yield return message.Await();
//					if (message.ReturnValue != null) {
//						yield break;
//					}
				}
				yield break;
			}
		}
	}

	Coroutine Send (object payload)
	{
		sendRoutine = WooroutineRunner.StartRoutine(SendRoutine(JsonUtility.ToJson(new WebserviceMessage(userId, "send", room, payload))));
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

	IEnumerator WaitForMessageRoutine()
	{
		var amount = receivedMessages.Count;
		while (amount <= receivedMessages.Count) {
			yield return null;
		}
		yield return receivedMessages[amount];
	}

	string GetJoinMessage ()
	{
		return "{\"type\":\"join\",\"room\":\"foo\"}"; //TODO change as soon as reconnect is available
	}
	#endregion
}