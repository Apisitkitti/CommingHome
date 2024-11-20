using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public abstract class ShowWaypointsTrafficBase : ShowWaypointsBase
    {
        protected WaypointDrawer waypointDrawer;
        protected TrafficSettingsLoader settingsLoader;

        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            waypointDrawer = CreateInstance<WaypointDrawer>();
            waypointDrawer.Initialize();
            waypointDrawer.onWaypointClicked += WaipointClicked;
            settingsLoader = LoadSettingsLoader();
            roadColors = settingsLoader.LoadRoadColors();
            base.Initialize(windowProperties, window);
            return this;
        }


        protected override void TopPart()
        {
            base.TopPart();
            EditorGUI.BeginChangeCheck();
            roadColors.waypointColor = EditorGUILayout.ColorField("Waypoint Color ", roadColors.waypointColor);

            EditorGUILayout.BeginHorizontal();
            save.showConnections = EditorGUILayout.Toggle("Show Connections", save.showConnections);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            save.showOtherLanes = EditorGUILayout.Toggle("Show Lane Change", save.showOtherLanes);
            roadColors.laneChangeColor = EditorGUILayout.ColorField(roadColors.laneChangeColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            save.showSpeed = EditorGUILayout.Toggle("Show Speed", save.showSpeed);
            roadColors.speedColor = EditorGUILayout.ColorField(roadColors.speedColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            save.showCars = EditorGUILayout.Toggle("Show Cars", save.showCars);
            roadColors.carsColor = EditorGUILayout.ColorField(roadColors.carsColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            save.showPriority = EditorGUILayout.Toggle("Show Waypoint Priority", save.showPriority);
            roadColors.priorityColor = EditorGUILayout.ColorField(roadColors.priorityColor);
            EditorGUILayout.EndHorizontal();

            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
        }


        protected TrafficSettingsLoader LoadSettingsLoader()
        {
            return new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
        }


        protected override void OpenEditWindow(int index)
        {
            SettingsWindow.SetSelectedWaypoint((WaypointSettings)waypointsOfInterest[index]);
            GleyUtilities.TeleportSceneCamera(waypointsOfInterest[index].transform.position);
            window.SetActiveWindow(typeof(EditWaypointWindow), true);
        }


        protected virtual void WaipointClicked(WaypointSettings clickedWaypoint, bool leftClick)
        {
            window.SetActiveWindow(typeof(EditWaypointWindow), true);
        }


        public override void DestroyWindow()
        {
            waypointDrawer.onWaypointClicked -= WaipointClicked;
            base.DestroyWindow();
        }
    }
}