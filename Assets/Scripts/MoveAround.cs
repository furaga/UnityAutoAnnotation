using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    public GameObject Target;
    public Vector3 Rotation;
    public float FoV = 0;

    public float Distance = 3;
    public float MinFoV = 15;
    public float MaxFoV = 60;
    public float TimePeriodFoV = 5.0f;

    public Vector3 MinRotation = new Vector3(-30, 0, -10);
    public Vector3 MaxRotation = new Vector3(30, 360, 10);
    public float TimePeriodRotation = 3.0f;

    void Start()
    {
    }

    void sync(Camera camera)
    {
        transform.position = camera.transform.position;
        transform.rotation = camera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }

        GetComponent<Camera>().fieldOfView = FoV;

        float tFoV = (Time.realtimeSinceStartup % TimePeriodFoV) / TimePeriodFoV;
        FoV = MinFoV * (1 - tFoV) + MaxFoV * tFoV;

        float tRot = (Time.realtimeSinceStartup % TimePeriodRotation) / TimePeriodRotation;
        Rotation = MinRotation * (1 - tRot) + MaxRotation * tRot;

        transform.rotation = Quaternion.Euler(Rotation);

        var pos = Target.transform.position;
        pos -= transform.rotation * new Vector3(0, 0, Distance);
        transform.position = pos;
    }
}