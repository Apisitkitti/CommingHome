using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.TrafficSystem;
public class SelectSpawn : MonoBehaviour
{
    
    public Transform[] spawnPoints;  // ตำแหน่ง Spawn
    public VehicleTypes vehicleType = VehicleTypes.Car; // ประเภทรถ
    public float spawnDelay = 3f;    // ความล่าช้าในการ Spawn รถแต่ละคัน

    [SerializeField] private int currentSpawnIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            // เลือกตำแหน่ง Spawn
            //Transform spawnPoint = spawnPoints[currentSpawnIndex];
            
            //Random Spawnpoint
            Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

            // ใช้ API.AddVehicle พร้อม Callback
            API.AddVehicle(spawnPoint.position, vehicleType);

            // หมุนไปยังจุด Spawn ถัดไป
            //currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Callback เมื่อรถ Spawn สำเร็จ
   /* private void OnVehicleSpawned(VehicleComponent vehicle, int index)
    {
        Debug.Log($"Vehicle spawned: {vehicle.name} at index {index}");
    }*/
}
