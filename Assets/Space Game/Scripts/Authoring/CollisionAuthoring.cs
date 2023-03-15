using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using UnityEngine;

public struct CollisionEntityBufferElement : IBufferElementData
{
	public Entity value;

	public static implicit operator Entity(CollisionEntityBufferElement e)
	{
		return e.value;
	}

	public static implicit operator CollisionEntityBufferElement(Entity e)
	{
		return new CollisionEntityBufferElement { value = e };
	}
}

public struct CollisionEntityPreviousBufferElement : IBufferElementData
{
	public Entity value;

	public static implicit operator Entity(CollisionEntityPreviousBufferElement e)
	{
		return e.value;
	}

	public static implicit operator CollisionEntityPreviousBufferElement(Entity e)
	{
		return new CollisionEntityPreviousBufferElement { value = e };
	}
}

public struct Collision : IComponentData
{
	public float3 currentVelocity;
	public float3 previousVelocity;
}

public class CollisionAuthoring : MonoBehaviour
{
	public float3 currentVelocity = float3.zero;
	public float3 previousVelocity = float3.zero;
}

public class CollisionBaker : Baker<CollisionAuthoring>
{
	public override void Bake(CollisionAuthoring authoring)
	{
		AddComponent(new Collision
		{
			currentVelocity = authoring.currentVelocity,
			previousVelocity = authoring.previousVelocity,
		});
		AddBuffer<CollisionEntityBufferElement>();
		AddBuffer<CollisionEntityPreviousBufferElement>();
	}
}

public readonly partial struct CollisionAspect : IAspect
{
	public readonly RefRO<Collision> collision;
	public readonly RigidBodyAspect rigidbodyAspect;
}
