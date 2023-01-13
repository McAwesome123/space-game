using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerMoveObj : IComponentData
{
	
}

public class PlayerMoveAuthoring : MonoBehaviour
{
	
}

public class PlayerMoveBaker : Baker<PlayerMoveAuthoring>
{
	public override void Bake(PlayerMoveAuthoring authoring)
	{
		AddComponent(new PlayerMoveObj
		{
			
		});
	}
}
