using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class PathFindingWindow : SetupWindowBase
    {
        private PathFindingSave save;
        private TrafficSettingsLoader settingsLoader;
        private List<WaypointSettings> waypointsOfInterest;
        private WaypointDrawer waypointDrawer;
        private float scrollAdjustment = 112;
        private List<int> penalties;
        private bool showPenaltyEditedWaypoints;
        private RoadColors roadColors;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            waypointDrawer = CreateInstance<WaypointDrawer>();
            settingsLoader = new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
            roadColors = settingsLoader.LoadRoadColors();
            save = settingsLoader.LoadPathFindingSettings();
            penalties = waypointDrawer.GetDifferentPenalties();
            if (save.penaltyColor.Count < penalties.Count)
            {
                int nrOfColors = penalties.Count - save.penaltyColor.Count;
                for (int i = 0; i < nrOfColors; i++)
                {
                    save.penaltyColor.Add(Color.white);
                    save.active.Add(true);
                }
            }
            waypointDrawer.onWaypointClicked += WaypointClicked;
            return this;
        }


        private void WaypointClicked(WaypointSettings clickedWaypoint, bool leftClick)
        {
            window.SetActiveWindow(typeof(EditWaypointWindow), true);
        }


        protected override void TopPart()
        {
            base.TopPart();
            save.enabled = EditorGUILayout.Toggle("Enable Path Finding", save.enabled);

            EditorGUI.BeginChangeCheck();
            showPenaltyEditedWaypoints = EditorGUILayout.Toggle("Show Edited Waypoints", showPenaltyEditedWaypoints);

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
           
        }


        protected override void ScrollPart(float width, float height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));
            if (showPenaltyEditedWaypoints)
            {
                if (waypointsOfInterest != null)
                {
                    if (waypointsOfInterest.Count == 0)
                    {
                        EditorGUILayout.LabelField("No " + GetWindowTitle());
                    }
                    for (int i = 0; i < waypointsOfInterest.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(waypointsOfInterest[i].name);
                        if (GUILayout.Button("View", GUILayout.Width(BUTTON_DIMENSION)))
                        {
                            GleyUtilities.TeleportSceneCamera(waypointsOfInterest[i].transform.position);
                            SceneView.RepaintAll();
                        }
                        if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_DIMENSION)))
                        {
                            OpenEditWindow(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No priority edited waypoints");
                }
            }
            else
            { 
                EditorGUILayout.LabelField("Waypoint Penalties: ");
                for (int i = 0; i < penalties.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(penalties[i].ToString(), GUILayout.MaxWidth(50));
                    save.penaltyColor[i] = EditorGUILayout.ColorField(save.penaltyColor[i]);
                    Color oldColor = GUI.backgroundColor;
                    if (save.active[i])
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    if (GUILayout.Button("View"))
                    {
                        save.active[i] = !save.active[i];
                        SceneView.RepaintAll();
                    }

                    GUI.backgroundColor = oldColor;
                    EditorGUILayout.EndHorizontal();
                } 
            }
            base.ScrollPart(width, height);
            EditorGUILayout.EndScrollView();
        }


        protected void OpenEditWindow(int index)
        {
            SettingsWindow.SetSelectedWaypoint((WaypointSettings)waypointsOfInterest[index]);
            GleyUtilities.TeleportSceneCamera(waypointsOfInterest[index].transform.position);
            window.SetActiveWindow(typeof(EditWaypointWindow), true);
        }


        public override void DrawInScene()
        {
            if (showPenaltyEditedWaypoints)
            {
                waypointsOfInterest = waypointDrawer.ShowPenaltyEditedWaypoints(roadColors.waypointColor);
            }
            else
            {
                for (int i = 0; i < penalties.Count; i++)
                {
                    if (save.active[i])
                    {
                        waypointDrawer.ShowPenalties(penalties[i], save.penaltyColor[i]);
                    }
                }
            }
            base.DrawInScene();
        }


        protected override void BottomPart()
        {
            if (GUILayout.Button("Save"))
            {
                    Save();
            }
            base.BottomPart();
        }


        private void Save()
        {
            Debug.Log("Save");
        }


        public override void DestroyWindow()
        {
            waypointDrawer.onWaypointClicked -= WaypointClicked;
            settingsLoader.SavePathFindingSettings(save);
            base.DestroyWindow();
        }
    }
}
