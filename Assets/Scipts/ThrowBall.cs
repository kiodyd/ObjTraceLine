using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public Vector2 LimitAngle;
    [Range(1, 10)]
    public float Sensitivity = 2;

    public GameObject SpawnBall;
    public Transform SpawnPosition;
    private float AddBallForceRate = 0; // 根据鼠标按压时间决定抛出力度

    public GameObject LR; // LINERENDERER


    private bool isStartThrow = false;
    private GameObject newBall;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        AdjustDirection();
        StartThrow();
    }

    void AdjustDirection()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.eulerAngles += new Vector3(-mouseY * Sensitivity, mouseX * Sensitivity, 0);
        transform.eulerAngles = new Vector3(ClampAngle(transform.eulerAngles.x, LimitAngle.x, LimitAngle.y), transform.eulerAngles.y, transform.eulerAngles.z);
    }

    float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    void StartThrow()
    {

        if (Input.GetMouseButtonDown(0) && newBall == null)
        {
            isStartThrow = true;
            newBall = Instantiate(SpawnBall, SpawnPosition.position, Quaternion.identity, transform);
            newBall.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (Input.GetMouseButton(0) && isStartThrow)
        {
            AddBallForceRate += 5f;
            AddBallForceRate = Mathf.Clamp(AddBallForceRate, 0, 500);
            LR.GetComponent<TraceLineRender>().DrawLine(newBall.GetComponent<Rigidbody>(), Vector3.zero, -newBall.GetComponent<Transform>().forward * AddBallForceRate);
        }
        if (Input.GetMouseButtonUp(0) && isStartThrow)
        {
            isStartThrow = false;
            LR.GetComponent<TraceLineRender>().RemoveLine();
            newBall.GetComponent<Rigidbody>().isKinematic = false;
            newBall.GetComponent<Rigidbody>().AddForce(-newBall.GetComponent<Transform>().forward * AddBallForceRate);
            newBall.GetComponent<Transform>().parent = null;
            newBall = null;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

}
