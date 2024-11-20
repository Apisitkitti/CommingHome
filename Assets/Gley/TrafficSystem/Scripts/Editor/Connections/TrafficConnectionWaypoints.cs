using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class TrafficConnectionWaypoints : ConnectionWaypoints
    {
        internal void GenerateConnectorWaypoints(ConnectionPool connections, int index, float waypointDistance)
        {
            RemoveConnectionWaipoints(connections.GetHolder(index));
            AddLaneConnectionWaypoints(connections, index, waypointDistance);
            EditorUtility.SetDirty(connections);
            AssetDatabase.SaveAssets();
        }


        internal void RemoveConnectionHolder(Transform holder)
        {
            RemoveConnectionWaipoints(holder);
            GleyPrefabUtilities.DestroyTransform(holder);
        }


        protected void AddLaneConnectionWaypoints(ConnectionPool connections, int index, float waypointDistance)
        {
            string roadName = connections.GetOriginRoad(index).name;
            List<int> allowedCars = ((ConnectionPool)connections).GetOutConnector<WaypointSettings>(index).allowedCars.Cast<int>().ToList();
            int maxSpeed = ((ConnectionPool)connections).GetOutConnector<WaypointSettings>(index).maxSpeed;
            float laneWidth = ((ConnectionPool)connections).GetOutConnector<WaypointSettings>(index).laneWidth;

            Path curve = connections.GetCurve(index);

            Vector3[] p = curve.GetPointsInSegment(0, connections.GetOffset(index));
            float estimatedCurveLength = Vector3.Distance(p[0], p[3]);
            float nrOfWaypoints = estimatedCurveLength / waypointDistance;
            if (nrOfWaypoints < 1.5f)
            {
                nrOfWaypoints = 1.5f;
            }
            float step = 1 / nrOfWaypoints;
            float t = 0;
            int nr = 0;
            List<Transform> connectorWaypoints = new List<Transform>();
            while (t < 1)
            {
                t += step;
                if (t < 1)
                {
                    string waypointName = roadName + "-" + UrbanAssets.Internal.Constants.laneNamePrefix + ((ConnectionPool)connections).GetLane(index) + "-" + UrbanAssets.Internal.Constants.connectionWaypointName + (++nr);
                    connectorWaypoints.Add(CreateInstance<WaypointGeneratorTraffic>().CreateWaypoint(connections.GetHolder(index), BezierCurve.CalculateCubicBezierPoint(t, p[0], p[1], p[2], p[3]), waypointName, allowedCars, maxSpeed, laneWidth, (ConnectionCurve)connections.GetLaneConnection(index)));
                }
            }

            WaypointSettingsBase currentWaypoint;
            WaypointSettingsBase connectedWaypoint;

            //set names
            connectorWaypoints[0].name = roadName + "-" + UrbanAssets.Internal.Constants.laneNamePrefix + ((ConnectionPool)connections).GetLane(index) + "-" + UrbanAssets.Internal.Constants.connectionEdgeName + nr;
            connectorWaypoints[connectorWaypoints.Count - 1].name = roadName + "-" + UrbanAssets.Internal.Constants.laneNamePrefix + ((ConnectionPool)connections).GetLane(index) + "-" + UrbanAssets.Internal.Constants.connectionEdgeName + (connectorWaypoints.Count - 1);

            //link middle waypoints
            for (int j = 0; j < connectorWaypoints.Count - 1; j++)
            {
                currentWaypoint = connectorWaypoints[j].GetComponent<WaypointSettingsBase>();
                connectedWaypoint = connectorWaypoints[j + 1].GetComponent<WaypointSettingsBase>();
                currentWaypoint.neighbors.Add(connectedWaypoint);
                connectedWaypoint.prev.Add(currentWaypoint);
            }

            //link first waypoint
            connectedWaypoint = connectorWaypoints[0].GetComponent<WaypointSettingsBase>();
            currentWaypoint = ((ConnectionPool)connections).GetOutConnector<WaypointSettingsBase>(index);
            currentWaypoint.neighbors.Add(connectedWaypoint);
            connectedWaypoint.prev.Add(currentWaypoint);
            EditorUtility.SetDirty(currentWaypoint);
            EditorUtility.SetDirty(connectedWaypoint);

            //link last waypoint
            connectedWaypoint = ((ConnectionPool)connections).GetInConnector(index);
            currentWaypoint = connectorWaypoints[connectorWaypoints.Count - 1].GetComponent<WaypointSettingsBase>();
            currentWaypoint.neighbors.Add(connectedWaypoint);
            connectedWaypoint.prev.Add(currentWaypoint);
            EditorUtility.SetDirty(currentWaypoint);
            EditorUtility.SetDirty(connectedWaypoint);
        }


        private void RemoveConnectionWaipoints(Transform holder)
        {
            if (holder)
            {
                for (int i = holder.childCount - 1; i >= 0; i--)
                {
                    WaypointSettingsBase waypoint = holder.GetChild(i).GetComponent<WaypointSettingsBase>();
                    for (int j = 0; j < waypoint.neighbors.Count; j++)
                    {
                        if (waypoint.neighbors[j] != null)
                        {
                            waypoint.neighbors[j].prev.Remove(waypoint);
                        }
                        else
                        {
                            Debug.LogError(waypoint.name + " has null neighbors", waypoint);
                        }
                    }
                    for (int j = 0; j < waypoint.prev.Count; j++)
                    {
                        if (waypoint.prev[j] != null)
                        {
                            waypoint.prev[j].neighbors.Remove(waypoint);
                        }
                        else
                        {
                            Debug.LogError(waypoint.name + " has null prevs", waypoint);
                        }
                    }
                    DestroyImmediate(waypoint.gameObject);
                }
            }
        }
    }
}