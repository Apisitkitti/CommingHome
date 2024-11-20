#if UNITY_EDITOR
#if GLEY_PEDESTRIAN_SYSTEM
using Gley.PedestrianSystem.Internal;
#endif
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gley.TrafficSystem.Internal
{
    /// <summary>
    /// Stores traffic lights intersection properties
    /// </summary>
    public class TrafficLightsIntersectionSettings : GenericIntersectionSettings
    {
        public float greenLightTime=10;
        public float yellowLightTime=2;
        public bool setGreenLightTimePerRoad;
        public List<IntersectionStopWaypointsSettings> stopWaypoints = new List<IntersectionStopWaypointsSettings>();
        public List<WaypointSettings> exitWaypoints = new List<WaypointSettings>();

        public override List<IntersectionStopWaypointsSettings> GetAssignedWaypoints()
        {
            return stopWaypoints;
        }


        public override List<WaypointSettings> GetExitWaypoints()
        {
            return exitWaypoints;
        }

#if GLEY_PEDESTRIAN_SYSTEM
        public List<PedestrianWaypointSettings> pedestrianWaypoints = new List<PedestrianWaypointSettings>();
        public List<PedestrianWaypointSettings> directionWaypoints = new List<PedestrianWaypointSettings>();
        public List<GameObject> redLightObjects = new List<GameObject>();
        public List<GameObject> greenLightObjects = new List<GameObject>();
        public float pedestrianGreenLightTime;
        public List<int> pedestrianStopWaypointsIndex;


        public override List<PedestrianWaypointSettings> GetPedestrianWaypoints()
        {
            return pedestrianWaypoints;
        }

        public override List<PedestrianWaypointSettings> GetDirectionWaypoints()
        {
            return directionWaypoints;
        }

        public override bool VerifyAsignements()
        {
            for (int i = directionWaypoints.Count - 1; i >= 0; i--)
            {
                if (directionWaypoints[i] == null)
                {
                    directionWaypoints.RemoveAt(i);
                }
                else
                {
                    if (!directionWaypoints[i].neighbors.Intersect(pedestrianWaypoints).Any() && !directionWaypoints[i].prev.Intersect(pedestrianWaypoints).Any())
                    {
                        directionWaypoints.RemoveAt(i);
                    }
                }
            }


            for (int i = pedestrianWaypoints.Count - 1; i >= 0; i--)
            {
                if (pedestrianWaypoints[i] == null)
                {
                    pedestrianWaypoints.RemoveAt(i);
                }
                else
                {
                    if (!pedestrianWaypoints[i].neighbors.Intersect(directionWaypoints).Any() && !pedestrianWaypoints[i].prev.Intersect(directionWaypoints).Any())
                    {
                        Debug.LogError($"Pedestrian waypoint {pedestrianWaypoints[i].name} from intersection {name} has no direction assigned", gameObject);
                        return false;
                    }
                }
            }

            return true;
        }
#endif
    }
}
#endif
