using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;

namespace Gley.TrafficSystem.Editor
{
    public class VehicleRoutesSetupWindow : AgentRoutesSetupWindowBase<WaypointSettings>
    {
        private TrafficSettingsLoader settingsLoader;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            settingsLoader = LoadSettingsLoader();
            save = settingsLoader.LoadCarRoutes();
            return base.Initialize(windowProperties, window);
        }


        protected override int GetNrOfDifferentAgents()
        {
            return System.Enum.GetValues(typeof(VehicleTypes)).Length;
        }


        protected override WaypointDrawerBase<WaypointSettings> SetWaypointDrawer()
        {
            return CreateInstance<WaypointDrawer>();
        }


        protected TrafficSettingsLoader LoadSettingsLoader()
        {
            return new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
        }


        protected override void WaypointClicked(WaypointSettingsBase clickedWaypoint, bool leftClick)
        {
            window.SetActiveWindow(typeof(EditWaypointWindow), true);
        }


        protected override string ConvertIndexToEnumName(int i)
        {
            return ((VehicleTypes)i).ToString();
        }

        public override void DestroyWindow()
        {
            settingsLoader.SaveCarRoutes(save);
            base.DestroyWindow();
        }
    }
    
}