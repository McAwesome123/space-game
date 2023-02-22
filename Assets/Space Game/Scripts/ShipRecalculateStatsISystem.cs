using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public readonly partial struct ShipRecalculateStatsISystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{

	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{

	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		//new ShipRecalculateStatsJob { }.ScheduleParallel();
	}
}

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct ShipRecalculateStatsJob : IJobEntity
{
	[BurstCompile]
	public void Execute(RefRW<ShipStats> shipStats)
	{
		//shipStats.ValueRW.maxSpeed = BaseShipStats.baseMaxSpeed * (1 + BaseShipStats.shipMaxSpeedPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipMaxSpeedPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipMaxSpeedPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)));
		//shipStats.ValueRW.acceleration = BaseShipStats.baseAcceleration * (1 + BaseShipStats.shipAccelerationPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipAccelerationPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipAccelerationPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)));
		//shipStats.ValueRW.translation = BaseShipStats.baseTranslationMult * (1 + BaseShipStats.shipTranslationPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipTranslationPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipTranslationPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)));
		//shipStats.ValueRW.rotationSpeedDeg = BaseShipStats.baseRotationSpeedDeg * (1 + BaseShipStats.shipRotationPerUpgradeLevelHyperbloicAdd * (BaseShipStats.shipRotationPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)) / (1 + BaseShipStats.shipRotationPerUpgradeLevelHyperbloicFactor * (shipStats.ValueRW.shipEngineUpgrades - 1)));
		//shipStats.ValueRW.rotationSpeedRad = math.radians(shipStats.ValueRW.rotationSpeedDeg);
	}
}
