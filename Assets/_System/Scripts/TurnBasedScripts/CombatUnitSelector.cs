using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitSelector : MonoBehaviour
{
    protected CombatMoverSelector moveSelector;

    protected void Awake()
    {
        moveSelector = GetComponent<CombatMoverSelector>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GridManager.Instance.m_HoveredGridTile != null)
            {
                var resultPiece = CombatManager.Instance.TrySelectUnitAtTile(GridManager.Instance.m_HoveredGridTile);
                if (resultPiece != null)
                {
                    ExitState(resultPiece);
                }
            }
        }
    }

    public void EnterState()
    {
        enabled = true;
    }

    protected void ExitState(GridObject movingPiece)
    {
        enabled = false;
        moveSelector.EnterState(movingPiece);
    }
}
