using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

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
	}

	public void Init(Player.Gender gender)
	{
		CreatePlayer(gender);
		allCards = GetCards();
		allTexts = GetTexts();
		RandomHelper.r = new System.Random(10000);
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

	void CreatePlayer (Player.Gender gender)
	{
		Player = new Player(gender);
	}

	public string GetPhaseName ()
	{
		// TODO Implement
		return "Impress her";
	}
}
