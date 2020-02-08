using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class CombatUnitSelector : MonoBehaviour
    {
        protected CombatMoverSelector moveSelector;

        private ServiceReference<ICombatManager> CombatManager = new ServiceReference<ICombatManager>();

        protected void Start()
        {
            moveSelector = GetComponent<CombatMoverSelector>();
        }

        // Update is called once per frame
        protected void Update()
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetButtonDown("Submit"))
            {
                return;
            }

            if (GridManager.Instance.m_HoveredGridTile == null)
            {
                return;
            }

            var resultPiece = CombatManager.Reference.TrySelectUnitAtTile(GridManager.Instance.m_HoveredGridTile);
            if (resultPiece != null)
            {
                ExitState(resultPiece);
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

}
