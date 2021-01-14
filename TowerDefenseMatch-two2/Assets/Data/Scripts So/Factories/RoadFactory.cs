using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoadFactory : GameObjectFactory
{
	[SerializeField]
	Road prefab = default;


	public Road Get()
	{
		Road instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}
	public void Reclaim(Road road)
	{
		Debug.Assert(road.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(road.gameObject);
	}
}
