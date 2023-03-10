using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSimulationGroup))]
public partial struct ShipDamageISystem : ISystem
{
	public partial struct CountNumCollisionEvents : ICollisionEventsJob
	{
		public NativeReference<int> NumCollisionEvents;

		public void Execute(CollisionEvent collisionEvent)
		{
			NumCollisionEvents.Value++;
			Debug.Log(string.Format("Entity A: {0}", math.length(World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PhysicsVelocity>(collisionEvent.EntityA).Linear)));
			Debug.Log(string.Format("Entity B: {0}", math.length(World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PhysicsVelocity>(collisionEvent.EntityB).Linear)));
		}
	}

	public void OnCreate(ref SystemState state) { }

	public void OnDestroy(ref SystemState state) { }

	public void OnUpdate(ref SystemState state)
	{
		NativeReference<int> numCollisionEvents = new NativeReference<int>(0, Allocator.TempJob);
	}
}
