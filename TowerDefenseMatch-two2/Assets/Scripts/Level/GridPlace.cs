using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlace : MonoBehaviour
{

    public Tower building = null;

    //public Road road = null;

    public int NumberGrid = -1;
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
}
