using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ShipMovement : IComponentData
{
	public float pitchMult;
	public float rollMult;
	public float yawMult;
	public float accelerationMult;
	public float verticalTranslationMult;
	public float horizontalTranslationMult;
}
