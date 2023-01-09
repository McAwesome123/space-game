using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ShipMovement : IComponentData
{
	public float pitchMult;
	public float rollMult;
	public float yawMult;
	public int currentPitchMult;
	public int currentRollMult;
	public int currentYawMult;
	public float accelerationMult;
	public float verticalTranslationMult;
	public float horizontalTranslationMult;
}
public class ShipMovementAuthoring : MonoBehaviour
{
	public float pitchMult = 0;
	public float rollMult = 0;
	public float yawMult = 0;
	public int currentPitchMult = 0;
	public int currentRollMult = 0;
	public int currentYawMult = 0;
	public float accelerationMult = 0;
	public float verticalTranslationMult = 0;
	public float horizontalTranslationMult = 0;
}

public class ShipMovementBaker : Baker<ShipMovementAuthoring>
{
	public override void Bake(ShipMovementAuthoring authoring)
	{
		AddComponent(new ShipMovement
		{
			pitchMult = authoring.pitchMult,
			rollMult = authoring.rollMult,
			yawMult = authoring.yawMult,
			currentPitchMult = authoring.currentPitchMult,
			currentRollMult = authoring.currentRollMult,
			currentYawMult = authoring.currentYawMult,
			accelerationMult = authoring.accelerationMult,
			verticalTranslationMult = authoring.verticalTranslationMult,
			horizontalTranslationMult = authoring.horizontalTranslationMult,
		});
	}
}
