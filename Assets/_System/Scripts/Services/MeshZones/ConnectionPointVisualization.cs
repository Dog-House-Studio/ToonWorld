using UnityEngine;
using DogHouse.ToonWorld.Services;

public class ConnectionPointVisualization : MonoBehaviour
{
    public Connection connection;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (Connection c in connection.connections)
        {
            Gizmos.DrawLine(transform.position, c.root);
        }

        Gizmos.color = Color.blue;
        foreach (Vector3 tile in connection.sharedTiles)
        {
            Gizmos.DrawCube(tile, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }
}
