using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ShipStats : IComponentData
{
	public float maxSpeed;
	public float acceleration;
	public float rotationSpeedDeg;
	public float rotationSpeedRad;

	public float maxHullPoints;
	public float maxArmorAmount;
	public float maxShieldAmount;
	public float currentHullPoints;
	public float currentArmorAmount;
	public float currentShieldAmount;
}
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

