using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarpPanel : MonoBehaviour
{
	public Global global;
	public Button[] starButtons = new Button[36];

	private bool initializeStars = true;

	// Start is called before the first frame update
	void Start()
	{
		if (!initializeStars)
		{
			return;
		}

		global = GameObject.Find("Global").GetComponent<Global>();

		if (global.numStars < 2)
		{
			global.GenerateSector(1);
		}

		starButtons = GetComponentsInChildren<Button>();

		for (int x = 0; x < starButtons.Length; x++)
		{
			if (x < global.numStars && x < global.stars.Length && x < starButtons.Length)
			{
				int index = x;
				starButtons[index].GetComponent<WarpStarObj>().star = global.stars[index];
				starButtons[index].transform.localPosition = new Vector2(global.stars[index].x, global.stars[index].y);
				starButtons[index].transform.localScale = new Vector2(global.stars[index].scale / 100f, global.stars[index].scale / 100f);
				starButtons[index].onClick.AddListener(delegate { Warp(starButtons[index].GetComponent<WarpStarObj>().star); });
			}
			else
			{
				starButtons[x].gameObject.SetActive(false);
			}
		}

		initializeStars = false;
	}

	void Warp(Star star)
	{
		global.currentStar.alreadyVisited = true;
		global.currentStar = star;
		global.recalculateStats = true;
		global.initializePlayerStats = true;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	void WarpSector()
	{
		global.ticksPassedInSector = 0;
		global.currentStar.alreadyVisited = true;
		global.GenerateSector(global.currentStar.nextSectorSeed);
		global.recalculateStats = true;
		global.initializePlayerStats = true;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}
}
