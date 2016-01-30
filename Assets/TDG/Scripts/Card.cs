using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public enum CardCategory
{
	Talk = 1,
	Action = 2,
	Emotion = 3
}

public enum TalkCategory 
{
	Compliment = 1,
	ShowOff = 2,
	Smalltalk = 3,
	Funny = 4,
	NerdOut = 5
}

public enum ActionCategory 
{
	TouchHand = 1,
	TouchHair = 2,
	Kiss = 3,
	Blink = 4,
	Fart = 5,
	PickYourNose = 6,
	NatureCalls = 7
}

public enum EmotionCategory 
{
	Laugh = 1,
	Cry = 2
}

public class Card
{

	public int id;
	public int category;
	public int subCategory;
	public int boozeLevel;

	public Card (int id, int category, int subCategory, int boozeLevel)
	{
		this.id = id;
		this.category = category;
		this.subCategory = subCategory;
		this.boozeLevel = boozeLevel;
	}

	public Text FetchText (int category, int subCategory)
	{
		return null;
	}
}

public class Text
{
	public int id;
	public int category;
	public int subCategory;
	public string text;
}