using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingUI : MonoBehaviour 
{
	IEnumerator Start ()
	{
		if (GameManager.Instance.FirstPhase) {
			yield return GameManager.Instance.StartDate();			
		}
		LoadDatingTable();
	}

	void LoadDatingTable ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_DATING_TABLE);
	}
}
