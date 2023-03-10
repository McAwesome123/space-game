using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public struct AutoPilotRandom : IComponentData
{
	public Unity.Mathematics.Random random;
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct AutoPilotISystem : ISystem
{
	byte gamePaused;
	AutoPilotRandom random;

	public void OnCreate(ref SystemState state)
	{
		gamePaused = GameObject.Find("Global").GetComponent<Global>().gamePaused;
		random.random = new Unity.Mathematics.Random(GameObject.Find("Global").GetComponent<Global>().currentStar.random.NextUInt());
		Debug.Log(random.random.NextInt());
	}

	public void OnDestroy(ref SystemState state)
	{ }

	public void OnUpdate(ref SystemState state)
	{
		if (gamePaused != 0)
		{
			return;
		}

		new AutoPilotAssignRandom { random = random.random }.Run();

		new GenerateAutoPilotJob { }.ScheduleParallel();

		JobHandle jobHandle = new EnemyAutoPilotJob { }.ScheduleParallel(state.Dependency);

		jobHandle.Complete();

		Entity playerShip;
		TransformAspect playerTransform;
		PhysicsVelocity playerVelocity;

		NativeArray<Entity> entityArray = state.EntityManager.CreateEntityQuery(typeof(PlayerMoveObj)).ToEntityArray(Allocator.Temp);
		if (entityArray.Length > 0)
		{
			playerShip = entityArray[0];
			playerTransform = state.EntityManager.GetAspectRO<TransformAspect>(playerShip);
			playerVelocity = state.EntityManager.GetComponentData<PhysicsVelocity>(playerShip);

			new EnemyFireJob
			{
				playerTransform = playerTransform,
				playerVelocity = playerVelocity,
			}.ScheduleParallel();
		}
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
#pragma warning disable CS0282 // There is no defined ordering between fields in multiple declarations of partial struct
public partial struct AutoPilotAssignRandom : IJobEntity
#pragma warning restore CS0282 // There is no defined ordering between fields in multiple declarations of partial struct
{
	public Unity.Mathematics.Random random;

	public void Execute(ref ShipAutoPilot shipAutoPilot)
	{
		if (shipAutoPilot.random.state == 0)
			shipAutoPilot.random = new Unity.Mathematics.Random(random.NextUInt() + 1);
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct GenerateAutoPilotJob : IJobEntity
{
	public void Execute(ref ShipAutoPilot shipAutoPilot)
	{
		if (shipAutoPilot.autoPilotPaused)
		{
			return;
		}

		if (shipAutoPilot.ticksLeft > 0)
		{
			shipAutoPilot.ticksLeft--;
			return;
		}

		shipAutoPilot.ticksLeft = shipAutoPilot.ticksLeftNext;
		shipAutoPilot.accelerationMult = shipAutoPilot.accelerationMultNext;
		shipAutoPilot.pitchMult = shipAutoPilot.pitchMultNext;
		shipAutoPilot.yawMult = shipAutoPilot.yawMultNext;
		shipAutoPilot.rollMult = shipAutoPilot.rollMultNext;

		shipAutoPilot.ticksLeftNext = shipAutoPilot.random.NextInt(ShipAutoPilot.ticksLeftMin, ShipAutoPilot.ticksLeftMax);

		shipAutoPilot.accelerationMultNext = math.pow(shipAutoPilot.random.NextFloat(0f, 1f), ShipAutoPilot.accelerationExp);

		shipAutoPilot.pitchMultNext = math.pow(shipAutoPilot.random.NextFloat(0f, 1f), ShipAutoPilot.rotationExp) * (shipAutoPilot.random.NextBool() ? -1 : 1);
		shipAutoPilot.yawMultNext = math.pow(shipAutoPilot.random.NextFloat(0f, 1f), ShipAutoPilot.rotationExp) * (shipAutoPilot.random.NextBool() ? -1 : 1);
		shipAutoPilot.rollMultNext = math.pow(shipAutoPilot.random.NextFloat(0f, 1f), ShipAutoPilot.rotationExp) * (shipAutoPilot.random.NextBool() ? -1 : 1);
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct EnemyAutoPilotJob : IJobEntity
{
#pragma warning disable IDE0060 // Remove unused parameter
	public void Execute(in ShipAutoPilot shipAutoPilot, ref ShipMovement shipMovement, in EnemyShipObj enemy)
#pragma warning restore IDE0060 // Remove unused parameter
	{
		if (shipAutoPilot.autoPilotPaused)
		{
			return;
		}

		shipMovement.currentAccelerationMult = shipAutoPilot.accelerationMult;
		shipMovement.pitchMult = shipAutoPilot.pitchMult;
		shipMovement.yawMult = shipAutoPilot.yawMult;
		shipMovement.rollMult = shipAutoPilot.rollMult;
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
#pragma warning disable CS0282 // There is no defined ordering between fields in multiple declarations of partial struct
public partial struct EnemyFireJob : IJobEntity
#pragma warning restore CS0282 // There is no defined ordering between fields in multiple declarations of partial struct
{
	[ReadOnly][NativeDisableUnsafePtrRestriction] public TransformAspect playerTransform;
	[ReadOnly] public PhysicsVelocity playerVelocity; 

	public void Execute(ref ShipAutoPilot shipAutoPilot, ref ShipMovement shipMovement, ref ShipStats shipStats, ref EnemyShipObj enemy, ref TransformAspect transformAspect)
	{
		shipStats.shipLaserCharge++;
		shipStats.shipKineticCharge++;
		shipStats.shipMissileCharge++;

		if (shipStats.shipLaserUpgrades > 0 && !enemy.shipFiringMissile && !enemy.shipFiringKinetic && !enemy.shipFiringLaser && shipStats.shipLaserCharge >= shipStats.shipLaserCooldown * 0.9f)
		{
			enemy.shipFiringLaser = true;
			enemy.shipReturnRotation = transformAspect.WorldRotation;
			shipAutoPilot.autoPilotPaused = true;
		}

		if (shipStats.shipKineticUpgrades > 0 && !enemy.shipFiringMissile && !enemy.shipFiringKinetic && !enemy.shipFiringLaser && shipStats.shipKineticCharge >= shipStats.shipKineticCooldown * 0.9f)
		{
			enemy.shipFiringKinetic = true;
			enemy.shipReturnRotation = transformAspect.WorldRotation;
			shipAutoPilot.autoPilotPaused = true;
		}

		if (shipStats.shipMissileUpgrades > 0 && !enemy.shipFiringMissile && !enemy.shipFiringKinetic && !enemy.shipFiringLaser && shipStats.shipMissileCharge >= shipStats.shipMissileCooldown * 0.9f)
		{
			enemy.shipFiringMissile = true;
			enemy.shipReturnRotation = transformAspect.WorldRotation;
			shipAutoPilot.autoPilotPaused = true;
		}

		float shotSpeed;

		if (enemy.shipFiringLaser)
		{
			shotSpeed = shipStats.shipLaserShotSpeed;
		}
		else if (enemy.shipFiringKinetic)
		{
			shotSpeed = shipStats.shipKineticShotSpeed;
		}
		else if (enemy.shipFiringMissile)
		{
			shotSpeed = shipStats.shipMissileShotSpeed;
		}
		//else
		{
			return;
		}

		/*
		// Gives "Attempting to read WriteOnly" error
		shotSpeed /= Global.tickRate;	// Units per tick

		float timeToReach = math.distance(playerTransform.WorldPosition, transformAspect.WorldPosition) / shotSpeed;	// Time in ticks
		float3 predictedPosition = playerTransform.WorldPosition + playerVelocity.Linear / Global.tickRate * timeToReach;

		quaternion newRotation = quaternion.EulerXYZ(Vector3.RotateTowards(transformAspect.Forward, predictedPosition - transformAspect.WorldPosition, shipStats.rotationSpeedRad / Global.tickRate, 0.0f));
		if (newRotation.Equals(transformAspect.WorldRotation))
		{
			Debug.Log("Ship rotated!");
		}
		*/
	}
}
