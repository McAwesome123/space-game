using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public readonly partial struct PlayerMoveAspect : IAspect
{
	private readonly RefRW<PlayerMoveObj> playerMove;
	private readonly RefRW<ShipMovement> shipMovement;

	public void PlayerMove()
	{

	}
}
