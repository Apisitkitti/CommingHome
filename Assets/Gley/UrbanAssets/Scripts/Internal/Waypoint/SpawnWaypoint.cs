using System.Collections.Generic;

namespace Gley.UrbanAssets.Internal
{
    /// <summary>
    /// This type of waypoint can spawn a vehicle, 
    /// used to store waypoint properties 
    /// </summary>
    [System.Serializable]
    public struct SpawnWaypoint
    {
        public int waypointIndex;
        public List<int> allowedVehicles;
        public int priority;
        public SpawnWaypoint(int waypointIndex, List<int> allowedVehicles, int priority)
        {
            this.waypointIndex = waypointIndex;
            this.allowedVehicles = allowedVehicles;
            this.priority = priority;
        }
    }
}
