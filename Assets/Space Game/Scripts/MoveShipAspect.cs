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

	//private readonly TransformAspect transformAspect;
	private readonly RefRO<ShipStats> shipStats;
	private readonly RefRO<ShipMovement> shipMovement;
	private readonly RefRW<PhysicsVelocity> physicsVelocity;

	public void MoveShip()
	{
		// Get the intended pitch, yaw, roll velocity
		float pitch = shipStats.ValueRO.rotationSpeedRad / Main.tickRate * shipMovement.ValueRO.pitchMult;
		float yaw = shipStats.ValueRO.rotationSpeedRad / Main.tickRate * shipMovement.ValueRO.yawMult;
		float roll = shipStats.ValueRO.rotationSpeedRad / Main.tickRate * shipMovement.ValueRO.rollMult;

		// Apply the intended velocity. Angular velocity does not have inertia like linear velocity so it uses 'intended velocity - current velocity'.
		physicsVelocity.ValueRW.Angular += new float3(pitch - physicsVelocity.ValueRO.Angular.x, yaw - physicsVelocity.ValueRO.Angular.y, roll - physicsVelocity.ValueRO.Angular.z);

		// Get the current speed
		double currentVelocity = math.length(physicsVelocity.ValueRO.Linear);

		// Calculate the translation acceleration multiplier then use it to clamp forward/backward acceleration multiplier
		double2 translationMult = new(shipMovement.ValueRO.horizontalTranslationMult * BaseShipStats.baseTranslationMult, shipMovement.ValueRO.verticalTranslationMult * BaseShipStats.baseTranslationMult);
		double accelerationClamp = math.sqrt(1 - translationMult.x * translationMult.x - translationMult.y * translationMult.y);

		// Calculate the intended xyz velocity change and apply it
		double3 currentAcceleration = new(shipStats.ValueRO.acceleration / Main.tickRate * translationMult.x,
						 shipStats.ValueRO.acceleration / Main.tickRate * translationMult.y,
						 shipStats.ValueRO.acceleration / Main.tickRate * math.clamp(shipMovement.ValueRO.accelerationMult, -accelerationClamp, +accelerationClamp));
		double3 newVelocity = physicsVelocity.ValueRO.Linear + currentAcceleration;

		// Calculate the velocity difference
		double velocityMagnitudeChange = math.length(newVelocity) - currentVelocity;

		// Calculate the velocity multiplier and apply it
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
	}
}
