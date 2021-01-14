using UnityEngine;

//[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{

	[SerializeField]
	Enemy[] prefab = default;


	public Enemy Get()
	{
		Enemy instance = CreateGameObjectInstance(prefab[0]);
		instance.OriginFactory = this;
		instance.Initialize();
		return instance;
	}
	public void Reclaim(Enemy enemy)
	{
		Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(enemy.gameObject);
	}

}
