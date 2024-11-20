#if UNITY_EDITOR
#if GLEY_PEDESTRIAN_SYSTEM
using Gley.PedestrianSystem;
using Gley.PedestrianSystem.Internal;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Gley.TrafficSystem.Internal
{
    public abstract class GenericIntersectionSettings : MonoBehaviour
    {
        public abstract List<IntersectionStopWaypointsSettings> GetAssignedWaypoints();
        public abstract List<WaypointSettings> GetExitWaypoints();
#if GLEY_PEDESTRIAN_SYSTEM
        public abstract List<PedestrianWaypointSettings> GetPedestrianWaypoints();
        public abstract List<PedestrianWaypointSettings> GetDirectionWaypoints();
        public abstract bool VerifyAsignements();
#endif
    }
}
#endif