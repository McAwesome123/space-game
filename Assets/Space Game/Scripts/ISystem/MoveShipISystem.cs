using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct PlayerMovementInput : IComponentData
{
	public float pitch;
	public float yaw;
	public float yawArrows;
	public float roll;
	public float rollArrows;
	public float acceleration;
	public float verticalTranslation;
	public float horizontalTranslation;
	public float rollModifier;
	public float setAccelerationMax;
	public float setAccelerationZero;
	public float setAccelerationMin;
	public float autoPilotEnable;
}

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct MoveShipISystem : ISystem
{


	[BurstCompile]
	public void OnCreate(ref SystemState state) { }

	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		byte gamePaused = SystemAPI.GetSingleton<GlobalEntity>().gamePaused;
		if (gamePaused != 0)
		{
			return;
		}

		PlayerMovementInput input = new()
		{
			pitch = Input.GetAxis("Pitch"),
			yaw = Input.GetAxis("Yaw"),
			yawArrows = Input.GetAxis("Yaw Arrows"),
			roll = Input.GetAxis("Roll"),
			rollArrows = Input.GetAxis("Roll Arrows"),
			acceleration = Input.GetAxis("Acceleration"),
			verticalTranslation = Input.GetAxis("Vertical Translation"),
			horizontalTranslation = Input.GetAxis("Horizontal Translation"),
			rollModifier = Input.GetAxis("Roll Modifier"),
			setAccelerationMax = Input.GetAxis("Set Acceleration Max"),
			setAccelerationZero = Input.GetAxis("Set Acceleration Zero"),
			setAccelerationMin = Input.GetAxis("Set Acceleration Min"),
			autoPilotEnable = Input.GetAxis("Toggle Autopilot"),
		};

		JobHandle jobHandle = new PlayerInputJob { playerMovementInput = input }.Schedule(state.Dependency);

		jobHandle.Complete();

		jobHandle = new SmoothRotationJob { }.ScheduleParallel(state.Dependency);

		jobHandle.Complete();

		new MoveShipJob { }.ScheduleParallel();
	}
}

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct MoveShipJob : IJobEntity
{
	[BurstCompile]
	public void Execute(MoveShipAspect moveShipAspect)
	{
		moveShipAspect.MoveShip();
	}
}

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct SmoothRotationJob : IJobEntity
{
	[BurstCompile]
	public void Execute(MoveShipAspect moveShipAspect)
	{
		moveShipAspect.SmoothRotation();
	}
}

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
#pragma warning disable CS0282 // There is no defined ordering between fields in multiple declarations of partial struct
public partial struct PlayerInputJob : IJobEntity
#pragma warning restore CS0282 // There is no defined ordering between fields in multiple declarations of partial struct
{
	public PlayerMovementInput playerMovementInput;

	[BurstCompile]
	public void Execute(ref PlayerMoveAspect playerMoveAspect)
	{
		playerMoveAspect.PlayerInput(playerMovementInput);
	}
}
