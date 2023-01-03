using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct MoveShipISystem : ISystem
{
	public void OnCreate(ref SystemState state) { }

	public void OnDestroy(ref SystemState state) { }

	public void OnUpdate(ref SystemState state)
	{
		foreach (MoveShipAspect moveShipAspect in SystemAPI.Query<MoveShipAspect>())
		{
			moveShipAspect.MoveShip();
		}
	}
}
