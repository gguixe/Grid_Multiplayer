using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder 
{
    public List<OverlayTile> GetTilesInRange(OverlayTile startingTile, int range) //GET ALL THE TILES IN OUR RANGE
    {
        var InRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        InRangeTiles.Add(startingTile);

        var tileforPreviousStep = new List<OverlayTile>();
        tileforPreviousStep.Add(startingTile);

        while(stepCount<range)
        {
            var surrodingTiles = new List<OverlayTile>();

            foreach (var item in tileforPreviousStep)
            {
                surrodingTiles.AddRange(MapManager.Instance.GetNeighbourTiles(item, new List<OverlayTile>()));
            }

            InRangeTiles.AddRange(surrodingTiles);
            tileforPreviousStep = surrodingTiles.Distinct().ToList();
            stepCount++;
        }

        return InRangeTiles.Distinct().ToList();
    }
}
