using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	Entity cameraFollowEntity;

	// Start is called before the first frame update
	void Start()
	{
		EntityQuery cameraFollowQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(CameraFollowObject));
		NativeArray<Entity> cameraFollowArray = cameraFollowQuery.ToEntityArray(Allocator.Temp);
		if (cameraFollowArray.Length > 0)
		{
			cameraFollowEntity = cameraFollowArray[0];
		}
		else
		{
			cameraFollowEntity = Entity.Null;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (cameraFollowEntity != null)
		{
			LocalToWorld localToWorldTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalToWorld>(cameraFollowEntity);
			transform.SetPositionAndRotation(localToWorldTransform.Position, localToWorldTransform.Rotation);
		}
	}
}
