using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class ViewRoadsWindow : ViewRoadsWindowBase
    {
        const float maxValueColor = 256;
        const float maxValue = 220;

        private TrafficSettingsLoader settingsLoader;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            settingsLoader = LoadSettingsLoader();
            save = settingsLoader.LoadViewRoadsSave();
            roadColors = settingsLoader.LoadRoadColors();
            return base.Initialize(windowProperties, window);
        }


        public override void DrawInScene()
        {
            base.DrawInScene();

            if (GleyUtilities.SceneCameraMoved())
            {
                SettingsWindowBase.TriggerRefreshWindowEvent();
            }

            for (int i = 0; i < allRoads.Count; i++)
            {
                if (allRoads[i].draw)
                {
                    roadDrawer.DrawPath(allRoads[i], MoveTools.None, roadColors.roadColor, roadColors.anchorPointColor, roadColors.controlPointColor, roadColors.textColor);
                    if (save.viewRoadsSettings.viewLanes)
                    {
                        LaneDrawer.DrawAllLanes(allRoads[i], save.viewRoadsSettings.viewWaypoints, save.viewRoadsSettings.viewLaneChanges, roadColors.laneColor,
                            roadColors.waypointColor, roadColors.disconnectedColor, roadColors.laneChangeColor, roadColors.textColor);
                    }
                }
            }
        }


        protected override void TopPart()
        {
            base.TopPart();
            string drawButton = drawAllRoadsText;
            if (drawAllRoads == true)
            {
                drawButton = "Clear All";
            }
            if (GUILayout.Button(drawButton))
            {
                drawAllRoads = !drawAllRoads;
                for (int i = 0; i < allRoads.Count; i++)
                {
                    allRoads[i].draw = drawAllRoads;
                }
                SceneView.RepaintAll();
            }

            EditorGUI.BeginChangeCheck();
            if (showCustomizations == false)
            {
                showCustomizations = EditorGUILayout.Toggle("Change Colors ", showCustomizations);
                save.viewRoadsSettings.viewLanes = EditorGUILayout.Toggle("View Lanes", save.viewRoadsSettings.viewLanes);
                if (save.viewRoadsSettings.viewLanes)
                {
                    scrollAdjustment = maxValue;
                    save.viewRoadsSettings.viewWaypoints = EditorGUILayout.Toggle("View Waypoints", save.viewRoadsSettings.viewWaypoints);
                    ViewLaneChangesToggle();

                }
                else
                {
                    scrollAdjustment = minValue;
                }
            }
            else
            {
                showCustomizations = EditorGUILayout.Toggle("Change Colors ", showCustomizations);


                EditorGUILayout.BeginHorizontal();
                save.viewRoadsSettings.viewLanes = EditorGUILayout.Toggle("View Lanes", save.viewRoadsSettings.viewLanes);
                roadColors.laneColor = EditorGUILayout.ColorField(roadColors.laneColor);
                EditorGUILayout.EndHorizontal();

                if (save.viewRoadsSettings.viewLanes)
                {
                    ShowCustomizations();
                }
                else
                {
                    scrollAdjustment = minValueColor;
                }
                roadColors.textColor = EditorGUILayout.ColorField("Text Color ", roadColors.textColor);
                roadColors.roadColor = EditorGUILayout.ColorField("Road Color", roadColors.roadColor);
            }
            EditorGUI.EndChangeCheck();

            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
            EditorGUILayout.Space();
        }


        void ShowCustomizations()
        {
            scrollAdjustment = maxValueColor;
            EditorGUILayout.BeginHorizontal();
            save.viewRoadsSettings.viewWaypoints = EditorGUILayout.Toggle("View Waypoints", save.viewRoadsSettings.viewWaypoints, GUILayout.Width(TOGGLE_WIDTH));
            roadColors.waypointColor = EditorGUILayout.ColorField(roadColors.waypointColor);
            roadColors.disconnectedColor = EditorGUILayout.ColorField(roadColors.disconnectedColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            save.viewRoadsSettings.viewLaneChanges = EditorGUILayout.Toggle("View Lane Changes", save.viewRoadsSettings.viewLaneChanges, GUILayout.Width(TOGGLE_WIDTH));
            roadColors.laneChangeColor = EditorGUILayout.ColorField(roadColors.laneChangeColor);
            EditorGUILayout.EndHorizontal();
        }


        protected TrafficSettingsLoader LoadSettingsLoader()
        {
            return new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
        }


        protected override List<RoadBase> LoadAllRoads()
        {
            return RoadsLoader.Initialize().LoadAllRoads<Road>().Cast<RoadBase>().ToList();
        }


        protected override void SelectWaypoint(RoadBase road)
        {
            SettingsWindow.SetSelectedRoad((Road)road);
            window.SetActiveWindow(typeof(EditRoadWindow), true);
        }


        protected override void DeleteCurrentRoad(RoadBase road)
        {
            DeletRoad(road);
        }


        protected override void SetTexts()
        {
            drawAllRoadsText = "Draw All Roads";
        }


        protected void DeletRoad(RoadBase road)
        {
            allRoads.Remove(road);
            RoadConnections roadConnections = CreateInstance<RoadConnections>();
            roadConnections.Initialize();
            for (int i = 0; i < roadConnections.ConnectionPools.Count; i++)
            {
                List<ConnectionCurve> curve = roadConnections.ConnectionPools[i].ContainsRoad(road);
                for (int j = 0; j < curve.Count; j++)
                {
                    roadConnections.DeleteConnection(curve[j]);
                }
            }
            Undo.DestroyObjectImmediate(road.gameObject);
            Undo.undoRedoPerformed += UndoPerformed;
        }


        public override void DestroyWindow()
        {
            base.DestroyWindow();
            settingsLoader.SaveViewRoadsSettings(save, roadColors);
        }
    }
}
