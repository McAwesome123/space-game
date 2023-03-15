using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct BaseShipStats : IComponentData
{
	public const float speedOfLight = 3e8f;				// = 300000000
	// public const float maxShipSpeed = speedOfLight / 1e4f;	// = 30000
	// public const float maxShipAcceleration = maxShipSpeed / 30;	// = 1000
	// public const float maxTranslationMult = 0.25f;
	// public const float maxShipRotationSpeedDeg = 180;

	public const float baseMaxSpeed = speedOfLight / 1e6f;		// = 300
	public const float baseAcceleration = baseMaxSpeed / 30;	// = 10
	public const float baseTranslationMult = 0.1f;
	public const float baseRotationSpeedDeg = 24;
	public const float baseRotationSpeedRad = baseRotationSpeedDeg * Mathf.Deg2Rad; // Do not modify; Used to be "1 * (360 / (6 * 180 / Mathf.PI))", I have no clue where that came from

	public const float baseRotationSpeedChange = 0.02f;
	public const float baseAccelerationChange = 0.01f;

	public const float baseShipHull = 100.0f;
	public const float baseShipArmor = 10.0f;
	public const float baseShipShield = 10.0f;
	public const float shipArmorProtection = 1 - 0.01f;
	public const float shipShieldProtection = 1 - 0.01f;
	public const float shipShieldAbsorptionFactor = 1 / 3f;		// Shield takes this percent of absorbed damage
	public const float shipShieldRegenFastPercent = 0.01f / Global.tickRate;
	public const float shipShieldRegenSlowPercent = 0.001f / Global.tickRate;
	public const int shipShieldRegenFastDelay = 20 * Global.tickRate;

	public const float baseShipLaserDamage = 20.0f;
	public const float baseShipLaserCooldown = 15 * Global.tickRate;
	public const float baseShipLaserShotSpeed = speedOfLight;
	public const int baseShipLaserShotsPerFire = 1 * Global.tickRate;
	public const int baseShipLaserTicksPerShot = baseShipLaserShotsPerFire / (1 * Global.tickRate);
	public const float baseShipKineticDamage = 20.0f;
	public const float baseShipKineticCooldown = 15 * Global.tickRate;
	public const float baseShipKineticShotSpeed = baseMaxSpeed * 5f;
	public const int baseShipKineticShotsPerFire = 5;
	public const int baseShipKineticTicksPerShot = baseShipKineticShotsPerFire / (1 * Global.tickRate);
	public const float baseShipMissileDamage = 20.0f;	
	public const float baseShipMissileCooldown = 15 * Global.tickRate;
	public const float baseShipMissileShotSpeed = baseMaxSpeed * 2f;
	public const int baseShipMissileShotsPerFire = 1;
	public const int baseShipMissileTicksPerShot = baseShipMissileShotsPerFire / (1 * Global.tickRate);

	// Formula: (Base + Add * Levels) * Mult^Levels
	public const float shipArmorPerUpgradeLevelAdd = 10.0f;
	public const float shipArmorPerUpgradeLevelMult = 1 + 0.0f;
	public const float shipShieldPerUpgradeLevelAdd = 10.0f;
	public const float shipShieldPerUpgradeLevelMult = 1 + 0.0f;
	public const float shipLaserDamagePerUpgradeLevelAdd = 0.0f;
	public const float shipLaserDamagePerUpgradeLevelMult = 1 + 0.1f;
	public const float shipKineticDamagePerUpgradeLevelAdd = 0.0f;
	public const float shipKineticDamagePerUpgradeLevelMult = 1 + 0.1f;
	public const float shipMissileDamagePerUpgradeLevelAdd = 0.0f;
	public const float shipMissileDamagePerUpgradeLevelMult = 1 + 0.1f;

	// Formula: Base * (1 + HyperbolicAdd * (HyperbolicFactor * Levels) / (1 + HyperbolicFactor * Levels))
	// The following factors are chosen for a +10% increase for the first upgrade
	public const float shipMaxSpeedPerUpgradeLevelHyperbloicAdd = 99.0f;
	public const float shipMaxSpeedPerUpgradeLevelHyperbloicFactor = 1 / 989f;
	public const float shipAccelerationPerUpgradeLevelHyperbloicAdd = 99.0f;
	public const float shipAccelerationPerUpgradeLevelHyperbloicFactor = 1 / 989f;
	public const float shipTranslationPerUpgradeLevelHyperbloicAdd = 1.5f;
	public const float shipTranslationPerUpgradeLevelHyperbloicFactor = 1 / 14f;
	public const float shipRotationPerUpgradeLevelHyperbloicAdd = 6.5f;
	public const float shipRotationPerUpgradeLevelHyperbloicFactor = 1 / 64f;

	// The following cooldown factors are chosen for a 5% reduction for the first upgrade
	// The shot speed factors are chosen for a 10% increase for the first upgrade
	// Shot speed does not affect lasers; They always travel at the speed of light (which is basically instant travel time)
	public const float shipLaserFireCooldownPerUpgradeLevelHyperbolicAdd = -0.5f;
	public const float shipLaserFireCooldownPerUpgradeLevelHyperbolicFactor = 1 / 9f;
	public const float shipLaserShotSpeedPerUpgradeLevelHyperbolicAdd = 0.0f;
	public const float shipLaserShotSpeedPerUpgradeLevelHyperbolicFactor = 0.0f;
	public const float shipKineticFireCooldownPerUpgradeLevelHyperbolicAdd = -0.5f;
	public const float shipKineticFireCooldownPerUpgradeLevelHyperbolicFactor = 1 / 9f;
	public const float shipKineticShotSpeedPerUpgradeLevelHyperbolicAdd = 99.0f;
	public const float shipKineticShotSpeedPerUpgradeLevelHyperbolicFactor = 1 / 989f;
	public const float shipMissileFireCooldownPerUpgradeLevelHyperbolicAdd = -0.5f;
	public const float shipMissileFireCooldownPerUpgradeLevelHyperbolicFactor = 1 / 9f;
	public const float shipMissileShotSpeedPerUpgradeLevelHyperbolicAdd = 99.0f;
	public const float shipMissileShotSpeedPerUpgradeLevelHyperbolicFactor = 1 / 989f;

	public const float shipPlayerDamageArmorBase = 0.0f;
	public const float shipPlayerDamageTick = 0.1f;
	public const float shipPlayerArmorAdd = 1.0f;
	public const float shipPlayerArmorAddPerHullDamagePercent = 0.01f;			// Percent of current hull
	public const float shipPlayerDamageArmorDecayPerTick = 0.1f;
	public const float shipPlayerDamageArmorDecayPercentPerTick = 1 - 0.001f;
	public const float shipPlayerDestructionMinDamageMult = 10.0f;				// The player ship must take at least (current hull * this) amount of damage to be destroyed
	public const int shipPlayerDamageArmorDecayBuffer = 2 * Global.tickRate;

	public const float shipPlayerArmorAddPerHullDamagePercentDifficultyMod = 0.0005f;	// Added or subtracted based on difficulty

	public const int baseShipEngineUpgrades = 1;
	public const int baseShipArmorUpgrades = 1;
	public const int baseShipShieldUpgrades = 0;
	public const int baseShipLaserUpgrades = 0;
	public const int baseShipKineticUpgrades = 1;
	public const int baseShipMissileUpgrades = 0;

	public const float shipBaseUpgradeCost = 50.0f;
	public const float shipUpgradeCostMin = 5.0f;
	public const float shipUpgradeCostPerLevelAdd = 2.0f;
	public const float shipUpgradeCostPerOtherLevelAdd = -1.0f;
	public const float shipUpgradeCostPerSectorFactor = 1 - 0.2f;

	public const int shipStartingResources = 100;
}
