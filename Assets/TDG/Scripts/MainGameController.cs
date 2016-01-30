using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour 
{
	const string SCENE_LOBBY = "Lobby";
	const string SCENE_GENDER_CHOICE = "GenderChoice";
	const string SCENE_PICKDECK = "PickDeck";
	const string SCENE_LOADING = "Loading";
	const string SCENE_DATING_TABLE = "DatingTable";

	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}

	void Start ()
	{
		SceneManager.LoadScene(SCENE_LOBBY);
	}

}
