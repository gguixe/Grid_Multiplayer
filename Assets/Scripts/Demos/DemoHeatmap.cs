//DESC: Demo for heatmap
//NAME: Gerard Guixé
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class DemoHeatmap : MonoBehaviour
{
    [SerializeField] private HeatMapVisualGeneric heatMapVisualGeneric;
    private Grid<HeatMapGridObject> grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid<HeatMapGridObject>(10, 10, 5f, Vector3.zero, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        heatMapVisualGeneric.SetGrid(grid);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if(heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
            }
        }   
    }
}
