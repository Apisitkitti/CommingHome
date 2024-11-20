using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class ShowStopWaypoints : ShowWaypointsTrafficBase
    {
        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            save = settingsLoader.LoadStopdWaypointsSave();
            return this;
        }


        protected override void ScrollPart(float width, float height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));
            base.ScrollPart(width, height);
            GUILayout.EndScrollView();
        }


        public override void DestroyWindow()
        {
            settingsLoader.SaveStopWaypointsSettings(save, roadColors);
            base.DestroyWindow();
        }


        protected override List<WaypointSettingsBase> GetWaypointsOfIntereset()
        {
            return waypointDrawer.ShowStopWaypoints(roadColors.waypointColor, save.showConnections, roadColors.waypointColor, save.showSpeed, roadColors.speedColor, save.showCars, roadColors.carsColor, save.showOtherLanes, roadColors.laneChangeColor, save.showPriority, roadColors.priorityColor);
        }
    }
}
