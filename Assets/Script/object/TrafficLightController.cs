using UnityEngine;
using System.Collections.Generic;

public class TrafficLightController : MonoBehaviour
{
   
    [SerializeField] List<MeshRenderer> lightBulb;
    [SerializeField] List<Material> color;
    private float timer;
    public float timerred = 2f;
    public float timeryellow =2f;
    public float timergreen =2f;
    private int state; // 0 = Red, 1 = Yellow, 2 = Green,3 = Black 

    void Start()
    {
        
        timer = 0f;
        state = 0;
        ChangeLight();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (state == 0 && timer >= timerred) // Red for 5 seconds
        {
            state = 1;
            timer = 0f;
            ChangeLight();
        }
        else if (state == 1 && timer >= timeryellow) // Yellow for 2 seconds
        {
            state = 2;
            timer = 0f;
            ChangeLight();
        }
        else if (state == 2 && timer >= timergreen) // Green for 5 seconds
        {
            state = 0;
            timer = 0f;
            ChangeLight();
        }
    }

    void ChangeLight()
    {
       
       switch (state)
        {
            case 0:
                lightBulb[2].material = color[3];
                lightBulb[0].material = color[0];
                break;
            case 1:
                lightBulb[0].material = color[3];
                lightBulb[1].material = color[1];
                break;
            case 2:
                lightBulb[1].material = color[3];
                lightBulb[2].material = color[2];
                break;
        }
    }
   

    public bool IsGreen()
    {
        return state == 2;
    }
}
