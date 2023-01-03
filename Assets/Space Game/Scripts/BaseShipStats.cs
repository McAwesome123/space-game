using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct BaseShipStats : IComponentData
{
	public const int tickRate = 100;

	public const float speedOfLight = 3e8f;				// = 300000000
	public const float maxShipSpeed = speedOfLight / 1e5f;		// = 3000
	public const float maxShipAcceleration = maxShipSpeed / 30;	// = 100
	public const float maxShipRotationSpeedDeg = 180;

	public const float baseMaxSpeed = speedOfLight / 1e7f;		// = 30
	public const float baseAcceleration = baseMaxSpeed / 30;	// = 1
	public const float baseRotationSpeedDeg = 24;
	public const float baseRotationSpeedRad = baseRotationSpeedDeg * Mathf.Deg2Rad;	// Do not modify; Used to be "1 * (360 / (6 * 180 / Mathf.PI))", I have no clue where that came from
}
