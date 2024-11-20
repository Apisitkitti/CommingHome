using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
namespace Gley.UrbanAssets.Editor
{
    public abstract class RoadConnectionsBase : UnityEditor.Editor
    {
        protected abstract List<RoadBase> LoadAllRoads();
        protected abstract T GetConnectionPool<T>() where T:ConnectionPoolBase;
        internal abstract void GenerateSelectedConnections(float waypointDistance);
    }
}
