using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class CombatMoverSelector : MonoBehaviour
    {
        public GameObject m_MoveLocationPrefab;
        public GameObject m_AttackLocationPrefab;

        protected GridObject _movingPiece;
        protected CombatUnitSelector _unitSelector;
        protected List<Vector2Int> _moveLocations;
        protected List<GameObject> _locationHighlights;

        private ServiceReference<ICombatManager> CombatManager = new ServiceReference<ICombatManager>();
        // Start is called before the first frame update
        protected void Start()
        {
            _unitSelector = GetComponent<CombatUnitSelector>();
            enabled = false;
        }

        protected void Update()
        {
            if (!CombatManager.CheckServiceRegistered())
            {
                return;
            }

            if (!Input.GetMouseButtonDown(0) && !Input.GetButtonDown("Submit"))
            {
                return;
            }

            if (GridManager.Instance.m_HoveredGridTile == null)
            {
                return;
            }

            if (CombatManager.Reference.FriendlyUnitAt(GridManager.Instance.m_HoveredGridTile.m_GridPosition))
            {
                SwapPiece(CombatManager.Reference.TrySelectUnitAtTile(GridManager.Instance.m_HoveredGridTile));
                return;
            }

            // Return is the target position is invalid
            if (!_moveLocations.Contains(GridManager.Instance.m_HoveredGridTile.m_GridPosition))
            {
                DeselectMovementPiece();
                return;
            }

            // Normal movement, just check if there is no GridObject at the target position
            if (GridManager.GetGridObjectAtPosition(GridManager.Instance.m_HoveredGridTile.m_GridPosition) == null)
            {
                _movingPiece.GetComponent<GridPathfinder>().SetNewDestination(GridManager.Instance.m_HoveredGridTile);
            }

            else
            {
                // Capture the piece
                ChessGameManager.Instance.CapturePieceAt(GridManager.Instance.m_HoveredGridTile.m_GridPosition);

                _movingPiece.GetComponent<GridMovement>().TryMoveTo(GridManager.Instance.m_HoveredGridTile, false, false, false);
            }

            ExitState();
        }

        public void EnterState(GridObject piece)
        {
            _movingPiece = piece;
            enabled = true;

            _moveLocations = CombatManager.Reference.MovesForUnit(_movingPiece);
            _locationHighlights = new List<GameObject>();

            foreach (Vector2Int position in _moveLocations)
            {
                //Highlight move and attack/capture positions
                GameObject highlight;
                var targetWorldPosition = GridManager.GetGridTileAtPosition(position).m_WorldPosition;
                if (GridManager.GetGridObjectAtPosition(position))
                {
                    highlight = Instantiate(m_AttackLocationPrefab, targetWorldPosition, Quaternion.identity, gameObject.transform);
                }
                else
                {
                    highlight = Instantiate(m_MoveLocationPrefab, targetWorldPosition, Quaternion.identity, gameObject.transform);
                }
                _locationHighlights.Add(highlight);
            }
        }

        protected void ExitState()
        {
            DeselectMovementPiece();
        }

        public void SwapPiece(GridObject piece)
        {
            ClearHighlights();
            EnterState(piece);
        }

        public void DeselectMovementPiece()
        {
            enabled = false;
            CombatManager.Reference.DeselectUnit();
            _movingPiece = null;
            _unitSelector.EnterState();
            ClearHighlights();
        }

        protected void ClearHighlights()
        {
            foreach (GameObject highlight in _locationHighlights)
            {
                Destroy(highlight);
            }
            _locationHighlights.Clear();
        }
    }
}

