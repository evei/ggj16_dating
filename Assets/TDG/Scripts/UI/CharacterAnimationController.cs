using UnityEngine;
using System.Collections.Generic;

public class CharacterAnimationController : MonoBehaviour 
{
	public AnimationClip fallbackClip;

	public List<TalkAnimConfig> talkConfig;
	public List<ActionAnimConfig> actionConfig;
	public List<EmotionAnimConfig> emotionConfig;
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