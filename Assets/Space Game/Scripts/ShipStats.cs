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
}
