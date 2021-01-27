using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : GameBehavior
{
	TowerController towerController;

    TargetPoint target;

	static Collider[] targetsBuffer = new Collider[1];

	const int enemyLayerMask = 1 << 9;

	[SerializeField, Range(1.5f, 10.5f)]
	float targetingRange = 1.5f;

	public float Scale { get; private set; }


	void Awake()
    {
		towerController = GetComponent<TowerController>();
    }

	public override bool GameUpdate()
	{
		if (TrackTarget() || AcquireTarget())
		{
			//Debug.Log("Locked on target!");
			towerController.Shot();
		}
		return false;
    }


    bool AcquireTarget()
	{
		Vector3 a = transform.localPosition;
		Vector3 b = a;
		b.y += 2f;
		int hits = Physics.OverlapCapsuleNonAlloc(
			a, b, targetingRange, targetsBuffer, enemyLayerMask
		);
		if (hits > 0)
		{
			target = targetsBuffer[0].GetComponent<TargetPoint>();
			Debug.Assert(target != null, "Targeted non-enemy!", targetsBuffer[0]);
			return true;
		}
		target = null;
		return false;
	}

	bool TrackTarget()
	{
		if (target == null)
		{
			return false;
		}
		Vector3 a = transform.localPosition;
		Vector3 b = target.Position;
		float x = a.x - b.x;
		float z = a.z - b.z;
		float r = targetingRange + 0.125f * target.Enemy.Scale;
		if (x * x + z * z > r * r)
		{
			target = null;
			return false;
		}
		return true;
	}
}
