using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class WaypointGeneratorTraffic : WaypointsGenerator
    {
        internal override Transform CreateWaypoint(Transform parent, Vector3 waypointPosition, string name, List<int> allowedCars, int maxSpeed, float laneWidth, ConnectionCurveBase connection)
        {
            Transform waypointTransform = base.CreateWaypoint(parent, waypointPosition, name, allowedCars, maxSpeed, laneWidth, connection);
            WaypointSettings waypointScript = waypointTransform.gameObject.AddComponent<WaypointSettings>();
            waypointScript.EditorSetup();
            waypointScript.connection = (ConnectionCurve)connection;
            waypointScript.allowedCars = allowedCars.Cast<VehicleTypes>().ToList();
            waypointScript.maxSpeed = maxSpeed;
            waypointScript.laneWidth = laneWidth;
            return waypointTransform;
        }


        public override void SwitchLaneDirection(RoadBase road, int laneNumber)
        {
            RemoveConnection(road.lanes[laneNumber].laneEdges.inConnector);
            RemoveConnection(road.lanes[laneNumber].laneEdges.outConnector);
            SwitchWaypointDirection(road.lanes[laneNumber].laneEdges.inConnector, road.lanes[laneNumber].laneEdges.outConnector);
            road.SwitchDirection(laneNumber);
        }


        private void RemoveConnection(WaypointSettingsBase waypoint)
        {
            if (waypoint == null)
                return;

            for (int i = 0; i < waypoint.neighbors.Count; i++)
            {
                if (((WaypointSettings)waypoint.neighbors[i]).connection != null)
                {
                    CreateInstance<RoadConnections>().Initialize().DeleteConnection(((WaypointSettings)waypoint.neighbors[i]).connection);
                }
            }


            for (int i = 0; i < waypoint.prev.Count; i++)
            {
                if (((WaypointSettings)waypoint.prev[i]).connection != null)
                {
                    CreateInstance<RoadConnections>().Initialize().DeleteConnection(((WaypointSettings)waypoint.prev[i]).connection);
                }
            }
        }


        private void SwitchWaypointDirection(WaypointSettingsBase startWaypoint, WaypointSettingsBase endWaypoint)
        {
            WaypointSettingsBase currentWaypoint = startWaypoint;
            bool continueSwitching = true;
            while (continueSwitching)
            {
                if (currentWaypoint == null)
                {
                    break;
                }
                if (currentWaypoint.neighbors == null)
                {
                    break;
                }

                if (currentWaypoint == endWaypoint)
                {
                    continueSwitching = false;
                }

                for (int i = currentWaypoint.neighbors.Count - 1; i >= 1; i--)
                {
                    currentWaypoint.neighbors[i].prev.Remove(currentWaypoint);
                    currentWaypoint.neighbors.RemoveAt(i);
                }

                for (int i = currentWaypoint.prev.Count - 1; i >= 1; i--)
                {
                    currentWaypoint.prev[i].neighbors.Remove(currentWaypoint);
                    currentWaypoint.prev.RemoveAt(i);
                }

                List<WaypointSettingsBase> aux = currentWaypoint.neighbors;
                currentWaypoint.neighbors = currentWaypoint.prev;
                currentWaypoint.prev = aux;
                if (currentWaypoint.prev.Count > 0)
                {
                    currentWaypoint = currentWaypoint.prev[0];
                }
            }
        }
    }
}
