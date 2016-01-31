using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour 
{
	public Text loadingLabel;
	IEnumerator Start ()
	{
		if (GameManager.Instance.FirstPhase) {
			yield return GameManager.Instance.StartDate();
		}
		else {
			loadingLabel.text = "Loading...";
		}
		Debug.Log(GameManager.Instance.CurrentState);
		if (GameManager.Instance.CurrentState == GameManager.GameState.Decision) {
			if (DateHasnRatedYet()) {
				loadingLabel.text = "Your date hasn't rated yet...";
				while (DateHasnRatedYet()) {
					yield return null;
				}				
			}
			SceneManager.LoadScene(MainGameController.SCENE_THE_DECISION);
		}
		else {
			SceneManager.LoadScene(MainGameController.SCENE_DATING_TABLE);
		}
	}

	bool DateHasnRatedYet ()
	{
		return GameManager.Instance.Player.ratesReceived.Count < GameManager.Instance.maxPhasesNumber;
	}
}
