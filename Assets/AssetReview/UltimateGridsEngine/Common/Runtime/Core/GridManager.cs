using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Grid))]
public class GridManager : Singleton<GridManager> {
    [Header("Grid Settings")]
    public Grid m_Grid;         // Reference to the current Grid being used
    public bool m_UsesHeight = false;   // Wether or not we should use height for the pathfinding

    [Header("Hovered Tile")]
    public GridTile m_HoveredGridTile = null;   // Currently highlighted GridTile

    // Grid tiles dictionary
    public static Dictionary<Vector2Int, GridTile> m_GridTiles = new Dictionary<Vector2Int, GridTile>();

    // Grid objects list
    public static List<GridObject> m_GridObjects = new List<GridObject>();

    // List of the default neighbors
    public List<Vector2Int> m_DefaultNeighbors = new List<Vector2Int>();
    
    protected virtual void Reset() {
        m_Grid = GetComponent<Grid>();
    }

    [ContextMenu("Set Default Rectangle Neighbors")]
    public virtual void SetDefaultRectNeighbours() {
        m_DefaultNeighbors = defaultRectangleDirections;
    }

    [ContextMenu("Set Default Hexagonal Neighbors")]
    public virtual void SetDefaultHexNeighbours() {
        m_DefaultNeighbors = defaultHexagonalDirections;
    }

    // Adds the tile to the tile dictionary if the position is not already occupied
    public static void AddGridTile(Vector2Int gridPosition, GridTile gridTile) {
        if (!ExistsTileAtPosition(gridPosition)) {
            m_GridTiles.Add(gridPosition, gridTile);
        }
    }


    // Removes the target tile from the tile dictionary
    public static void RemoveGridTile(GridTile gridTile) {
        if (m_GridTiles.ContainsValue(gridTile)) {
            m_GridTiles.Remove(gridTile.m_GridPosition);
        }
    }

    // Removes the tile at the target position
    public static void RemoveGridTileAtPosition(Vector2Int gridPosition) {
        if (m_GridTiles.ContainsKey(gridPosition))
            m_GridTiles.Remove(gridPosition);

    }

    // Checks if a tile exists at the target position
    public static bool ExistsTileAtPosition(Vector2Int gridPosition) {
        return (m_GridTiles.ContainsKey(gridPosition));
    }

    // Returns the tile at the target position, if there is one
    public static GridTile GetGridTileAtPosition(Vector2Int gridPosition) {
        if (!ExistsTileAtPosition(gridPosition)) {
            return null;
        }

        return m_GridTiles[gridPosition];
    }

    // Sets the currently hovered tile
    public void SetHoveredTile(GridTile gridTile) {
        m_HoveredGridTile = gridTile;
    }

    // Unsets the currently hovered tile
    public void UnsetHoveredTile(GridTile gridTile) {
        if (m_HoveredGridTile == gridTile)
            m_HoveredGridTile = null;
    }

    // Adds a GridObject to the GridObject List
    public void AddGridObject(GridObject gridObject) {
        if (!m_GridObjects.Contains(gridObject)) {
            m_GridObjects.Add(gridObject);
        }
    }

    // Removes a GridObject to the GridObject List
    public void RemoveGridObject(GridObject gridObject) {
        if (m_GridObjects.Contains(gridObject)) {
            m_GridObjects.Remove(gridObject);
        }
    }

    public static GridObject GetGridObjectAtPosition(Vector2Int pos) {
        foreach (GridObject gObj in m_GridObjects) {
            if (gObj.m_GridPosition == pos) {
                return gObj;
            }
        }

        return null;
    }

    // Default rectangle directions 
    public static List<Vector2Int> defaultRectangleDirections = new List<Vector2Int>() {
        new Vector2Int( -1, 0 ), // left
        new Vector2Int( 0, 1 ),  // top
        new Vector2Int( 1, 0 ),  // right
        new Vector2Int( 0, -1 ) // bottom
    };

    // Desfault hexagonal directions
    public static List<Vector2Int> defaultHexagonalDirections = new List<Vector2Int>() {
        new Vector2Int( -1, 0 ), // left
        new Vector2Int( 0, 1 ),  // top 
        new Vector2Int( 1, 0 ),  // right
        new Vector2Int( 0, -1 ), // bottom
        new Vector2Int(-1, 1 ),  // top-left, comment it out for 4-direction movement
        new Vector2Int( -1, -1 ) // bottom-left, comment it out for 4-direction movement
    };

    // Returns the neighbor of the tile at the target position, if there is one
    public virtual GridTile NeighborAtPosition(GridTile gridTile, Vector2Int gridPosition) {
        var neighbors = Neighbors(gridTile);

        foreach (GridTile tile in neighbors) {
            if (tile.m_GridPosition == gridPosition) {
                return tile;
            }
        }

        return null;
    }

    // Returns a list with the neighbors of the tile
    public virtual List<GridTile> Neighbors(GridTile gridTile) {
        List<GridTile> results = new List<GridTile>();

        foreach (Vector2Int dir in m_DefaultNeighbors) {
            Vector2Int newVector = dir + gridTile.m_GridPosition;
            if (ExistsTileAtPosition(newVector)) {
                GridTile targetTile = GetGridTileAtPosition(newVector);

                if (targetTile != null) {
                    if (targetTile.CanMoveToTile() /*|| targetTile == goalTile*/) {
                        if (m_UsesHeight) {
                            if (Mathf.Abs(Mathf.Abs(gridTile.m_TileHeight) - Mathf.Abs(targetTile.m_TileHeight)) <= 1) {
                                results.Add(targetTile);
                            }
                        } else {
                            results.Add(targetTile);
                        }
                    }
                }
            }
        }

        // Add manual neighbors to the result
        foreach (GridTile tile in gridTile.m_manualNeighbors) {
            if (!results.Contains(tile)) {
                results.Add(tile);
            }
        }

        return results;
    }
}