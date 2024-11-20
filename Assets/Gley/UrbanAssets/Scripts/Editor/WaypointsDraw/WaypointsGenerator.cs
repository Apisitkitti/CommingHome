using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public class WaypointsGenerator : UnityEditor.Editor
    {
        internal virtual Transform CreateWaypoint(Transform parent, Vector3 waypointPosition, string name, List<int> allowedCars, int maxSpeed, float laneWidth, ConnectionCurveBase connection)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(parent);
            go.transform.position = waypointPosition;
            go.name = name;
            go.tag = Constants.editorTag;
            return go.transform;
        }


        internal void GenerateWaypoints(RoadBase road, int groundLayerMask)
        {
            ClearOldWaypointConnections(road.transform);

            GleyPrefabUtilities.ClearAllChildObjects(road.transform);

            List<Transform> helpingPoints = SplitBezierIntoPoints.CreatePoints(road);

            AddFinalWaypoints(road, helpingPoints, groundLayerMask);

            LinkNeighbors(road);

            for (int i = 0; i < road.lanes.Count; i++)
            {
                if (road.lanes[i].laneDirection == true)
                {
                    SwitchLaneDirection(road, i);
                }
            }

            DeleteHelpingPoints(helpingPoints);
            GleyPrefabUtilities.ApplyPrefabInstance(road.gameObject);
        }


        public virtual void SwitchLaneDirection(RoadBase road, int laneNumber)
        {

        }


        private static void ClearOldWaypointConnections(Transform holder)
        {
            WaypointSettingsBase[] allWaypoints = holder.GetComponentsInChildren<WaypointSettingsBase>();
            for (int i = 0; i < allWaypoints.Length; i++)
            {
                WaypointSettingsBase waypoint = allWaypoints[i];
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
            }
        }


        private void AddFinalWaypoints(RoadBase road, List<Transform> helpingPoints, int roadLayerMask)
        {
            float startPosition;
            if (road.nrOfLanes % 2 == 0)
            {
                startPosition = -road.laneWidth / 2;
            }
            else
            {
                startPosition = 0;
            }

            int laneModifier = 0;

            Transform lanesHolder = new GameObject(Constants.lanesHolderName).transform;
            lanesHolder.SetParent(road.transform, false);

            for (int i = 0; i < road.nrOfLanes; i++)
            {
                Transform laneHolder = AddLaneHolder(lanesHolder, Constants.laneNamePrefix + i);
                if (i % 2 == 0)
                {
                    laneModifier = -laneModifier;
                }
                else
                {
                    laneModifier = Mathf.Abs(laneModifier) + 1;
                }

                List<Transform> finalPoints = new List<Transform>();
                string waypointName;
                Vector3 waypointPosition;

                for (int j = 0; j < helpingPoints.Count - 1; j++)
                {
                    waypointPosition = helpingPoints[j].position + (startPosition + laneModifier * road.laneWidth) * helpingPoints[j].right;
                    if (PositionIsValid(helpingPoints, waypointPosition, Mathf.Abs(startPosition + laneModifier * road.laneWidth) - 0.1f))
                    {
                        waypointPosition = PutWaypointOnRoad(waypointPosition, helpingPoints[j].up, roadLayerMask);
                        if (PositionIsValid(finalPoints, waypointPosition, road.waypointDistance))
                        {
                            waypointName = road.name + "-" + Constants.laneNamePrefix + i + "-" + Constants.waypointNamePrefix + j;
                            finalPoints.Add(CreateWaypoint(laneHolder, waypointPosition, waypointName, road.GetAllowedCars(i), road.lanes[i].laneSpeed, road.laneWidth, null));
                        }
                    }
                }

                //add last point from the list
                if (!road.path.IsClosed)
                {
                    waypointPosition = helpingPoints[helpingPoints.Count - 1].position + (startPosition + laneModifier * road.laneWidth) * helpingPoints[helpingPoints.Count - 1].right;
                    waypointPosition = PutWaypointOnRoad(waypointPosition, helpingPoints[helpingPoints.Count - 1].up, roadLayerMask);
                    waypointName = road.name + "-" + Constants.laneNamePrefix + i + "-" + Constants.waypointNamePrefix + helpingPoints.Count;
                    finalPoints.Add(CreateWaypoint(laneHolder, waypointPosition, waypointName, road.GetAllowedCars(i), road.lanes[i].laneSpeed, road.laneWidth, null));
                }

                road.AddLaneConnector(finalPoints[0].GetComponent<WaypointSettingsBase>(), finalPoints[finalPoints.Count - 1].GetComponent<WaypointSettingsBase>(), i);
            }
        }


        private static void LinkNeighbors(RoadBase road)
        {
            for (int i = 0; i < road.nrOfLanes; i++)
            {
                Transform laneHolder = road.transform.Find(Constants.lanesHolderName).Find(Constants.laneNamePrefix + i);
                WaypointSettingsBase previousWaypoint = laneHolder.GetChild(0).GetComponent<WaypointSettingsBase>();
                for (int j = 1; j < laneHolder.childCount; j++)
                {
                    string waypointName = laneHolder.GetChild(j).name;
                    WaypointSettingsBase waypointScript = laneHolder.GetChild(j).GetComponent<WaypointSettingsBase>();
                    if (previousWaypoint != null)
                    {
                        previousWaypoint.neighbors.Add(waypointScript);
                        waypointScript.prev.Add(previousWaypoint);
                    }

                    if (!waypointName.Contains("Output"))
                    {
                        previousWaypoint = waypointScript;
                    }
                    else
                    {
                        previousWaypoint = null;
                    }
                }
                if(road.path.IsClosed)
                {
                    WaypointSettingsBase first = laneHolder.GetChild(0).GetComponent<WaypointSettingsBase>();
                    WaypointSettingsBase last = laneHolder.GetChild(laneHolder.childCount-1).GetComponent<WaypointSettingsBase>();
                    last.neighbors.Add(first);
                    first.prev.Add(last);
                }
            }
        }


        private static void DeleteHelpingPoints(List<Transform> helpingPoints)
        {
            DestroyImmediate(helpingPoints[0].transform.parent.gameObject);
        }


        private static Vector3 PutWaypointOnRoad(Vector3 waypointPosition, Vector3 perpendicular, int groundLayermask)
        {
            if (GleyPrefabUtilities.EditingInsidePrefab())
            {
                if (GleyPrefabUtilities.GetScenePrefabRoot().scene.GetPhysicsScene().Raycast(waypointPosition + 5 * perpendicular, -perpendicular, out RaycastHit hitInfo, Mathf.Infinity, groundLayermask))
                {
                    return hitInfo.point;
                }
            }
            else
            {
                if (Physics.Raycast(waypointPosition + 5 * perpendicular, -perpendicular, out RaycastHit hitInfo, Mathf.Infinity, groundLayermask))
                {
                    return hitInfo.point;
                }
            }
            return waypointPosition;
        }


        private static bool PositionIsValid(List<Transform> helpingPoints, Vector3 waypointPosition, float limit)
        {
            for (int i = 0; i < helpingPoints.Count; i++)
            {
                if (Vector3.Distance(helpingPoints[i].position, waypointPosition) < limit)
                {
                    return false;
                }
            }
            return true;
        }


        private static Transform AddLaneHolder(Transform parent, string laneName)
        {
            GameObject lane = new GameObject(laneName);
            lane.transform.SetParent(parent, false);
            return lane.transform;
        }
    }
}
