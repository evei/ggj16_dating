using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickDeckUI : MonoBehaviour 
{
	public Button backButton;
	public Button readyButton;

	void Awake ()
	{
		backButton.onClick.AddListener(HandleBackButton);
		readyButton.onClick.AddListener(HandleReadyButton);
	}

	void HandleBackButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_GENDER_CHOICE);
	}

	void HandleReadyButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_LOADING);
	}
}
