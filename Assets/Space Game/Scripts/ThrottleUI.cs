using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ThrottleUI : MonoBehaviour
{
	public Slider throttleSlider;
	public TMP_Text throttleText;
	public Entity playerShip;

	// Start is called before the first frame update
	void Start()
	{
		throttleSlider = GetComponentInChildren<Slider>();
		throttleText = throttleSlider.GetComponentInChildren<TMP_Text>();
		NativeArray<Entity> entityArray = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerMoveObj), typeof(ShipMovement)).ToEntityArray(Allocator.Persistent);
		if (entityArray.Length > 0)
		{
			playerShip = entityArray[0];
		}
	}

	// Update is called once per frame
	void Update()
	{
		float throttle = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShipMovement>(playerShip).currentAccelerationMult * 100;
		throttleSlider.value = throttle;
		throttle = math.round(throttle * 10);
		throttleText.text = string.Format("{2}{0}.{1}%", math.trunc(math.abs(throttle) / 10), math.trunc(math.abs(throttle) % 10), throttle < 0 ? "-" : "");
	}
}
