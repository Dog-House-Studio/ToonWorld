﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
public class GridMovement : MonoBehaviour {
    public delegate void StartMovementHandler(GridMovement gridMovement, GridTile startGridTile, GridTile endGridTile);
    public delegate void EndMovementHandler(GridMovement gridMovement, GridTile startGridTile, GridTile endGridTile);
    public event StartMovementHandler OnMovementStart;
    public event EndMovementHandler OnMovementEnd;

    [Header("Movement Cooldown")]
    [Tooltip("Helper for cooldown period between tile movements.")]
    [SerializeField] protected Cooldown _cooldown;
    public Cooldown Cooldown { get { return _cooldown; } }

    [Header ("Rotating triggers Cooldown")]
    [Tooltip("This will make this object rotate slowly and reset the movement cooldown whenever it does rotate.")]
    [SerializeField]
    public bool _rotationCausesMovementCooldown;

    [Header ("Movement Animation")]
    [Tooltip("Time in seconds to animate the movement to another tile")]
    public float MoveAnimDuration = 0.4f;
    public AnimationCurve MoveAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Range(0f, 1f), Tooltip("The point from 0-1 along the movement animation at which the GridObject occupies the destination tile.")]
    [Header ("Occupy Target Tile Treshold")]
    public float SwapPositionThreshold = 0f;

    [Header ("Rotation Animation")]
    [Tooltip("Time in seconds to animate to face a new direction, should be snappy and happens before/during movement")]
    public float RotateAnimDuration = 0.1f;
    public AnimationCurve RotateAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header ("Facing Direction Treshold")]
    [Range(0f, 1f), Tooltip("The point from 0-1 along the movement animation at which the GridObject occupies the destination tile.")]
    public float SwapFacingDirectionThreshold = 0f;
    protected IEnumerator _animateMoveRoutine;
    protected IEnumerator _animateRotateRoutine;
    protected GridObject _gridObject;

    [Header("Animator")]
    public Animator m_Animator;
    public string m_MoveAnimationTriggerName = "Move";
    public string m_RotateAnimationTriggerName = "Rotate";

    public bool m_IsMoving {
        get;
        protected set;
    }

    public GridTile m_StartGridTile {
        get;
        protected set;
    }

    public GridTile m_TargetGridTile {
        get;
        protected set;
    }

    public Vector2Int m_TargetDirection {
        get;
        protected set;
    }

    protected virtual void Awake() {
        _gridObject = GetComponent<GridObject>();

        if (_gridObject == null) {
            Debug.LogError("GridObject was null for this GridMovement component", transform);
        }
    }

    protected virtual void Update() {
        Cooldown.Update();
    }

    protected virtual void StartMovement(GridTile targetGridTile) {
        // Set the movement to true
        m_IsMoving = true;

        // Invoke the onMovementStart event
        if (OnMovementStart != null) {
            OnMovementStart(this, _gridObject.m_PreviousGridTile, _gridObject.m_CurrentGridTile);
        }
    }

    public virtual void EndMovement() {
        // Since we've already moved set the target gridtile to null
        m_TargetGridTile = null;
        // Since we're already rotated set the targetdirection to null
        m_TargetDirection = Vector2Int.zero;
        // Reset the ismoving variable
        m_IsMoving = false;

        // Invoke the onMovementEnd event
        if (OnMovementEnd != null) {
            OnMovementEnd(this, _gridObject.m_PreviousGridTile, _gridObject.m_CurrentGridTile);
        }
    }

    // Try to move to a neighbor of the current tile in the target position
    public virtual MovementResult TryMoveToNeighborInPosition(Vector2Int gridPosition, bool animate = true, bool andRotate = true, bool checkMovementCooldown = true) {
        var targetGridTile = GridManager.Instance.NeighborAtPosition(_gridObject.m_CurrentGridTile, gridPosition);

        return targetGridTile == null ? MovementResult.NoTileAtPosition : TryMoveTo(targetGridTile, animate, andRotate, checkMovementCooldown);
    }

    // Try to move using a target position in the grid
    public virtual MovementResult TryMoveToPosition(Vector2Int gridPosition, bool animate = true, bool andRotate = true, bool checkMovementCooldown = true) {
        var targetGridTile = GridManager.GetGridTileAtPosition(gridPosition);

        return targetGridTile == null ? MovementResult.NoTileAtPosition : TryMoveTo(targetGridTile, animate, andRotate, checkMovementCooldown);
    }

    // Try to move using a target tile in the grid
    public virtual MovementResult TryMoveTo(GridTile targetGridTile, bool animate = true, bool andRotate = true, bool checkMovementCooldown = true) {
        // Set the current and target tile
        m_StartGridTile = _gridObject.m_CurrentGridTile;
        m_TargetGridTile = targetGridTile;
        // Current position and position we'd like to move to
        Vector2Int fromGridPos = _gridObject.m_GridPosition;
        Vector2Int toGridPos = targetGridTile.m_GridPosition;


        // Checks for the cooldown or if it is moving
        if (checkMovementCooldown && (m_IsMoving || Cooldown.InProgress)) {
            return MovementResult.Cooldown;
        }

        // Rotation
        if (andRotate) {
            // Calculate the direction
            Vector2Int direction = (toGridPos - fromGridPos);
            // Try to rotate the object in the direction
            var rotated = TryRotateTo(direction, animate);
            // Return the correct movementresult if we actually turned and triggered the cooldown
            if (rotated && _rotationCausesMovementCooldown && Cooldown.InProgress) {
                return MovementResult.Turned;
            }
        }

        // Declare a new local variable to store wether the movement was succesful or not 
        MovementResult movementResult = MovementResult.Moved;
        // Move if possible else return the failed result
        if (_gridObject.m_CurrentGridTile != null && targetGridTile.CanMoveToTile()) {
            var moved = MoveTo(targetGridTile, animate);
            if (!moved) {
                movementResult = MovementResult.Failed;
            }
        } else {
            movementResult = MovementResult.Failed;
        }

        return movementResult;
    }

    // Don't pass any argument to reset it to its default value (set on the inspector)
    public virtual void TriggerCooldown(float? time = null) {
        Cooldown.Reset((!time.HasValue) ? null : time);
    }

    public virtual bool MoveTo(GridTile gridTile, bool animate = true, bool triggerCooldown = true) {
        // If the tile is occupied return
        if (gridTile.IsTileOccupied()) {
            Debug.Log("Couldn't move to the gridtile because it is occupied");
            return false;
        }

        // Animated movement
        if (animate) {
            AnimateMoveTo(gridTile);
        } else {
            // Non animated movement (instant)
            // Update the movement variables
            StartMovement(gridTile);
            // Update variables on the owner GridObject
            SwapPosition(gridTile);
            // Set the transform's position to the target GridTile's world position
            transform.position = gridTile.m_WorldPosition;
            // Reset the movement variables
            EndMovement();
        }

        // Trigger the cooldown
        if (triggerCooldown) {
            TriggerCooldown();
        }

        return true;
    }

    public virtual bool TryRotateTo(Vector2Int direction, bool animate = true, bool checkMovementCooldown = true) {
        // If we are already facing that direction return
        if (direction == _gridObject.m_FacingDirection) {
            return true;
        }
        // If the desired direction is zero return
        if (direction == Vector2Int.zero) {
            return false;
        }
        // Checks if the cooldown is in progress
        if (checkMovementCooldown && Cooldown.InProgress) {
            return false;
        }

        // Rotate 
        RotateTo(direction, animate);
        return true;
    }

    public virtual void RotateTo(Vector2Int targetDirection, bool animate = true) {
        // Set the target direction
        m_TargetDirection = targetDirection;

        // Animated movement
        if (animate) {
            AnimateRotateTo(targetDirection);
        } else {
            // Non animated rotation (instant)
            // Update the transforms rotation
            Rotate(targetDirection);
            // Set the facing direction variable on the owner gridobject
            SwapFacingDirection(targetDirection);
        }
        // Trigger the Cooldown 
        if (_rotationCausesMovementCooldown) {
            Cooldown.Reset(null);
        }
    }

    public virtual void AnimateRotateTo(Vector2Int targetDirection) {
        // Stop the last animate movement routine if it has not stopped yet
        StopRotateRoutine();

        // Assign and start the animations coroutine
        _animateRotateRoutine = AnimateRotateToRoutine(targetDirection);
        StartCoroutine(_animateRotateRoutine);
    }

    public virtual IEnumerator AnimateRotateToRoutine(Vector2Int targetDirection) {
        // Trigger the animation on the animator
        if (m_Animator != null) {
            m_Animator.SetTriggerIfExists(m_RotateAnimationTriggerName);
        }

        var originallocalRotation = transform.localRotation;
        var targetlocalRotation = DirectionToRotation(targetDirection);
        float t = 0;
        for (t = Time.deltaTime / RotateAnimDuration; t < 1; t += Time.deltaTime / MoveAnimDuration) {
            // Lerp rotation based on the animation curve
            transform.localRotation = Quaternion.LerpUnclamped(originallocalRotation, targetlocalRotation, RotateAnimCurve.Evaluate(t));
            // Swap position if we reached the desired threshold
            if (t >= SwapFacingDirectionThreshold) {
                if (_gridObject.m_FacingDirection != targetDirection) {
                    SwapFacingDirection(targetDirection);
                }
            }

            // wait for the next frame
            yield return null;
        }

        // Swap the facing rotation
        if (_gridObject.m_FacingDirection != targetDirection) {
            SwapFacingDirection(targetDirection);
        }

        // Set the position to the tile's worldposition
        transform.localRotation = targetlocalRotation;
    }

    public virtual void AnimateMoveTo(GridTile targetGridTile) {
        // Stop the last animate movement routine if it has not stopped yet
        StopMoveRoutine();

        // Update the movement variables)
        StartMovement(targetGridTile);
        // Assign and start the animations coroutine
        _animateMoveRoutine = AnimateMoveToRoutine(targetGridTile);
        StartCoroutine(_animateMoveRoutine);
    }

    public virtual IEnumerator AnimateMoveToRoutine(GridTile targetGridTile) {
        // Trigger the animation on the animator
        if (m_Animator != null) {
            m_Animator.SetTriggerIfExists(m_MoveAnimationTriggerName);
        }

        var originalGridTile = _gridObject.m_CurrentGridTile;
        float t = 0;
        for (t = Time.deltaTime / MoveAnimDuration; t < 1; t += Time.deltaTime / MoveAnimDuration) {
            // Lerp position based on the animation curve
            transform.position = Vector3.LerpUnclamped(originalGridTile.m_WorldPosition, targetGridTile.m_WorldPosition, MoveAnimCurve.Evaluate(t));
            // Swap position if we reached the desired threshold
            if (t >= SwapPositionThreshold) {
                if (_gridObject.m_CurrentGridTile != targetGridTile) {
                    SwapPosition(targetGridTile);
                }
            }
            // wait for the next frame
            yield return null;
        }

        if (_gridObject.m_CurrentGridTile != targetGridTile) {
            SwapPosition(targetGridTile);
        }

        // Set the position to the tile's worldposition
        transform.position = targetGridTile.m_WorldPosition;

        // Reset the movement variables
        EndMovement();
    }

    public virtual void Rotate(Vector2Int targetDirection) {
        transform.localRotation = DirectionToRotation(targetDirection);
        //var targetDirection = new Vector3((float)direction.x, 0f, (float)direction.y);
        //var targetPosition = targetDirection + transform.position;
        //transform.LookAt(targetPosition);

        //var originallocalRotation = transform.localRotation;
        //var targetlocalRotation = Quaternion.LookRotation(targetDirection.ToVector3X0Z());
    }

    public virtual Quaternion DirectionToRotation(Vector2Int targetDirection) {
        var rotation = transform.localRotation;
        if (GridManager.Instance.m_Grid.cellSwizzle == GridLayout.CellSwizzle.XZY) {
            rotation = Quaternion.LookRotation(targetDirection.ToVector3X0Z());
        } else if (GridManager.Instance.m_Grid.cellSwizzle == GridLayout.CellSwizzle.XYZ) {
            //targetLocation.z = myLocation.z; // ensure there is no 3D rotation
            var rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * targetDirection.ToVector3XY0();
            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            rotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
        }

        return rotation;
    }

    // Reset the previous tile, swap your grid position to the new tile and occupy it 
    public virtual void SwapPosition(GridTile targetGridTile) {
        // Update variables on the owner GridObject
        _gridObject.RemoveFromTile();
        _gridObject.SetCurrentGridTile(targetGridTile);
        _gridObject.AddToTile(targetGridTile);
        _gridObject.m_GridPosition = targetGridTile.m_GridPosition;
    }

    // Swap the facing direction a new one 
    public virtual void SwapFacingDirection(Vector2Int targetDirection) {
        // Update variables on the owner GridObject
        _gridObject.m_FacingDirection = targetDirection;

    }

    public bool TryStop() {
        if (!m_IsMoving) {
            return false;
        }
        CancelMovement();
        return true;
    }

    protected void CancelMovement() {
        if (!m_IsMoving || !transform) {
            return;
        }

        // Resolve position
        if (m_TargetGridTile) {
            if (m_TargetGridTile == _gridObject.m_CurrentGridTile) {
                transform.position = m_TargetGridTile.m_WorldPosition;
            } else {
                transform.position = m_StartGridTile.m_WorldPosition;
            }
        }

        // Resolve rotation
        if (m_TargetDirection != Vector2Int.zero) {
            if (m_TargetDirection == _gridObject.m_FacingDirection) {
                Rotate(m_TargetDirection);
            } else {
                Rotate(_gridObject.m_FacingDirection);
            }
        }

        StopMoveRoutine();
        StopRotateRoutine();
        EndMovement();
    }

    private void StopMoveRoutine() {
        if (_animateMoveRoutine == null) {
            return;
        }
        StopCoroutine(_animateMoveRoutine);
        _animateMoveRoutine = null;
    }

    private void StopRotateRoutine() {
        if (_animateRotateRoutine == null) {
            return;
        }
        StopCoroutine(_animateRotateRoutine);
        _animateRotateRoutine = null;
    }
}
