using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpButton : MonoBehaviour
{
	public Button warpButton;
	public GameObject warpPanel;

	// Start is called before the first frame update
	void Start()
	{
		warpButton = GetComponent<Button>();
		warpButton.onClick.AddListener(TaskOnClick);
		foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])   // GameObject.Find only finds active game objects
		{
			if (go.name == "Warp Panel")
			{
				warpPanel = go;
				break;
			}
		}
		warpPanel.SetActive(false);
	}

	void TaskOnClick()
	{
		warpPanel.SetActive(!warpPanel.activeSelf);
	}
}
