using UnityEngine;

public abstract class WarEntity : GameBehavior
{

	WarFactory originFactory;

	[SerializeField]
	GameObject model;

	public GameObject Model
	{
		get { return model; }
	}

	protected float damage = 0;

	public float GetDamage
    {
		get { return damage; }
    }

	public void SetDamage(float damage)
	{
		this.damage = damage;
	}

	public WarFactory OriginFactory
	{
		get => originFactory;
		set
		{
			Debug.Assert(originFactory == null, "Redefined origin factory!");
			originFactory = value;
		}
	}

	public void Recycle()
	{
		originFactory.Reclaim(this);
	}
}