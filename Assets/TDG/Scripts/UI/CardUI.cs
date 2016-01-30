using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour 
{

	public Button cardButton;

	public Text categoryText;
	public Text subCategoryText;
	public Image icon;
	public Image cardBackground;

	public Sprite talkSprite;
	public Sprite emotionSprite;
	public Sprite actionSprite;

	public Card Card { get; private set;}

	public void Init (Card card)
	{
		Card = card;

		categoryText.text = card.category.ToString();
		subCategoryText.text = card.SubCategoryName.SplitCamelCase();
	
		switch (card.category) {
		case CardCategory.Talk:
			cardBackground.overrideSprite = talkSprite;
			break;
		case CardCategory.Emotion:
			cardBackground.overrideSprite = emotionSprite;
			break;
		case CardCategory.Action:
			cardBackground.overrideSprite = actionSprite;
			break;
		}
	}

}
