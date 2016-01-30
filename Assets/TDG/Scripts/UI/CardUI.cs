using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour 
{

	public Button cardButton;

	public Text categoryText;
	public Text subCategoryText;
	public Image icon;
	public Image cardBackground;

	public Color talkColor;
	public Color emotionColor;
	public Color actionColor;

	Card card;

	public void Init (Card card)
	{
		this.card = card;

		categoryText.text = card.category.ToString();
		subCategoryText.text = card.SubCategoryName.SplitCamelCase();
	
		switch (card.category) {
		case CardCategory.Talk:
			cardBackground.color = talkColor;
			break;
		case CardCategory.Emotion:
			cardBackground.color = emotionColor;
			break;
		case CardCategory.Action:
			cardBackground.color = actionColor;
			break;
		}
	}

}
