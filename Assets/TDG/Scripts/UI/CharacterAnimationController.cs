using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CharacterAnimationController : MonoBehaviour 
{
	public Animator characterAnimator;
	public AnimationClip fallbackClip;

	public List<TalkAnimConfig> talkConfig;
	public List<ActionAnimConfig> actionConfig;
	public List<EmotionAnimConfig> emotionConfig;

	public void PlayCard (Card card, Action onAnimationFinished)
	{
		switch (card.category) {
		case CardCategory.Talk:
			var talkClip = talkConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory).clip;
			characterAnimator.SetTrigger(talkClip.name);
			break;	
		case CardCategory.Emotion:
			var emotionClip = emotionConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory).clip;
			characterAnimator.SetTrigger(emotionClip.name);
			break;
		case CardCategory.Action:
			var actionClip = actionConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory).clip;
			characterAnimator.SetTrigger(actionClip.name);
			break;
		default:
			characterAnimator.SetTrigger(fallbackClip.name);
			break;
		}
	}
}

[System.Serializable]
public class TalkAnimConfig
{
	public TalkCategory subcategory;
	public AnimationClip clip;
}

[System.Serializable]
public class ActionAnimConfig
{
	public ActionCategory subcategory;
	public AnimationClip clip;
}

[System.Serializable]
public class EmotionAnimConfig
{
	public EmotionCategory subcategory;
	public AnimationClip clip;
}