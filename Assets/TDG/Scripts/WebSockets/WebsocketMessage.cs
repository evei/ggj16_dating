using System;

[Serializable]
public class WebsocketMessage<T> where T : Payload
{	
	public int id;
	public string user_id;
	public string type;
	public string room;
	public T payload;

	public WebsocketMessage ()
	{
	}

	public WebsocketMessage (string user_id, string type, string room, T payload)
	{
		this.user_id = user_id;
		this.type = type;
		this.room = room;
		this.payload = payload;
	}		
}

public class WebsocketMessage : WebsocketMessage<Payload>
{
	public const string TYPE_JOIN = "join";
	public const string TYPE_JOINED = "joined";
	public const string TYPE_MATCH = "match";
	public const string TYPE_SEND = "send";
	public const string TYPE_SENT = "sent";
	public const string TYPE_MESSAGE = "message";

	public WebsocketMessage ()
	{
	}	

	public WebsocketMessage (string user_id, string type, string room, Payload payload) : base(user_id, type, room, payload)
	{
	}	
}

[Serializable]
public class JoinMessage 
{
	public string user_id;
	public string type = WebsocketMessage.TYPE_JOIN;
	public string room;
	public int last_id;
}

[Serializable]
public class MatchMessage
{
	public string user_id;
	public string type = WebsocketMessage.TYPE_MATCH;
}

[Serializable]
public class SendMessage<T> where T : Payload
{
	public string room;
	public string type = WebsocketMessage.TYPE_SEND;
	public T payload;

	public SendMessage (string room, T payload)
	{
		this.room = room;
		this.payload = payload;
	}
}

public enum PayloadType
{
	None,
	PlayCard,
	Drink,
	RatePhase,
	Decision,
	Flee
}

[Serializable]
public class Payload
{
	public PayloadType type;

	public Payload (PayloadType type)
	{
		this.type = type;
	}	
}

[Serializable]
public class PlayCardPayload : Payload
{
	public int card;
	public int text;
	public bool positive;
	public int index;

	public PlayCardPayload () : base(PayloadType.PlayCard)
	{
	}
	
	public PlayCardPayload (int card, int text, bool positive, int index) : this()
	{
		this.card = card;
		this.text = text;
		this.positive = positive;
		this.index = index;
	}		
}

[Serializable]
public class RatePhasePayload : Payload
{
	public bool positive;
	public RatePhasePayload () : base(PayloadType.RatePhase)
	{
	}
	public RatePhasePayload (bool positive) : this()
	{
		this.positive = positive;
	}	
}

[Serializable]
public class DecisionPayload : Payload
{
	public bool positive;
	public DecisionPayload () : base(PayloadType.Decision)
	{
	}
	public DecisionPayload (bool positive) : this()
	{
		this.positive = positive;
	}	
}


[Serializable]
public class DrinkBoozePayload : Payload
{
	public int boozeLevel; 
	public DrinkBoozePayload () : base(PayloadType.Drink)
	{
	}
	public DrinkBoozePayload (int boozeLevel) : this()
	{
		this.boozeLevel = boozeLevel;
	}	
}
