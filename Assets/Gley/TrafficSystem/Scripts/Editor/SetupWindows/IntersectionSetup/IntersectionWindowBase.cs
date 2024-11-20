using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class IntersectionWindowBase : SetupWindowBase
    {
        protected List<IntersectionStopWaypointsSettings> stopWaypoints = new List<IntersectionStopWaypointsSettings>();
        protected List<WaypointSettings> exitWaypoints = new List<WaypointSettings>();
        protected GenericIntersectionSettings selectedIntersection;
        protected IntersectionSave intersectionSave;
        protected RoadColors save;
        protected WaypointDrawer waypointDrawer;
        protected int selectedRoad;
        protected bool hideWaypoints;
        protected float trafficWaypointSize = 1;
        protected float pedestrianWaypointSize = 0.5f;

        private TrafficSettingsLoader settingsLoader;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            selectedRoad = -1;
            name = selectedIntersection.name;
            waypointDrawer = CreateInstance<WaypointDrawer>();
            waypointDrawer.Initialize();

            settingsLoader = new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
            save = settingsLoader.LoadRoadColors();
            intersectionSave = settingsLoader.LoadIntersectionsSettings();
            if (stopWaypoints == null)
            {
                stopWaypoints = new List<IntersectionStopWaypointsSettings>();
            }
            if (exitWaypoints == null)
            {
                exitWaypoints = new List<WaypointSettings>();
            }

            return this;
        }


        protected void DrawExitWaypoints()
        {
            for (int i = 0; i < exitWaypoints.Count; i++)
            {
                if (exitWaypoints[i] != null)
                {
                    if (exitWaypoints[i].draw)
                    {
                        IntersectionDrawer.DrawIntersectionWaypoint(exitWaypoints[i], intersectionSave.exitWaypointsColor, 0, save.textColor, trafficWaypointSize);
                    }
                }
            }
        }


        protected void DrawStopWaypoints(bool includePedestrians)
        {
            for (int i = 0; i < stopWaypoints.Count; i++)
            {
                for (int j = 0; j < stopWaypoints[i].roadWaypoints.Count; j++)
                {
                    if (stopWaypoints[i].roadWaypoints[j].draw)
                    {
                        IntersectionDrawer.DrawIntersectionWaypoint(stopWaypoints[i].roadWaypoints[j], intersectionSave.stopWaypointsColor, i + 1, save.textColor, trafficWaypointSize);
                    }
                }
                if (includePedestrians)
                {
#if GLEY_PEDESTRIAN_SYSTEM
                    IntersectionDrawer.DrawListWaypoints(stopWaypoints[i].pedestrianWaypoints, intersectionSave.stopWaypointsColor, -1, Color.white, pedestrianWaypointSize);
                    IntersectionDrawer.DrawListWaypoints(stopWaypoints[i].directionWaypoints, intersectionSave.exitWaypointsColor, -1, Color.white, pedestrianWaypointSize);
#endif
                }
            }
        }


        protected void DisplayList<T>(List<T> list, ref bool globalDraw) where T : WaypointSettingsBase
        {
            Color oldColor;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    continue;
                }
                EditorGUILayout.BeginHorizontal();

                list[i] = (T)EditorGUILayout.ObjectField(list[i], typeof(T), true);

                oldColor = GUI.backgroundColor;
                if (list[i].draw == true)
                {
                    GUI.backgroundColor = Color.green;
                }
                if (GUILayout.Button("View"))
                {
                    ViewWaypoint(list[i], ref globalDraw);
                }
                GUI.backgroundColor = oldColor;

                if (GUILayout.Button("Delete"))
                {
                    list.RemoveAt(i);
                    SceneView.RepaintAll();
                }
                EditorGUILayout.EndHorizontal();
            }
        }


        private void ViewWaypoint(WaypointSettingsBase waypoint, ref bool globalDraw)
        {
            waypoint.draw = !waypoint.draw;
            if (waypoint.draw == false)
            {
                globalDraw = false;
            }
            SceneView.RepaintAll();
        }


        protected override void TopPart()
        {
            name = EditorGUILayout.TextField("Intersection Name", name);
            if (GUILayout.Button("Save"))
            {
                SaveSettings();
            }
            EditorGUI.BeginChangeCheck();
            hideWaypoints = EditorGUILayout.Toggle("Hide Waypoints ", hideWaypoints);
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                window.BlockClicks(!hideWaypoints);
            }
            base.TopPart();
        }


        protected void AddLightObjects(string title, List<GameObject> objectsList)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(title + ":");
            for (int i = 0; i < objectsList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                objectsList[i] = (GameObject)EditorGUILayout.ObjectField(objectsList[i], typeof(GameObject), true);

                if (GUILayout.Button("Delete"))
                {
                    objectsList.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add " + title + " Objects"))
            {
                objectsList.Add(null);
            }
            EditorGUILayout.EndVertical();
        }


        protected void AddWaypointToList<T>(T waypoint, List<T> listToAdd) where T : WaypointSettingsBase
        {
            if (!listToAdd.Contains(waypoint))
            {
                waypoint.draw = true;
                listToAdd.Add(waypoint);
            }
            else
            {
                listToAdd.Remove(waypoint);
            }
        }


        private void SaveSettings()
        {
            selectedIntersection.gameObject.name = name;
            if (stopWaypoints.Count > 0)
            {
                Vector3 position = new Vector3();
                for (int i = 0; i < stopWaypoints.Count; i++)
                {
                    position += stopWaypoints[i].roadWaypoints[0].transform.position;
                }
                selectedIntersection.transform.position = position / stopWaypoints.Count;
            }
        }
    }
}
