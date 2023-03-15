using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct LaserWeapon : IComponentData
{
	void FireLaserWeapon()
	{
		Debug.Log("Hello");
	}
}

public class LaserWeaponAuthoring : MonoBehaviour
{

}
