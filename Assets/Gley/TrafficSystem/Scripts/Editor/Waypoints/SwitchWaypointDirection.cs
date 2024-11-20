using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEngine;
namespace Gley.TrafficSystem.Editor
{
    public class SwitchWaypointDirection : UnityEditor.Editor
    {
        public static void SwitchAll()
        {
            WaypointSettingsBase[] allWaypoints = FindObjectsByType<WaypointSettings>(FindObjectsSortMode.None);
            Dictionary<WaypointSettingsBase, List<WaypointSettingsBase>> otherLanesChanges = new Dictionary<WaypointSettingsBase, List<WaypointSettingsBase>>();
            foreach (var waypoint in allWaypoints)
            {
                var aux = waypoint.neighbors;
                waypoint.neighbors = waypoint.prev;
                waypoint.prev = aux;
                if (waypoint.otherLanes != null)
                {
                    for (int i = 0; i < waypoint.otherLanes.Count; i++)
                    {
                        if (otherLanesChanges.ContainsKey(waypoint.otherLanes[i]))
                        {
                            List<WaypointSettingsBase> list = otherLanesChanges[waypoint.otherLanes[i]];
                            if (!list.Contains(waypoint))
                            {
                                list.Add(waypoint);
                            }
                            otherLanesChanges[waypoint.otherLanes[i]] = list;
                        }
                        else
                        {
                            otherLanesChanges.Add(waypoint.otherLanes[i], new List<WaypointSettingsBase> { waypoint });
                        }
                    }
                }
            }

            //apply other lanes
            foreach (var waypoint in allWaypoints)
            {
                otherLanesChanges.TryGetValue(waypoint, out waypoint.otherLanes);
            }
            Debug.Log("Done switching waypoints!");
        }
    }
}
