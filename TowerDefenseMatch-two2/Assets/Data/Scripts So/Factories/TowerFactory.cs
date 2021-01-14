using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class TowerFactory : GameObjectFactory
{
	public Tower Get(Tower prefab)
	{
		Tower instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		instance.Initialize();
		return instance;
	}
	public void Reclaim(Tower tower)
	{
		Debug.Assert(tower.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(tower.gameObject);
	}
}
