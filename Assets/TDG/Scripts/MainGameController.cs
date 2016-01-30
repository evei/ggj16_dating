using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour 
{
	public const string SCENE_LOBBY = "Lobby";
	public const string SCENE_GENDER_CHOICE = "GenderChoice";
	public const string SCENE_PICKDECK = "PickDeck";
	public const string SCENE_LOADING = "Loading";
	public const string SCENE_DATING_TABLE = "DatingTable";

	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}

	void Start ()
	{
		SceneManager.LoadScene(SCENE_LOBBY);
	}

}
