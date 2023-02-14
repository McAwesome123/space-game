using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct BaseShipStats : IComponentData
{
	public const float speedOfLight = 3e8f;				// = 300000000
	public const float maxShipSpeed = speedOfLight / 1e4f;		// = 30000
	public const float maxShipAcceleration = maxShipSpeed / 30;     // = 1000
	public const float maxTranslationMult = 0.25f;
	public const float maxShipRotationSpeedDeg = 180;

	public const float baseMaxSpeed = speedOfLight / 1e6f;		// = 300
	public const float baseAcceleration = baseMaxSpeed / 30;        // = 10
	public const float baseTranslationMult = 0.1f;
	public const float baseRotationSpeedDeg = 24;
	public const float baseRotationSpeedRad = baseRotationSpeedDeg * Mathf.Deg2Rad; // Do not modify; Used to be "1 * (360 / (6 * 180 / Mathf.PI))", I have no clue where that came from

	public const float baseRotationSpeedChange = 0.02f;
	public const float baseAccelerationChange = 0.01f;
}
