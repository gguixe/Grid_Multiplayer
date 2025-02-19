//DESC: Example of grid objects
//NAME: Gerard Guix�
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class GridObjects : MonoBehaviour
{
    //USED FOR TESTING GRID OBJECTS.
    //[SerializeField] private HeatMapVisual heatMapVisual;
    //[SerializeField] private HeatMapVisualBool heatMapVisualBool;
    [SerializeField] private HeatMapVisualGeneric heatMapVisualGeneric;
    private Grid<HeatMapGridObject> grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid<HeatMapGridObject>(3, 3, 10f, new Vector3(-20, -20), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        //heatMapVisual.SetGrid(grid);
        //heatMapVisualBool.SetGrid(grid);
        heatMapVisualGeneric.SetGrid(grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            //OBJECT
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
            }
            //BOOL
            //grid.SetValue(position, true);
            //INT
            //int value = grid.GetValue(position);
            //grid.SetValue(position, value + 5);
        }
    }
}

public class HeatMapGridObject //Heatmap object for grid
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;
    public int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += Mathf.Clamp(addValue, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float getValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

public class StringGridObject //Object to add text into grid
{
    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;

    private string letters;
    private string numbers;

    public StringGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = "";
    }

    public void AddLetter(string letter)
    {
        letters += letter;
    }

    public void AddNumber(string number)
    {
        numbers += number;
    }

    public override string ToString()
    {
        return letters + "\n" + numbers;
    }
}
