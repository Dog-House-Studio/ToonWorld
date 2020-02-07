using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanUnit : BaseUnitClass
{
    public int movementAmount;

    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(dir);

        Dictionary<GridTile,float> Costs = new Dictionary<GridTile,float>() ;
        HashSet<GridTile> UI = new HashSet<GridTile>();
        HashSet<GridTile> tempUI = new HashSet<GridTile>();
        HashSet<GridTile> final = new HashSet<GridTile>();

        GridTile start = GridManager.GetGridTileAtPosition(gridPoint);
        final.Add(start);
        List<GridTile> tempTiles = GridManager.Instance.Neighbors(start);
        foreach (GridTile neighbor in tempTiles)
        {
            Costs.Add(neighbor, neighbor.m_costOfMovingToTile);
            if(movementAmount - Costs[neighbor] >= 0)
            {
                UI.Add(neighbor);
            }
        }
        final.UnionWith(UI);
        while (UI.Count > 0)
        {
            foreach(GridTile g in UI)
            {
                foreach (GridTile neighbor in GridManager.Instance.Neighbors(g))
                {
                    if(!final.Contains(neighbor))
                    {
                        if(!Costs.ContainsKey(neighbor))
                        {
                            float cost = neighbor.m_costOfMovingToTile + Costs[g];
                            Costs.Add(neighbor, cost);
                            if (movementAmount - Costs[neighbor] >= 0)
                            {
                                tempUI.Add(neighbor);
                            }
                        }
                        
                    } 
                }
            }
            
            UI = tempUI;
            final.UnionWith(UI);
            tempUI = new HashSet<GridTile>();
        }

        foreach(GridTile tile in final)
        {
            Vector2Int position = tile.m_GridPosition;
            locations.Add(position);
        }
        return locations;
    }

}
