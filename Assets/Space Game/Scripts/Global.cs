using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Global : MonoBehaviour
{
	public const int tickRate = 100;

	public bool recalculateStats = false;

	public int playerResources = 50;
	public int playerSectorsPassed = 0;

	public int playerEngineUpgrades = 1;
	public int playerArmorUpgrades = 1;
	public int playerShieldUpgrades = 1;
	public int playerLaserUpgrades = 1;
	public int playerKineticUpgrades = 1;
	public int playerMissileUpgrades = 1;

	public int playerEngineUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
	public int playerArmorUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
	public int playerShieldUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
	public int playerLaserUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
	public int playerKineticUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
	public int playerMissileUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);

	private void Start()
	{
		recalculateStats = false;

		playerResources = 50;
		playerSectorsPassed = 0;

		playerEngineUpgrades = 1;
		playerArmorUpgrades = 1;
		playerShieldUpgrades = 1;
		playerLaserUpgrades = 1;
		playerKineticUpgrades = 1;
		playerMissileUpgrades = 1;

		playerEngineUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerArmorUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerShieldUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerLaserUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerKineticUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerMissileUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);

	}

	// This is a slightly modified version of public static Vector3 operator *(Quaternion rotation, Vector3 point)
	public static double3 ApplyRotation(quaternion rotation, double3 point)
	{
		double num = rotation.value.x * 2f;
		double num2 = rotation.value.y * 2f;
		double num3 = rotation.value.z * 2f;
		double num4 = rotation.value.x * num;
		double num5 = rotation.value.y * num2;
		double num6 = rotation.value.z * num3;
		double num7 = rotation.value.x * num2;
		double num8 = rotation.value.x * num3;
		double num9 = rotation.value.y * num3;
		double num10 = rotation.value.w * num;
		double num11 = rotation.value.w * num2;
		double num12 = rotation.value.w * num3;
		double3 result = default;
		result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
		result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
		result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
		return result;
	}
}
