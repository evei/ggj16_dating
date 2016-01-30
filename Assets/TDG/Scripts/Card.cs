public enum CardCategory
{
	None = -1,
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
	Cry = 2,
	Blush = 3,
	Drool = 4,
	Yawn = 5
}

public class Card
{
	public int id;
	public CardCategory category;
	public int subCategory;
	public int phase;
	public int boozeLevel;

	public Card (int id, CardCategory category, int subCategory, int phase = -1, int boozeLevel = -1)
	{
		this.id = id;
		this.category = category;
		this.subCategory = subCategory;
		this.phase = phase;
		this.boozeLevel = boozeLevel;
	}

	public string SubCategoryName {
		get {
			switch (category) {
				case CardCategory.Talk:
					return ((TalkCategory)subCategory).ToString();				
				case CardCategory.Emotion:
					return ((EmotionCategory)subCategory).ToString();
				case CardCategory.Action:
					return ((ActionCategory)subCategory).ToString();
			}
			return null;
		}
	}
}

public class CardText
{
	public int id;
	public CardCategory category;
	public int subCategory;
	public string text;

	public CardText (int id, CardCategory category, int subCategory, string text)
	{
		this.id = id;
		this.category = category;
		this.subCategory = subCategory;
		this.text = text;
	}	
}