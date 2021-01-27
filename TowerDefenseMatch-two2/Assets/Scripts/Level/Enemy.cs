using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;

    [SerializeField]
    GameObject model;


    public GameObject Model
    {
        get { return model; }
    }

    float scale = 1;

    public float Scale
    {
        get { return scale; }
    }


    [SerializeField]
    BoxCollider boxCollider;

    public BoxCollider BoxCollider
    {
        get { return boxCollider; }
    }

    [SerializeField]
    SphereCollider sphereCollider;

    public SphereCollider SphereCollider
    {
        get { return sphereCollider; }
    }

    public void Initialize()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    float damage = 0;

    public void TakeDamage(float damage)
    {
        this.damage += damage;
    }

    public float GetDamage()
    {
        float data = damage;
        damage = 0;
        return data;
    }

    List<GameObject> ammo = new List<GameObject>();

    public void WhatAmmo(GameObject ammo)
    {
        this.ammo.Add(ammo);
    }

    public List<GameObject> AmmoData()
    {
        List<GameObject> data = ammo;
        ammo.Clear();
        return data;
    }

    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }


}
