using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenderChoiceUI : MonoBehaviour 
{
	public Button maleButton;
	public Button femaleButton;
	public Button backButton;

	void Awake ()
	{
		maleButton.onClick.AddListener(HandleMaleButton);
		femaleButton.onClick.AddListener(HandleFemaleButton);
		backButton.onClick.AddListener(HandleBackButton);
	}

	void HandleMaleButton ()
	{
		CreateNewPlayer(Player.Gender.Male);
		SceneManager.LoadScene(MainGameController.SCENE_PICKDECK);
	}

	void HandleFemaleButton ()
	{
		CreateNewPlayer(Player.Gender.Female);
		SceneManager.LoadScene(MainGameController.SCENE_PICKDECK);
	}

	void HandleBackButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_LOBBY);
	}

	void CreateNewPlayer(Player.Gender gender)
	{
		GameManager.Instance.Init(gender);
	}
}
