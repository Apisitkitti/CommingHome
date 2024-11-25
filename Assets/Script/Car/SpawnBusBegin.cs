using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.TrafficSystem;

public class SpawnBusBegin : MonoBehaviour
{
    public Transform spawnPoint;       // Position to spawn the bus
    GameObject instantiateLaterVehicle;
    public VehicleTypes vehicleType = VehicleTypes.Bus; // Type of vehicle (Bus)

    void Start()
    {
        // Start a coroutine to wait for the traffic system to initialize
        StartCoroutine(InitializeAndSpawnBus());
    }

    IEnumerator InitializeAndSpawnBus()
    {
        // Wait until the traffic system is initialized
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed

        // After the delay, call the method to spawn the bus
        OnTrafficInitializedCallback();
    }

    // Callback when the traffic system is initialized
    void OnTrafficInitializedCallback()
    {
        // Get the complete list of excluded vehicles
        List<VehicleComponent> excludedVehicles = API.GetExcludedVehicleList();

        bool vehicleFound = false;

        // Find the excluded vehicle that matches the desired type
        foreach (VehicleComponent vehicleComponent in excludedVehicles)
        {
            if (vehicleComponent.vehicleType == vehicleType)
            {
                instantiateLaterVehicle = vehicleComponent.gameObject;
                vehicleFound = true;
                break; // Exit the loop once we find the correct vehicle
            }
        }

        if (vehicleFound && instantiateLaterVehicle != null)
        {
            // If the vehicle is found, instantiate it at the spawn point
            InstantiateExcludedVehicle();
        }
        else
        {
            Debug.LogError("Vehicle type not found or excluded vehicle list is empty.");
        }
    }

    // Instantiate the excluded vehicle at the spawn point
    void InstantiateExcludedVehicle()
    {
        if (instantiateLaterVehicle != null && spawnPoint != null)
        {
            // Add the vehicle to the system at the specified spawn point
            API.AddExcludedVehicle(API.GetExcludedVehicleIndex(instantiateLaterVehicle), spawnPoint.position);
            Debug.Log($"Bus spawned at {spawnPoint.position}");
        }
        else
        {
            Debug.LogError("Unable to instantiate vehicle. Either the vehicle or spawn point is missing.");
        }
    }
}
