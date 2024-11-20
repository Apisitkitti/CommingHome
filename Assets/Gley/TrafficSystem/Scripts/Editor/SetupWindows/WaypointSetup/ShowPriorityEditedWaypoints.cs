using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class ShowPriorityEditedWaypoints : ShowWaypointsTrafficBase
    {
        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            save = settingsLoader.LoadPriorityEditedWaypointsSave();
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
            settingsLoader.SavePriorityEditedWaypointsSettings(save, roadColors);
            base.DestroyWindow();
        }


        protected override List<WaypointSettingsBase> GetWaypointsOfIntereset()
        {
            return waypointDrawer.ShowPriorityEditedWaypoints(roadColors.waypointColor).Cast<WaypointSettingsBase>().ToList();
        }
    }
}
