using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.ToonWorld.Services;

public class ConnectionPointVisualization : MonoBehaviour
{
    public List<Connection> connections = new List<Connection>();

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach(Connection connection in connections)
        {
            Gizmos.DrawLine(transform.position, connection.root);
        }
    }
}
