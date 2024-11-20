using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using System.Collections.Generic;
using UnityEditor;

namespace Gley.TrafficSystem.Editor
{
    public class TrafficSettingsLoader : SettingsLoader
    {
        public TrafficSettingsLoader(string path) : base(path)
        {
        }


        internal void SaveEditRoadSettings(EditRoadSave editRoadSave, bool[] allowedCarIndex, RoadColors roadColors, RoadDefaults roadDefaults)
        {
            TrafficSettingsWindowData settingsWindowData = LoadSettingsAsset<TrafficSettingsWindowData>();
            editRoadSave.globalCarList = new List<VehicleTypes>();
            for (int i = 0; i < allowedCarIndex.Length; i++)
            {
                if (allowedCarIndex[i] == true)
                {
                    editRoadSave.globalCarList.Add((VehicleTypes)i);
                }
            }
            settingsWindowData.editRoadSave = editRoadSave;
            settingsWindowData.roadColors = roadColors;
            settingsWindowData.roadDefaults = roadDefaults;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal EditRoadSave LoadEditRoadSave()
        {
            return (LoadSettingsAsset<TrafficSettingsWindowData>()).editRoadSave;
        }


        internal void SaveSpeedRoutes(SpeedRoutesSave speedRoutesSave)
        {
            TrafficSettingsWindowData settingsWindowData = LoadSettingsAsset<TrafficSettingsWindowData>();
            settingsWindowData.speedRoutesSave = speedRoutesSave;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal SpeedRoutesSave LoadSpeedRoutes()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().speedRoutesSave;
        }


        internal void SavePriorityWaypoints(WaypointPrioritySave waypointPrioritySave)
        {
            TrafficSettingsWindowData settingsWindowData = LoadSettingsAsset<TrafficSettingsWindowData>();
            settingsWindowData.waypointPrioritySave = waypointPrioritySave;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal WaypointPrioritySave LoadPriorityWaypoints()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().waypointPrioritySave;
        }


        internal void SaveIntersectionsSettings(IntersectionSave intersectionSave)
        {
            TrafficSettingsWindowData settingsWindowData = (LoadSettingsAsset<TrafficSettingsWindowData>());
            settingsWindowData.intersectionSave = intersectionSave;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal PathFindingSave LoadPathFindingSettings()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().pathFindingSave;
        }


        internal void SavePathFindingSettings(PathFindingSave pathFindingSave)
        {
            TrafficSettingsWindowData settingsWindowData = (LoadSettingsAsset<TrafficSettingsWindowData>());
            settingsWindowData.pathFindingSave = pathFindingSave;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal IntersectionSave LoadIntersectionsSettings()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().intersectionSave;
        }


        internal void SaveGiveWayWaypointsSettings(ViewWaypointsSettings giveWayWaypointsSettings, RoadColors roadColors)
        {
            TrafficSettingsWindowData settingsWindowData = (LoadSettingsAsset<TrafficSettingsWindowData>());
            settingsWindowData.giveWayWaypointsSettings = giveWayWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadGiveWayWaypointsSave()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().giveWayWaypointsSettings;
        }


        internal void SaveSpeedEditedWaypointsSettings(ViewWaypointsSettings speedEditedWaypointsSettings, RoadColors roadColors)
        {
            TrafficSettingsWindowData settingsWindowData = LoadSettingsAsset<TrafficSettingsWindowData>();
            settingsWindowData.speedEditedWaypointsSettings = speedEditedWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadSpeedEditedWaypointsSave()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().speedEditedWaypointsSettings;
        }


        internal void SavePriorityEditedWaypointsSettings(ViewWaypointsSettings priorityEditedWaypointsSettings, RoadColors roadColors)
        {
            TrafficSettingsWindowData settingsWindowData = LoadSettingsAsset<TrafficSettingsWindowData>();
            settingsWindowData.priorityEditedWaypointsSettings = priorityEditedWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadPriorityEditedWaypointsSave()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().priorityEditedWaypointsSettings;
        }


        internal void SaveStopWaypointsSettings(ViewWaypointsSettings stopWaypointsSettings, RoadColors roadColors)
        {
            TrafficSettingsWindowData settingsWindowData = LoadSettingsAsset<TrafficSettingsWindowData>();
            settingsWindowData.stopWaypointsSettings = stopWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadStopdWaypointsSave()
        {
            return LoadSettingsAsset<TrafficSettingsWindowData>().stopWaypointsSettings;
        }
    }
}
