using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct GlobalEntity : IComponentData
{
	public byte gamePaused;
}

public class GlobalEntityAuthoring : MonoBehaviour
{
	public byte gamePaused = 0;
}

public class GlobalEntityBaker : Baker<GlobalEntityAuthoring>
{
	public override void Bake(GlobalEntityAuthoring authoring)
	{
		AddComponent(new GlobalEntity
		{
			gamePaused = authoring.gamePaused,
		});
	}
}
