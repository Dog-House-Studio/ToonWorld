using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCubeMovement : GridMovement {

    [Header("Model To Rotate")]
    public Transform m_ModelHolder;

    protected override void Update() {
        base.Update();
    }

    public override IEnumerator AnimateMoveToRoutine(GridTile targetGridTile) {
        var originalGridTile = _gridObject.m_CurrentGridTile;
        // Rotating 
        var directionOfMovement = targetGridTile.m_GridPosition - originalGridTile.m_GridPosition;
        var forward = new Vector3(0, -directionOfMovement.y, 0);
        var up = new Vector3(directionOfMovement.x, 0f, 0f);
        // Only rotate around the direction axis (forward or sideways)
        var rotationAdd = forward != Vector3.zero ? Quaternion.LookRotation(forward) : Quaternion.LookRotation(Vector3.forward, up);

        // Store the starting and final rotations to lerp between them
        var startingRotation = m_ModelHolder.localRotation;
        var finalRotation = rotationAdd * m_ModelHolder.localRotation;

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

            // Rotating animation
            m_ModelHolder.localRotation = Quaternion.LerpUnclamped(startingRotation, finalRotation, MoveAnimCurve.Evaluate(t));

            // wait for the next frame
            yield return null;
        }
        // Set the position to the tile's worldposition
        transform.position = targetGridTile.m_WorldPosition;

        // Set the rotation to the final rotation
        m_ModelHolder.localRotation = finalRotation;

        // Reset the movement variables
        EndMovement();
    }
}
