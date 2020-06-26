using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TraceLineRender : MonoBehaviour
{
    public int CalculateStep;  //计算的步数
    public float TimeBeteenStep;  //每步之间的间隔时间

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(Rigidbody MoveObj, Vector3 AddSpeed, Vector3 AddForce)
    {
        var tracePoint = MoveObj.CalculateMovement(CalculateStep,TimeBeteenStep,AddSpeed,AddForce);
        _lineRenderer.positionCount = CalculateStep + 1;
        _lineRenderer.SetPosition(0, MoveObj.position);
        for(int i = 0; i < tracePoint.Length; ++i) {
            _lineRenderer.SetPosition(i+1, tracePoint[i]);
        }
    }
    public void RemoveLine() 
    {
        _lineRenderer.positionCount = 0;
    }

}
