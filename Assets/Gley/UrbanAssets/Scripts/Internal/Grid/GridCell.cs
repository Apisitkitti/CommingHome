using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gley.UrbanAssets.Internal
{
    [System.Serializable]
    public class GridCell
    {
        public List<int> waypointsInCell;
        public List<int> pedestriansWaypointsInCell;
        public List<SpawnWaypoint> spawnWaypoints;
        public List<SpawnWaypoint> pedestrianSpawnWaypoints;
        public List<int> intersectionsInCell;
        public Vector3 center;
        public Vector3 size;
        public int row;
        public int column;


        /// <summary>
        /// Create a grid cell
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="center"></param>
        /// <param name="cellSize"></param>
        public GridCell(int column, int row, Vector3 center, int cellSize)
        {
            this.row = row;
            this.column = column;
            this.center = center;
            size = new Vector3(cellSize, 0, cellSize);
            ClearTrafficReferences();
            ClearPedestrianReferences();
        }


        /// <summary>
        /// Reset references
        /// </summary>
        public void ClearTrafficReferences()
        {
            waypointsInCell = new List<int>();
            spawnWaypoints = new List<SpawnWaypoint>();
            intersectionsInCell = new List<int>();
        }


        /// <summary>
        /// Reset pedestrian references
        /// </summary>
        public void ClearPedestrianReferences()
        {
            pedestriansWaypointsInCell = new List<int>();
            pedestrianSpawnWaypoints = new List<SpawnWaypoint>();
        }


        /// <summary>
        /// Add a waypoint to grid cell
        /// </summary>
        /// <param name="waypointIndex"></param>
        /// <param name="name"></param>
        /// <param name="allowedCars"></param>
        public void AddTrafficWaypoint(int waypointIndex, string name, List<int> allowedCars, bool isInIntersection, int priority)
        {
            waypointsInCell.Add(waypointIndex);
            if (!name.Contains(Constants.connect) && !name.Contains(Constants.outWaypointEnding) && isInIntersection == false)
            {
                if (priority <= 0)
                {
                    priority = 1;
                }
                spawnWaypoints.Add(new SpawnWaypoint(waypointIndex, allowedCars, priority));
            }
        }


        /// <summary>
        /// Add a waypoint to grid cell
        /// </summary>
        /// <param name="waypointIndex"></param>
        /// <param name="name"></param>
        /// <param name="allowedCars"></param>
        public void AddPedestrianWaypoint(int waypointIndex, string name, List<int> allowedPedestrians, bool isInIntersection, int priority)
        {
            pedestriansWaypointsInCell.Add(waypointIndex);
            if (!name.Contains(Constants.connect) && isInIntersection == false)
            {
                pedestrianSpawnWaypoints.Add(new SpawnWaypoint(waypointIndex, allowedPedestrians, priority));
            }
        }


        /// <summary>
        /// Add an intersection to grid cell
        /// </summary>
        /// <param name="intersection"></param>
        public void AddIntersection(int intersection)
        {
            if (!intersectionsInCell.Contains(intersection))
            {
                intersectionsInCell.Add(intersection);
            }
        }


        public void RemoveSpawnWaypoint(List<int> waypointsToRemove)
        {
            for (int i = 0; i < waypointsToRemove.Count; i++)
            {
                try
                {
                    spawnWaypoints.Remove(spawnWaypoints.First(cond => cond.waypointIndex == waypointsToRemove[i]));
                }
                catch
                {
                }
            }
        }
    }
}
