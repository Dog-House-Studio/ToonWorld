using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// A connection ring is a ring of connections. A
    /// ring must always start and end with the same
    /// connection.
    /// </summary>
    public class ConnectionRing
    {
        public List<Connection> Ring = new List<Connection>();
    }
}
