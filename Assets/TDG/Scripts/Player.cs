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
	public List<RatePhasePayload> ratesReceived = new List<RatePhasePayload>();
	public List<RatePhasePayload> ratesGiven = new List<RatePhasePayload>();

	public bool startsPhase;

	public DecisionPayload playerInLove;
	public DecisionPayload dateInLove;

	public Player (Gender gender)
	{
		this.gender = gender;
		cards = new List<Card>();
	}
}