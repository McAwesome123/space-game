using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
	Button playButton;
	Button exitButton;
	Global global;
	Entity loadingScene;

	// Start is called before the first frame update
	void Start()
	{
		playButton = GameObject.Find("Play Button").GetComponent<Button>();
		exitButton = GameObject.Find("Exit Button").GetComponent<Button>();
		global = GameObject.Find("Global").GetComponent<Global>();

		playButton.onClick.AddListener(PlayButton);
		exitButton.onClick.AddListener(ExitButton);
	}

	void PlayButton()
	{
		global.Invoke("Start", 0);
		Unity.Mathematics.Random random = new(1);
		global.GenerateSector(random.NextUInt() + 1);
		global.recalculateStats = true;
		global.initializePlayerStats = true;

		SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
	}

	void ExitButton()
	{
		Application.Quit();
	}
}
