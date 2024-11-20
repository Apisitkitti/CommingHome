using Gley.UrbanAssets.Internal;
using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public abstract class EditRoadWindowBase : SetupWindowBase
    {
        const float minValue = 350;

        protected bool[] allowedCarIndex;
        protected RoadBase selectedRoad;
        protected ViewRoadsSettings viewRoadsSettings;
        protected RoadColors roadColors;
        protected MoveTools moveTool;
        protected string agentName;
        protected int nrOfCars;
        protected RoadDrawer roadDrawer;
        protected float scrollAdjustment;

        private bool showCustomizations;

        protected abstract void SetTexts();
        protected abstract void SetTopText();
        protected abstract void UpdateLaneNumber();
        protected abstract void GenerateWaypoints();
        protected abstract void DrawSpeedProperties(int currentLane);
        protected abstract void ToggleAgentTypes(int currentLane, int agentIndex);
        protected abstract void SetRoadDrawer();


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            SetRoadDrawer();
            SetTexts();
            return this;
        }


        public override void MouseMove(Vector3 mousePosition)
        {
            if (selectedRoad == null)
            {
                Debug.LogWarning("No road selected");
                return;
            }
            base.MouseMove(mousePosition);
            roadDrawer.SelectSegmentIndex(selectedRoad, mousePosition);
        }


        public override void RightClick(Vector3 mousePosition)
        {
            if (selectedRoad == null)
            {
                Debug.LogWarning("No road selected");
                return;
            }
            roadDrawer.Delete(selectedRoad, mousePosition);
            base.RightClick(mousePosition);
        }


        protected override void TopPart()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            SetTopText();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal();
            viewRoadsSettings.viewWaypoints = EditorGUILayout.Toggle("View Waypoints", viewRoadsSettings.viewWaypoints);
            ViewLaneChanges();

            EditorGUILayout.EndHorizontal();

            SetLaneProperties();
            selectedRoad.waypointDistance = EditorGUILayout.FloatField("Waypoint distance ", selectedRoad.waypointDistance);

            EditorGUI.BeginChangeCheck();
            moveTool = (MoveTools)EditorGUILayout.EnumPopup("Select move tool ", moveTool);

            showCustomizations = EditorGUILayout.Toggle("Change Colors ", showCustomizations);
            if (showCustomizations == true)
            {
                ShowCustomizations();

            }
            else
            {
                scrollAdjustment = minValue;
            }
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }

            base.TopPart();
        }


        protected virtual void SetLaneProperties()
        {

        }


        protected virtual void ShowCustomizations()
        {

        }


        protected virtual void ViewLaneChanges()
        {
            viewRoadsSettings.viewLaneChanges = EditorGUILayout.Toggle("View Lane Changes", viewRoadsSettings.viewLaneChanges);
        }


        protected override void ScrollPart(float width, float height)
        {
            if (selectedRoad == null)
            {
                Debug.LogWarning("No road selected");
                return;
            }
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Global Lane Settings", EditorStyles.boldLabel);
            SelectAllowedCars();

            ShowSpeedSetup();

            EditorGUILayout.Space();

            ApplyAllSettings();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            IndividualLaneSettings();
            EditorGUILayout.Space();
            GUILayout.EndScrollView();

            base.ScrollPart(width, height);
        }


        protected virtual void IndividualLaneSettings()
        {

        }


        protected virtual void ShowSpeedSetup()
        {

        }


        protected virtual void ApplyAllSettings()
        {

        }


        protected override void BottomPart()
        {
            if (selectedRoad == null)
            {
                Debug.LogWarning("No road selected");
                return;
            }

            if (GUILayout.Button("Generate waypoints"))
            {
                viewRoadsSettings.viewWaypoints = true;

                if (selectedRoad.nrOfLanes <= 0)
                {
                    Debug.LogError("Nr of lanes has to be >0");
                    return;
                }

                if (selectedRoad.waypointDistance <= 0)
                {
                    Debug.LogError("Waypoint distance needs to be >0");
                    return;
                }

                if (selectedRoad.laneWidth <= 0)
                {
                    Debug.LogError("Lane width has to be >0");
                    return;
                }
                GenerateWaypoints();

                EditorUtility.SetDirty(selectedRoad);
                AssetDatabase.SaveAssets();
            }

            DrawLinkOtherLanes();

            base.BottomPart();
        }


        protected virtual void DrawLinkOtherLanes()
        {

        }


        public override void DestroyWindow()
        {
            base.DestroyWindow();
        }


        protected virtual void SelectAllowedCars()
        {

            if (GUILayout.Button("Apply Global " + agentName + " Settings"))
            {
                ApplyGlobalCarSettings();
            }
        }


        protected virtual void ApplyGlobalCarSettings()
        {
            for (int i = 0; i < selectedRoad.lanes.Count; i++)
            {
                for (int j = 0; j < allowedCarIndex.Length; j++)
                {
                    selectedRoad.lanes[i].allowedCars[j] = allowedCarIndex[j];
                }
            }
        }
    }
}