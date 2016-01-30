using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DatingTableUI : MonoBehaviour 
{
	public Button quitButton;

	void Awake ()
	{
		quitButton.onClick.AddListener(HandleQuitButton);
	}

	void HandleQuitButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_LOBBY);
	}
}
