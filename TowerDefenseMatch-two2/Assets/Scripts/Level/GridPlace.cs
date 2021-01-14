using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlace : MonoBehaviour
{

    public Tower building = null;

    //public Road road = null;

    public int NumberGrid = -1;

    GridFactory originFactory;


    void Start()
    {

    }


    void Update()
    {

    }

    public void DestroyBuilding()
    {
        //if (building != null)
        {
           // building.DestroyObject();
        }
    }

    public void DestroyRoad()
    {
       // if (road != null)
        {
            //road.DestroyObject();
        }
    }
    public GridFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }
}
