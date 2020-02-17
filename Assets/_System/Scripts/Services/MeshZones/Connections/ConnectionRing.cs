﻿using System.Collections.Generic;

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
        public bool IsClosed => Ring[0].HasConnection(Ring[Ring.Count - 1]);

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

        public static bool IsMergable(ConnectionRing a, ConnectionRing b)
        {
            if (a.Ring.Count == 0) return false;
            if (b.Ring.Count == 0) return false;
            if (a.Ring[0] == b.Ring[0]) return true;
            if (a.Ring[0] == b.Ring[b.Ring.Count - 1]) return true;
            if (a.Ring[a.Ring.Count - 1] == b.Ring[0]) return true;
            if (a.Ring[a.Ring.Count - 1] == b.Ring[b.Ring.Count - 1]) return true;
            return false;
        }

        public void JoinRing(ConnectionRing ring)
        {
            int connectionPoint = CalculateJoinPoint(ring);
            if(connectionPoint == -1)
            {
                UnityEngine.Debug.LogError("Connection point error");
                return;
            }

            if (connectionPoint == 0) Ring.Reverse();

            int joiningRingConnectionPoint = ring.CalculateJoinPoint(this);
            if(joiningRingConnectionPoint == -1)
            {
                UnityEngine.Debug.LogError("Connection point error");
                return;
            }

            if (joiningRingConnectionPoint != 0) ring.Ring.Reverse();
            Ring.AddRange(ring.Ring);
        }

        public void MergeRing(ConnectionRing ring)
        {
            int mergePoint = CalculateMergePoint(ring);
            if (mergePoint == -1)
            {
                UnityEngine.Debug.LogError("Merge point error");
                return;
            }

            if (mergePoint == 0) Ring.Reverse();

            int mergingRingConnectionPoint = ring.CalculateMergePoint(this);
            if(mergingRingConnectionPoint == -1)
            {
                UnityEngine.Debug.LogError("Merge point error");
                return;
            }

            if (mergingRingConnectionPoint != 0) ring.Ring.Reverse();
            Ring.AddRange(ring.Ring);
        }

        public int CalculateJoinPoint(ConnectionRing joining)
        {
            if (Ring.Count == 0) return -1;
            if (joining.Ring.Count == 0) return -1;
            if (Ring[0].HasConnection(joining.Ring[0])) return 0;
            if (Ring[0].HasConnection(joining.Ring[joining.Ring.Count - 1])) return 0;
            if (Ring[Ring.Count - 1].HasConnection(joining.Ring[0])) return Ring.Count - 1;
            if (Ring[Ring.Count - 1].HasConnection(joining.Ring[joining.Ring.Count - 1])) return Ring.Count - 1;
            return -1;
        }

        public int CalculateMergePoint(ConnectionRing merging)
        {
            if (Ring.Count == 0) return -1;
            if (merging.Ring.Count == 0) return -1;
            if (Ring[0] == merging.Ring[0]) return 0;
            if (Ring[0] == merging.Ring[merging.Ring.Count - 1]) return 0;
            if (Ring[Ring.Count - 1] == merging.Ring[0]) return Ring.Count - 1;
            if (Ring[Ring.Count - 1] == merging.Ring[merging.Ring.Count - 1]) return Ring.Count - 1;
            return -1;
        }
    }
}
