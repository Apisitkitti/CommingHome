using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Splines;
public class MovFollowSpilnes : MonoBehaviour
{
  public SplineContainer[] spline; // อ้างอิง Spline ที่ต้องการให้วัตถุเคลื่อนที่ตาม
  public float speed = 1f;       // ความเร็วในการเคลื่อนที่
  private float distancePercentage = 0f; // ตำแหน่งตาม spline (0 ถึง 1)
  public int spawnChoosed;
  private float splineLength;
  private Vector3 velocity = Vector3.zero; // ตัวแปรความเร็วสำหรับ SmoothDamp
  public float smoothTime = 0.3f;         // เวลาในการหน่วงของ SmoothDamp

  private void Start()
  {
    foreach (var spline in spline)
      splineLength = spline.CalculateLength(); // คำนวณความยาวทั้งหมดของ spline
  }

  void Update()
  {
    moveBySpline(spawnChoosed);
  }

  void moveBySpline(int spawnPointNumb)
  {
    // อัปเดตตำแหน่งของวัตถุตาม spline
    distancePercentage += speed * Time.deltaTime / splineLength;
    // if (distancePercentage > 1f)
    // {
    //   distancePercentage = 0f; // รีเซ็ตตำแหน่งเมื่อถึงจุดสิ้นสุดของ spline
    // }

    // ตำแหน่งเป้าหมายใน spline
    Vector3 targetPosition = spline[spawnChoosed].EvaluatePosition(distancePercentage);

    // ใช้ SmoothDamp เพื่อเคลื่อนที่ไปยังตำแหน่งเป้าหมายอย่างนุ่มนวล
    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    // ตั้งค่าการหมุนให้หันไปตาม spline
    Vector3 nextPosition = spline[spawnChoosed].EvaluatePosition(distancePercentage + 0.01f);
    Vector3 direction = nextPosition - targetPosition;
    Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
  }
}
