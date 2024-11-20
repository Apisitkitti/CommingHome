using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gley.TrafficSystem.Internal
{
    /// <summary>
    /// Performs waypoint operations
    /// </summary>
    public class WaypointManager : WaypointManagerBase
    {
        protected Dictionary<int, int> playerTarget;
        private Dictionary<int, Queue<int>> pathToDestination;
        private bool[] hasPath;


        internal WaypointManager Initialize(Waypoint[] allWaypoints, int nrOfVehicles, bool debugWaypoints, bool debugDisabledWaypoints)
        {
            WaypointEvents.onTrafficLightChanged += TrafficLightChanged;
            playerTarget = new Dictionary<int, int>();
            pathToDestination = new Dictionary<int, Queue<int>>();
            base.Initialize(allWaypoints, nrOfVehicles, debugWaypoints, debugDisabledWaypoints);
            hasPath = new bool[nrOfVehicles];
            return this;
        }


        internal void RegisterPlayer(int id, int waypointIndex)
        {
            if (!playerTarget.ContainsKey(id))
            {
                playerTarget.Add(id, waypointIndex);
            }
        }


        internal void SetVehiclePath(int vehicleID, Queue<int> pathWaypoints)
        {
            if (!pathToDestination.ContainsKey(vehicleID))
            {
                pathToDestination.Add(vehicleID, pathWaypoints);
                hasPath[vehicleID] = true;
            }
            pathToDestination[vehicleID] = pathWaypoints;
        }


        internal bool HasPath(int vehicleID)
        {
            return hasPath[vehicleID];
        }


        internal void RemoveVehiclePath(int vehicleID)
        {
            pathToDestination.Remove(vehicleID);
            hasPath[vehicleID] = false;
        }


        internal Queue<int> GetPath(int vehicleIndex)
        {
            if (pathToDestination.ContainsKey(vehicleIndex))
            {
                return pathToDestination[vehicleIndex];
            }
            return new Queue<int>();
        }


        internal void UpdatePlayerWaypoint(int id, int waypointIndex)
        {
            playerTarget[id] = waypointIndex;
        }


        /// <summary>
        /// Check if waypoint is a target for another agent
        /// </summary>
        /// <param name="waypointIndex"></param>
        /// <returns></returns>
        public bool IsThisWaypointATarget(int waypointIndex)
        {
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == waypointIndex)
                {
                    return true;
                }
            }

            return playerTarget.ContainsValue(waypointIndex);
        }


        internal float GetLaneWidth(int vehicleIndex)
        {
            try
            {
                return GetTargetWaypointOfAgent<Waypoint>(vehicleIndex).laneWidth;
            }
            catch
            {
                return 0;
            }
        }


        internal bool AreTheseWaypointsATarget(List<int> waypointsToCheck)
        {
            return target.Intersect(waypointsToCheck).Any() || playerTarget.Values.Any(v => waypointsToCheck.Contains(v));
        }


        protected override void TrafficLightChanged(int waypointIndex, bool newValue)
        {
            if (GetWaypoint<Waypoint>(waypointIndex).stop != newValue)
            {
                GetWaypoint<Waypoint>(waypointIndex).stop = newValue;
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i] == waypointIndex)
                    {
                        WaypointEvents.TriggerStopStateChangedEvent(i, GetWaypoint<Waypoint>(waypointIndex).stop);
                    }
                }
            }

        }


        internal bool HaveCommonNeighbors(Waypoint fromWaypoint, Waypoint toWaypoint, int level = 0)
        {
            if (level == 0)
            {
                if (fromWaypoint.neighbors.Intersect(toWaypoint.neighbors).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Set next waypoint and trigger the required events
        /// </summary>
        /// <param name="vehicleIndex"></param>
        /// <param name="waypointIndex"></param>
        internal override void SetNextWaypoint(int vehicleIndex, int waypointIndex)
        {
            bool stop = false;
            if (hasPath[vehicleIndex])
            {
                Queue<int> queue;
                if (pathToDestination.TryGetValue(vehicleIndex, out queue))
                {
                    queue.Dequeue();
                    if (queue.Count == 0)
                    {
                        stop = true;
                    }
                }
            }

            base.SetNextWaypoint(vehicleIndex, waypointIndex);
            Waypoint targetWaypoint = GetTargetWaypointOfAgent<Waypoint>(vehicleIndex);
            if (stop == true)
            {
                WaypointEvents.TriggerStopStateChangedEvent(vehicleIndex, true);
            }
            if (targetWaypoint.stop == true)
            {
                WaypointEvents.TriggerStopStateChangedEvent(vehicleIndex, targetWaypoint.stop);
            }
            if (targetWaypoint.giveWay == true)
            {
                WaypointEvents.TriggerGiveWayStateChangedEvent(vehicleIndex, targetWaypoint.giveWay);
            }

            if (targetWaypoint.complexGiveWay == true)
            {
                WaypointEvents.TriggerGiveWayStateChangedEvent(vehicleIndex, targetWaypoint.complexGiveWay);
            }

        }


        public bool CanContinueStraight(int vehicleIndex, int carType)
        {
            Waypoint targetWaypoint = GetTargetWaypointOfAgent<Waypoint>(vehicleIndex);
            if (targetWaypoint.neighbors.Count > 0)
            {
                if (hasPath[vehicleIndex])
                {
                    Queue<int> queue;
                    if (pathToDestination.TryGetValue(vehicleIndex, out queue))
                    {
                        if (queue.Count > 0)
                        {
                            int nextWaypoint = queue.Peek();
                            if (!targetWaypoint.neighbors.Contains(nextWaypoint))
                            {
                                return false;
                            }
                        }
                    }
                }
                for (int i = 0; i < targetWaypoint.neighbors.Count; i++)
                {
                    if (GetWaypoint<Waypoint>(targetWaypoint.neighbors[i]).allowedAgents.Contains(carType) && !IsDisabled(targetWaypoint.neighbors[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Check if target waypoint of the vehicle is in intersection
        /// </summary>
        /// <param name="vehicleIndex"></param>
        /// <returns></returns>
        internal bool IsInIntersection(int vehicleIndex)
        {
            return GetTargetWaypointOfAgent<Waypoint>(vehicleIndex).IsInIntersection();
        }


        /// <summary>
        /// Check if can switch to target waypoint
        /// </summary>
        /// <param name="vehicleIndex"></param>
        /// <returns></returns>
        internal bool CanEnterIntersection(int vehicleIndex)
        {
            return GetTargetWaypointOfAgent<Waypoint>(vehicleIndex).CanChange();
        }


        /// <summary>
        /// Check if the previous waypoints are free
        /// </summary>
        /// <param name="vehicleIndex"></param>
        /// <param name="freeWaypointsNeeded"></param>
        /// <param name="possibleWaypoints"></param>
        /// <returns></returns>
        internal bool AllPreviousWaypointsAreFree(int vehicleIndex, float distance, int waypointToCheck, ref int incomingCarIndex)
        {
            return IsTargetFree(GetWaypoint<WaypointBase>(waypointToCheck), distance, GetTargetWaypointOfAgent<WaypointBase>(vehicleIndex), vehicleIndex, ref incomingCarIndex);
        }


        /// <summary>
        /// Check what vehicle is in front
        /// </summary>
        /// <param name="vehicleIndex1"></param>
        /// <param name="vehicleIndex2"></param>
        /// <returns>
        /// 1-> if 1 is in front of 2
        /// 2-> if 2 is in front of 1
        /// 0-> if it is not possible to determine
        /// </returns>
        internal int IsInFront(int vehicleIndex1, int vehicleIndex2)
        {
            //compares waypoints to determine which vehicle is in front 
            int distance = 0;
            //if no neighbors are available -> not possible to determine
            if (GetTargetWaypointOfAgent<WaypointBase>(vehicleIndex1).neighbors.Count == 0)
            {
                return 0;
            }

            //check next 10 waypoints to find waypoint 2
            int startWaypointIndex = GetTargetWaypointOfAgent<WaypointBase>(vehicleIndex1).neighbors[0];
            while (startWaypointIndex != GetTargetWaypointIndex(vehicleIndex2) && distance < 10)
            {
                distance++;
                if (GetWaypoint<WaypointBase>(startWaypointIndex).neighbors.Count == 0)
                {
                    //if not found -> not possible to determine
                    return 0;
                }
                startWaypointIndex = GetWaypoint<WaypointBase>(startWaypointIndex).neighbors[0];
            }


            int distance2 = 0;
            if (GetTargetWaypointOfAgent<WaypointBase>(vehicleIndex2).neighbors.Count == 0)
            {
                return 0;
            }

            startWaypointIndex = GetTargetWaypointOfAgent<WaypointBase>(vehicleIndex2).neighbors[0];
            while (startWaypointIndex != GetTargetWaypointIndex(vehicleIndex1) && distance2 < 10)
            {
                distance2++;
                if (GetWaypoint<WaypointBase>(startWaypointIndex).neighbors.Count == 0)
                {
                    //if not found -> not possible to determine
                    return 0;
                }
                startWaypointIndex = GetWaypoint<WaypointBase>(startWaypointIndex).neighbors[0];
            }

            //if no waypoints found -> not possible to determine
            if (distance == 10 && distance2 == 10)
            {
                return 0;
            }

            if (distance2 > distance)
            {
                return 2;
            }

            return 1;
        }


        /// <summary>
        /// Check if 2 vehicles have the same target
        /// </summary>
        /// <param name="vehicleIndex1"></param>
        /// <param name="VehicleIndex2"></param>
        /// <returns></returns>
        internal bool IsSameTarget(int vehicleIndex1, int VehicleIndex2)
        {
            return GetTargetWaypointIndex(vehicleIndex1) == GetTargetWaypointIndex(VehicleIndex2);
        }


        /// <summary>
        /// Get waypoint speed
        /// </summary>
        /// <param name="vehicleIndex"></param>
        /// <returns></returns>
        internal float GetMaxSpeed(int vehicleIndex)
        {
            return GetTargetWaypointOfAgent<Waypoint>(vehicleIndex).maxSpeed;
        }


        /// <summary>
        /// Check if previous waypoints are free
        /// </summary>
        /// <param name="waypoint"></param>
        /// <param name="level"></param>
        /// <param name="initialWaypoint"></param>
        /// <returns></returns>
        private bool IsTargetFree(WaypointBase waypoint, float distance, WaypointBase initialWaypoint, int currentCarIndex, ref int incomingCarIndex)
        {
#if UNITY_EDITOR
            if (debugWaypoints)
            {
                Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.green, 1);
            }
#endif
            if (distance <= 0)
            {
#if UNITY_EDITOR
                if (debugWaypoints)
                {
                    Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.green, 1);
                }
#endif
                return true;
            }
            if (waypoint == initialWaypoint)
            {
#if UNITY_EDITOR
                if (debugWaypoints)
                {
                    Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.white, 1);
                }
#endif
                return true;
            }
            if (IsThisWaypointATarget(waypoint.listIndex))
            {
                incomingCarIndex = GetAgentIndexAtTarget(waypoint.listIndex);
                if (GetTargetWaypointIndex(currentCarIndex) == waypoint.listIndex)
                {
#if UNITY_EDITOR
                    if (debugWaypoints)
                    {
                        Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.blue, 1);
                    }
#endif
                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    if (debugWaypoints)
                    {
                        Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.red, 1);
                    }
#endif
                    return false;
                }
            }
            else
            {
                if (waypoint.prev.Count <= 0)
                {
                    return true;
                }
                distance -= Vector3.Distance(waypoint.position, initialWaypoint.position);
                for (int i = 0; i < waypoint.prev.Count; i++)
                {
                    if (!IsTargetFree(GetWaypoint<WaypointBase>(waypoint.prev[i]), distance, initialWaypoint, currentCarIndex, ref incomingCarIndex))
                    {
                        if (debugWaypoints)
                        {
                            Debug.DrawLine(waypoint.position, initialWaypoint.position, Color.magenta, 1);
                        }
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Get rotation of the target waypoint
        /// </summary>
        /// <param name="agentIndex"></param>
        /// <returns></returns>
        internal Quaternion GetTargetWaypointRotation(int agentIndex)
        {
            if (GetTargetWaypointOfAgent<WaypointBase>(agentIndex).neighbors.Count == 0)
            {
                return Quaternion.identity;
            }
            return Quaternion.LookRotation(GetWaypoint<WaypointBase>(GetTargetWaypointOfAgent<WaypointBase>(agentIndex).neighbors[0]).position - GetTargetWaypointOfAgent<WaypointBase>(agentIndex).position);
        }


        /// <summary>
        /// Check if a change of lane is possible
        /// Used to overtake and give way
        /// </summary>
        /// <param name="agentIndex"></param>
        /// <param name="agentType"></param>
        /// <returns></returns>
        internal int GetOtherLaneWaypointIndex(int agentIndex, int agentType, RoadSide side = RoadSide.Any, Vector3 forwardVector = default)
        {
            int waypointIndex = PeekPoint(agentIndex);
            if (waypointIndex != -1)
            {
                return waypointIndex;
            }

            WaypointBase currentWaypoint = GetTargetWaypointOfAgent<WaypointBase>(agentIndex);

            if (currentWaypoint.otherLanes.Count > 0)
            {
                WaypointBase[] possibleWaypoints = GetAllWaypoints(currentWaypoint.otherLanes).Where(cond => cond.allowedAgents.Contains(agentType)).ToArray();
                if (possibleWaypoints.Length > 0)
                {
                    return GetSideWaypoint(possibleWaypoints, currentWaypoint, side, forwardVector);
                }
            }

            return -1;
        }


        internal override int GetCurrentLaneWaypointIndex(int agentIndex, int agentType)
        {
            int waypointIndex = PeekPoint(agentIndex);
            if (waypointIndex != -1)
            {
                return waypointIndex;
            }
            return base.GetCurrentLaneWaypointIndex(agentIndex, agentType);
        }


        private int PeekPoint(int agentIndex)
        {
            Queue<int> queue;
            if (pathToDestination.TryGetValue(agentIndex, out queue))
            {
                if (queue.Count > 0)
                {
                    return queue.Peek();
                }
                return -2;
            }
            return -1;
        }


        private int GetSideWaypoint(WaypointBase[] waypoints, WaypointBase currentWaypoint, RoadSide side, Vector3 forwardVector)
        {
            switch (side)
            {
                case RoadSide.Any:
                    return waypoints[Random.Range(0, waypoints.Length)].listIndex;
                case RoadSide.Left:
                    for (int i = 0; i < waypoints.Length; i++)
                    {
                        if (Vector3.SignedAngle(waypoints[i].position - currentWaypoint.position, forwardVector, Vector3.up) > 5)
                        {
                            return waypoints[i].listIndex;
                        }
                    }
                    break;
                case RoadSide.Right:
                    for (int i = 0; i < waypoints.Length; i++)
                    {
                        if (Vector3.SignedAngle(waypoints[i].position - currentWaypoint.position, forwardVector, Vector3.up) < -5)
                        {
                            return waypoints[i].listIndex;
                        }
                    }
                    break;
            }

            return -1;
        }


        /// <summary>
        /// Cleanup
        /// </summary>
        private void OnDestroy()
        {
            WaypointEvents.onTrafficLightChanged -= TrafficLightChanged;
        }


        internal void AddWaypointEvent(int waypointIndex, string data)
        {
            Waypoint waypoint = GetWaypoint<Waypoint>(waypointIndex);
            if (waypoint != null)
            {
                waypoint.triggerEvent = true;
                waypoint.eventData = data;
            }
        }


        internal void RemoveWaypointEvent(int waypointIndex)
        {
            Waypoint waypoint = GetWaypoint<Waypoint>(waypointIndex);
            if (waypoint != null)
            {
                waypoint.triggerEvent = false;
                waypoint.eventData = string.Empty;
            }
        }
    }
}