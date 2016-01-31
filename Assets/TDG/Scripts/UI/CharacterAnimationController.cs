using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class CharacterAnimationController : MonoBehaviour 
{
	public Animator characterAnimator;
	public AnimationClip fallbackClip;
	public AnimationClip passoutClip;
	public AnimationClip fleeClip;

	public List<TalkAnimConfig> talkConfig;
	public List<ActionAnimConfig> actionConfig;
	public List<EmotionAnimConfig> emotionConfig;

	public void PlayCard (Card card, Action onAnimationFinished)
	{
		switch (card.category) {
		case CardCategory.Talk:
			var talkClip = talkConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory).clip;
			StartCoroutine(PlayAnimation(talkClip.name, onAnimationFinished));
			break;	
		case CardCategory.Emotion:
			var emotionClip = emotionConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory).clip;
			StartCoroutine(PlayAnimation(emotionClip.name, onAnimationFinished));
			break;
		case CardCategory.Action:
			var actionClip = actionConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory).clip;
			StartCoroutine(PlayAnimation(actionClip.name, onAnimationFinished));
			break;
		default:
			StartCoroutine(PlayAnimation(fallbackClip.name, onAnimationFinished));
			break;
		}
	}

	IEnumerator PlayAnimation (string animationName, Action onAnimationFinished) 
	{
		characterAnimator.SetTrigger(animationName);

		while (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{
			Debug.Log("Playing animation " + animationName );
			yield return null;
		}

		onAnimationFinished();
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