using System.Collections.Generic;
using UnityEngine;
namespace Gley.UrbanAssets.Internal
{
    public class WaypointSettingsBase : MonoBehaviour
    {
        public List<WaypointSettingsBase> neighbors;
        public List<WaypointSettingsBase> prev;
        public List<WaypointSettingsBase> otherLanes;
        public List<WaypointSettingsBase> giveWayList;
        public bool stop;
        public bool draw = true;

        //path finding
        public List<int> distance;
        public int penalty;

        public void EditorSetup()
        {
            neighbors = new List<WaypointSettingsBase>();
            prev = new List<WaypointSettingsBase>();
            otherLanes = new List<WaypointSettingsBase>();
            giveWayList = new List<WaypointSettingsBase>();
        }

        public void Initialize()
        {
        }
    }
}