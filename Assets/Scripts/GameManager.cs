
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	public static GameManager _gameManager;
	public bool LevelComplete;
	public GameObject[] Levels;
	public bool playable;

	public GameObject MenuUI;
	public GameObject CompleteUI;

	void Start()
	{
		MenuUI.SetActive(true);
		CompleteUI.SetActive(false);
	}

	public void NextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void PlaYButton()
	{
		MenuUI.SetActive(false);
		Instantiate(Levels[0], transform.position, Quaternion.identity);
		playable = true;
	}
}
