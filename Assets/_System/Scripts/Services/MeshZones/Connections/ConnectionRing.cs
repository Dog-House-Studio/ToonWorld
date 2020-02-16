using System.Collections.Generic;

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

        public bool IsValidConnection(Connection connection)
        {
            if (Ring.Count == 0) return true;
            if (Ring.Count > 2 && Ring[0] == Ring[Ring.Count - 1]) return false;

            if (Ring[0].HasConnection(connection)) return true;
            if (Ring[Ring.Count - 1].HasConnection(connection)) return true;
            return false;
        }

        public void AddConnection(Connection connection)
        {
            if(Ring.Count == 0)
            {
                Ring.Add(connection);
                return;
            }

            if(Ring[0].HasConnection(connection))
            {
                Ring.Insert(0, connection);
                return;
            }

            if(Ring[Ring.Count -1].HasConnection(connection))
            {
                Ring.Add(connection);
                return;
            }
        }

        public static bool IsJoinable(ConnectionRing a, ConnectionRing b)
        {
            if (a.Ring.Count == 0) return false;
            if (b.Ring.Count == 0) return false;
            if (a.Ring[0].HasConnection(b.Ring[0])) return true;
            if (a.Ring[0].HasConnection(b.Ring[b.Ring.Count - 1])) return true;
            if (a.Ring[a.Ring.Count - 1].HasConnection(b.Ring[0])) return true;
            if (a.Ring[a.Ring.Count - 1].HasConnection(b.Ring[b.Ring.Count - 1])) return true;

            return false;
        }

        public void JoinRing(ConnectionRing ring)
        {

        }
    }
}
