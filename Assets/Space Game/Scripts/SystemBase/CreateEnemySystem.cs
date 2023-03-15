using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CreateEnemySystemGlobal : IComponentData
{
	public Global global;

	public CreateEnemySystemGlobal()
	{
		global = GameObject.Find("Global").GetComponent<Global>();
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class CreateEnemySystem : SystemBase
{
	readonly CreateEnemySystemGlobal global = new();
	ulong ticksPassed;
	ulong ticksPassedInSector;
	Unity.Mathematics.Random random;

	protected override void OnUpdate()
	{
		ticksPassed = global.global.ticksPassed;
		ticksPassedInSector = global.global.ticksPassedInSector;
		random = new(global.global.currentStar.random.NextUInt());

		foreach ((RefRW<ShipStats> stats, RefRW<EnemyShipObj> enemy) in SystemAPI.Query<RefRW<ShipStats>, RefRW<EnemyShipObj>>())
		{
			if (!enemy.ValueRO.shipInitializeStats)
			{
				return;
			}

			long resources = BaseShipStats.shipStartingResources / 2;
			try
			{
				resources = (long)(resources * math.pow(2, ticksPassed / Global.tickRate / 3600.0) * (1 + 0.04 * ticksPassedInSector / Global.tickRate / 60));
				int modifier = random.NextInt(-100, 101);
				if (modifier < 0)
				{
					modifier = (int)(modifier * 1f / 3f);
				}
				else if (modifier > 0)
				{
					modifier = (int)(modifier * 0.2f);
				}
				resources = (long)(modifier * 0.01f);
			}
			catch (System.OverflowException)
			{
				resources = long.MaxValue;
			}

			int engineUpgradeCost = (int)BaseShipStats.shipBaseUpgradeCost;
			int armorUpgradeCost = (int)BaseShipStats.shipBaseUpgradeCost;
			int shieldUpgradeCost = (int)BaseShipStats.shipBaseUpgradeCost;
			int laserUpgradeCost = (int)BaseShipStats.shipBaseUpgradeCost;
			int kineticUpgradeCost = (int)BaseShipStats.shipBaseUpgradeCost;
			int missileUpgradeCost = (int)BaseShipStats.shipBaseUpgradeCost;

			// Trying to avoid using "while (true)". Loop variables are not used for anything besides ensuring it's not endless.
			for (int num1 = 0; num1 < 1000; num1++)
			{
				if (num1 == 999)
				{
					Debug.LogError("Too many loops!");
				}
				for (int num2 = 0; num2 < 1000; num2++)
				{
					if (num2 == 999)
					{
						Debug.LogError("Too many loops!");
					}
					int upgrade = random.NextInt(0, 6);
					switch (upgrade)
					{
						case 0:
							if (stats.ValueRO.shipEngineUpgrades < 1)
							{
								continue;
							}

							stats.ValueRW.shipEngineUpgrades++;
							resources -= engineUpgradeCost;
							engineUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerLevelAdd;
							engineUpgradeCost -= (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
							break;
						case 1:
							if (stats.ValueRO.shipArmorUpgrades < 1)
							{
								continue;
							}

							stats.ValueRW.shipArmorUpgrades++;
							resources -= armorUpgradeCost;
							armorUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerLevelAdd;
							armorUpgradeCost -= (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
							break;
						case 2:
							if (stats.ValueRO.shipShieldUpgrades < 1)
							{
								continue;
							}

							stats.ValueRW.shipShieldUpgrades++;
							resources -= shieldUpgradeCost;
							shieldUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerLevelAdd;
							shieldUpgradeCost -= (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
							break;
						case 3:
							if (stats.ValueRO.shipLaserUpgrades < 1)
							{
								continue;
							}

							stats.ValueRW.shipLaserUpgrades++;
							resources -= laserUpgradeCost;
							laserUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerLevelAdd;
							laserUpgradeCost -= (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
							break;
						case 4:
							if (stats.ValueRO.shipKineticUpgrades < 1)
							{
								continue;
							}

							stats.ValueRW.shipKineticUpgrades++;
							resources -= kineticUpgradeCost;
							kineticUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerLevelAdd;
							kineticUpgradeCost -= (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
							break;
						case 5:
							if (stats.ValueRO.shipMissileUpgrades < 1)
							{
								continue;
							}

							stats.ValueRW.shipMissileUpgrades++;
							resources -= missileUpgradeCost;
							missileUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerLevelAdd;
							missileUpgradeCost -= (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
							break;
						default:
							continue;
					}
					engineUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
					armorUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
					shieldUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
					laserUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
					kineticUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
					missileUpgradeCost += (int)BaseShipStats.shipUpgradeCostPerOtherLevelAdd;
					break;
				}
				if (
					resources < engineUpgradeCost
					&& resources < armorUpgradeCost
					&& resources < shieldUpgradeCost
					&& resources < laserUpgradeCost
					&& resources < kineticUpgradeCost
					&& resources < missileUpgradeCost
				)
				{
					break;
				}
			}

			enemy.ValueRW.shipInitializeStats = false;
			global.global.recalculateStats = true;
		}
	}
}
