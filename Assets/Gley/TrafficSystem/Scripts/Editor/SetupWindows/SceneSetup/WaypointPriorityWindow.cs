using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class WaypointPriorityWindow : SetupWindowBase
    {
        private List<int> priorities;
        private WaypointPrioritySave save;
        private float scrollAdjustment = 112;
        private WaypointDrawer waypointDrawer;
        private TrafficSettingsLoader settingsLoader;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            waypointDrawer = CreateInstance<WaypointDrawer>();
            priorities = waypointDrawer.GetDifferentPriorities();
            settingsLoader = new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
            save = settingsLoader.LoadPriorityWaypoints();
            if (save.priorityColor.Count < priorities.Count)
            {
                int nrOfColors = priorities.Count - save.priorityColor.Count;
                for (int i = 0; i < nrOfColors; i++)
                {
                    save.priorityColor.Add(Color.white);
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


        public override void DrawInScene()
        {
            for (int i = 0; i < priorities.Count; i++)
            {
                if (save.active[i])
                {
                    waypointDrawer.ShowPriorities(priorities[i], save.priorityColor[i]);
                }
            }

            base.DrawInScene();
        }


        protected override void ScrollPart(float width, float height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));
            EditorGUILayout.LabelField("Waypoint Priorities: ");
            for (int i = 0; i < priorities.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(priorities[i].ToString(), GUILayout.MaxWidth(50));
                save.priorityColor[i] = EditorGUILayout.ColorField(save.priorityColor[i]);
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

            base.ScrollPart(width, height);
            EditorGUILayout.EndScrollView();
        }


        public override void DestroyWindow()
        {
            waypointDrawer.onWaypointClicked -= WaypointClicked;
            settingsLoader.SavePriorityWaypoints(save);
            base.DestroyWindow();
        }
    }
}
