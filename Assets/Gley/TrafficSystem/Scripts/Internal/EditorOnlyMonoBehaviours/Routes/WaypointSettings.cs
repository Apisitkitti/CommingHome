using Gley.UrbanAssets.Internal;
using System.Collections.Generic;

namespace Gley.TrafficSystem.Internal
{
    /// <summary>
    /// Stores waypoint properties
    /// </summary>
    public class WaypointSettings : WaypointSettingsBase
    {
        public ConnectionCurve connection;
        public List<VehicleTypes> allowedCars;
        public int maxSpeed;
        public int priority;
        public bool giveWay;
        public bool enter;
        public bool exit;
        public bool speedLocked;
        public bool priorityLocked;
        public bool penaltyLocked;
        public bool carsLocked;
        public bool complexGiveWay;
        public bool zipperGiveWay;
        public bool triggerEvent;
        public float laneWidth;
        public string eventData;
    }
}