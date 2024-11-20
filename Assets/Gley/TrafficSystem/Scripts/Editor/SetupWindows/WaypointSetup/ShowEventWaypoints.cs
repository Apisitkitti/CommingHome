using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class ShowEventWaypoints : ShowWaypointsTrafficBase
    {
        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            save = settingsLoader.LoadGiveWayWaypointsSave();
            return this;
        }


        public override void DrawInScene()
        {
            base.DrawInScene();
            for(int i=0;i<waypointsOfInterest.Count;i++)
            {
                Handles.Label(waypointsOfInterest[i].transform.position, ((WaypointSettings)waypointsOfInterest[i]).eventData);
            }
        }


        protected override void ScrollPart(float width, float height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));
            base.ScrollPart(width, height);
            GUILayout.EndScrollView();
        }


        public override void DestroyWindow()
        {
            settingsLoader.SaveGiveWayWaypointsSettings(save, roadColors);
            base.DestroyWindow();
        }


        protected override List<WaypointSettingsBase> GetWaypointsOfIntereset()
        {
            return waypointDrawer.ShowEventWaypoints(roadColors.waypointColor, save.showConnections, roadColors.waypointColor, save.showSpeed, roadColors.speedColor, save.showCars, roadColors.carsColor, save.showOtherLanes, roadColors.laneChangeColor, save.showPriority, roadColors.priorityColor).Cast<WaypointSettingsBase>().ToList();
        }
    }
}