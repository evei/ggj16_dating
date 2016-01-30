using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public List<Card> allCards;

	public int maxBoozeLevel = 5;

	void Awake ()
	{
		allCards = GetCards();


	}

	List<Card> GetCards ()
	{
		//return JsonUtility.FromJson<List<Card>>();

		var list = new List<Card>();
		int i = 0;
		for (i = 0; i < 30; i++) {
			list.Add(CreateCard(i, typeof(TalkCategory)));
		}

		for (; i < 50; i++) {
			list.Add(CreateCard(i, typeof(EmotionCategory)));
		}

		for (; i < 70; i++) {
			list.Add(CreateCard(i, typeof(ActionCategory)));
		}

		return list;
	}

	Card CreateCard (int i, System.Type enumType)
	{
		var subcategories = System.Enum.GetValues(enumType);
		int subCategory = (int)subcategories.GetValue(Random.Range(0, subcategories.Length-1));
		return new Card(i, (int)CardCategory.Talk, subCategory, Random.Range(0, maxBoozeLevel));
	}
}
