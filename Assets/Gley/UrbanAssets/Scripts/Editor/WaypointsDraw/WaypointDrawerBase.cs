using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public abstract class WaypointDrawerBase<T> : UnityEditor.Editor where T : WaypointSettingsBase
    {
        public delegate void WaypointClicked(T clickedWaypoint, bool leftClick);
        public event WaypointClicked onWaypointClicked;
        protected virtual void TriggetWaypointClickedEvent(T clickedWaypoint, bool leftClick)
        {
            if (onWaypointClicked != null)
            {
                onWaypointClicked(clickedWaypoint, leftClick);
            }
        }


        protected List<T> allWaypoints;
        protected GUIStyle style;

        protected List<WaypointSettingsBase> disconnectedWaypoints;
        protected List<WaypointSettingsBase> stopWaypoints;

        internal abstract void DrawWaypointsForCar(int car, Color color);


        internal void Initialize()
        {
            style = new GUIStyle();
            LoadWaypoints();
        }


        internal void DrawSelectedWaypoint(WaypointSettingsBase selectedWaypoint, Color color)
        {
            Handles.color = color;
            Handles.CubeHandleCap(0, selectedWaypoint.transform.position, Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up), 1, EventType.Repaint);
        }


        protected virtual void LoadWaypoints()
        {
            if (!GleyPrefabUtilities.EditingInsidePrefab())
            {
                allWaypoints = FindObjectsByType<T>(FindObjectsSortMode.None).ToList();
            }
            else
            {
                allWaypoints = GleyPrefabUtilities.GetScenePrefabRoot().GetComponentsInChildren<T>().ToList();
            }
            UpdateDisconnectedWaypoints();
            UpdateStopWaypoints();
        }


        protected void DrawClickableButton(T waypoint, Color color)
        {
            if (!waypoint)
                return;
            Handles.color = color;
            if (Handles.Button(waypoint.transform.position, Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up), 0.5f, 0.5f, Handles.DotHandleCap))
            {
                TriggetWaypointClickedEvent(waypoint, Event.current.button == 0);
            }

        }


        protected void UpdateDisconnectedWaypoints()
        {
            disconnectedWaypoints = new List<WaypointSettingsBase>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].neighbors != null)
                {
                    if (allWaypoints[i].neighbors.Count == 0)
                    {
                        disconnectedWaypoints.Add(allWaypoints[i]);
                    }
                    else
                    {
                        for (int j = 0; j < allWaypoints[i].neighbors.Count; j++)
                        {
                            if (allWaypoints[i].neighbors[j] == null)
                            {
                                allWaypoints[i].neighbors.RemoveAt(j);
                                disconnectedWaypoints.Add(allWaypoints[i]);
                            }
                        }
                    }
                }
                else
                {
                    allWaypoints[i].neighbors = new List<WaypointSettingsBase>();
                    disconnectedWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        protected void UpdateStopWaypoints()
        {
            stopWaypoints = new List<WaypointSettingsBase>();
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].stop == true)
                {
                    stopWaypoints.Add(allWaypoints[i]);
                }
            }
        }


        public void DrawDirectionWaypoint(WaypointSettingsBase stopWaypoint, Color stopWaypointsColor)
        {
            Handles.color = stopWaypointsColor;
            if (stopWaypoint != null)
            {
                Handles.DrawSolidDisc(stopWaypoint.transform.position, Vector3.up, 1);
            }
        }
    }
}
