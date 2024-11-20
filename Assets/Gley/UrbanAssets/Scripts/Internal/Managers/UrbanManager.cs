using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Gley.TrafficSystem;
#if GLEY_PEDESTRIAN_SYSTEM || GLEY_TRAFFIC_SYSTEM
using Unity.Mathematics;
#endif

namespace Gley.UrbanAssets.Internal
{
    public class UrbanManager : MonoBehaviour
    {
        protected GridManager gridManager;
        internal static UrbanManager urbanManagerInstance;

        internal GridManager GetGridManager()
        {
            if (gridManager == null)
            {
                Debug.LogWarning("Grid manager is null");
            }
            return gridManager;
        }

#if GLEY_PEDESTRIAN_SYSTEM || GLEY_TRAFFIC_SYSTEM
        protected void AddGridManager(CurrentSceneData currentSceneData, NativeArray<float3> activeCameraPositions, SpawnWaypointSelector spawnWaypointSelector)
        {

            gridManager = currentSceneData.gameObject.GetComponent<GridManager>();
            if (gridManager == null)
            {
                gridManager = currentSceneData.gameObject.AddComponent<GridManager>().Initialize(currentSceneData, activeCameraPositions, spawnWaypointSelector);
            }
        }
#endif
    }
}