using Gley.UrbanAssets.Internal;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public class RoadCreator
    {
        static Transform roadWaypointsHolder;


        internal T Create<T, R>(Vector3 startPosition, string trafficWaypointsHolderName, RoadDefaults roadDefaults, string prefix) where T : RoadBase where R : MonoBehaviour
        {
            int roadNumber = GetFreeRoadNumber<R>(trafficWaypointsHolderName, prefix);
            GameObject roadHolder = new GameObject(prefix + "_" + roadNumber);
            roadHolder.tag = Constants.editorTag;
            roadHolder.transform.SetParent(GetRoadWaypointsHolder<R>(trafficWaypointsHolderName));
            roadHolder.transform.SetSiblingIndex(roadNumber);
            roadHolder.transform.position = startPosition;
            T road = roadHolder.AddComponent<T>();
            road.SetDefaults(roadDefaults.nrOfLanes, roadDefaults.laneWidth, roadDefaults.waypointDistance);

            EditorUtility.SetDirty(road);
            AssetDatabase.SaveAssets();
            return road;
        }


        public static Transform GetRoadWaypointsHolder<T>(string trafficWaypointsHolderName) where T : MonoBehaviour
        {
            bool editingInsidePrefab = GleyPrefabUtilities.EditingInsidePrefab();

            if (roadWaypointsHolder == null || roadWaypointsHolder.name != trafficWaypointsHolderName)
            {
                GameObject holder = null;
                if (editingInsidePrefab)
                {
                    GameObject prefabRoot = GleyPrefabUtilities.GetScenePrefabRoot();
                    Transform waypointsHolder = prefabRoot.transform.Find(trafficWaypointsHolderName);
                    if (waypointsHolder == null)
                    {
                        waypointsHolder = new GameObject(trafficWaypointsHolderName).transform;
                        waypointsHolder.SetParent(prefabRoot.transform);
                        waypointsHolder.gameObject.AddComponent<T>();
                        waypointsHolder.gameObject.tag = Constants.editorTag;
                    }
                    holder = waypointsHolder.gameObject;
                }
                else
                {
                    GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None).Where(obj => obj.name == trafficWaypointsHolderName).ToArray();
                    if (allObjects.Length > 0)
                    {
                        for (int i = 0; i < allObjects.Length; i++)
                        {
                            if (!GleyPrefabUtilities.IsInsidePrefab(allObjects[i]))
                            {
                                holder = allObjects[i];
                                break;
                            }
                        }
                    }
                    if (holder == null)
                    {
                        holder = new GameObject(trafficWaypointsHolderName);
                        holder.AddComponent<T>();
                    }
                }
                roadWaypointsHolder = holder.transform;
            }
            return roadWaypointsHolder;
        }


        private int GetFreeRoadNumber<T>(string trafficWaypointsHolderName, string prefix) where T : MonoBehaviour
        {
            int nr = 0;
            for (int i = 0; i < GetRoadWaypointsHolder<T>(trafficWaypointsHolderName).childCount; i++)
            {
                if (nr.ToString() != roadWaypointsHolder.GetChild(i).name.Split('_')[1])
                {
                    return nr;
                }
                nr++;
            }
            return nr;
        }
    }
}
