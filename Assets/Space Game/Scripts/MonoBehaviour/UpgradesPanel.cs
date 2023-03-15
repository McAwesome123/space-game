using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesPanel : MonoBehaviour
{
	public const float toolTipFade = 0.5f;

	public Button upgradeEngine;
	public Button upgradeArmor;
	public Button upgradeShield;
	public Button upgradeLaser;
	public Button upgradeKinetic;
	public Button upgradeMissile;

	public TMP_Text upgradeEngineLevels;
	public TMP_Text upgradeArmorLevels;
	public TMP_Text upgradeShieldLevels;
	public TMP_Text upgradeLaserLevels;
	public TMP_Text upgradeKineticLevels;
	public TMP_Text upgradeMissileLevels;

	public TMP_Text upgradeEngineCost;
	public TMP_Text upgradeArmorCost;
	public TMP_Text upgradeShieldCost;
	public TMP_Text upgradeLaserCost;
	public TMP_Text upgradeKineticCost;
	public TMP_Text upgradeMissileCost;

	public TMP_Text availableResourcesText;

	public Global global;

	public Entity playerShip;

	// Start is called before the first frame update
	void Awake()
	{
		upgradeEngine = GameObject.Find("Engine Upgrade").GetComponent<Button>();
		upgradeArmor = GameObject.Find("Armor Upgrade").GetComponent<Button>();
		upgradeShield = GameObject.Find("Shield Upgrade").GetComponent<Button>();
		upgradeLaser = GameObject.Find("Laser Upgrade").GetComponent<Button>();
		upgradeKinetic = GameObject.Find("Kinetic Upgrade").GetComponent<Button>();
		upgradeMissile = GameObject.Find("Missile Upgrade").GetComponent<Button>();

		upgradeEngineLevels = GameObject.Find("Engine Levels Text").GetComponent<TMP_Text>();
		upgradeArmorLevels = GameObject.Find("Armor Levels Text").GetComponent<TMP_Text>();
		upgradeShieldLevels = GameObject.Find("Shield Levels Text").GetComponent<TMP_Text>();
		upgradeLaserLevels = GameObject.Find("Laser Levels Text").GetComponent<TMP_Text>();
		upgradeKineticLevels = GameObject.Find("Kinetic Levels Text").GetComponent<TMP_Text>();
		upgradeMissileLevels = GameObject.Find("Missile Levels Text").GetComponent<TMP_Text>();

		upgradeEngineCost = GameObject.Find("Engine Cost Text").GetComponent<TMP_Text>();
		upgradeArmorCost = GameObject.Find("Armor Cost Text").GetComponent<TMP_Text>();
		upgradeShieldCost = GameObject.Find("Shield Cost Text").GetComponent<TMP_Text>();
		upgradeLaserCost = GameObject.Find("Laser Cost Text").GetComponent<TMP_Text>();
		upgradeKineticCost = GameObject.Find("Kinetic Cost Text").GetComponent<TMP_Text>();
		upgradeMissileCost = GameObject.Find("Missile Cost Text").GetComponent<TMP_Text>();

		availableResourcesText = GameObject.Find("Available Resources Text").GetComponent<TMP_Text>();

		global = GameObject.Find("Global").GetComponent<Global>();

		NativeArray<Entity> entityArray = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerMoveObj), typeof(ShipStats)).ToEntityArray(Allocator.Temp);
		if (entityArray.Length > 0)
		{
			playerShip = entityArray[0];
		}

		upgradeEngine.onClick.AddListener(UpgradeEngineClick);
		upgradeArmor.onClick.AddListener(UpgradeArmorClick);
		upgradeShield.onClick.AddListener(UpgradeShieldClick);
		upgradeLaser.onClick.AddListener(UpgradeLaserClick);
		upgradeKinetic.onClick.AddListener(UpgradeKineticClick);
		upgradeMissile.onClick.AddListener(UpgradeMissileClick);
	}

	private void Update()
	{
		upgradeEngineLevels.text = string.Format("Engine\nLevel:\n{0}", global.playerEngineUpgrades);
		upgradeArmorLevels.text = string.Format("Armor\nLevel:\n{0}", global.playerArmorUpgrades);
		upgradeShieldLevels.text = string.Format("Shield\nLevel:\n{0}", global.playerShieldUpgrades);
		upgradeLaserLevels.text = string.Format("Laser\nLevel:\n{0}", global.playerLaserUpgrades);
		upgradeKineticLevels.text = string.Format("Kinetic\nLevel:\n{0}", global.playerKineticUpgrades);
		upgradeMissileLevels.text = string.Format("Missile\nLevel:\n{0}", global.playerMissileUpgrades);

		upgradeEngineCost.text = string.Format("Cost:\n{0}", global.playerEngineUpgrades > 0 ? math.max(global.playerEngineUpgradeCost, BaseShipStats.shipUpgradeCostMin) : "N/A");
		upgradeArmorCost.text = string.Format("Cost:\n{0}", global.playerArmorUpgrades > 0 ? math.max(global.playerArmorUpgradeCost, BaseShipStats.shipUpgradeCostMin) : "N/A");
		upgradeShieldCost.text = string.Format("Cost:\n{0}", global.playerShieldUpgrades > 0 ? math.max(global.playerShieldUpgradeCost, BaseShipStats.shipUpgradeCostMin) : "N/A");
		upgradeLaserCost.text = string.Format("Cost:\n{0}", global.playerLaserUpgrades > 0 ? math.max(global.playerLaserUpgradeCost, BaseShipStats.shipUpgradeCostMin) : "N/A");
		upgradeKineticCost.text = string.Format("Cost:\n{0}", global.playerKineticUpgrades > 0 ? math.max(global.playerKineticUpgradeCost, BaseShipStats.shipUpgradeCostMin) : "N/A");
		upgradeMissileCost.text = string.Format("Cost:\n{0}", global.playerMissileUpgrades > 0 ? math.max(global.playerMissileUpgradeCost, BaseShipStats.shipUpgradeCostMin) : "N/A");

		upgradeEngine.interactable = global.playerResources >= global.playerEngineUpgradeCost && global.playerEngineUpgrades > 0;
		upgradeArmor.interactable = global.playerResources >= global.playerArmorUpgradeCost && global.playerArmorUpgrades > 0;
		upgradeShield.interactable = global.playerResources >= global.playerShieldUpgradeCost && global.playerShieldUpgrades > 0;
		upgradeLaser.interactable = global.playerResources >= global.playerLaserUpgradeCost && global.playerLaserUpgrades > 0;
		upgradeKinetic.interactable = global.playerResources >= global.playerKineticUpgradeCost && global.playerKineticUpgrades > 0;
		upgradeMissile.interactable = global.playerResources >= global.playerMissileUpgradeCost && global.playerMissileUpgrades > 0;

		availableResourcesText.text = string.Format("Available Resources: {0}", global.playerResources);
	}

	void UpgradeEngineClick()
	{
		global.playerResources -= (int)math.round(math.max(global.playerEngineUpgradeCost, BaseShipStats.shipUpgradeCostMin));
		if (global.playerEngineUpgrades == math.max(global.playerEngineUpgrades, math.max(global.playerArmorUpgrades, math.max(global.playerShieldUpgrades, math.max(global.playerLaserUpgrades, math.max(global.playerKineticUpgrades, global.playerMissileUpgrades))))))
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		else
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerEngineUpgradeCost -= (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		global.playerEngineUpgrades++;
		global.recalculateStats = true;
	}

	void UpgradeArmorClick()
	{
		global.playerResources -= (int)math.round(math.max(global.playerArmorUpgradeCost, BaseShipStats.shipUpgradeCostMin));
		if (global.playerArmorUpgrades == math.max(global.playerEngineUpgrades, math.max(global.playerArmorUpgrades, math.max(global.playerShieldUpgrades, math.max(global.playerLaserUpgrades, math.max(global.playerKineticUpgrades, global.playerMissileUpgrades))))))
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		else
		{
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerArmorUpgradeCost -= (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		global.playerArmorUpgrades++;
		global.recalculateStats = true;
	}

	void UpgradeShieldClick()
	{
		global.playerResources -= (int)math.round(math.max(global.playerShieldUpgradeCost, BaseShipStats.shipUpgradeCostMin));
		if (global.playerShieldUpgrades == math.max(global.playerEngineUpgrades, math.max(global.playerArmorUpgrades, math.max(global.playerShieldUpgrades, math.max(global.playerLaserUpgrades, math.max(global.playerKineticUpgrades, global.playerMissileUpgrades))))))
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		else
		{
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerShieldUpgradeCost -= (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		global.playerShieldUpgrades++;
		global.recalculateStats = true;
	}

	void UpgradeLaserClick()
	{
		global.playerResources -= (int)math.round(math.max(global.playerLaserUpgradeCost, BaseShipStats.shipUpgradeCostMin));
		if (global.playerLaserUpgrades == math.max(global.playerEngineUpgrades, math.max(global.playerArmorUpgrades, math.max(global.playerShieldUpgrades, math.max(global.playerLaserUpgrades, math.max(global.playerKineticUpgrades, global.playerMissileUpgrades))))))
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		else
		{
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerLaserUpgradeCost -= (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		global.playerLaserUpgrades++;
		global.recalculateStats = true;
	}

	void UpgradeKineticClick()
	{
		global.playerResources -= (int)math.round(math.max(global.playerKineticUpgradeCost, BaseShipStats.shipUpgradeCostMin));
		if (global.playerKineticUpgrades == math.max(global.playerEngineUpgrades, math.max(global.playerArmorUpgrades, math.max(global.playerShieldUpgrades, math.max(global.playerLaserUpgrades, math.max(global.playerKineticUpgrades, global.playerMissileUpgrades))))))
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		else
		{
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerKineticUpgradeCost -= (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		global.playerKineticUpgrades++;
		global.recalculateStats = true;
	}

	void UpgradeMissileClick()
	{
		global.playerResources -= (int)math.round(math.max(global.playerMissileUpgradeCost, BaseShipStats.shipUpgradeCostMin));
		if (global.playerMissileUpgrades == math.max(global.playerEngineUpgrades, math.max(global.playerArmorUpgrades, math.max(global.playerShieldUpgrades, math.max(global.playerLaserUpgrades, math.max(global.playerKineticUpgrades, global.playerMissileUpgrades))))))
		{
			global.playerEngineUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerArmorUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerShieldUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerLaserUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerKineticUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
		}
		else
		{
			global.playerMissileUpgradeCost += (int)math.round(BaseShipStats.shipUpgradeCostPerLevelAdd);
			global.playerMissileUpgradeCost -= (int)math.round(BaseShipStats.shipUpgradeCostPerOtherLevelAdd);
		}
		global.playerMissileUpgrades++;
		global.recalculateStats = true;
	}
}
