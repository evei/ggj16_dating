using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RatingPanel : MonoBehaviour 
{
	public Button goodButton;
	public Button badButton;

	GameManager GameManager { get { return GameManager.Instance; } }

	void Awake ()
	{
		goodButton.onClick.AddListener(HandleGoodButton);
		badButton.onClick.AddListener(HandleBadButton);
	}

	void Start ()
	{
		gameObject.SetActive(false);
	}

	public void Show ()
	{
		GameManager.PauseWebsocketListener(true);
		gameObject.SetActive(true);
	}

	void HandleGoodButton ()
	{
		GameManager.SendRatePhase(true);
		ContinueGame();
	}

	void HandleBadButton ()
	{
		GameManager.SendRatePhase(false);
		ContinueGame();
	}

	void ContinueGame ()
	{
		GameManager.phase++;
		if (GameManager.phase >= GameManager.maxPhasesNumber) { // End Game
			GameManager.CurrentState = GameManager.GameState.Decision;
			GameManager.PauseWebsocketListener(false);
			SceneManager.LoadScene(MainGameController.SCENE_LOADING);
		} else {
			GameManager.Player.startsPhase = !GameManager.Player.startsPhase;
			SceneManager.LoadScene(MainGameController.SCENE_PICKDECK);
		}
	}
}
