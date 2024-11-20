using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class EditRoadWindow : EditRoadWindowBase
    {
        const float maxValue = 449;

        private EditRoadSave save;
        protected TrafficSettingsLoader settingsLoader;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            selectedRoad = SettingsWindow.GetSelectedRoad();
            settingsLoader = new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
            save = settingsLoader.LoadEditRoadSave();
            moveTool = save.moveTool;
            viewRoadsSettings = save.viewRoadsSettings;
            roadColors = settingsLoader.LoadRoadColors();
            nrOfCars = System.Enum.GetValues(typeof(VehicleTypes)).Length;
            allowedCarIndex = new bool[nrOfCars];
            for (int i = 0; i < allowedCarIndex.Length; i++)
            {
                if (save.globalCarList.Contains((VehicleTypes)i))
                {
                    allowedCarIndex[i] = true;
                }
            }
            return this;
        }


        public override void DrawInScene()
        {
            if (selectedRoad == null)
            {
                Debug.LogWarning("No road selected");
                return;
            }

            roadDrawer.DrawPath(selectedRoad, moveTool, roadColors.roadColor, roadColors.anchorPointColor, roadColors.controlPointColor, roadColors.textColor);
            LaneDrawer.DrawAllLanes(selectedRoad, viewRoadsSettings.viewWaypoints, viewRoadsSettings.viewLaneChanges, roadColors.laneColor,
                roadColors.waypointColor, roadColors.disconnectedColor, roadColors.laneChangeColor, roadColors.textColor);
            base.DrawInScene();
        }


        protected override void ShowCustomizations()
        {
            scrollAdjustment = maxValue;
            roadColors.roadColor = EditorGUILayout.ColorField("Road Color", roadColors.roadColor);
            roadColors.laneColor = EditorGUILayout.ColorField("Lane Color", roadColors.laneColor);
            roadColors.waypointColor = EditorGUILayout.ColorField("Waypoint Color", roadColors.waypointColor);
            roadColors.disconnectedColor = EditorGUILayout.ColorField("Disconnected Color", roadColors.disconnectedColor);
            roadColors.laneChangeColor = EditorGUILayout.ColorField("Lane Change Color", roadColors.laneChangeColor);
            roadColors.controlPointColor = EditorGUILayout.ColorField("Control Point Color", roadColors.controlPointColor);
            roadColors.anchorPointColor = EditorGUILayout.ColorField("Anchor Point Color", roadColors.anchorPointColor);
            base.ShowCustomizations();
        }


        protected override void ApplyAllSettings()
        {
            base.ApplyAllSettings();
            if (GUILayout.Button("Apply All Settings"))
            {
                ApplyGlobalCarSettings();
            }
        }


        protected override void SetRoadDrawer()
        {
            roadDrawer = CreateInstance<RoadDrawer>();
        }


        protected override void UpdateLaneNumber()
        {
            selectedRoad.UpdateLaneNumber(save.maxSpeed, System.Enum.GetValues(typeof(VehicleTypes)).Length);
        }


        protected override void GenerateWaypoints()
        {
            CreateInstance<WaypointGeneratorTraffic>().GenerateWaypoints(selectedRoad, window.GetGroundLayer());
        }


        protected override void ShowSpeedSetup()
        {
            EditorGUILayout.BeginHorizontal();
            save.maxSpeed = EditorGUILayout.IntField("Global Max Speed", save.maxSpeed);
            if (GUILayout.Button("Apply Speed"))
            {
                SetSpeedOnLanes(selectedRoad, save.maxSpeed);
            }
            EditorGUILayout.EndHorizontal();
        }


        private void SetSpeedOnLanes(RoadBase selectedRoad, int maxSpeed)
        {
            for (int i = 0; i < selectedRoad.lanes.Count; i++)
            {
                selectedRoad.lanes[i].laneSpeed = maxSpeed;
            }
        }


        protected override void ApplyGlobalCarSettings()
        {
            SetSpeedOnLanes(selectedRoad, save.maxSpeed);
            base.ApplyGlobalCarSettings();
        }


        protected override void DrawLinkOtherLanes()
        {
            EditorGUILayout.BeginHorizontal();
            ((Road)selectedRoad).otherLaneLinkDistance = EditorGUILayout.IntField("Link distance", ((Road)selectedRoad).otherLaneLinkDistance);
            if(((Road)selectedRoad).otherLaneLinkDistance<1)
            {
                ((Road)selectedRoad).otherLaneLinkDistance = 1;
            }
            if (GUILayout.Button("Link other lanes"))
            {
                viewRoadsSettings.viewWaypoints = true;
                viewRoadsSettings.viewLaneChanges = true;
                LinkOtherLanes.Link((Road)selectedRoad);
                EditorUtility.SetDirty(selectedRoad);
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Unlink other lanes"))
            {
                LinkOtherLanes.Unlinck((Road)selectedRoad);
                EditorUtility.SetDirty(selectedRoad);
                AssetDatabase.SaveAssets();
            }
            
        }


        public override void DestroyWindow()
        {
            save.moveTool = moveTool;
            save.viewRoadsSettings = viewRoadsSettings;
            settingsLoader.SaveEditRoadSettings(save, allowedCarIndex, roadColors, new RoadDefaults(selectedRoad.nrOfLanes, selectedRoad.laneWidth, selectedRoad.waypointDistance));
            base.DestroyWindow();
        }


        protected override void SelectAllowedCars()
        {
            GUILayout.Label("Allowed Car Types:");
            for (int i = 0; i < nrOfCars; i++)
            {
                allowedCarIndex[i] = EditorGUILayout.Toggle(((VehicleTypes)i).ToString(), allowedCarIndex[i]);
            }
            base.SelectAllowedCars();
        }


        protected override void ToggleAgentTypes(int currentLane, int agentIndex)
        {
            selectedRoad.lanes[currentLane].allowedCars[agentIndex] = EditorGUILayout.Toggle(((VehicleTypes)agentIndex).ToString(), selectedRoad.lanes[currentLane].allowedCars[agentIndex]);
        }


        protected override void SetTopText()
        {
            EditorGUILayout.LabelField("Press SHIFT + Left Click to add a road point");
            EditorGUILayout.LabelField("Press SHIFT + Right Click to remove a road point");
        }


        protected override void SetTexts()
        {
            agentName = "Car";
        }


        protected override void DrawSpeedProperties(int currentLane)
        {
            GUILayout.BeginHorizontal();
            selectedRoad.lanes[currentLane].laneSpeed = EditorGUILayout.IntField("Lane " + currentLane + ", Lane Speed:", selectedRoad.lanes[currentLane].laneSpeed);
            string buttonLebel = "<--";
            if (selectedRoad.lanes[currentLane].laneDirection == false)
            {
                buttonLebel = "-->";
            }
            if (GUILayout.Button(buttonLebel))
            {
                selectedRoad.lanes[currentLane].laneDirection = !selectedRoad.lanes[currentLane].laneDirection;
                CreateInstance<WaypointGeneratorTraffic>().SwitchLaneDirection(selectedRoad, currentLane);
            }
            GUILayout.EndHorizontal();
        }


        public override void LeftClick(Vector3 mousePosition, bool clicked)
        {
            if (selectedRoad == null)
            {
                Debug.LogWarning("No road selected");
                return;
            }

            roadDrawer.AddPathPoint(mousePosition, selectedRoad);
            base.LeftClick(mousePosition, clicked);
        }


        protected override void IndividualLaneSettings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Individual Lane Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            LoadLanes();
            EditorGUILayout.EndVertical();
            base.IndividualLaneSettings();
        }


        private void LoadLanes()
        {
            if (selectedRoad)
            {
                if (selectedRoad.lanes != null)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    for (int i = 0; i < selectedRoad.lanes.Count; i++)
                    {
                        if (selectedRoad.lanes[i].laneDirection == true)
                        {
                            DrawLaneButton(i);
                        }
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    for (int i = 0; i < selectedRoad.lanes.Count; i++)
                    {
                        if (selectedRoad.lanes[i].laneDirection == false)
                        {
                            DrawLaneButton(i);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }


        void DrawLaneButton(int currentLane)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawSpeedProperties(currentLane);

            EditorGUILayout.LabelField("Allowed " + agentName + " types on this lane:");
            for (int i = 0; i < nrOfCars; i++)
            {
                if (i >= selectedRoad.lanes[currentLane].allowedCars.Length)
                {
                    selectedRoad.lanes[currentLane].UpdateAllowedCars(nrOfCars);
                }
                ToggleAgentTypes(currentLane, i);
            }
            EditorGUILayout.EndVertical();
        }


        protected override void SetLaneProperties()
        {
            EditorGUI.BeginChangeCheck();
            selectedRoad.nrOfLanes = EditorGUILayout.IntField("Nr of lanes", selectedRoad.nrOfLanes);
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                UpdateLaneNumber();
            }

            selectedRoad.laneWidth = EditorGUILayout.FloatField("Lane width (m)", selectedRoad.laneWidth);
            base.SetLaneProperties();
        }
    }
}
