using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ShipStats : IComponentData
{
	public float maxSpeed;
	public float acceleration;
	public float translation;
	public float rotationSpeedDeg;
	public float rotationSpeedRad;

	public float maxHullPoints;
	public float maxArmorAmount;
	public float maxShieldAmount;
	public float currentHullPoints;
	public float currentArmorAmount;
	public float currentShieldAmount;

	public int shipEngineUpgrades;
	public int shipArmorUpgrades;
	public int shipShieldUpgrades;
	public int shipLaserUpgrades;
	public int shipKineticUpgrades;
	public int shipMissileUpgrades;

	public float shipLaserDamage;
	public float shipLaserCooldown;
	public float shipLaserShotSpeed;
	public float shipKineticDamage;
	public float shipKineticCooldown;
	public float shipKineticShotSpeed;
	public float shipMissileDamage;
	public float shipMissileCooldown;
	public float shipMissileShotSpeed;
}
public class ShipStatsAuthoring : MonoBehaviour
{
	public float maxSpeed = BaseShipStats.baseMaxSpeed;
	public float acceleration = BaseShipStats.baseAcceleration;
	public float translation = BaseShipStats.baseTranslationMult;
	public float rotationSpeedDeg = BaseShipStats.baseRotationSpeedDeg;
	public float rotationSpeedRad = BaseShipStats.baseRotationSpeedRad;

	public float maxHullPoints = BaseShipStats.baseShipHull;
	public float maxArmorAmount = BaseShipStats.baseShipArmor;
	public float maxShieldAmount = BaseShipStats.baseShipShield;
	public float currentHullPoints = BaseShipStats.baseShipHull;
	public float currentArmorAmount = BaseShipStats.baseShipArmor;
	public float currentShieldAmount = BaseShipStats.baseShipHull;

	public int shipEngineUpgrades = BaseShipStats.baseShipEngineUpgrades;
	public int shipArmorUpgrades = BaseShipStats.baseShipArmorUpgrades;
	public int shipShieldUpgrades = BaseShipStats.baseShipShieldUpgrades;
	public int shipLaserUpgrades = BaseShipStats.baseShipLaserUpgrades;
	public int shipKineticUpgrades = BaseShipStats.baseShipKineticUpgrades;
	public int shipMissileUpgrades = BaseShipStats.baseShipMissileUpgrades;

	public float shipLaserDamage = BaseShipStats.baseShipLaserDamage;
	public float shipLaserCooldown = BaseShipStats.baseShipLaserCooldown;
	public float shipLaserShotSpeed = BaseShipStats.baseShipLaserShotSpeed;
	public float shipKineticDamage = BaseShipStats.baseShipKineticDamage;
	public float shipKineticCooldown = BaseShipStats.baseShipKineticCooldown;
	public float shipKineticShotSpeed = BaseShipStats.baseShipKineticShotSpeed;
	public float shipMissileDamage = BaseShipStats.baseShipMissileDamage;
	public float shipMissileCooldown = BaseShipStats.baseShipMissileCooldown;
	public float shipMissileShotSpeed = BaseShipStats.baseShipMissileShotSpeed;
}

public class ShipAccelerationBaker : Baker<ShipStatsAuthoring>
{
	public override void Bake(ShipStatsAuthoring authoring)
	{
		AddComponent(new ShipStats
		{
			maxSpeed = authoring.maxSpeed,
			acceleration = authoring.acceleration,
			translation = authoring.translation,
			rotationSpeedDeg = authoring.rotationSpeedDeg,
			rotationSpeedRad = authoring.rotationSpeedRad,

			maxHullPoints = authoring.maxHullPoints,
			maxArmorAmount = authoring.maxArmorAmount,
			maxShieldAmount = authoring.maxShieldAmount,
			currentHullPoints = authoring.currentHullPoints,
			currentArmorAmount = authoring.currentArmorAmount,
			currentShieldAmount = authoring.currentShieldAmount,

			shipLaserDamage = authoring.shipLaserDamage,
			shipLaserCooldown = authoring.shipLaserCooldown,
			shipLaserShotSpeed = authoring.shipLaserShotSpeed,
			shipKineticDamage = authoring.shipKineticDamage,
			shipKineticCooldown = authoring.shipKineticCooldown,
			shipKineticShotSpeed = authoring.shipKineticShotSpeed,
			shipMissileDamage = authoring.shipMissileDamage,
			shipMissileCooldown = authoring.shipMissileCooldown,
			shipMissileShotSpeed = authoring.shipMissileShotSpeed,
		});
	}
}

