using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerMoveObj : IComponentData
{
	public int setAccelerationMax;
	public int setAccelerationZero;
	public int setAccelerationMin;
}

public class PlayerMoveAuthoring : MonoBehaviour
{
	public int setAccelerationMax = 0;
	public int setAccelerationZero = 0;
	public int setAccelerationMin = 0;
}

public class PlayerMoveBaker : Baker<PlayerMoveAuthoring>
{
	public override void Bake(PlayerMoveAuthoring authoring)
	{
		AddComponent(new PlayerMoveObj
		{
			setAccelerationMax = authoring.setAccelerationMax,
			setAccelerationZero = authoring.setAccelerationZero,
			setAccelerationMin = authoring.setAccelerationMin,
		});
	}
}
