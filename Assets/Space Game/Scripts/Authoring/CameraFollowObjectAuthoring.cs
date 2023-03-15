using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public struct CameraFollowObject : IComponentData { }

public class CameraFollowObjectAuthoring : MonoBehaviour { }

public class CameraFollowObjectBaker : Baker<CameraFollowObjectAuthoring>
{
	public override void Bake(CameraFollowObjectAuthoring authoring)
	{
		AddComponent(new CameraFollowObject { });
	}
}
