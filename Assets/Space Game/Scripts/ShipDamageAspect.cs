using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public readonly partial struct ShipDamageEnemyAspect : IAspect
{
	public readonly RefRW<ShipStats> shipStats;
	public readonly RefRO<EnemyShipObj> enemy;

	public void TakeDamageEnemy(float damage, bool isLaser)
	{
		float dealtDamage = damage;

		if (isLaser)
		{
			dealtDamage *= math.pow(BaseShipStats.shipShieldProtection, shipStats.ValueRO.currentShieldAmount);
			shipStats.ValueRW.currentShieldAmount -= (damage - dealtDamage) * BaseShipStats.shipShieldAbsorptionFactor;
		}

		dealtDamage *= math.pow(BaseShipStats.shipArmorProtection, shipStats.ValueRO.currentArmorAmount);

		shipStats.ValueRW.currentHullPoints -= dealtDamage;
	}
}

public readonly partial struct ShipDamagePlayerAspect : IAspect
{
	public readonly RefRW<ShipStats> shipStats;
	public readonly RefRO<PlayerMoveObj> player;

	public void TakeDamagePlayer(float damage, bool isLaser)
	{
		float dealtDamage = damage;

		if (isLaser)
		{
			dealtDamage *= math.pow(BaseShipStats.shipShieldProtection, shipStats.ValueRO.currentShieldAmount);
			shipStats.ValueRW.currentShieldAmount -= (damage - dealtDamage) * BaseShipStats.shipShieldAbsorptionFactor;
		}

		dealtDamage *= math.pow(BaseShipStats.shipArmorProtection, shipStats.ValueRO.currentArmorAmount + shipStats.ValueRO.currentDamageArmorAmount);

		bool tooMuchDamage = false;
		if (dealtDamage > shipStats.ValueRO.currentHullPoints * BaseShipStats.shipPlayerDestructionMinDamageMult)
		{
			tooMuchDamage = true;
		}

		float damageLeft = dealtDamage;
		for (; damageLeft > BaseShipStats.shipPlayerDamageTick && shipStats.ValueRO.currentHullPoints > BaseShipStats.shipPlayerDamageTick; )
		{
			float addDamageArmor = BaseShipStats.shipPlayerArmorAdd * BaseShipStats.shipPlayerDamageTick / shipStats.ValueRO.currentHullPoints / BaseShipStats.shipPlayerArmorAddPerHullDamagePercent;
			shipStats.ValueRW.currentHullPoints -= BaseShipStats.shipPlayerDamageTick;
			damageLeft -= BaseShipStats.shipPlayerDamageTick;
			damage *= math.pow(BaseShipStats.shipArmorProtection, addDamageArmor);
			shipStats.ValueRW.currentDamageArmorAmount += addDamageArmor;
		}

		if (shipStats.ValueRO.currentHullPoints <= damageLeft)
		{
			damageLeft -= shipStats.ValueRO.currentHullPoints;
			shipStats.ValueRW.currentHullPoints = 0;

			if (tooMuchDamage)
			{
				shipStats.ValueRW.currentHullPoints -= damageLeft;
			}
		}
		else
		{
			float addDamageArmor = BaseShipStats.shipPlayerArmorAdd * damageLeft / shipStats.ValueRO.currentHullPoints / BaseShipStats.shipPlayerArmorAddPerHullDamagePercent;
			shipStats.ValueRW.currentHullPoints -= damageLeft;
			shipStats.ValueRW.currentDamageArmorAmount += addDamageArmor;
		}
	}
}
