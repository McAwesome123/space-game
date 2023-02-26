using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
	public Button upgradeButton;
	public GameObject upgradePanel;

	// Start is called before the first frame update
	void Start()
	{
		upgradeButton = GetComponent<Button>();
		upgradeButton.onClick.AddListener(TaskOnClick);
		foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])	// GameObject.Find only finds active game objects
		{
			if (go.name == "Upgrade Panel")
			{
				upgradePanel = go;
				break;
			}
		}
		upgradePanel.SetActive(false);
	}

	void TaskOnClick()
	{
		upgradePanel.SetActive(!upgradePanel.activeSelf);
	}
}
