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
	public bool positive;
	public bool used;

	public Card (int id, CardCategory category, int subCategory, bool positive = true, int phase = -1, int boozeLevel = -1)
	{
		this.id = id;
		this.category = category;
		this.subCategory = subCategory;
		this.positive = true;
		this.phase = phase;
		this.boozeLevel = boozeLevel;
	}

	public string SubCategoryName { get { return GetSubCategoryName(category, subCategory); } }

	public static string GetSubCategoryName(CardCategory category, int subCategory)
	{
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
	string good;
	public string Good {
		get {
			return good ?? string.Format("{0}.{1} <GOOD>", category, Card.GetSubCategoryName(category, subCategory));
		}
	}
	string bad;
	public string Bad {
		get {
			return bad ?? string.Format("{0}.{1} <BAD>", category, Card.GetSubCategoryName(category, subCategory));
		}
	}

	public CardText (int id, CardCategory category, int subCategory, string good, string bad)
	{
		this.id = id;
		this.category = category;
		this.subCategory = subCategory;
		this.good = good;
		this.bad = bad;
	}	
}