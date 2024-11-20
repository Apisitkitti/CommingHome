using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class NewRoadWindow : NewRoadWindowBase
    {
        protected TrafficSettingsLoader settingsLoader;
        protected TrafficSettingsLoader LoadSettingsLoader()
        {
            return new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
        }


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            settingsLoader = LoadSettingsLoader();
            save = settingsLoader.LoadCreateRoadSave();
            roadColors = settingsLoader.LoadRoadColors();
            return base.Initialize(windowProperties, window);
        }


        protected override void TopPart()
        {
            base.TopPart();
            EditorGUILayout.Space();
            save.leftSideTraffic = EditorGUILayout.Toggle("LeftSideTraffic", save.leftSideTraffic);
        }


        public override void DrawInScene()
        {
            if (firstClick != Vector3.zero)
            {
                Handles.SphereHandleCap(0, firstClick, Quaternion.identity, 1, EventType.Repaint);
            }

            if (save.viewOtherRoads)
            {
                for (int i = 0; i < allRoads.Count; i++)
                {
                    roadDrawer.DrawPath(allRoads[i], MoveTools.None, roadColors.roadColor, roadColors.controlPointColor, roadColors.controlPointColor, roadColors.textColor);
                    if (save.viewRoadsSettings.viewLanes)
                    {
                        DrawAllLanes(i);
                    }
                }
            }
            base.DrawInScene();
        }


        protected override List<RoadBase> LoadAllRoads()
        {
            return RoadsLoader.Initialize().LoadAllRoads<Road>().Cast<RoadBase>().ToList();
        }


        protected override void ScrollPart(float width, float height)
        {
            EditorGUI.BeginChangeCheck();
            roadColors.textColor = EditorGUILayout.ColorField("Text Color", roadColors.textColor);

            EditorGUILayout.BeginHorizontal();
            save.viewOtherRoads = EditorGUILayout.Toggle("View Other Roads", save.viewOtherRoads, GUILayout.Width(TOGGLE_WIDTH));
            roadColors.roadColor = EditorGUILayout.ColorField(roadColors.roadColor);
            EditorGUILayout.EndHorizontal();

            if (save.viewOtherRoads)
            {
                EditorGUILayout.BeginHorizontal();
                save.viewRoadsSettings.viewLanes = EditorGUILayout.Toggle("View Lanes", save.viewRoadsSettings.viewLanes, GUILayout.Width(TOGGLE_WIDTH));
                roadColors.laneColor = EditorGUILayout.ColorField(roadColors.laneColor);
                EditorGUILayout.EndHorizontal();

                if (save.viewRoadsSettings.viewLanes)
                {
                    EditorGUILayout.BeginHorizontal();
                    save.viewRoadsSettings.viewWaypoints = EditorGUILayout.Toggle("View Waypoints", save.viewRoadsSettings.viewWaypoints, GUILayout.Width(TOGGLE_WIDTH));
                    roadColors.waypointColor = EditorGUILayout.ColorField(roadColors.waypointColor);
                    roadColors.disconnectedColor = EditorGUILayout.ColorField(roadColors.disconnectedColor);
                    if (save.viewRoadsSettings.viewWaypoints == false)
                    {
                        save.viewRoadsSettings.viewLaneChanges = false;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    save.viewRoadsSettings.viewLaneChanges = EditorGUILayout.Toggle("View Lane Changes", save.viewRoadsSettings.viewLaneChanges, GUILayout.Width(TOGGLE_WIDTH));
                    if (save.viewRoadsSettings.viewLaneChanges == true)
                    {
                        save.viewRoadsSettings.viewWaypoints = true;
                    }
                    roadColors.laneChangeColor = EditorGUILayout.ColorField(roadColors.laneChangeColor);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUI.EndChangeCheck();

            if (GUI.changed)
            {

                SceneView.RepaintAll();
            }
            base.ScrollPart(width, height);
        }


        protected override void DrawAllLanes(int roadIndex)
        {
            LaneDrawer.DrawAllLanes(allRoads[roadIndex], save.viewRoadsSettings.viewWaypoints, save.viewRoadsSettings.viewLaneChanges, roadColors.laneColor,
                            roadColors.waypointColor, roadColors.disconnectedColor, roadColors.laneChangeColor, roadColors.textColor);
            base.DrawAllLanes(roadIndex);
        }


        protected override void CreateRoad()
        {
            Road selectedRoad = new RoadCreator().Create<Road, ConnectionPool>(firstClick, Internal.Constants.trafficWaypointsHolderName, settingsLoader.LoadRoadDefaultsSave(), "Road");
            selectedRoad.CreatePath(firstClick, secondClick);
            selectedRoad.SetRoadProperties(settingsLoader.LoadEditRoadSave().maxSpeed, System.Enum.GetValues(typeof(VehicleTypes)).Length, save.leftSideTraffic);
            SettingsWindow.SetSelectedRoad(selectedRoad);
            window.SetActiveWindow(typeof(EditRoadWindow), false);
            base.CreateRoad();
        }


        protected override void SetTopText()
        {
            EditorGUILayout.LabelField("Press SHIFT + Left Click to add a road point");
            EditorGUILayout.LabelField("Press SHIFT + Right Click to remove a road point");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("If you are not able to draw, make sure your ground/road is on the layer marked as Road inside Layer Setup");
        }


        public override void DestroyWindow()
        {
            settingsLoader.SaveCreateRoadSettings(save, roadColors);
            base.DestroyWindow();
        }
    }
}