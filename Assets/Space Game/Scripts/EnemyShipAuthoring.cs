using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct EnemyShipObj : IComponentData
{
	public bool shipInitializeStats;
	public bool shipFiringLaser;
	public bool shipFiringKinetic;
	public bool shipFiringMissile;
	public int shipFiringTimeLeft;

	public quaternion shipReturnRotation;
}

public class EnemyShipAuthoring : MonoBehaviour
{
	public bool shipInitializeStats = true;
	public bool shipFiringLaser = false;
	public bool shipFiringKinetic = false;
	public bool shipFiringMissile = false;
	public int shipFiringTimeLeft = 0;

	public quaternion shipReturnRotation = new();
}

public class EnemyShipBaker : Baker<EnemyShipAuthoring>
{
	public override void Bake(EnemyShipAuthoring authoring)
	{
		AddComponent(new EnemyShipObj
		{
			shipInitializeStats = authoring.shipInitializeStats,
			shipFiringLaser = authoring.shipFiringLaser,
			shipFiringKinetic = authoring.shipFiringKinetic,
			shipFiringMissile = authoring.shipFiringMissile,
			shipFiringTimeLeft = authoring.shipFiringTimeLeft,
			shipReturnRotation = authoring.shipReturnRotation,
		});
	}
}
