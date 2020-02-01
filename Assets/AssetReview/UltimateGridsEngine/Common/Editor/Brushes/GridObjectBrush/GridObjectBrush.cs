using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps {
    [CustomGridBrush(false, true, false, "GridObject Brush")]
    [CreateAssetMenu(fileName = "New GridObject Brush", menuName = "Brushes/GridObject Brush")]
    public class GridObjectBrush : GridBrush {

        public GridObject m_GridObjectPrefab;
        protected GameObject prev_brushTarget;
        protected Vector3Int prev_position;
        public static Transform GridObjectsHolder;
        public static Transform GridTilesHolder;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if (position == prev_position) {
                return;
            }
            prev_position = position;
            if (brushTarget) {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            // Update the holders
            GetHolders(brushTarget);

            // Do not allow to place the GridObject if there is no tile at the target position
            GridTile tileAtPos = GetGridTileInCell(grid, GridTilesHolder, position);
            if (tileAtPos == null) {
                Debug.Log("There is no GridTile in that position, make sure to place one at this GridObject's position");
            }

            GridObject gridObjectAtPos = GetGridObjectInCell(grid, GridObjectsHolder, position);
            if (gridObjectAtPos != null) {
                Debug.Log("There is another GridObject at the target position");
                return;
            }

            GridObject prefab = m_GridObjectPrefab;
            GridObject instance = (GridObject)PrefabUtility.InstantiatePrefab(prefab);
            if (instance != null) {
                Undo.MoveGameObjectToScene(instance.gameObject, brushTarget.scene, "Paint GridObjectPrefab");
                Undo.RegisterCreatedObjectUndo((Object)instance, "Paint GridObjectPrefab");
                instance.transform.SetParent(GridObjectsHolder);
                var posOffSet = new Vector3(grid.cellSize.x % 2 != 0 ? 0.5f * grid.cellSize.x : 0f,
                                            grid.cellSize.y % 2 != 0 ? 0.5f * grid.cellSize.y : 0f,
                                            0f);

                instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, position.z) + posOffSet));
                instance.m_GridPosition = position.ToVector2IntXY();

                if (tileAtPos != null) {
                    instance.SetCurrentGridTile(tileAtPos);
                    instance.AddToTile(tileAtPos);
                }
            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if (brushTarget) {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            // Update holders
            GetHolders(brushTarget);

            // Check if there is a tile at the target position and erase it
            GridObject erased = GetGridObjectInCell(grid, GridObjectsHolder, position);
            if (erased != null) {
                Undo.DestroyObjectImmediate(erased.gameObject);
            }
        }

        protected virtual void GetHolders(GameObject brushTarget) {
            if (GridObjectsHolder == null) {
                GridObjectsHolder = brushTarget.transform.Find("GridObjects");
                if (GridObjectsHolder == null) {
                    GridObjectsHolder = new GameObject("GridObjects").transform;
                    GridObjectsHolder.SetParent(brushTarget.transform);
                    GridObjectsHolder.localPosition = Vector3.zero;
                }
            }

            if (GridTilesHolder == null) {
                GridTilesHolder = brushTarget.transform.Find("GridTiles");
                if (GridTilesHolder == null) {
                    GridTilesHolder = new GameObject("GridTiles").transform;
                    GridTilesHolder.SetParent(brushTarget.transform);
                    GridTilesHolder.localPosition = Vector3.zero;
                }
            }
        }

        private static GridTile GetGridTileInCell(GridLayout grid, Transform parent, Vector3Int position) {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++) {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position)) {
                    var gridTileComp = child.GetComponent<GridTile>();
                    if (gridTileComp)
                        return gridTileComp;
                }
            }

            return null;
        }

        private static GridObject GetGridObjectInCell(GridLayout grid, Transform parent, Vector3Int position) {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++) {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position)) {
                    var gridObjectComp = child.GetComponent<GridObject>();
                    if (gridObjectComp)
                        return gridObjectComp;
                }
            }

            return null;
        }
    }

    [CustomEditor(typeof(GridObjectBrush))]
    public class GridObjectBrushEditor : GridBrushEditor {
        //private GridObjectBrush gridObjectBrush { get { return target as GridObjectBrush; } }

        //private SerializedProperty m_GridTilePrefab;
        //private SerializedObject m_SerializedObject;
        //
        //protected override void OnEnable() {
        //    base.OnEnable();
        //    m_SerializedObject = new SerializedObject(target);
        //    m_GridTilePrefab = m_SerializedObject.FindProperty("m_GridTilePrefab");
        //}

        // Draw the current Coordinates
        public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing) {
            base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);

            var labelText = "Pos: " + position.position;
            if (position.size.x > 1 || position.size.y > 1) {
                labelText += " Size: " + position.size;
            }

            Handles.Label(grid.CellToWorld(position.position), labelText);
        }
    }
}

