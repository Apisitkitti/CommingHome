using UnityEngine;

namespace Gley.UrbanAssets.Internal
{
    [System.Serializable]
    public class ConnectionCurveBase
    {
        [HideInInspector]
        public string name;
        public Transform holder;
        public Path curve;
        public RoadBase fromRoad;
        public RoadBase toRoad;
        public bool draw;
        public bool drawWaypoints;

        public ConnectionCurveBase(Path curve, RoadBase fromRoad, RoadBase toRoad, bool draw, Transform holder)
        {
            name = holder.name;
            this.curve = curve;
            this.fromRoad = fromRoad;           
            this.toRoad = toRoad;
            this.draw = draw;
            this.holder = holder;
        }
    }
}