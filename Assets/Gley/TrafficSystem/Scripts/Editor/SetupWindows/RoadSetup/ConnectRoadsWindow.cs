using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class ConnectRoadsWindow : ConnectRoadsWindowBase
    {
        protected TrafficSettingsLoader settingsLoader;
        DrawRoadConnectors drawRoadConnectors;
        protected RoadConnections roadConnections;
        const float maxValue = 372;
        const float minValue = 246;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            settingsLoader = LoadSettingsLoader();
            save = settingsLoader.LoadConnectRoadsSave();
            roadColors = settingsLoader.LoadRoadColors();
            roadConnections = LoadRoadConnections();
            drawRoadConnectors = (DrawRoadConnectors)CreateInstance<DrawRoadConnectors>().Initialize();
            nrOfCars = System.Enum.GetValues(typeof(VehicleTypes)).Length;
            allowedCarIndex = new bool[nrOfCars];
            for (int i = 0; i < allowedCarIndex.Length; i++)
            {
                allowedCarIndex[i] = true;
            }

            return this;
        }


        public override void DrawInScene()
        {
            if (GleyUtilities.SceneCameraMoved())
            {
                SettingsWindowBase.TriggerRefreshWindowEvent();
            }

            MakeClickConnection(allRoads, roadConnections.ConnectionPools, roadColors.connectorLaneColor, roadColors.anchorPointColor,
            roadColors.roadConnectorColor, roadColors.selectedRoadConnectorColor, roadColors.disconnectedColor, save.waypointDistance, roadColors.textColor);


            for (int i = 0; i < allRoads.Count; i++)
            {
                roadDrawer.DrawPath(allRoads[i], MoveTools.None, roadColors.roadColor, roadColors.anchorPointColor, roadColors.controlPointColor, roadColors.textColor);
                LaneDrawer.DrawAllLanes(allRoads[i], save.viewRoadsSettings.viewWaypoints, save.viewRoadsSettings.viewLaneChanges, roadColors.laneColor,
                    roadColors.waypointColor, roadColors.disconnectedColor, roadColors.laneChangeColor, roadColors.textColor);
            }

            base.DrawInScene();
        }


        protected override void ShowCustomizations()
        {
            if (showCustomizations == false)
            {
                scrollAdjustment = minValue;
                showCustomizations = EditorGUILayout.Toggle("Change Colors ", showCustomizations);
                save.viewRoadsSettings.viewWaypoints = EditorGUILayout.Toggle("View Waypoints", save.viewRoadsSettings.viewWaypoints);
                save.viewRoadsSettings.viewLaneChanges = EditorGUILayout.Toggle("View Lane Changes", save.viewRoadsSettings.viewLaneChanges);
            }
            else
            {
                scrollAdjustment = maxValue;
                showCustomizations = EditorGUILayout.Toggle("Change Colors ", showCustomizations);
                EditorGUILayout.BeginHorizontal();
                save.viewRoadsSettings.viewWaypoints = EditorGUILayout.Toggle("View Waypoints", save.viewRoadsSettings.viewWaypoints, GUILayout.Width(TOGGLE_WIDTH));
                roadColors.waypointColor = EditorGUILayout.ColorField(roadColors.waypointColor);
                roadColors.disconnectedColor = EditorGUILayout.ColorField(roadColors.disconnectedColor);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                save.viewRoadsSettings.viewLaneChanges = EditorGUILayout.Toggle("View Lane Changes", save.viewRoadsSettings.viewLaneChanges, GUILayout.Width(TOGGLE_WIDTH));
                roadColors.laneChangeColor = EditorGUILayout.ColorField(roadColors.laneChangeColor);
                EditorGUILayout.EndHorizontal();

                roadColors.textColor = EditorGUILayout.ColorField("Text Color", roadColors.textColor);
                roadColors.roadColor = EditorGUILayout.ColorField("Road Color", roadColors.roadColor);
                roadColors.laneColor = EditorGUILayout.ColorField("Lane Color", roadColors.laneColor);
                roadColors.connectorLaneColor = EditorGUILayout.ColorField("Connector Lane Color", roadColors.connectorLaneColor);
                roadColors.anchorPointColor = EditorGUILayout.ColorField("Anchor Point Color", roadColors.anchorPointColor);
                roadColors.roadConnectorColor = EditorGUILayout.ColorField("Road Connector Color", roadColors.roadConnectorColor);
                roadColors.selectedRoadConnectorColor = EditorGUILayout.ColorField("Selected Connector Color", roadColors.selectedRoadConnectorColor);
            }
            base.ShowCustomizations();
        }


        protected override void DrawButton()
        {
            base.DrawButton();
            drawAllConnections = !drawAllConnections;
            for (int i = 0; i < roadConnections.ConnectionPools.Count; i++)
            {
                for (int j = 0; j < roadConnections.ConnectionPools[i].GetNrOfConnections(); j++)
                {
                    roadConnections.ConnectionPools[i].connectionCurves[j].draw = drawAllConnections;
                    if (drawAllConnections == false)
                    {
                        roadConnections.ConnectionPools[i].connectionCurves[j].drawWaypoints = false;
                    }
                }
            }
        }


        protected TrafficSettingsLoader LoadSettingsLoader()
        {
            return new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
        }


        protected RoadConnections LoadRoadConnections()
        {
            return CreateInstance<RoadConnections>().Initialize();
        }


        protected override List<RoadBase> LoadAllRoads()
        {
            return RoadsLoader.Initialize().LoadAllRoads<Road>().Cast<RoadBase>().ToList();
        }


        protected override void GenerateSelectedConnections()
        {
            CreateInstance<RoadConnections>().Initialize().GenerateSelectedConnections(save.waypointDistance);
            base.GenerateSelectedConnections();
        }


        protected void MakeClickConnection(List<RoadBase> allRoads, List<ConnectionPool> allConnections, Color connectorLaneColor, Color anchorPointColor, Color roadConnectorColor, Color selectedRoadConnectorColor, Color disconnectedColor, float waypointDistance, Color textColor)
        {
            drawRoadConnectors.MakeCnnections(allRoads, allConnections, connectorLaneColor, anchorPointColor, roadConnectorColor, selectedRoadConnectorColor, disconnectedColor, waypointDistance, textColor);
        }


        protected override void View(int i, int j)
        {
            GleyUtilities.TeleportSceneCamera(roadConnections.ConnectionPools[i].GetOutConnector<WaypointSettingsBase>(j).gameObject.transform.position);
            base.View(i, j);
        }


        protected override void DrawConnections()
        {
            for (int i = 0; i < roadConnections.ConnectionPools.Count; i++)
            {
                for (int j = 0; j < roadConnections.ConnectionPools[i].GetNrOfConnections(); j++)
                {
                    if (((ConnectionPool)roadConnections.ConnectionPools[i]).GetInConnector(j) == null || ((ConnectionPool)roadConnections.ConnectionPools[i]).GetOutConnector<WaypointSettingsBase>(j) == null)
                    {
                        roadConnections.DeleteConnection(roadConnections.ConnectionPools[i].connectionCurves[j]);
                        GUILayout.EndScrollView();
                        return;
                    }


                    if (GleyUtilities.IsPointInsideView(((ConnectionPool)roadConnections.ConnectionPools[i]).GetInConnector(j).transform.position) ||
                       GleyUtilities.IsPointInsideView(((ConnectionPool)roadConnections.ConnectionPools[i]).GetOutConnector<WaypointSettingsBase>(j).transform.position))
                    {
                        EditorGUILayout.BeginHorizontal();

                        roadConnections.ConnectionPools[i].connectionCurves[j].draw = EditorGUILayout.Toggle(roadConnections.ConnectionPools[i].connectionCurves[j].draw, GUILayout.Width(TOGGLE_DIMENSION));
                        EditorGUILayout.LabelField(roadConnections.ConnectionPools[i].GetName(j));
                        Color oldColor = GUI.backgroundColor;
                        if (roadConnections.ConnectionPools[i].connectionCurves[j].drawWaypoints == true)
                        {
                            GUI.backgroundColor = Color.green;
                        }

                        if (GUILayout.Button("Waypoints", GUILayout.Width(BUTTON_DIMENSION)))
                        {
                            roadConnections.ConnectionPools[i].connectionCurves[j].drawWaypoints = !roadConnections.ConnectionPools[i].connectionCurves[j].drawWaypoints;
                        }
                        GUI.backgroundColor = oldColor;

                        if (GUILayout.Button("View", GUILayout.Width(BUTTON_DIMENSION)))
                        {
                            View(i, j);
                        }

                        if (GUILayout.Button("Delete", GUILayout.Width(BUTTON_DIMENSION)))
                        {
                            if (EditorUtility.DisplayDialog("Delete " + roadConnections.ConnectionPools[i].connectionCurves[j].name + "?", "Are you sure you want to delete " + roadConnections.ConnectionPools[i].connectionCurves[j].name + "? \nYou cannot undo this operation.", "Delete", "Cancel"))
                            {
                                roadConnections.DeleteConnection(roadConnections.ConnectionPools[i].connectionCurves[j]);
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            base.DrawConnections();
        }


        public override void DestroyWindow()
        {
            settingsLoader.SaveConnectRoadsSettings(save, roadColors);
            base.DestroyWindow();
        }
    }
}
