using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitSelector : MonoBehaviour
{
    protected CombatMoverSelector moveSelector;

    protected void Start()
    {
        moveSelector = GetComponent<CombatMoverSelector>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0)||Input.GetButtonDown("Submit"))
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
        
        moveSelector.EnterState(movingPiece);
        enabled = false;
    }
}
