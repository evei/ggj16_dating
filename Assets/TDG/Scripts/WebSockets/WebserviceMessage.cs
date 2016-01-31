using System.Collections.Generic;

public class WebserviceMessage
{
	public int id;
	public string user_id;
	public string type;
	public string room;
	public object payload;
	public string[] users;

	public WebserviceMessage ()
	{
	}

	public WebserviceMessage (string user_id, string type, string room, object payload)
	{
		this.user_id = user_id;
		this.type = type;
		this.room = room;
		this.payload = payload;
	}		
}

public class PlayCardPayload
{
	public int card;
	public int text;
	public bool positive;

	public PlayCardPayload ()
	{
	}
	
	public PlayCardPayload (int card, int text, bool positive)
	{
		this.card = card;
		this.text = text;
		this.positive = positive;
	}		
}

public class RatePhasePayload
{
	public bool positive;
	public RatePhasePayload ()
	{
	}
	public RatePhasePayload (bool positive)
	{
		this.positive = positive;
	}	
}

class DrinkBoozePayload 
{
	public int boozeLevel; 
	public DrinkBoozePayload ()
	{
	}
	public DrinkBoozePayload (int boozeLevel)
	{
		this.boozeLevel = boozeLevel;
	}	
}

