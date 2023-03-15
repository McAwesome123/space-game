using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

public struct SceneLoader : IComponentData
{
	//public EntitySceneReference sceneReference;
}

public class SceneLoaderAuthoring : MonoBehaviour
{
	//public UnityEditor.SceneAsset scene;
}

public class SceneLoaderBaker : Baker<SceneLoaderAuthoring>
{
	public override void Bake(SceneLoaderAuthoring authoring)
	{
		//var reference = new EntitySceneReference(authoring.scene);
		AddComponent(new SceneLoader
		{
			//sceneReference = reference,
		});
	}
}
