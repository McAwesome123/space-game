using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Global : IComponentData
{
	public const int tickRate = 100;

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
