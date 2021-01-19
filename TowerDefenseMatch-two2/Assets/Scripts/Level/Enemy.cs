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

    public void Initialize()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
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
