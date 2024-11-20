using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class WaypointDrawer : WaypointDrawerBase<WaypointSettings>
    {
        private List<WaypointSettings> speedEditedWaypoints;
        private List<WaypointSettings> penaltyEditedWaypoints;
        private List<WaypointSettings> priorityEditedWaypoints;
        private List<WaypointSettings> giveWayWaypoints;
        private List<WaypointSettings> complexGiveWayWaypoints;
        private List<WaypointSettings> eventWaypoints;
        private List<WaypointSettings> zipperGiveWayWaypoints;
        private List<WaypointSettings> pathProblems;
        private List<WaypointSettings> carTypeEditedWaypoints;


        protected override void TriggetWaypointClickedEvent(WaypointSettings clickedWaypoint, bool leftClick)
        {
            SettingsWindow.SetSelectedWaypoint(clickedWaypoint);
            base.TriggetWaypointClickedEvent(clickedWaypoint, leftClick);
        }


        protected override void LoadWaypoints()
        {
            base.LoadWaypoints();
            UpdateCarTypeEditedWaypoints();
            UpdateSpeedEditedWaypoints();
            UpdatePenaltyEditedWaypoints();
            UpdatePriorityEditedWaypoints();
            UpdateGiveWayWaypoints();
            UpdateComplexGiveWayWaypoints();
            UpdateZipperGiveWayWaypoints();
            UpdateEventWaypoints();
        }


        protected void DrawWaypointConnections(WaypointSettingsBase waypoint, Color color, bool drawPrev, Color prevColor)
        {
            if (!waypoint)
                return;
            Handles.color = color;
            if (waypoint.neighbors.Count > 0)
            {
                for (int i = 0; i < waypoint.neighbors.Count; i++)
                {
                    if (waypoint.neighbors[i] != null)
                    {
                        Handles.DrawLine(waypoint.transform.position, waypoint.neighbors[i].transform.position);

                        Vector3 direction = (waypoint.transform.position - waypoint.neighbors[i].transform.position).normalized;
                        Vector3 point1 = (waypoint.transform.position + waypoint.neighbors[i].transform.position) / 2;

                        Vector3 point2 = point1 + Quaternion.Euler(0, -25, 0) * direction;

                        Vector3 point3 = point1 + Quaternion.Euler(0, 25, 0) * direction;

                        Handles.DrawPolyLine(point1, point2, point3, point1);
                    }
                    else
                    {
                        Debug.LogWarning("waypoint " + waypoint.name + " has missing neighbors", waypoint);
                    }
                }
            }



            if (drawPrev)
            {
                Handles.color = prevColor;
                for (int i = 0; i < waypoint.prev.Count; i++)
                {
                    if (waypoint.prev[i] != null)
                    {
                        Handles.DrawLine(waypoint.transform.position, waypoint.prev[i].transform.position);
                    }
                }
            }
        }


        internal List<WaypointSettings> ShowSpeedEditedWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].speedLocked)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                }
            }
            if (nr != speedEditedWaypoints.Count)
            {
                UpdateSpeedEditedWaypoints();
            }

            return speedEditedWaypoints;
        }


        internal List<WaypointSettings> ShowPenaltyEditedWaypoints(Color waypointColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].penaltyLocked)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, false, default, false, default, false, default, false, default, false, default);
                }
            }
            if (nr != penaltyEditedWaypoints.Count)
            {
                UpdatePenaltyEditedWaypoints();
            }

            return penaltyEditedWaypoints;
        }


        internal List<WaypointSettings> ShowPriorityEditedWaypoints(Color waypointColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].priorityLocked)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, false, default, false, default, false, default, false, default, false, default);
                }
            }
            if (nr != priorityEditedWaypoints.Count)
            {
                UpdatePriorityEditedWaypoints();
            }

            return priorityEditedWaypoints;
        }


        private void UpdateSpeedEditedWaypoints()
        {
            speedEditedWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].speedLocked == true)
                {
                    speedEditedWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        private void UpdatePriorityEditedWaypoints()
        {
            priorityEditedWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].priorityLocked == true)
                {
                    priorityEditedWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        private void UpdatePenaltyEditedWaypoints()
        {
            penaltyEditedWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].penaltyLocked == true)
                {
                    penaltyEditedWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        internal List<WaypointSettings> ShowGiveWayWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].giveWay)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                }
            }

            if (nr != giveWayWaypoints.Count)
            {
                UpdateGiveWayWaypoints();
            }

            return giveWayWaypoints;
        }


        internal List<WaypointSettings> ShowComplexGiveWayWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].complexGiveWay)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority,priorityColor);
                }
            }

            if (nr != complexGiveWayWaypoints.Count)
            {
                UpdateComplexGiveWayWaypoints();
            }

            return complexGiveWayWaypoints;
        }


        internal List<WaypointSettings> ShowEventWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].triggerEvent)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                }
            }

            if (nr != eventWaypoints.Count)
            {
                UpdateEventWaypoints();
            }

            return eventWaypoints;
        }


        internal List<WaypointSettings> ShowZipperGiveWayWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].zipperGiveWay)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                }
            }

            if (nr != zipperGiveWayWaypoints.Count)
            {
                UpdateZipperGiveWayWaypoints();
            }

            return zipperGiveWayWaypoints;
        }


        internal List<WaypointSettings> ShowVehicleProblems(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            pathProblems = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (GleyUtilities.IsPointInsideView(allWaypoints[i].transform.position))
                {
                    int nr = allWaypoints[i].allowedCars.Count;
                    for (int j = 0; j < allWaypoints[i].allowedCars.Count; j++)
                    {
                        for (int k = 0; k < allWaypoints[i].neighbors.Count; k++)
                        {
                            if (((WaypointSettings)allWaypoints[i].neighbors[k]).allowedCars.Contains(allWaypoints[i].allowedCars[j]))
                            {
                                nr--;
                                break;
                            }
                        }
                    }
                    if (nr != 0)
                    {
                        pathProblems.Add(allWaypoints[i]);
                        DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, true, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);

                        for (int k = 0; k < allWaypoints[i].neighbors.Count; k++)
                        {
                            for (int j = 0; j < ((WaypointSettings)allWaypoints[i].neighbors[k]).allowedCars.Count; j++)
                            {
                                DrawCompleteWaypoint(allWaypoints[i].neighbors[k], connectionColor, showConnections, connectionColor, showSpeed, speedColor, true, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                            }
                        }
                    }
                }

            }
            return pathProblems;
        }


        internal List<int> GetDifferentSpeeds()
        {
            List<int> result = new List<int>();
            LoadWaypoints();

            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (!result.Contains(allWaypoints[i].maxSpeed))
                {
                    result.Add(allWaypoints[i].maxSpeed);
                }
            }
            return result;
        }


        internal List<int> GetDifferentPriorities()
        {
            List<int> result = new List<int>();
            LoadWaypoints();

            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (!result.Contains(allWaypoints[i].priority))
                {
                    result.Add(allWaypoints[i].priority);
                }
            }
            return result;
        }


        internal List<int> GetDifferentPenalties()
        {
            List<int> result = new List<int>();
            LoadWaypoints();

            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (!result.Contains(allWaypoints[i].penalty))
                {
                    result.Add(allWaypoints[i].penalty);
                }
            }
            return result;
        }


        internal void ShowSpeedLimits(int speed, Color color)
        {
            if (color.a == 0)
            {
                color = Color.white;
            }
            Handles.color = color;
            if (allWaypoints == null)
            {
                LoadWaypoints();
            }

            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (GleyUtilities.IsPointInsideView(allWaypoints[i].transform.position))
                {
                    if (allWaypoints[i].maxSpeed == speed)
                    {
                        if (Handles.Button(allWaypoints[i].transform.position, Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up), 0.5f, 0.5f, Handles.DotHandleCap))
                        {
                            TriggetWaypointClickedEvent(allWaypoints[i], Event.current.button == 0);
                        }
                        for (int j = 0; j < allWaypoints[i].neighbors.Count; j++)
                        {
                            Handles.DrawLine(allWaypoints[i].transform.position, allWaypoints[i].neighbors[j].transform.position);
                        }
                    }
                }
            }
        }


        internal void ShowPriorities(int priority, Color color)
        {
            if (color.a == 0)
            {
                color = Color.white;
            }
            Handles.color = color;
            if (allWaypoints == null)
            {
                LoadWaypoints();
            }

            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (GleyUtilities.IsPointInsideView(allWaypoints[i].transform.position))
                {
                    if (allWaypoints[i].priority == priority)
                    {
                        if (Handles.Button(allWaypoints[i].transform.position, Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up), 0.5f, 0.5f, Handles.DotHandleCap))
                        {
                            TriggetWaypointClickedEvent(allWaypoints[i], Event.current.button == 0);
                        }
                        for (int j = 0; j < allWaypoints[i].neighbors.Count; j++)
                        {
                            Handles.DrawLine(allWaypoints[i].transform.position, allWaypoints[i].neighbors[j].transform.position);
                        }
                    }
                }
            }
        }


        internal void ShowPenalties(int penalty, Color color)
        {
            if (color.a == 0)
            {
                color = Color.white;
            }
            Handles.color = color;
            if (allWaypoints == null)
            {
                LoadWaypoints();
            }

            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (GleyUtilities.IsPointInsideView(allWaypoints[i].transform.position))
                {
                    if (allWaypoints[i].penalty == penalty)
                    {
                        if (Handles.Button(allWaypoints[i].transform.position, Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up), 0.5f, 0.5f, Handles.DotHandleCap))
                        {
                            TriggetWaypointClickedEvent(allWaypoints[i], Event.current.button == 0);
                        }
                        for (int j = 0; j < allWaypoints[i].neighbors.Count; j++)
                        {
                            Handles.DrawLine(allWaypoints[i].transform.position, allWaypoints[i].neighbors[j].transform.position);
                        }
                    }
                }
            }
        }


        internal override void DrawWaypointsForCar(int car, Color color)
        {
            Handles.color = color;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (GleyUtilities.IsPointInsideView(allWaypoints[i].transform.position))
                {
                    if (allWaypoints[i].allowedCars.Contains((VehicleTypes)car))
                    {
                        if (Handles.Button(allWaypoints[i].transform.position, Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up), 0.5f, 0.5f, Handles.DotHandleCap))
                        {
                            TriggetWaypointClickedEvent(allWaypoints[i], Event.current.button == 0);
                        }
                        for (int j = 0; j < allWaypoints[i].neighbors.Count; j++)
                        {
                            Handles.DrawLine(allWaypoints[i].transform.position, allWaypoints[i].neighbors[j].transform.position);
                        }
                    }
                }
            }
        }


        internal List<WaypointSettings> ShowCarTypeEditedWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].carsLocked)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                }
            }

            if (nr != carTypeEditedWaypoints.Count)
            {
                UpdateCarTypeEditedWaypoints();
            }

            return carTypeEditedWaypoints;
        }


        private void UpdateCarTypeEditedWaypoints()
        {
            carTypeEditedWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].carsLocked == true)
                {
                    carTypeEditedWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        private void UpdateGiveWayWaypoints()
        {
            giveWayWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].giveWay == true)
                {
                    giveWayWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        private void UpdateComplexGiveWayWaypoints()
        {
            complexGiveWayWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].complexGiveWay == true)
                {
                    complexGiveWayWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        private void UpdateEventWaypoints()
        {
            eventWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].triggerEvent == true)
                {
                    eventWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        private void UpdateZipperGiveWayWaypoints()
        {
            zipperGiveWayWaypoints = new List<WaypointSettings>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].zipperGiveWay == true)
                {
                    zipperGiveWayWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        protected void DrawCompleteWaypoint(WaypointSettingsBase waypoint, Color color, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor, bool drawPrev = false, Color prevColor = new Color())
        {
            if (!waypoint)
                return;
            if (GleyUtilities.IsPointInsideView(waypoint.transform.position))
            {
                DrawClickableButton((WaypointSettings)waypoint, color);
                if (showConnections)
                {
                    DrawTrafficWaypointConnections(waypoint, connectionColor, drawOtherLanes, otherLanesColor, drawPrev, prevColor);
                }
                if (showSpeed)
                {
                    ShowSpeed((WaypointSettings)waypoint, speedColor);
                }
                if (showCars)
                {
                    ShowCars((WaypointSettings)waypoint, carsColor);
                }
                if (showPriority)
                {
                    ShowPriority((WaypointSettings)waypoint, priorityColor);
                }
            }
        }


        private void ShowSpeed(WaypointSettings waypoint, Color color)
        {
            if (!waypoint)
                return;
            style.normal.textColor = color;
            Handles.Label(waypoint.transform.position, waypoint.maxSpeed.ToString(), style);
        }


        private void ShowPriority(WaypointSettings waypoint, Color color)
        {
            if (!waypoint)
                return;
            style.normal.textColor = color;
            Handles.Label(waypoint.transform.position, waypoint.priority.ToString(), style);
        }


        private void ShowCars(WaypointSettings waypoint, Color color)
        {
            if (!waypoint)
                return;
            style.normal.textColor = color;
            string text = "";
            for (int j = 0; j < waypoint.allowedCars.Count; j++)
            {
                text += waypoint.allowedCars[j] + "\n";
            }
            Handles.Label(waypoint.transform.position, text, style);
        }


        protected void DrawTrafficWaypointConnections(WaypointSettingsBase waypoint, Color color, bool drawOtherLanes, Color otherLanesColor, bool drawPrev, Color prevColor)
        {
            DrawWaypointConnections(waypoint, color, drawPrev, prevColor);

            if (drawOtherLanes)
            {
                if (waypoint.otherLanes != null)
                {
                    for (int i = 0; i < waypoint.otherLanes.Count; i++)
                    {
                        if (waypoint.otherLanes[i] != null)
                        {
                            DrawTriangle(waypoint.transform.position, waypoint.otherLanes[i].transform.position, otherLanesColor, true);
                        }
                    }
                }
            }
        }


        private void DrawTriangle(Vector3 start, Vector3 end, Color waypointColor, bool drawLane)
        {
            Handles.color = waypointColor;
            Vector3 direction = (start - end).normalized;

            Vector3 point2 = end + Quaternion.Euler(0, -25, 0) * direction;

            Vector3 point3 = end + Quaternion.Euler(0, 25, 0) * direction;

            Handles.DrawPolyLine(end, point2, point3, end);

            if (drawLane)
            {
                Handles.DrawLine(start, end);
            }
        }


        internal void DrawAllWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
            }
        }


        internal List<WaypointSettingsBase> ShowDisconnectedWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].neighbors.Count == 0)
                {
                    nr++;
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, false, Color.white, showPriority, priorityColor);
                }
            }

            if (nr != disconnectedWaypoints.Count)
            {
                UpdateDisconnectedWaypoints();
            }
            return disconnectedWaypoints;
        }


        internal List<WaypointSettingsBase> ShowStopWaypoints(Color waypointColor, bool showConnections, Color connectionColor, bool showSpeed, Color speedColor, bool showCars, Color carsColor, bool drawOtherLanes, Color otherLanesColor, bool showPriority, Color priorityColor)
        {
            int nr = 0;
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (GleyUtilities.IsPointInsideView(allWaypoints[i].transform.position))
                {
                    if (allWaypoints[i].stop)
                    {
                        nr++;
                        DrawCompleteWaypoint(allWaypoints[i], waypointColor, showConnections, connectionColor, showSpeed, speedColor, showCars, carsColor, drawOtherLanes, otherLanesColor, showPriority, priorityColor);
                    }
                }
            }

            if (nr != stopWaypoints.Count)
            {
                UpdateStopWaypoints();
            }

            return stopWaypoints;
        }


        internal void DrawWaypointsForLink(WaypointSettingsBase currentWaypoint, List<WaypointSettingsBase> neighborsList, List<WaypointSettingsBase> otherLinesList, Color waypointColor, Color speedColor)
        {
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i] != currentWaypoint && !neighborsList.Contains(allWaypoints[i]) && !otherLinesList.Contains(allWaypoints[i]))
                {
                    DrawCompleteWaypoint(allWaypoints[i], waypointColor, true, waypointColor, true, speedColor, false, Color.white, false, Color.white, false, Color.white);
                }
            }
        }


        internal void DrawCurrentWaypoint(WaypointSettingsBase waypoint, Color selectedColor, Color waypointColor, Color otherLaneColor, Color prevColor, Color giveWayColor)
        {
            DrawCompleteWaypoint(waypoint, selectedColor, true, waypointColor, false, Color.white, false, Color.white, true, otherLaneColor, true, prevColor);
            for (int i = 0; i < waypoint.neighbors.Count; i++)
            {
                if (waypoint.neighbors[i] != null)
                {
                    DrawCompleteWaypoint(waypoint.neighbors[i], waypointColor, false, waypointColor, false, Color.white, false, Color.white, true, otherLaneColor, false, prevColor);
                }
            }
            for (int i = 0; i < waypoint.prev.Count; i++)
            {
                if (waypoint.prev[i] != null)
                {
                    DrawCompleteWaypoint(waypoint.prev[i], prevColor, false, waypointColor, false, Color.white, false, Color.white, true, otherLaneColor, false, prevColor);
                }
            }
            for (int i = 0; i < waypoint.otherLanes.Count; i++)
            {
                if (waypoint.otherLanes != null)
                {
                    DrawCompleteWaypoint(waypoint.otherLanes[i], otherLaneColor, false, waypointColor, false, Color.white, false, Color.white, true, otherLaneColor, false, prevColor);
                }
            }

            for (int i = 0; i < waypoint.giveWayList.Count; i++)
            {
                if (waypoint.otherLanes != null)
                {
                    DrawCompleteWaypoint(waypoint.giveWayList[i], giveWayColor, false, waypointColor, false, Color.white, false, Color.white, true, otherLaneColor, false, prevColor);
                }

            }
        }
    }
}
