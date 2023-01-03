using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct MoveShipAspect : IAspect
{
	//private readonly Entity entity;

	private readonly TransformAspect transformAspect;
	private readonly RefRO<ShipStats> shipStats;
	private readonly RefRO<ShipMovement> shipMovement;

	public void MoveShip()
	{
		transformAspect.WorldPosition += new float3(0, 0, shipStats.ValueRO.maxSpeed / BaseShipStats.tickRate);
	}
}
