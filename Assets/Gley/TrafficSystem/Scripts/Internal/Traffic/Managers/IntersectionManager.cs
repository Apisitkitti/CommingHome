using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Internal
{
    /// <summary>
    /// Updates all intersections
    /// </summary>
    public class IntersectionManager : MonoBehaviour
    {
        private GenericIntersection[] allIntersections;
        private List<GenericIntersection> activeIntersections;
        private WaypointManager waypointManager;
        private bool debugIntersections;
        float realtimeSinceStartup;

#if PHOTON_UNITY_NETWORKING
        public void setRealTimeSinceStartup(float t) => this.realtimeSinceStartup = t;
        public float getRealtimeSinceStartup() => this.realtimeSinceStartup;
#endif

        /// <summary>
        /// Initialize intersection manager
        /// </summary>
        /// <param name="allIntersections"></param>
        /// <param name="activeIntersections"></param>
        /// <param name="waypointManager"></param>
        /// <param name="greenLightTime"></param>
        /// <param name="yellowLightTime"></param>
        /// <param name="debugIntersections"></param>
        /// <param name="stopIntersectionUpdate"></param>
        /// <returns></returns>
        public IntersectionManager Initialize(GenericIntersection[] allIntersections, List<GenericIntersection> activeIntersections, WaypointManager waypointManager, float greenLightTime, float yellowLightTime, bool debugIntersections, TrafficLightsBehaviour trafficLightsBehaviour)
        {
            IntersectionEvents.onActiveIntersectionsChanged += SetActiveIntersection;
            this.debugIntersections = debugIntersections;
            this.allIntersections = allIntersections;
            this.waypointManager = waypointManager;

            for (int i = 0; i < allIntersections.Length; i++)
            {
                allIntersections[i].SetTrafficLightsBehaviour(trafficLightsBehaviour);
                allIntersections[i].Initialize(waypointManager, greenLightTime, yellowLightTime);
            }

            SetActiveIntersection(activeIntersections);
            return this;
        }


        /// <summary>
        /// Initialize all active intersections
        /// </summary>
        /// <param name="activeIntersections"></param>
        public void SetActiveIntersection(List<GenericIntersection> activeIntersections)
        {
            for (int i = 0; i < activeIntersections.Count; i++)
            {
                if (this.activeIntersections != null)
                {
                    if (!this.activeIntersections.Contains(activeIntersections[i]))
                    {
                        activeIntersections[i].ResetIntersection();
                    }
                }
            }
            this.activeIntersections = activeIntersections;
        }


        public void RemoveVehicle(int index)
        {
            for (int i = 0; i < activeIntersections.Count; i++)
            {
                activeIntersections[i].RemoveCar(index);
            }
        }


        /// <summary>
        /// Called on every frame to update active intersection road status
        /// </summary>
        public void UpdateIntersections()
        {
            realtimeSinceStartup += Time.deltaTime;

            for (int i = 0; i < activeIntersections.Count; i++)
            {
                activeIntersections[i].UpdateIntersection(realtimeSinceStartup);
            }
        }


        internal void SetTrafficLightsBehaviour(TrafficLightsBehaviour trafficLightsBehaviour)
        {
            for (int i = 0; i < allIntersections.Length; i++)
            {
                allIntersections[i].SetTrafficLightsBehaviour(trafficLightsBehaviour);
            }
        }


        internal void SetRoadToGreen(string intersectionName, int roadIndex, bool doNotChangeAgain)
        {
            for (int i = 0; i < allIntersections.Length; i++)
            {
                if (allIntersections[i].name == intersectionName)
                {
                    allIntersections[i].SetGreenRoad(roadIndex, doNotChangeAgain);
                }
            }
        }

#if GLEY_PEDESTRIAN_SYSTEM
#if GLEY_TRAFFIC_SYSTEM
        private void OnDestroy()
        {
            for(int i=0;i<allIntersections.Length;i++)
            {
                allIntersections[i].DestroyIntersection();
            }
        }
#endif
#endif


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (debugIntersections)
            {
                for (int k = 0; k < allIntersections.Length; k++)
                {
                    List<IntersectionStopWaypointsIndex> stopWaypoints = allIntersections[k].GetWaypoints();
                    for (int i = 0; i < stopWaypoints.Count; i++)
                    {

                        for (int j = 0; j < stopWaypoints[i].roadWaypoints.Count; j++)
                        {
                            if (waypointManager.GetWaypoint<Waypoint>(stopWaypoints[i].roadWaypoints[j]).stop == true)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(waypointManager.GetWaypoint<Waypoint>(stopWaypoints[i].roadWaypoints[j]).position, 1);
                            }
                        }
                    }
                    //priority intersections
                    if (allIntersections[k].GetType().Equals(typeof(PriorityIntersection)))
                    {
                        PriorityIntersection intersection = (PriorityIntersection)allIntersections[k];
                        Handles.Label(intersection.GetPosition(), "Cars " + intersection.GetCarsInIntersection());
                        for(int i=0;i<intersection.GetWaypointsToCkeck().Count;i++)
                        {
                            Handles.color = intersection.GetWaypointColors()[i];
                            Handles.DrawWireDisc(waypointManager.GetWaypointPosition(intersection.GetWaypointsToCkeck()[i]), Vector3.up, 1);
                        }
                    }

#if GLEY_PEDESTRIAN_SYSTEM
#if GLEY_TRAFFIC_SYSTEM
                    List<int> pedestrianStopWaypoints = allIntersections[k].GetPedStopWaypoint();
                    for (int l = 0; l < pedestrianStopWaypoints.Count; l++)
                    {
                        if (Gley.PedestrianSystem.Internal.PedestriansManager.Instance.waypointManager.GetWaypoint<Gley.PedestrianSystem.Internal.PedestrianWaypoint>(pedestrianStopWaypoints[l]).stop == true)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawSphere(Gley.PedestrianSystem.Internal.PedestriansManager.Instance.waypointManager.GetWaypoint<Gley.PedestrianSystem.Internal.PedestrianWaypoint>(pedestrianStopWaypoints[l]).position, 1);
                        }
                    }
#endif
#endif
                }
            }
        }
#endif
    }
}
