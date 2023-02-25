using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShipRecalculateStatsGlobal : IComponentData
{
	public Global global;

	public ShipRecalculateStatsGlobal()
	{
		global = GameObject.Find("Global").GetComponent<Global>();
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class ShipRecalculateStatsSystem : SystemBase
{
	readonly ShipRecalculateStatsGlobal global = new();

	protected override void OnUpdate()
	{
		if (global.global.recalculateStats)
		{
			Entities.ForEach((ref ShipStats stats, in PlayerMoveObj player) =>
			{
				stats.shipEngineUpgrades = global.global.playerEngineUpgrades;
				stats.shipArmorUpgrades = global.global.playerArmorUpgrades;
				stats.shipShieldUpgrades = global.global.playerShieldUpgrades;
				stats.shipLaserUpgrades = global.global.playerLaserUpgrades;
				stats.shipKineticUpgrades = global.global.playerKineticUpgrades;
				stats.shipMissileUpgrades = global.global.playerMissileUpgrades;
			}).WithoutBurst().Run();

			Entities.ForEach((ref ShipStats stats) =>
			{
				if (stats.shipEngineUpgrades > 0)
				{
					stats.maxSpeed = BaseShipStats.baseMaxSpeed * (1 + BaseShipStats.shipMaxSpeedPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipMaxSpeedPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipMaxSpeedPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)));
					stats.acceleration = BaseShipStats.baseAcceleration * (1 + BaseShipStats.shipAccelerationPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipAccelerationPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipAccelerationPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)));
					stats.translation = BaseShipStats.baseTranslationMult * (1 + BaseShipStats.shipTranslationPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipTranslationPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipTranslationPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)));
					stats.rotationSpeedDeg = BaseShipStats.baseRotationSpeedDeg * (1 + BaseShipStats.shipRotationPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipRotationPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipRotationPerUpgradeLevelHyperbloicFactor * (stats.shipEngineUpgrades - 1)));
					stats.rotationSpeedRad = math.radians(stats.rotationSpeedDeg);
				}
				else
				{
					stats.maxSpeed = 0;
					stats.acceleration = 0;
					stats.translation = 0;
					stats.rotationSpeedDeg = 0;
					stats.rotationSpeedRad = 0;
				}

				if (stats.shipArmorUpgrades > 0)
				{
					float armorChange = stats.maxArmorAmount;
					stats.maxArmorAmount = (BaseShipStats.baseShipArmor + BaseShipStats.shipArmorPerUpgradeLevelAdd * (stats.shipArmorUpgrades - 1)) * math.pow(BaseShipStats.shipArmorPerUpgradeLevelMult, stats.shipArmorUpgrades - 1);
					if (armorChange == 0)
					{
						stats.currentArmorAmount = stats.maxArmorAmount;
					}
					else
					{
						armorChange = stats.maxArmorAmount / armorChange;
						stats.currentArmorAmount *= armorChange;
					}
				}
				else
				{
					stats.maxArmorAmount = 0;
					stats.currentArmorAmount = 0;
				}

				if (stats.shipShieldUpgrades > 0)
				{
					float shieldChange = stats.maxShieldAmount;
					stats.maxShieldAmount = (BaseShipStats.baseShipShield + BaseShipStats.shipShieldPerUpgradeLevelAdd * (stats.shipShieldUpgrades - 1)) * math.pow(BaseShipStats.shipShieldPerUpgradeLevelMult, stats.shipShieldUpgrades - 1);
					if (stats.currentShieldAmount == 0)
					{
						stats.currentShieldAmount = stats.maxShieldAmount;
					}
					else
					{
						shieldChange = stats.maxShieldAmount / shieldChange;
						stats.currentShieldAmount *= shieldChange;
					}
				}
				else
				{
					stats.maxShieldAmount = 0;
					stats.currentShieldAmount = 0;
				}

				stats.shipLaserDamage = (BaseShipStats.baseShipLaserDamage + BaseShipStats.shipLaserDamagePerUpgradeLevelAdd * (stats.shipLaserUpgrades - 1)) * math.pow(BaseShipStats.shipLaserDamagePerUpgradeLevelMult, stats.shipLaserUpgrades - 1);
				stats.shipLaserCooldown = BaseShipStats.baseShipLaserCooldown * (1 + BaseShipStats.shipLaserFireCooldownPerUpgradeLevelHyperbolicAdd * (BaseShipStats.shipLaserFireCooldownPerUpgradeLevelHyperbolicFactor * (stats.shipLaserUpgrades - 1)) / (1 + BaseShipStats.shipLaserFireCooldownPerUpgradeLevelHyperbolicFactor * (stats.shipLaserUpgrades - 1)));
				stats.shipLaserShotSpeed = BaseShipStats.baseShipLaserShotSpeed * (1 + BaseShipStats.shipLaserShotSpeedPerUpgradeLevelHyperbolicAdd * (BaseShipStats.shipLaserShotSpeedPerUpgradeLevelHyperbolicFactor * (stats.shipLaserUpgrades - 1)) / (1 + BaseShipStats.shipLaserShotSpeedPerUpgradeLevelHyperbolicFactor * (stats.shipLaserUpgrades - 1)));

				stats.shipKineticDamage = (BaseShipStats.baseShipKineticDamage + BaseShipStats.shipKineticDamagePerUpgradeLevelAdd * (stats.shipKineticUpgrades - 1)) * math.pow(BaseShipStats.shipKineticDamagePerUpgradeLevelMult, stats.shipKineticUpgrades - 1);
				stats.shipKineticCooldown = BaseShipStats.baseShipKineticCooldown * (1 + BaseShipStats.shipKineticFireCooldownPerUpgradeLevelHyperbolicAdd * (BaseShipStats.shipKineticFireCooldownPerUpgradeLevelHyperbolicFactor * (stats.shipKineticUpgrades - 1)) / (1 + BaseShipStats.shipKineticFireCooldownPerUpgradeLevelHyperbolicFactor * (stats.shipKineticUpgrades - 1)));
				stats.shipKineticShotSpeed = BaseShipStats.baseShipKineticShotSpeed * (1 + BaseShipStats.shipKineticShotSpeedPerUpgradeLevelHyperbolicAdd * (BaseShipStats.shipKineticShotSpeedPerUpgradeLevelHyperbolicFactor * (stats.shipKineticUpgrades - 1)) / (1 + BaseShipStats.shipKineticShotSpeedPerUpgradeLevelHyperbolicFactor * (stats.shipKineticUpgrades - 1)));

				stats.shipMissileDamage = (BaseShipStats.baseShipMissileDamage + BaseShipStats.shipMissileDamagePerUpgradeLevelAdd * (stats.shipMissileUpgrades - 1)) * math.pow(BaseShipStats.shipMissileDamagePerUpgradeLevelMult, stats.shipMissileUpgrades - 1);
				stats.shipMissileCooldown = BaseShipStats.baseShipMissileCooldown * (1 + BaseShipStats.shipMissileFireCooldownPerUpgradeLevelHyperbolicAdd * (BaseShipStats.shipMissileFireCooldownPerUpgradeLevelHyperbolicFactor * (stats.shipMissileUpgrades - 1)) / (1 + BaseShipStats.shipMissileFireCooldownPerUpgradeLevelHyperbolicFactor * (stats.shipMissileUpgrades - 1)));
				stats.shipMissileShotSpeed = BaseShipStats.baseShipMissileShotSpeed * (1 + BaseShipStats.shipMissileShotSpeedPerUpgradeLevelHyperbolicAdd * (BaseShipStats.shipMissileShotSpeedPerUpgradeLevelHyperbolicFactor * (stats.shipMissileUpgrades - 1)) / (1 + BaseShipStats.shipMissileShotSpeedPerUpgradeLevelHyperbolicFactor * (stats.shipMissileUpgrades - 1)));
			}).ScheduleParallel();

			global.global.recalculateStats = false;
		}
	}
}
