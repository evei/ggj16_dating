using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUI : MonoBehaviour 
{
	void Start ()
	{
		Invoke("LoadDatingTable", 2f);
	}

	void LoadDatingTable ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_DATING_TABLE);
	}
}
