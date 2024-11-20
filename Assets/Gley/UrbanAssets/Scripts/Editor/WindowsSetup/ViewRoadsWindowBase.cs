using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public abstract class ViewRoadsWindowBase : SetupWindowBase
    {
        protected const float minValue = 184;

        protected float minValueColor = 220;
        protected string drawAllRoadsText;
        protected List<RoadBase> allRoads;
        protected RoadDrawer roadDrawer;
        protected ViewRoadsSave save;
        protected RoadColors roadColors;
        protected float scrollAdjustment;
        protected bool drawAllRoads;
        protected bool showCustomizations;

        protected abstract void SetTexts();
        protected abstract List<RoadBase> LoadAllRoads();
        protected abstract void DeleteCurrentRoad(RoadBase road);
        protected abstract void SelectWaypoint(RoadBase road);


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);

            roadDrawer = CreateInstance<RoadDrawer>();
          
            scrollAdjustment = minValue;
            allRoads = LoadAllRoads();
            SetTexts();

            return this;
        }


        protected virtual void ViewLaneChangesToggle()
        {
            save.viewRoadsSettings.viewLaneChanges = EditorGUILayout.Toggle("View Lane Changes", save.viewRoadsSettings.viewLaneChanges);
        }


        protected override void ScrollPart(float width, float height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));

            for (int i = 0; i < allRoads.Count; i++)
            {
                MakeSelectRoadRow(allRoads[i]);
            }
            GUILayout.EndScrollView();
        }


        private void MakeSelectRoadRow(RoadBase road)
        {
            if (road.isInsidePrefab && !GleyPrefabUtilities.EditingInsidePrefab())
                return;
            if (GleyUtilities.IsPointInsideView(road.path[0]) || GleyUtilities.IsPointInsideView(road.path[road.path.NumPoints - 1]))
            {
                EditorGUILayout.BeginHorizontal();
                road.draw = EditorGUILayout.Toggle(road.draw, GUILayout.Width(TOGGLE_DIMENSION));
                GUILayout.Label(road.gameObject.name);



                if (GUILayout.Button("View"))
                {
                    GleyUtilities.TeleportSceneCamera(road.transform.position);
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button("Select"))
                {
                    SelectWaypoint(road);
                }
                if (GUILayout.Button("Delete"))
                {
                    EditorGUI.BeginChangeCheck();
                    if (EditorUtility.DisplayDialog("Delete " + road.name + "?", "Are you sure you want to delete " + road.name + "? \nYou cannot undo this operation.", "Delete", "Cancel"))
                    {
                        DeleteCurrentRoad(road);
                    }
                    EditorGUI.EndChangeCheck();
                }

                if (GUI.changed)
                {
                    SceneView.RepaintAll();
                }

                EditorGUILayout.EndHorizontal();
            }
        }


        protected void UndoPerformed()
        {
            allRoads = LoadAllRoads();
        }


        public override void DestroyWindow()
        {
            Undo.undoRedoPerformed -= UndoPerformed;

            base.DestroyWindow();
        }
    }
}
