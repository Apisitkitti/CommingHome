using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public class SettingsLoader
    {
        protected string path;


        public SettingsLoader(string path)
        {
            this.path = path;
        }


        protected T LoadSettingsAsset<T>() where T : SettingsWindowData
        {
            T settingsWindowData = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));

            if (settingsWindowData == null)
            {
                SettingsWindowData asset = ScriptableObject.CreateInstance<T>();
                string[] pathFolders = path.Split('/');
                string tempPath = pathFolders[0];
                if (path.Contains("Pedestrian"))
                {
                    asset.roadDefaults = new RoadDefaults(1, 1, 4);
                }
                else
                {
                    asset.roadDefaults = new RoadDefaults(2, 4, 4);
                }
                for (int i = 1; i < pathFolders.Length - 1; i++)
                {
                    if (!AssetDatabase.IsValidFolder(tempPath + "/" + pathFolders[i]))
                    {
                        AssetDatabase.CreateFolder(tempPath, pathFolders[i]);
                        AssetDatabase.Refresh();
                    }

                    tempPath += "/" + pathFolders[i];
                }

                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                settingsWindowData = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            }

            return settingsWindowData;
        }


        internal void SaveCreateRoadSettings(CreateRoadSave createRoadSave, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.createRoadSave = createRoadSave;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal CreateRoadSave LoadCreateRoadSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().createRoadSave;
        }


        internal void SaveViewRoadsSettings(ViewRoadsSave viewRoadsSave, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.roadColors = roadColors;
            settingsWindowData.viewRoadsSave = viewRoadsSave;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewRoadsSave LoadViewRoadsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().viewRoadsSave;
        }


        internal void SaveConnectRoadsSettings(ConnectRoadsSave connectRoadsSave, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.connectRoadsSave = connectRoadsSave;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ConnectRoadsSave LoadConnectRoadsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().connectRoadsSave;
        }


        internal RoadDefaults LoadRoadDefaultsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().roadDefaults;
        }


        internal RoadColors LoadRoadColors()
        {
            return LoadSettingsAsset<SettingsWindowData>().roadColors;
        }


        internal void SaveRoadColors(RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal void SaveCarRoutes(CarRoutesSave carRoutesSave)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.carRoutesSave = carRoutesSave;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal CarRoutesSave LoadCarRoutes()
        {
            return LoadSettingsAsset<SettingsWindowData>().carRoutesSave;
        }


        internal void SaveAllWaypointsSettings(ViewWaypointsSettings allWaypointsSettings, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.allWaypointsSettings = allWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }

        internal ViewWaypointsSettings LoadAllWaypointsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().allWaypointsSettings;
        }


        internal void SaveDisconnectedWaypointsSettings(ViewWaypointsSettings disconnectedWaypointsSettings, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.disconnectedWaypointsSettings = disconnectedWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadDisconnectedWaypointsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().disconnectedWaypointsSettings;
        }


        internal void SaveCarEditedWaypointsSettings(ViewWaypointsSettings carEditedWaypointsSettings, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.carEditedWaypointsSettings = carEditedWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadCarEditedWaypointsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().carEditedWaypointsSettings;
        }


        internal void SavePathProblemsWaypointsSettings(ViewWaypointsSettings pathProblemsWaypointsSettings, RoadColors roadColors)
        {
            SettingsWindowData settingsWindowData = LoadSettingsAsset<SettingsWindowData>();
            settingsWindowData.pathProblemsWaypointsSettings = pathProblemsWaypointsSettings;
            settingsWindowData.roadColors = roadColors;
            EditorUtility.SetDirty(settingsWindowData);
        }


        internal ViewWaypointsSettings LoadPathProblemsWaypointsSave()
        {
            return LoadSettingsAsset<SettingsWindowData>().pathProblemsWaypointsSettings;
        }
    }
}