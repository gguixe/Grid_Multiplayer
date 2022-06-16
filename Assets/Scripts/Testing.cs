using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
//TUTORIAL HELP: https://www.youtube.com/watch?v=waEsGu--9P8&t=111s


public class Testing : MonoBehaviour
{

    private Grid grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid(4, 2, 10f); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56); //For 3D camera GetMouseWorldPositionWithZ
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }

}
