using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class ShipDamageSystemGlobal : IComponentData
{
	public Global global;

	public ShipDamageSystemGlobal()
	{
		global = GameObject.Find("Global").GetComponent<Global>();
	}
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
//[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial class ShipDamageSystem : SystemBase
{
	readonly ShipDamageSystemGlobal global = new();
	private bool noPhysics = false;

	protected override void OnStartRunning()
	{
		base.OnStartRunning();
	}

	protected override void OnUpdate()
	{
		RefRW<PhysicsStep> physicsStep = SystemAPI.GetSingletonRW<PhysicsStep>();
		if (global.global.gamePaused != 0)
		{
			physicsStep.ValueRW.SimulationType = SimulationType.NoPhysics;
			return;
		}
		else if (physicsStep.ValueRO.SimulationType == SimulationType.NoPhysics)
		{
			physicsStep.ValueRW.SimulationType = SimulationType.UnityPhysics;
			return;
		}

		foreach((RefRW<Collision> collision, RefRO<PhysicsVelocity> velocity) in SystemAPI.Query<RefRW<Collision>, RefRO<PhysicsVelocity>>())
		{
			collision.ValueRW.previousVelocity = collision.ValueRO.currentVelocity;
			collision.ValueRW.currentVelocity = velocity.ValueRO.Linear;
		}
			
		foreach((DynamicBuffer<CollisionEntityPreviousBufferElement> previousCollision, DynamicBuffer<CollisionEntityBufferElement> collision) in SystemAPI.Query<DynamicBuffer<CollisionEntityPreviousBufferElement>, DynamicBuffer <CollisionEntityBufferElement>>())
		{
			previousCollision.CopyFrom(collision.Reinterpret<CollisionEntityPreviousBufferElement>());
		}

		try
		{
			foreach (DynamicBuffer<CollisionEntityBufferElement> collision in SystemAPI.Query<DynamicBuffer<CollisionEntityBufferElement>>())
			{
				collision.Clear();
			}

			CollisionEvents collisionEvents = SystemAPI.GetSingleton<SimulationSingleton>().AsSimulation().CollisionEvents;
			foreach (CollisionEvent collisionEvent in collisionEvents)
			{
				bool skipCollisionEvent = false;
				foreach (CollisionEntityBufferElement buffer in EntityManager.GetBuffer<CollisionEntityBufferElement>(collisionEvent.EntityA))
				{
					if (buffer.value.Equals(collisionEvent.EntityB))
					{
						skipCollisionEvent = true;
					}
				}
				if (skipCollisionEvent)
				{
					continue;
				}

				EntityManager.GetBuffer<CollisionEntityBufferElement>(collisionEvent.EntityA).Add(collisionEvent.EntityB);
				EntityManager.GetBuffer<CollisionEntityBufferElement>(collisionEvent.EntityB).Add(collisionEvent.EntityA);

				//Debug.Log(string.Format("Collision Event Normal {0}, Entity A {1}, Entity B {2}", collisionEvent.Normal, math.length(EntityManager.GetComponentData<PhysicsVelocity>(collisionEvent.EntityA).Linear), math.length(EntityManager.GetComponentData<PhysicsVelocity>(collisionEvent.EntityB).Linear)));
			}

			foreach((ShipDamageEnemyAspect shipDamage, DynamicBuffer<CollisionEntityBufferElement> entityBuffer, DynamicBuffer<CollisionEntityPreviousBufferElement> previousEntityBuffer) in SystemAPI.Query<ShipDamageEnemyAspect, DynamicBuffer<CollisionEntityBufferElement>, DynamicBuffer<CollisionEntityPreviousBufferElement>>())
			{
				foreach(CollisionEntityBufferElement buffer in entityBuffer)
				{
					bool skipCollision = false;
					foreach(CollisionEntityPreviousBufferElement previousBuffer in previousEntityBuffer)
					{
						if (previousBuffer.value.Equals(buffer.value))
						{
							skipCollision = true;
							break;
						}
					}
					if (skipCollision)
					{
						break;
					}

					CollisionAspect other = EntityManager.GetAspectRO<CollisionAspect>(buffer.value);
					float relativeSpeed = math.abs(math.length(shipDamage.collisionAspect.collision.ValueRO.previousVelocity) - math.length(other.collision.ValueRO.previousVelocity));

					if (relativeSpeed <= shipDamage.shipStats.ValueRO.maxSpeed / 30f)
					{
						continue;
					}

					shipDamage.TakeDamageEnemy((relativeSpeed - shipDamage.shipStats.ValueRO.maxSpeed / 30f) * (other.rigidbodyAspect.Mass / shipDamage.collisionAspect.rigidbodyAspect.Mass), false);
					Debug.Log(string.Format("Enemy took {0} damage", relativeSpeed - shipDamage.shipStats.ValueRO.maxSpeed / 30f));
				}
			}

			foreach((ShipDamagePlayerAspect shipDamage, DynamicBuffer<CollisionEntityBufferElement> entityBuffer, DynamicBuffer<CollisionEntityPreviousBufferElement> previousEntityBuffer) in SystemAPI.Query<ShipDamagePlayerAspect, DynamicBuffer<CollisionEntityBufferElement>, DynamicBuffer<CollisionEntityPreviousBufferElement>>())
			{
				foreach (CollisionEntityBufferElement buffer in entityBuffer)
				{
					bool skipCollision = false;
					foreach (CollisionEntityPreviousBufferElement previousBuffer in previousEntityBuffer)
					{
						if (previousBuffer.value.Equals(buffer.value))
						{
							skipCollision = true;
							break;
						}
					}
					if (skipCollision)
					{
						break;
					}

					CollisionAspect other = EntityManager.GetAspectRO<CollisionAspect>(buffer.value);
					float relativeSpeed = math.abs(math.length(shipDamage.collisionAspect.collision.ValueRO.previousVelocity) - math.length(other.collision.ValueRO.previousVelocity));

					if (relativeSpeed <= shipDamage.shipStats.ValueRO.maxSpeed / 30f)
					{
						continue;
					}

					shipDamage.TakeDamagePlayer(math.pow(relativeSpeed - shipDamage.shipStats.ValueRO.maxSpeed / 30f, 1.2f) * (other.rigidbodyAspect.Mass / shipDamage.collisionAspect.rigidbodyAspect.Mass), false);
					Debug.Log(string.Format("Player took {0} damage", math.pow(relativeSpeed - shipDamage.shipStats.ValueRO.maxSpeed / 30f, 1.2f)));
				}
			}
		}
		catch (InvalidOperationException e)
		{
			if (e.Message.Contains("NoPhysics") && !noPhysics)
			{
				Debug.Log("No Physics!");
				noPhysics = true;
			}
			else
			{
				Debug.LogError(e);
			}
		}

		foreach(ShipDamagePlayerAspect shipDamage in SystemAPI.Query<ShipDamagePlayerAspect>())
		{
			if (shipDamage.shipStats.ValueRO.damageArmorDecayBuffer > 0)
			{
				shipDamage.shipStats.ValueRW.damageArmorDecayBuffer--;
				continue;
			}

			if (shipDamage.shipStats.ValueRO.currentDamageArmorAmount > 0)
			{
				shipDamage.shipStats.ValueRW.currentDamageArmorAmount *= BaseShipStats.shipPlayerDamageArmorDecayPercentPerTick;
				shipDamage.shipStats.ValueRW.currentDamageArmorAmount -= BaseShipStats.shipPlayerDamageArmorDecayPerTick;
			}
			else
			{
				shipDamage.shipStats.ValueRW.currentDamageArmorAmount = 0;
			}
		}
	}
}
