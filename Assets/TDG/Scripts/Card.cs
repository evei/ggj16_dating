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
	Joke = 4,
	NerdOut = 5,
	ExRelationship = 6,
	Work = 7,
	Parents = 8,
	Food = 9,
	Childhood = 10,
	Pets = 11,
	StayQuiet = 12
}

public enum ActionCategory 
{
	Kiss = 1,
	Wink = 2,
	Fart = 3,
	PickYourNose = 4,
	GoToRestroom = 5,
	TapOnTable = 6,
	Texting = 7,
	Leave = 8,
	Frown = 9,
	Smile = 10,
	Pokerface = 11,
	SmokeAZigarette = 12
}

public enum EmotionCategory 
{
	Laugh = 1,
	Cry = 2,
	Blush = 3,
	Drool = 4,
	Yawn = 5,
	Panic = 6,
	Anger = 7
}

public class Card
{
	public int id;
	public CardCategory category;
	public int subCategory;
	public int phase;
	public int boozeLevel;
	public bool used;

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

	public override string ToString ()
	{
		return string.Format("[Card: category={0}, subCategory={1}]", category, SubCategoryName);
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