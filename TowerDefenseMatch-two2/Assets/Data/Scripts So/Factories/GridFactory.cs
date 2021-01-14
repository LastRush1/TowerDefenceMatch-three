using UnityEngine;

//[CreateAssetMenu]
public class GridFactory : GameObjectFactory
{
	[SerializeField]
	GridPlace prefab = default;


	public GridPlace Get()
	{
		GridPlace instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}
	public void Reclaim(GridPlace grid)
	{
		Debug.Assert(grid.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(grid.gameObject);
	}
}
