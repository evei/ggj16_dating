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
		gameObject.SetActive(true);
	}

	void HandleGoodButton ()
	{
		// TODO Add score
		ContinueGame();
	}

	void HandleBadButton ()
	{
		ContinueGame();
	}

	void ContinueGame ()
	{
		if (GameManager.phase + 1 >= GameManager.maxPhasesNumber) { // End Game
			// TODO Display game end result.
		} else {
			GameManager.phase++;
			SceneManager.LoadScene(MainGameController.SCENE_PICKDECK);
		}
	}
}
