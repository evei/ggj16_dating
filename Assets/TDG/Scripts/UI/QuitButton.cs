using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Button))]
public class QuitButton : MonoBehaviour 
{
	void Start () 
	{
		GetComponent<Button>().onClick.AddListener(HandleQuitButton);
	}

	void HandleQuitButton ()
	{
		SceneManager.LoadScene(MainGameController.SCENE_LOBBY);
	}
}
