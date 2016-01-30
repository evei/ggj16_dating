using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
	public List<Card> allCards;

	public List<Text> allTexts;

	public int maxBoozeLevel = 5;

	public int phase;

	const int CARDS_FOR_PHASE = 20;

	const int MAX_DRAW_CARDS = 3;

	void Awake ()
	{
		allCards = GetCards();
		allTexts = GetTexts();
		RandomHelper.r = new System.Random(10000);
	}

	public List<Card> GetSelectableCardsForPhase(int phase, int boozeLevel)
	{
		//TODO predefined sets?/
		var cardsForPhase = allCards.Where(c => c.phase == phase).ToList();

		var result = new List<Card>();
		while (result.Count < CARDS_FOR_PHASE && cardsForPhase.Count > 0) {
			var index = RandomHelper.Next(cardsForPhase.Count);
			var card = cardsForPhase[index];
			result.Add(card);
			cardsForPhase.RemoveAt(index);
		}

		return result;
	}

	public List<Card> DrawCards(int boozeLevel)
	{
		return GetSelectableCardsForPhase(phase, boozeLevel).Take(Math.Max(MAX_DRAW_CARDS - boozeLevel, 1)).ToList();
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

	List<Text> GetTexts ()
	{
		//return JsonUtility.FromJson<List<Text>>();

		return new List<Text>();
	}

	Card CreateCard (int i, CardCategory cardCategory, System.Type enumType)
	{
		var subcategories = System.Enum.GetValues(enumType);
		int subCategory = (int)subcategories.GetValue(RandomHelper.Next(subcategories.Length));
		return new Card(i, cardCategory, subCategory, -1, RandomHelper.Next(maxBoozeLevel + 1));
	}

	public Text GetTextForCard(Card card)
	{
		var possibleTexts = allTexts.Where(t => t.category == card.category && t.subCategory == card.subCategory).ToList();
		if (possibleTexts.Count > 0) {
			return possibleTexts[RandomHelper.Next(possibleTexts.Count)];
		}

		Debug.LogWarningFormat("No text found for card {0}.{1}", card.category, card.SubCategoryName);
	}
}
