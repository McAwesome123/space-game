using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct MoveShipAspect : IAspect
{
	//private readonly Entity entity;

	private readonly TransformAspect transformAspect;
	private readonly RefRO<ShipStats> shipStats;
	private readonly RefRW<ShipMovement> shipMovement;
	private readonly RefRW<PhysicsVelocity> physicsVelocity;

	public void SmoothRotation()
	{
		if (shipMovement.ValueRO.currentPitchMult + BaseShipStats.baseRotationSpeedChange <= shipMovement.ValueRO.pitchMult)
		{
			shipMovement.ValueRW.currentPitchMult += BaseShipStats.baseRotationSpeedChange;
		}
		else if (shipMovement.ValueRO.currentPitchMult - BaseShipStats.baseRotationSpeedChange >= shipMovement.ValueRO.pitchMult)
		{
			shipMovement.ValueRW.currentPitchMult -= BaseShipStats.baseRotationSpeedChange;
		}
		else
		{
			shipMovement.ValueRW.currentPitchMult = shipMovement.ValueRO.pitchMult;
		}

		if (shipMovement.ValueRO.currentYawMult + BaseShipStats.baseRotationSpeedChange <= shipMovement.ValueRO.yawMult)
		{
			shipMovement.ValueRW.currentYawMult += BaseShipStats.baseRotationSpeedChange;
		}
		else if (shipMovement.ValueRO.currentYawMult - BaseShipStats.baseRotationSpeedChange >= shipMovement.ValueRO.yawMult)
		{
			shipMovement.ValueRW.currentYawMult -= BaseShipStats.baseRotationSpeedChange;
		}
		else
		{
			shipMovement.ValueRW.currentYawMult = shipMovement.ValueRO.yawMult;
		}

		if (shipMovement.ValueRO.currentRollMult + BaseShipStats.baseRotationSpeedChange <= shipMovement.ValueRO.rollMult)
		{
			shipMovement.ValueRW.currentRollMult += BaseShipStats.baseRotationSpeedChange;
		}
		else if (shipMovement.ValueRO.currentRollMult - BaseShipStats.baseRotationSpeedChange >= shipMovement.ValueRO.rollMult)
		{
			shipMovement.ValueRW.currentRollMult -= BaseShipStats.baseRotationSpeedChange;
		}
		else
		{
			shipMovement.ValueRW.currentRollMult = shipMovement.ValueRO.rollMult;
		}
	}

	public void MoveShip()
	{
		// Rotation
		// Get the intended pitch, yaw, roll velocity.
		float pitch = shipStats.ValueRO.rotationSpeedRad * shipMovement.ValueRO.currentPitchMult;
		float yaw = shipStats.ValueRO.rotationSpeedRad * shipMovement.ValueRO.currentYawMult;
		float roll = shipStats.ValueRO.rotationSpeedRad * shipMovement.ValueRO.currentRollMult;

		// Apply the intended velocity. Angular velocity does not have inertia like linear velocity so it uses 'intended velocity - current velocity'.
		physicsVelocity.ValueRW.Angular += new float3(pitch - physicsVelocity.ValueRO.Angular.x, yaw - physicsVelocity.ValueRO.Angular.y, roll - physicsVelocity.ValueRO.Angular.z);

		// Movement
		// Get the current speed.
		double currentVelocity = math.length(physicsVelocity.ValueRO.Linear);

		// Calculate the translation acceleration multiplier then use it to clamp forward/backward acceleration multiplier.
		double2 translationMult = new(shipMovement.ValueRO.horizontalTranslationMult * shipStats.ValueRO.translation, shipMovement.ValueRO.verticalTranslationMult * shipStats.ValueRO.translation);
		double accelerationClamp = math.sqrt(1 - translationMult.x * translationMult.x - translationMult.y * translationMult.y);

		// Calculate the intended xyz velocity change and apply it.
		double3 currentAcceleration = new(shipStats.ValueRO.acceleration / Global.tickRate * translationMult.x,
						 shipStats.ValueRO.acceleration / Global.tickRate * translationMult.y,
						 shipStats.ValueRO.acceleration / Global.tickRate * math.clamp(shipMovement.ValueRO.currentAccelerationMult, -accelerationClamp, +accelerationClamp));
		double3 newVelocity = physicsVelocity.ValueRO.Linear + Global.ApplyRotation(transformAspect.LocalRotation, currentAcceleration);

		// Calculate the velocity difference.
		double velocityMagnitudeChange = math.length(newVelocity) - currentVelocity;

		// Calculate the velocity multiplier and apply it.
		double trueAccelerationMult = 1 / (1 + (currentVelocity * velocityMagnitudeChange) / (shipStats.ValueRO.maxSpeed * shipStats.ValueRO.maxSpeed));
		newVelocity *= trueAccelerationMult;
		physicsVelocity.ValueRW.Linear = (float3)newVelocity;

		//double expectedVelocity = (currentVelocity + velocityMagnitudeChange) / (1 + (currentVelocity * velocityMagnitudeChange) / (shipStats.ValueRO.maxSpeed * shipStats.ValueRO.maxSpeed));
		//Debug.LogFormat("Expected speed: ({0} + {1}) / (1 + ({0} * {1}) / {2}^2) = {3} ({4})", currentVelocity, velocityMagnitudeChange, shipStats.ValueRO.maxSpeed, expectedVelocity, (float)expectedVelocity);
		//Debug.LogFormat("Actual speed: {0}", math.length(physicsVelocity.ValueRO.Linear));
		//if (math.length(physicsVelocity.ValueRO.Linear) == (float)expectedVelocity)
		//	Debug.Log("Match = True");
		//else
		//	Debug.LogWarning("Match = False");

		CapSpeed();
	}

	public void CapSpeed()
	{
		if (math.length(physicsVelocity.ValueRO.Linear) > shipStats.ValueRO.maxSpeed)
		{
			double mult = shipStats.ValueRO.maxSpeed / math.length(physicsVelocity.ValueRO.Linear) * 0.99999;
			double3 newVelocity = (double3)physicsVelocity.ValueRO.Linear * mult;
			physicsVelocity.ValueRW.Linear = (float3)newVelocity;
			// I have no clue if converting to a double is necessary
			// What I do know is that I despise float precision and do not feel like taking chances
			// Plus, ideally this should not be ran much (if at all)
		}
	}
}
