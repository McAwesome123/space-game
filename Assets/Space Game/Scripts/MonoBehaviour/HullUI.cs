using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HullUI : MonoBehaviour
{
	public Slider hullSlider;
	public TMP_Text hullText;
	public Entity playerShip;
	// public RectTransform fillArea;

	// Start is called before the first frame update
	void Start()
	{
		hullSlider = GetComponentInChildren<Slider>();
		hullText = GetComponentInChildren<TMP_Text>();
		NativeArray<Entity> entityArray = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerMoveObj), typeof(ShipStats)).ToEntityArray(Allocator.Temp);
		if (entityArray.Length > 0)
		{
			playerShip = entityArray[0];
		}
	}

	// Update is called once per frame
	void Update()
	{
		float maxHull = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShipStats>(playerShip).maxHullPoints;
		float hull = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShipStats>(playerShip).currentHullPoints;
		hullSlider.maxValue = maxHull;
		hullSlider.value = hull;
		maxHull = math.round(maxHull * 10);
		hull = math.round(hull * 10);
		hullText.text = string.Format("Hull Points: {0}{1}.{2} / {3}{4}.{5}", hull < 0 ? "-" : "", math.trunc(math.abs(hull) / 10), math.trunc(math.abs(hull) % 10), maxHull < 0 ? "-" : "", math.trunc(math.abs(maxHull) / 10), math.trunc(math.abs(maxHull) % 10));
	}
}
