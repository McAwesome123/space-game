using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarpPanel : MonoBehaviour
{
	public Global global;
	public Button[] starButtons = new Button[36];

	private bool initializeStars = true;

	// Start is called before the first frame update
	void Start()
	{
		if (!initializeStars)
		{
			return;
		}

		global = GameObject.Find("Global").GetComponent<Global>();

		if (global.numStars < 2)
		{
			global.GenerateSector(1);
		}

		starButtons = GetComponentsInChildren<Button>();

		for (int x = 0; x < starButtons.Length; x++)
		{
			if (x < global.numStars && x < global.stars.Length && x < starButtons.Length)
			{
				int index = x;
				starButtons[index].GetComponent<WarpStarObj>().star = global.stars[index];
				starButtons[index].transform.localPosition = new Vector2(global.stars[index].x, global.stars[index].y);
				starButtons[index].transform.localScale = new Vector2(global.stars[index].scale / 100f, global.stars[index].scale / 100f);
				starButtons[index].onClick.AddListener(delegate { Warp(starButtons[index].GetComponent<WarpStarObj>().star); });
			}
			else
			{
				starButtons[x].gameObject.SetActive(false);
			}
		}

		initializeStars = false;
	}

	void Warp(Star star)
	{
		global.currentStar.alreadyVisited = true;
		global.currentStar = star;
		global.GenerateSector(global.currentStar.nextSectorSeed);
		global.recalculateStats = true;
		global.initializePlayerStats = true;

		Entity playerShip;
		Entity enemyShip;

		NativeArray<Entity> entityArray = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerMoveObj), typeof(ShipMovement)).ToEntityArray(Allocator.Temp);
		if (entityArray.Length > 0)
		{
			playerShip = entityArray[0];
		}
		else
		{
			Debug.LogError("Player array empty");
			return;
		}

		entityArray = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(EnemyShipObj), typeof(ShipMovement)).ToEntityArray(Allocator.Temp);
		if (entityArray.Length > 0)
		{
			enemyShip = entityArray[0];
		}
		else
		{
			Debug.LogError("Enemy array empty");
			return;
		}

		// Reset Player
		ShipStats playerStats = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShipStats>(playerShip);
		playerStats.currentHullPoints = playerStats.maxHullPoints;
		playerStats.currentArmorAmount = playerStats.maxArmorAmount;
		playerStats.currentDamageArmorAmount = 0;
		playerStats.currentShieldAmount = playerStats.maxShieldAmount;
		playerStats.shipLaserCharge = 0;
		playerStats.shipKineticCharge = 0;
		playerStats.shipMissileCharge = 0;
		playerStats.shipLaserFire = 0;
		playerStats.shipKineticFire = 0;
		playerStats.shipMissileFire = 0;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(playerShip, playerStats);

		PhysicsVelocity playerVelocity = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PhysicsVelocity>(playerShip);
		playerVelocity.Linear = float3.zero;
		playerVelocity.Angular = float3.zero;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(playerShip, playerVelocity);

		TransformAspect playerTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetAspect<TransformAspect>(playerShip);
		playerTransform.WorldPosition = float3.zero;
		playerTransform.WorldRotation = quaternion.EulerXYZ(float3.zero);

		// Reset Enemy
		ShipStats enemyStats = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShipStats>(enemyShip);
		enemyStats.currentHullPoints = playerStats.maxHullPoints;
		enemyStats.currentArmorAmount = playerStats.maxArmorAmount;
		enemyStats.currentDamageArmorAmount = 0;
		enemyStats.currentShieldAmount = playerStats.maxShieldAmount;
		enemyStats.shipLaserCharge = 0;
		enemyStats.shipKineticCharge = 0;
		enemyStats.shipMissileCharge = 0;
		enemyStats.shipLaserFire = 0;
		enemyStats.shipKineticFire = 0;
		enemyStats.shipMissileFire = 0;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(enemyShip, playerStats);

		PhysicsVelocity enemyVelocity = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PhysicsVelocity>(enemyShip);
		enemyVelocity.Linear = float3.zero;
		enemyVelocity.Angular = float3.zero;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(enemyShip, playerVelocity);

		TransformAspect enemyTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetAspect<TransformAspect>(enemyShip);
		enemyTransform.WorldPosition = new float3(100, 0, 0);
		enemyTransform.WorldRotation = quaternion.EulerXYZ(float3.zero);

		EnemyShipObj enemyShipObj = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<EnemyShipObj>(enemyShip);
		enemyShipObj.shipIsHostile = true;
		enemyShipObj.shipInitializeStats = true;
		enemyShipObj.shipFiringLaser = false;
		enemyShipObj.shipFiringKinetic = false;
		enemyShipObj.shipFiringMissile = false;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(enemyShip, enemyShipObj);

		//SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	void WarpSector()
	{
		global.ticksPassedInSector = 0;
		global.currentStar.alreadyVisited = true;
		global.GenerateSector(global.currentStar.nextSectorSeed);
		global.recalculateStats = true;
		global.initializePlayerStats = true;

		//SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}
}
