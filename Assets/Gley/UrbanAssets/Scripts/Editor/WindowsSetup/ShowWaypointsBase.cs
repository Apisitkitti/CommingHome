using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public abstract class ShowWaypointsBase : SetupWindowBase
    {
        protected List<WaypointSettingsBase> waypointsOfInterest;
        protected ViewWaypointsSettings save;
        protected RoadColors roadColors;
        protected float scrollAdjustment = 220;

        private bool waypointsLoaded = false;

        protected abstract void OpenEditWindow(int index);
        protected abstract List<WaypointSettingsBase> GetWaypointsOfIntereset();


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
          
            return this;
        }


        protected override void ScrollPart(float width, float height)
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
                EditorGUILayout.LabelField("No " + GetWindowTitle());
            }
            base.ScrollPart(width, height);
        }


        public override void DrawInScene()
        {
            waypointsOfInterest = GetWaypointsOfIntereset(); 

            if (waypointsLoaded == false)
            {
                SettingsWindowBase.TriggerRefreshWindowEvent();
                waypointsLoaded = true;
            }
            base.DrawInScene();
        }
    }
}
