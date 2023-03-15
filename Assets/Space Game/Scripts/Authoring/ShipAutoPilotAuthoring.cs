using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ShipAutoPilot : IComponentData
{
	public const int ticksLeftMin = 200;
	public const int ticksLeftMax = 500;
	public const float accelerationExp = 0.1f;
	public const float rotationExp = 1/3f;

	public bool autoPilotEnabled;
	public bool autoPilotPaused;

	public int ticksLeft;
	public int ticksLeftNext;

	public float accelerationMult;
	public float pitchMult;
	public float yawMult;
	public float rollMult;
	public float accelerationMultNext;
	public float pitchMultNext;
	public float yawMultNext;
	public float rollMultNext;

	public Unity.Mathematics.Random random;
}

public class ShipAutoPilotAuthoring : MonoBehaviour
{
	public bool autoPilotEnabled = false;
	public bool autoPilotPaused = false;

	public int ticksLeft = 0;
	public int ticksLeftNext = 0;

	public float accelerationMult = 0f;
	public float pitchMult = 0f;
	public float yawMult = 0f;
	public float rollMult = 0f;
	public float accelerationMultNext = 0f;
	public float pitchMultNext = 0f;
	public float yawMultNext = 0f;
	public float rollMultNext = 0f;

	public Unity.Mathematics.Random random = new();
}

public class ShipAutoPilotBaker : Baker<ShipAutoPilotAuthoring>
{
	public override void Bake(ShipAutoPilotAuthoring authoring)
	{
		AddComponent(new ShipAutoPilot
		{
			autoPilotEnabled = authoring.autoPilotEnabled,
			autoPilotPaused = authoring.autoPilotPaused,
			ticksLeft = authoring.ticksLeft,
			ticksLeftNext = authoring.ticksLeftNext,
			accelerationMult = authoring.accelerationMult,
			pitchMult = authoring.pitchMult,
			yawMult = authoring.yawMult,
			rollMult = authoring.rollMult,
			accelerationMultNext = authoring.accelerationMultNext,
			pitchMultNext = authoring.pitchMultNext,
			yawMultNext = authoring.yawMultNext,
			rollMultNext = authoring.rollMultNext,
			random = authoring.random,
		});
	}
}
