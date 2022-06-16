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
        grid = new Grid(3, 3, 10f, new Vector3(-20, -20)); 
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

    //private class HeatMapVisual
    //{
    //    private Grid grid;
    //    private Mesh mesh;

    //    public HeatMapVisual(Grid grid, MeshFilter meshFilter)
    //    {
    //        this.grid = grid;

    //        mesh = new Mesh();

    //        UpdateHeatMapVisual();

    //        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    //    }
    //}

    //private void Grid_OnGridValueChanged(object sender, System.EventArgs e)
    //{
    //    UpdateHeatMapVisual();
    //}

    //public void UpdateHeatMapVisual()
    //{
    //    Vector3[] vertices;
    //    Vector2[] uv;
    //    int[] triangles;

    //    MeshUtils.CreateEmptyMeshArray(grid.GetWidth() * grid.GetHeight(), out vertices, out uv, out triangles);

    //    for(int x = 0; x < grid.GetWidth(); x++)
    //    {
    //        for (int y = 0; y < grid.GetHeight(); y++)
    //        {
    //            int index = x * grid.GetHeight() + y;
    //            Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();
    //            int gridValue = grid.GetValue(x, y);
    //            int maxGridValue = 100;
    //            float gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
    //            Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);
    //            MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + baseSize * .5f, 0f, baseSize, gridCellUV, Vector3.zero);

    //        }
    //    }

    //    mesh.vertices = vertices;
    //    mesh.uv = uv;
    //    mesh.triangles = triangles;
    //}

}
