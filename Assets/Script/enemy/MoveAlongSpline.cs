using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer; // ตัว Spline ที่ต้องการให้เดินตาม
    [SerializeField] private float speed = 1f; // ความเร็วเริ่มต้นในการเคลื่อนที่
    [SerializeField] private float speedIncrease = 5f;
    [SerializeField] private Animator animatorRun;
    private float progress = 0f; // ตำแหน่งปัจจุบันบน Spline (0-1)
  
    void Update()
    {
        if (splineContainer == null) return;

        // เพิ่ม progress ตามเวลาและความเร็ว
        progress += speed * Time.deltaTime / splineContainer.Spline.GetLength();

        // Reset progress เมื่อเกิน 1 (วนลูป)
        // if (progress > 1f)
        //     progress -= 1f;

        // คำนวณตำแหน่งบน Spline
        Vector3 position = splineContainer.EvaluatePosition(progress);
        transform.position = position;
       
        // คำนวณการหมุนเพื่อให้หันตามทิศทาง
        Vector3 direction = splineContainer.EvaluateTangent(progress);
        if (direction != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }

    public void ScavengerHuntMode(){
        speed = speedIncrease;
    }

    public void SetBoolAni(bool value){
        animatorRun.SetBool("IsRunning",value);
    }


}

