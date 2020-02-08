using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// RadialMovementType is a script that contains
    /// the logic for the radial movement type. This is
    /// a type that moves in equally radial movement patterns.
    /// </summary>
    [CreateAssetMenu(menuName = "Dog House/ToonWorld/Unit/Movement/Radial", fileName = "MyNewRadialMovement")]
    public class RadialMovementType : BaseMovementType
    {
        #region Main Methods
        public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
        {
            List<Vector2Int> locations = new List<Vector2Int>();

            Dictionary<GridTile, float> Costs = new Dictionary<GridTile, float>();
            HashSet<GridTile> UI = new HashSet<GridTile>();
            HashSet<GridTile> tempUI = new HashSet<GridTile>();
            HashSet<GridTile> final = new HashSet<GridTile>();

            GridTile start = GridManager.GetGridTileAtPosition(gridPoint);
            final.Add(start);

            foreach (GridTile neighbor in GridManager.Instance.Neighbors(start))
            {
                Costs.Add(neighbor, neighbor.m_costOfMovingToTile);
                if (m_movementAmount - Costs[neighbor] >= 0)
                {
                    UI.Add(neighbor);
                }
            }
            final.UnionWith(UI);
            while (UI.Count > 0)
            {
                foreach (GridTile g in UI)
                {
                    foreach (GridTile neighbor in GridManager.Instance.Neighbors(g))
                    {
                        if (!final.Contains(neighbor))
                        {
                            if (!Costs.ContainsKey(neighbor))
                            {
                                float cost = neighbor.m_costOfMovingToTile + Costs[g];
                                Costs.Add(neighbor, cost);
                                if (m_movementAmount - Costs[neighbor] >= 0)
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

            foreach (GridTile tile in final)
            {
                Vector2Int position = tile.m_GridPosition;
                locations.Add(position);
            }
            return locations;
        }
        #endregion
    }
}
