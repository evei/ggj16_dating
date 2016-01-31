using System.Collections.Generic;
using System;

public class Player
{
	public enum Gender
	{
		Male,
		Female
	}

	public string id = Guid.NewGuid().ToString();

	public int boozeLevel;
	public Gender gender;
	public List<Card> cards;
	public int heartsWon;

	public Player (Gender gender)
	{
		this.gender = gender;
		cards = new List<Card>();
	}
}