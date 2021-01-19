using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class WarFactory : GameObjectFactory
{
	//[SerializeField]
	//Explosion explosionPrefab = default;

	[SerializeField]
	Shell shellPrefab = default;

	//[SerializeField]
	//Arow arowPrefab = default;

	//public Explosion _Explosion => Get(explosionPrefab);

	public Shell _Shell => Get(shellPrefab);

	//public Arow _Arow => Get(arowPrefab);



	public T Get<T>(T prefab) where T : WarEntity
	{
		T instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}

	public void Reclaim(WarEntity entity)
	{
		Debug.Assert(entity.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(entity.gameObject);
	}
}
