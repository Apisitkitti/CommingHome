#if GLEY_PEDESTRIAN_SYSTEM
using Gley.PedestrianSystem;
using Gley.PedestrianSystem.Internal;
#endif
#if GLEY_TRAFFIC_SYSTEM
using Gley.TrafficSystem;
using Gley.TrafficSystem.Internal;
#endif
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
#if GLEY_PEDESTRIAN_SYSTEM || GLEY_TRAFFIC_SYSTEM
using Unity.Mathematics;
#endif
using UnityEngine;

namespace Gley.UrbanAssets.Internal
{
    public class GridManager : MonoBehaviour
    {
#if GLEY_PEDESTRIAN_SYSTEM || GLEY_TRAFFIC_SYSTEM
        protected CurrentSceneData currentSceneData;
        protected List<Vector2Int> activeCells;

        private List<Vector2Int> currentCells;

        private NativeArray<float3> activeCameraPositions;

#if GLEY_TRAFFIC_SYSTEM
        private List<GenericIntersection> activeIntersections = new List<GenericIntersection>();

        private SpawnWaypointSelector spawnWaypointSelector;
#endif


        /// <summary>
        /// Initialize grid
        /// </summary>
        /// <typeparam name="T">Type of class that extends the grid manager</typeparam>
        /// <param name="currentSceneData">all waypoint information</param>
        /// <param name="activeCameraPositions">all active cameras</param>
        /// <returns></returns>
        internal GridManager Initialize(CurrentSceneData currentSceneData, NativeArray<float3> activeCameraPositions, SpawnWaypointSelector spawnWaypointSelector)
        {
            this.currentSceneData = currentSceneData;
            this.activeCameraPositions = activeCameraPositions;
            this.spawnWaypointSelector = spawnWaypointSelector;
            currentCells = new List<Vector2Int>();
            for (int i = 0; i < activeCameraPositions.Length; i++)
            {
                currentCells.Add(new Vector2Int());
            }
            UpdateActiveCells(activeCameraPositions, 1);
            return this;
        }


#if GLEY_TRAFFIC_SYSTEM
        /// <summary>
        /// Update active grid cells. Only active grid cells are allowed to perform additional operations like updating traffic lights  
        /// </summary>
        internal void UpdateGrid(int level, NativeArray<float3> activeCameraPositions)
        {
            UpdateActiveCells(activeCameraPositions, level);
        }
#endif





        /// <summary>
        /// Get all specified neighbors for the specified depth
        /// </summary>
        /// <param name="row">current row</param>
        /// <param name="column">current column</param>
        /// <param name="depth">how far the cells should be</param>
        /// <param name="justEdgeCells">ignore middle cells</param>
        /// <returns>Returns the neighbors of the given cells</returns>
        internal List<Vector2Int> GetCellNeighbors(int row, int column, int depth, bool justEdgeCells)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            int rowMinimum = row - depth;
            if (rowMinimum < 0)
            {
                rowMinimum = 0;
            }

            int rowMaximum = row + depth;
            if (rowMaximum >= currentSceneData.grid.Length)
            {
                rowMaximum = currentSceneData.grid.Length - 1;
            }


            int columnMinimum = column - depth;
            if (columnMinimum < 0)
            {
                columnMinimum = 0;
            }

            int columnMaximum = column + depth;
            if (columnMaximum >= currentSceneData.grid[row].row.Length)
            {
                columnMaximum = currentSceneData.grid[row].row.Length - 1;
            }

            for (int i = rowMinimum; i <= rowMaximum; i++)
            {
                for (int j = columnMinimum; j <= columnMaximum; j++)
                {
                    if (justEdgeCells)
                    {
                        if (i == row + depth || i == row - depth || j == column + depth || j == column - depth)
                        {
                            result.Add(new Vector2Int(i, j));
                        }
                    }
                    else
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
            return result;
        }

        internal List<Vector2Int> GetCellNeighbors(GridCell cell, int depth, bool justEdgeCells)
        {
            return GetCellNeighbors(cell.row, cell.column, depth, justEdgeCells);
        }


        /// <summary>
        /// Convert indexes to Grid cell
        /// </summary>
        /// <param name="xPoz"></param>
        /// <param name="zPoz"></param>
        /// <returns></returns>
        internal GridCell GetCell(float xPoz, float zPoz)
        {
            return currentSceneData.GetCell(xPoz, zPoz);
        }

        /// <summary>
        /// Convert position to Grid cell
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal GridCell GetCell(Vector3 position)
        {
            return currentSceneData.GetCell(position);
        }

        /// <summary>
        /// Convert cell index to Grid cell
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        internal GridCell GetCell(Vector2Int cellIndex)
        {
            return currentSceneData.grid[cellIndex.x].row[cellIndex.y];
        }

        internal float GetCellSize()
        {
            return GetCell(0, 0).size.x;
        }

        /// <summary>
        /// Get active cell for the active camera position
        /// </summary>
        /// <param name="activeCameraIndex"></param>
        /// <returns></returns>
        internal GridCell GetCell(int activeCameraIndex)
        {
            return GetCell(activeCameraPositions[activeCameraIndex].x, activeCameraPositions[activeCameraIndex].z);
        }

        /// <summary>
        /// Get position of the cell at index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        internal Vector3 GetCellPosition(Vector2Int cellIndex)
        {
            return GetCell(cellIndex).center;
        }


        /// <summary>
        /// Convert position to cell index
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Vector2Int GetCellIndex(Vector3 position)
        {
            int rowIndex = Mathf.FloorToInt(Mathf.Abs((currentSceneData.gridCorner.z - position.z) / currentSceneData.gridCellSize));
            int columnIndex = Mathf.FloorToInt(Mathf.Abs((currentSceneData.gridCorner.x - position.x) / currentSceneData.gridCellSize));
            return new Vector2Int(currentSceneData.grid[rowIndex].row[columnIndex].row, currentSceneData.grid[rowIndex].row[columnIndex].column);
        }


        /// <summary>
        /// Update active cells based on player position
        /// </summary>
        /// <param name="activeCameraPositions">position to check</param>
        private void UpdateActiveCells(NativeArray<float3> activeCameraPositions, int level)
        {
            this.activeCameraPositions = activeCameraPositions;

            if (currentCells.Count != activeCameraPositions.Length)
            {
                currentCells = new List<Vector2Int>();
                for (int i = 0; i < activeCameraPositions.Length; i++)
                {
                    currentCells.Add(new Vector2Int());
                }
            }

            bool changed = false;
            for (int i = 0; i < activeCameraPositions.Length; i++)
            {
                Vector2Int temp = GetCellIndex(activeCameraPositions[i]);
                if (currentCells[i] != temp)
                {
                    currentCells[i] = temp;
                    changed = true;
                }
            }

            if (changed)
            {
                activeCells = new List<Vector2Int>();
                for (int i = 0; i < activeCameraPositions.Length; i++)
                {
                    activeCells.AddRange(GetCellNeighbors(currentCells[i].x, currentCells[i].y, level, false));
                }
#if GLEY_TRAFFIC_SYSTEM
                UpdateActiveIntersections();
#endif
            }
        }
#endif


#if GLEY_PEDESTRIAN_SYSTEM
        private PedestrianSpawnWaypointSelector1 pedestrianSpawnWaypointSelector;
        private PedestrianSpawnWaypointSelector1 PeedstrianSpawnWaypointSelector2
        {
            get
            {
                if (pedestrianSpawnWaypointSelector == null)
                {
                    pedestrianSpawnWaypointSelector = Gley.PedestrianSystem.Internal.GetBestNeighbor.GetRandomSpawnWaypoint;
                }
                return pedestrianSpawnWaypointSelector;
            }
        }

        /// <summary>
        /// Should be overridden in derived class
        /// </summary>
        /// <param name="neighbors">cell neighbors</param>
        /// <param name="playerPosition">position of the player</param>
        /// <param name="playerDirection">heading of the player</param>
        /// <param name="carType">type of car to instantiate</param>
        /// <returns></returns>
        protected int ApplyNeighborSelectorMethod(List<Vector2Int> neighbors, Vector3 playerPosition, Vector3 playerDirection, PedestrianTypes carType)
        {
            try
            {
                return PeedstrianSpawnWaypointSelector2(neighbors, playerPosition, playerDirection, carType);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Your neighbor selector method has the following error: " + e.Message);
                return Gley.PedestrianSystem.Internal.GetBestNeighbor.GetRandomSpawnWaypoint(neighbors, playerPosition, playerDirection, carType);
            }
        }

        internal int GetNeighborCellWaypoint(int row, int column, int depth, PedestrianTypes pedestrianType, Vector3 playerPosition, Vector3 playerDirection)
        {
            //get all cell neighbors for the specified depth
            List<Vector2Int> neighbors = GetCellNeighbors(row, column, depth, false);
            for (int i = neighbors.Count - 1; i >= 0; i--)
            {
                if (currentSceneData.grid[neighbors[i].x].row[neighbors[i].y].pedestrianSpawnWaypoints.Count == 0)
                {
                    neighbors.RemoveAt(i);
                }
            }
            //if neighbors exists
            if (neighbors.Count > 0)
            {
                return ApplyNeighborSelectorMethod(neighbors, playerPosition, playerDirection, pedestrianType);
            }
            return -1;
        }

        internal List<SpawnWaypoint> GetSpawnWaypointsForCell(Vector2Int cellIndex, PedestrianTypes agentType)
        {
            return currentSceneData.grid[cellIndex.x].row[cellIndex.y].pedestrianSpawnWaypoints.Where(cond1 => cond1.allowedVehicles.Contains((int)agentType)).ToList();
        }

#endif

#if GLEY_TRAFFIC_SYSTEM


        /// <summary>
        /// Should be overriden in derived class
        /// </summary>
        /// <param name="neighbors">cell neighbors</param>
        /// <param name="playerPosition">position of the player</param>
        /// <param name="playerDirection">heading of the player</param>
        /// <param name="carType">type of car to instantiate</param>
        /// <returns></returns>
        protected int ApplyNeighborSelectorMethod(List<Vector2Int> neighbors, Vector3 playerPosition, Vector3 playerDirection, VehicleTypes carType, bool useWaypointPriority)
        {
            try
            {
                return spawnWaypointSelector(neighbors, playerPosition, playerDirection, carType, useWaypointPriority);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Your neighbor selector method has the following error: " + e.Message);
                return DefaultDelegates.GetRandomSpawnWaypoint(neighbors, playerPosition, playerDirection, carType, useWaypointPriority);
            }
        }


        /// <summary>
        /// Set the default waypoint generating method
        /// </summary>
        /// <param name="spawnWaypointSelector"></param>
        internal void SetSpawnWaypointSelector(SpawnWaypointSelector spawnWaypointSelector)
        {
            this.spawnWaypointSelector = spawnWaypointSelector;
        }




        #region Intersections

        /// <summary>
        /// Get active all intersections 
        /// </summary>
        /// <returns></returns>
        internal List<GenericIntersection> GetActiveIntersections()
        {
            return activeIntersections;
        }


        /// <summary>
        /// Create a list of active intersections
        /// </summary>
        void UpdateActiveIntersections()
        {
            List<int> intersectionIndexes = new List<int>();
            for (int i = 0; i < activeCells.Count; i++)
            {
                intersectionIndexes.AddRange(GetCell(activeCells[i]).intersectionsInCell.Except(intersectionIndexes));
            }

            List<GenericIntersection> result = new List<GenericIntersection>();
            for (int i = 0; i < intersectionIndexes.Count; i++)
            {
                switch (currentSceneData.allIntersections[intersectionIndexes[i]].type)
                {
                    case IntersectionType.TrafficLights:
                        result.Add(currentSceneData.allLightsIntersections[currentSceneData.allIntersections[intersectionIndexes[i]].index]);
                        break;
                    case IntersectionType.Priority:
                        result.Add(currentSceneData.allPriorityIntersections[currentSceneData.allIntersections[intersectionIndexes[i]].index]);
                        break;
                }
            }

            if (activeIntersections.Count == result.Count && activeIntersections.All(result.Contains))
            {

            }
            else
            {
                activeIntersections = result;
                IntersectionEvents.TriggetActiveIntersectionsChangedEvent(activeIntersections);
            }
        }


        /// <summary>
        /// Return all intersections
        /// </summary>
        /// <returns></returns>
        internal GenericIntersection[] GetAllIntersections()
        {
            GenericIntersection[] result = new GenericIntersection[currentSceneData.allIntersections.Length];
            for (int i = 0; i < currentSceneData.allIntersections.Length; i++)
            {
                switch (currentSceneData.allIntersections[i].type)
                {
                    case IntersectionType.TrafficLights:
                        result[i] = currentSceneData.allLightsIntersections[currentSceneData.allIntersections[i].index];
                        break;
                    case IntersectionType.Priority:
                        result[i] = currentSceneData.allPriorityIntersections[currentSceneData.allIntersections[i].index];
                        break;
                }
            }
            return result;
        }

        #endregion

        internal int GetNeighborCellWaypoint(int row, int column, int depth, VehicleTypes carType, Vector3 playerPosition, Vector3 playerDirection, bool useWaypointPriority)
        {
            //get all cell neighbors for the specified depth
            List<Vector2Int> neighbors = GetCellNeighbors(row, column, depth, false);

            for (int i = neighbors.Count - 1; i >= 0; i--)
            {
                if (currentSceneData.grid[neighbors[i].x].row[neighbors[i].y].spawnWaypoints.Count == 0)
                {
                    neighbors.RemoveAt(i);
                }
            }

            //if neighbors exists
            if (neighbors.Count > 0)
            {
                return ApplyNeighborSelectorMethod(neighbors, playerPosition, playerDirection, carType, useWaypointPriority);
            }
            return -1;
        }

        internal List<SpawnWaypoint> GetSpawnWaypointsForCell(Vector2Int cellIndex, VehicleTypes agentType)
        {
            List<SpawnWaypoint> spawnWaypoints = currentSceneData.grid[cellIndex.x].row[cellIndex.y].spawnWaypoints;

            return currentSceneData.grid[cellIndex.x].row[cellIndex.y].spawnWaypoints.Where(cond1 => cond1.allowedVehicles.Contains((int)agentType)).ToList();
        }

        internal List<int> GetAllWaypoints(Vector2Int cellIndex)
        {
            return currentSceneData.grid[cellIndex.x].row[cellIndex.y].waypointsInCell; ;
        }
#endif
    }
}
