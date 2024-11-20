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
    /// Stores priority intersection properties
    /// </summary>
    public class PriorityIntersectionSettings : GenericIntersectionSettings
    {
        public List<IntersectionStopWaypointsSettings> enterWaypoints = new List<IntersectionStopWaypointsSettings>();
        public List<WaypointSettings> exitWaypoints = new List<WaypointSettings>();


        public override List<IntersectionStopWaypointsSettings> GetAssignedWaypoints()
        {
            return enterWaypoints;
        }


        public override List<WaypointSettings> GetExitWaypoints()
        {
            return exitWaypoints;
        }
#if GLEY_PEDESTRIAN_SYSTEM
        public override List<PedestrianWaypointSettings> GetPedestrianWaypoints()
        {
            List<PedestrianWaypointSettings> result = new List<PedestrianWaypointSettings>();

            for (int i = 0; i < enterWaypoints.Count; i++)
            {
                result.AddRange(enterWaypoints[i].pedestrianWaypoints);
            }
            return result;
        }

        public override List<PedestrianWaypointSettings> GetDirectionWaypoints()
        {
            List<PedestrianWaypointSettings> result = new List<PedestrianWaypointSettings>();

            for (int i = 0; i < enterWaypoints.Count; i++)
            {
                result.AddRange(enterWaypoints[i].directionWaypoints);
            }
            return result;
        }

        public override bool VerifyAsignements()
        {
            for (int i = 0; i < enterWaypoints.Count; i++)
            {
                for (int j = enterWaypoints[i].directionWaypoints.Count - 1; j >= 0; j--)
                {
                    if (enterWaypoints[i].directionWaypoints[j] == null)
                    {
                        enterWaypoints[i].directionWaypoints.RemoveAt(j);
                    }
                    else
                    {
                        if (!enterWaypoints[i].directionWaypoints[j].neighbors.Intersect(enterWaypoints[i].pedestrianWaypoints).Any() && !enterWaypoints[i].directionWaypoints[j].prev.Intersect(enterWaypoints[i].pedestrianWaypoints).Any())
                        {
                            enterWaypoints[i].directionWaypoints.RemoveAt(j);
                        }
                    }
                }

                for (int j = 0; j < enterWaypoints[i].pedestrianWaypoints.Count; j++)
                {
                    if (enterWaypoints[i].pedestrianWaypoints[j] == null)
                    {
                        enterWaypoints[i].pedestrianWaypoints.RemoveAt(j);
                    }
                    else
                    {
                        if (!enterWaypoints[i].pedestrianWaypoints[j].neighbors.Intersect(enterWaypoints[i].directionWaypoints).Any() && !enterWaypoints[i].pedestrianWaypoints[j].prev.Intersect(enterWaypoints[i].directionWaypoints).Any())
                        {
                            Debug.LogError($"Pedestrian waypoint {enterWaypoints[i].pedestrianWaypoints[j].name} from intersection {name} road {i} has no direction assigned", gameObject);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
#endif
    }
}
#endif