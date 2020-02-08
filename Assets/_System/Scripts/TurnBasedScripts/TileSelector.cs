using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class TileSelector : MonoBehaviour
    {
        public Vector2Int currentPos;
        private GridTile targetTile;
        private Vector2Int moveDir;
        // Start is called before the first frame update
        void Start()
        {
            moveDir = new Vector2Int(0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            if (moveDir != new Vector2Int(0, 0))
            {
                MoveSelector();
            }
        }

        private void MoveSelector()
        {
            targetTile = GridManager.GetGridTileAtPosition(currentPos + moveDir);

            if (targetTile != null)
            {
                GridManager.Instance.UnsetHoveredTile(GridManager.GetGridTileAtPosition(currentPos));
                currentPos += moveDir;
                GridManager.Instance.SetHoveredTile(GridManager.GetGridTileAtPosition(currentPos));
                transform.position = GridManager.GetGridTileAtPosition(currentPos).m_WorldPosition;
            }
            moveDir = new Vector2Int(0, 0);
            targetTile = null;
        }

        private void GetInput()
        {
            //Switch to the input service implementation.
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveDir = new Vector2Int(1, 0);
                return;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveDir = new Vector2Int(-1, 0);
                return;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveDir = new Vector2Int(0, -1);
                return;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDir = new Vector2Int(0, 1);
                return;
            }

        }
    }
}

