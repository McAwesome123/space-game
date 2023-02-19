using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public readonly partial struct PlayerMoveAspect : IAspect
{
	private readonly RefRW<PlayerMoveObj> playerMove;
	private readonly RefRW<ShipMovement> shipMovement;

	public void PlayerInput(PlayerMovementInput playerMovementInput)
	{
		// Rotation Input
		shipMovement.ValueRW.pitchMult = playerMovementInput.pitch;
		if (playerMovementInput.rollModifier != 0 && playerMovementInput.yawArrows != 0)
		{
			if (playerMovementInput.yaw != 0)
			{
				shipMovement.ValueRW.yawMult = (playerMovementInput.yaw + playerMovementInput.yawArrows) / 2;
			}
			else
			{
				shipMovement.ValueRW.yawMult = playerMovementInput.yawArrows;
			}
		}
		else
		{
			shipMovement.ValueRW.yawMult = playerMovementInput.yaw;
		}
		if (playerMovementInput.rollModifier == 0 && playerMovementInput.rollArrows != 0)
		{
			if (playerMovementInput.roll != 0)
			{
				shipMovement.ValueRW.rollMult = (playerMovementInput.roll + playerMovementInput.rollArrows) / 2;
			}
			else
			{
				shipMovement.ValueRW.rollMult = playerMovementInput.rollArrows;
			}
		}
		else
		{
			shipMovement.ValueRW.rollMult = playerMovementInput.roll;
		}

		// Set acceleration
		if (playerMovementInput.setAccelerationMax > 0)
		{
			playerMove.ValueRW.setAccelerationMax++;
		}
		else
		{
			playerMove.ValueRW.setAccelerationMax = 0;
		}
		if (playerMovementInput.setAccelerationZero > 0)
		{
			playerMove.ValueRW.setAccelerationZero++;
		}
		else
		{
			playerMove.ValueRW.setAccelerationZero = 0;
		}
		if (playerMovementInput.setAccelerationMin > 0)
		{
			playerMove.ValueRW.setAccelerationMin++;
		}
		else
		{
			playerMove.ValueRW.setAccelerationMin = 0;
		}

		if (playerMove.ValueRO.setAccelerationMin == 1)
		{
			shipMovement.ValueRW.currentAccelerationMult = -1;
		}
		if (playerMove.ValueRO.setAccelerationMax == 1)
		{
			shipMovement.ValueRW.currentAccelerationMult = 1;
		}
		if (playerMove.ValueRO.setAccelerationZero == 1)
		{
			shipMovement.ValueRW.currentAccelerationMult = 0;
		}

		// Fine acceleration and translation
		shipMovement.ValueRW.accelerationMult = playerMovementInput.acceleration;
		shipMovement.ValueRW.verticalTranslationMult = playerMovementInput.verticalTranslation;
		shipMovement.ValueRW.horizontalTranslationMult = playerMovementInput.horizontalTranslation;

		if (shipMovement.ValueRO.accelerationMult != 0)
		{
			shipMovement.ValueRW.currentAccelerationMult += shipMovement.ValueRO.accelerationMult * BaseShipStats.baseAccelerationChange;
		}
		shipMovement.ValueRW.currentAccelerationMult = math.clamp(shipMovement.ValueRO.currentAccelerationMult, -1, 1);
	}
}
