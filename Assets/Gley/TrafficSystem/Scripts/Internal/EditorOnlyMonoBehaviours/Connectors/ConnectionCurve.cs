using Gley.UrbanAssets.Internal;
using UnityEngine;
namespace Gley.TrafficSystem.Internal
{
    /// <summary>
    /// Store connection curve parameters
    /// </summary>
    [System.Serializable]
    public class ConnectionCurve : ConnectionCurveBase
    {
        public int fromIndex;
        public int toIndex;


        public ConnectionCurve(Path curve, RoadBase fromRoad, int fromIndex, RoadBase toRoad, int toIndex, bool draw, Transform holder) :
           base(curve, fromRoad, toRoad, draw, holder)
        {
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
        }
    }
}