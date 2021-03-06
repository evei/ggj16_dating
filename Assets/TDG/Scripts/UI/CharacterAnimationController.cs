﻿using UnityEngine;
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
	public AnimationClip drinkBoozeClip;

	public List<TalkAnimConfig> talkConfig;
	public List<ActionAnimConfig> actionConfig;
	public List<EmotionAnimConfig> emotionConfig;

	public void PlayCard (Card card, Action onAnimationFinished)
	{
		StopAllCoroutines();

		switch (card.category) {

		case CardCategory.Talk:
			var tConfig = talkConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory);
			if (tConfig != null) {
				var talkClip = tConfig.clip;
				StartCoroutine(PlayAnimation(talkClip.name, onAnimationFinished));
			} else {
				Debug.LogWarning("Animation config not found: " + card.category + " - " + card.subCategory);
				PlayFalbackAnimation(onAnimationFinished);
			}
			break;	

		case CardCategory.Emotion:
			var eConfig = emotionConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory);
			if (eConfig != null) {
				var emotionClip = eConfig.clip;
				StartCoroutine(PlayAnimation(emotionClip.name, onAnimationFinished));
			} else {
				Debug.LogWarning("Animation config not found: " + card.category + " - " + card.subCategory);
				PlayFalbackAnimation(onAnimationFinished);
			}
			break;

		case CardCategory.Action:
			var aConfig = actionConfig.FirstOrDefault(c => (int)c.subcategory == card.subCategory);
			if (aConfig != null) {
				var actionClip = aConfig.clip;
				StartCoroutine(PlayAnimation(actionClip.name, onAnimationFinished));
			} else {
				Debug.LogWarning("Animation config not found: " + card.category + " - " + card.subCategory);
				PlayFalbackAnimation(onAnimationFinished);
			}
			break;

		default:
			PlayFalbackAnimation(onAnimationFinished);
			break;
		}
	}

	IEnumerator PlayAnimation (string animationName, Action onAnimationFinished) 
	{
		Debug.Log(">>>> Playing animation " + animationName);
		characterAnimator.SetTrigger(animationName);

		yield return null; // Wait one frame

		while (characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
		{
			Debug.Log(">>>>> Waiting to finish Playing animation " + animationName );
			yield return null;
		}

		if (onAnimationFinished != null) {
			onAnimationFinished();
		}
	}

	public void PlayFalbackAnimation (Action onAnimationFinished)
	{
		StartCoroutine(PlayAnimation(fallbackClip.name, onAnimationFinished));
	}

	public void PlayFleeAnimation (Action onAnimationFinished)
	{
		StartCoroutine(PlayAnimation(fleeClip.name, onAnimationFinished));
	}

	public void PlayPassoutAnimation (Action onAnimationFinished)
	{
		StartCoroutine(PlayAnimation(passoutClip.name, onAnimationFinished));
	}

	public void PlayDrinkBoozeAnimation (Action onAnimationFinished)
	{
		StartCoroutine(PlayAnimation(drinkBoozeClip.name, onAnimationFinished));
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