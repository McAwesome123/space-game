using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Star
{
	public int x = 0;
	public int y = 0;
	public int scale = 100;
	public bool alreadyVisited = false;
	public bool canJumpSector = false;
	public uint nextSectorSeed = 1;
	public Unity.Mathematics.Random random = new();

	public Star()
	{
		random = new Unity.Mathematics.Random(1);
	}

	public Star(int x, int y, int scale)
	{
		this.x = x;
		this.y = y;
		this.scale = scale;
	}
}

public class Global : MonoBehaviour
{
	public const int tickRate = 100;
	public const int minNumStars = 28;
	public const int maxNumStars = 35;

	public int numStars;
	public Star[] stars;
	public Star currentStar = new();

	public bool recalculateStats;
	public bool initializePlayerStats;

	public int playerResources;
	public int playerSectorsPassed;
	public byte gamePaused;

	public float currentHullPoints;
	public float currentArmorAmount;
	public float currentShieldAmount;

	public int playerEngineUpgrades;
	public int playerArmorUpgrades;
	public int playerShieldUpgrades;
	public int playerLaserUpgrades;
	public int playerKineticUpgrades;
	public int playerMissileUpgrades;

	public int playerEngineUpgradeCost;
	public int playerArmorUpgradeCost;
	public int playerShieldUpgradeCost;
	public int playerLaserUpgradeCost;
	public int playerKineticUpgradeCost;
	public int playerMissileUpgradeCost;

	public float shipLaserDamage;
	public float shipLaserCooldown;
	public float shipLaserShotSpeed;
	public float shipKineticDamage;
	public float shipKineticCooldown;
	public float shipKineticShotSpeed;
	public float shipMissileDamage;
	public float shipMissileCooldown;
	public float shipMissileShotSpeed;

	public ulong ticksPassed;
	public ulong ticksPassedInSector;

	private int heldCancelKey;

	public enum PauseTypes
	{
		NoPause =	0b00000000,
		PlayerPause =	0b00000001,
		MenuPause =	0b00000010,
		CutscenePause =	0b00000100,
	};

	private void Start()
	{
		Debug.Log("Global Initialized");
		DontDestroyOnLoad(this.gameObject);

		numStars = 0;
		stars = new Star[maxNumStars];
		currentStar = new();
		currentStar.random = new Unity.Mathematics.Random(1);

		recalculateStats = true;
		initializePlayerStats = true;

		playerResources = BaseShipStats.shipStartingResources;
		playerSectorsPassed = 0;
		gamePaused = 0;

		currentHullPoints = BaseShipStats.baseShipHull;
		currentArmorAmount = BaseShipStats.baseShipArmor;
		currentShieldAmount = BaseShipStats.baseShipShield;

		playerEngineUpgrades = BaseShipStats.baseShipEngineUpgrades;
		playerArmorUpgrades = BaseShipStats.baseShipArmorUpgrades;
		playerShieldUpgrades = BaseShipStats.baseShipShieldUpgrades;
		playerLaserUpgrades = BaseShipStats.baseShipLaserUpgrades;
		playerKineticUpgrades = BaseShipStats.baseShipKineticUpgrades;
		playerMissileUpgrades = BaseShipStats.baseShipMissileUpgrades;

		playerEngineUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerArmorUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerShieldUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerLaserUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerKineticUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);
		playerMissileUpgradeCost = (int)math.round(BaseShipStats.shipBaseUpgradeCost);

		shipLaserDamage = BaseShipStats.baseShipLaserDamage;
		shipLaserCooldown = BaseShipStats.baseShipLaserCooldown;
		shipLaserShotSpeed = BaseShipStats.baseShipLaserShotSpeed;
		shipKineticDamage = BaseShipStats.baseShipKineticDamage;
		shipKineticCooldown = BaseShipStats.baseShipKineticCooldown;
		shipKineticShotSpeed = BaseShipStats.baseShipKineticShotSpeed;
		shipMissileDamage = BaseShipStats.baseShipMissileDamage;
		shipMissileCooldown = BaseShipStats.baseShipMissileCooldown;
		shipMissileShotSpeed = BaseShipStats.baseShipMissileShotSpeed;

		ticksPassed = 0;
		ticksPassedInSector = 0;

		heldCancelKey = 0;
	}

	private void FixedUpdate()
	{
		if (Input.GetAxis("Cancel") != 0)
		{
			heldCancelKey++;
		}
		else
		{
			heldCancelKey = 0;
		}

		if (heldCancelKey > 3 * tickRate)
		{
			Application.Quit();
		}

		if (gamePaused != 0)
		{
			return;
		}

		if (ticksPassed < ulong.MaxValue)
			ticksPassed++;

		if (ticksPassedInSector < ulong.MaxValue)
			ticksPassedInSector++;
	}

	public void GenerateSector(uint seed)
	{
		const int minX = -270;
		const int maxX = 270;
		const int startX = -260;
		const int minY = -90;
		const int maxY = 130;
		const int startY = -80;
		const int stepX = 40;
		const int stepY = 40;
		const int stepsX = 14;
		const int stepsY = 6;
		const int minVarianceX = -15;
		const int maxVarianceX = 15;
		const int minVarianceY = -15;
		const int maxVarianceY = 15;
		const int scaleVariance = 40;
		const int minScale = 60;
		const int maxScale = 120;
		const int normalScale = 100;

		Unity.Mathematics.Random random = new(seed);

		numStars = random.NextInt(minNumStars, maxNumStars + 1);

		for (int x = 0; x < numStars; x++)
		{
			bool isUnique = false;
			for (int loop = 0; loop < 1000 && !isUnique; loop++)
			{
				isUnique = true;
				int scale = random.NextInt(normalScale - scaleVariance, normalScale + scaleVariance + 1);
				if (scale > normalScale)
				{
					scale = (int)(normalScale + (scale - normalScale) * ((maxScale - normalScale) / (float)scaleVariance)); // I love integer division (/s)
				}
				else
				{
					scale = (int)(normalScale + (scale - normalScale) * ((normalScale - minScale) / (float)scaleVariance)); // Same as above
				}
				stars[x] = new Star(random.NextInt(0, stepsX) * stepX + startX,
							random.NextInt(0, stepsY) * stepY + startY,
							scale);
				for (int y = 0; y < x; y++)
				{
					if (stars[y].x == stars[x].x && stars[y].y == stars[x].y)
					{
						isUnique = false;
						break;
					}
				}
				if (loop == 999)
				{
					Debug.LogError("Too many loops!");
				}
			}
			stars[x].random = new Unity.Mathematics.Random(random.NextUInt() + 1);
		}
		for (int x = 0; x < numStars; x++)
		{
			stars[x].x += random.NextInt(minVarianceX, maxVarianceX + 1);
			stars[x].y += random.NextInt(minVarianceY, maxVarianceY + 1);

			if (stars[x].x > maxX)
			{
				stars[x].x = maxX;
			}
			else if(stars[x].x < minX)
			{
				stars[x].x = minX;
			}
			if (stars[x].y > maxY)
			{
				stars[x].y = maxY;
			}
			else if (stars[x].y < minY)
			{
				stars[x].y = minY;
			}

			if (x == 0)
			{
				currentStar = stars[x];
			}
			else if (x <= 3)
			{
				stars[x].canJumpSector = random.NextFloat() < 1.0f / x;
				stars[x].nextSectorSeed = random.NextUInt() + 1;
			}
		}

		Debug.Log(string.Format("Generated sector with seed {0}", seed));
	}

	// This is a slightly modified version of public static Vector3 operator *(Quaternion rotation, Vector3 point)
	public static double3 ApplyRotation(quaternion rotation, double3 point)
	{
		double num = rotation.value.x * 2f;
		double num2 = rotation.value.y * 2f;
		double num3 = rotation.value.z * 2f;
		double num4 = rotation.value.x * num;
		double num5 = rotation.value.y * num2;
		double num6 = rotation.value.z * num3;
		double num7 = rotation.value.x * num2;
		double num8 = rotation.value.x * num3;
		double num9 = rotation.value.y * num3;
		double num10 = rotation.value.w * num;
		double num11 = rotation.value.w * num2;
		double num12 = rotation.value.w * num3;
		double3 result = default;
		result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
		result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
		result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
		return result;
	}
}
