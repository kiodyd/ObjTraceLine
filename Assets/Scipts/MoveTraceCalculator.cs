using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveTraceCalculator
{

    public static Vector3[] CalculateMovement(this Rigidbody that,
               int stepCount, float timeBeteenStep, Vector3 addedSpeed, Vector3 addedForce)
    {
        //var v = (that.isKinematic == false ? that.velocity : Vector3.zero);
        var v = that.velocity;
        var a = (that.useGravity ? Physics.gravity : Vector3.zero);
        return CalculateMovement(that.transform.position, v, a, stepCount, timeBeteenStep, addedSpeed, addedForce, that.mass, that.drag);
    }

    public static Vector3[] CalculateMovement(Vector3 position, Vector3 velocity, Vector3 acc,
               int stepCount, float timeBeteenStep, Vector3 addedSpeed, Vector3 addedForce, float mass, float drag)
    {
        var ret = new Vector3[stepCount];

        var addedV = (addedForce / mass) * Time.fixedDeltaTime;
        var v = velocity + addedSpeed + addedV;
        var a = acc;

        var x = position;
        var calc = new Vector3[] { x, v };
        for (var i = 0; i < stepCount; ++i)
        {
            calc = CalculateNewPos(calc[0], calc[1], a, drag, timeBeteenStep);
            ret[i] = calc[0];
        }
        return ret;
    }

    private static Vector3[] CalculateNewPos(Vector3 x, Vector3 v, Vector3 a, float drag, float deltaTimeCount)
    {
        var dt = Time.fixedDeltaTime;
        var aDt = a * dt;
        var dragDt = 1 - drag * dt;
        dragDt = dragDt < 0 ? 0 : dragDt;
        var acc = .5f * a * dt * dt;
        for (int i = 0; i < deltaTimeCount; ++i)
        {
            v = (v + aDt) * dragDt;
            x = x + v * dt + acc;
        }
        return new Vector3[] { x, v };
    }



}
