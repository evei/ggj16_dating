using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour 
{
	public Button startButton;

	void Awake ()
	{
		startButton.onClick.AddListener(HandleStartButton);
	}

	void HandleStartButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_GENDER_CHOICE);
	}
}
