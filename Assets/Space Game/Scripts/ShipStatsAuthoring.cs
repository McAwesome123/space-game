using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShipStatsAuthoring : MonoBehaviour
{
	public float maxSpeed = BaseShipStats.baseMaxSpeed;
	public float acceleration = BaseShipStats.baseAcceleration;
	public float rotationSpeedDeg = BaseShipStats.baseRotationSpeedDeg;
	public float rotationSpeedRad = BaseShipStats.baseRotationSpeedRad;
}

public class ShipAccelerationBaker : Baker<ShipStatsAuthoring>
{
	public override void Bake(ShipStatsAuthoring authoring)
	{
		AddComponent(new ShipStats
		{
			maxSpeed = authoring.maxSpeed,
			acceleration = authoring.acceleration,
			rotationSpeedDeg = authoring.rotationSpeedDeg,
			rotationSpeedRad = authoring.rotationSpeedRad,
		});
	}
}
