using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class CurveRouteSegment : RouteSegment {
    public const float SubAngleLength = 2;
    public float Angle;
    public float Radius;
    public float Permil;

    public CurveRouteSegment(float angle, float radius, float permil = 0) {
        Angle = angle;
        Radius = radius;
        Permil = permil;
    }

    public override IEnumerable<RouteSegment> ToSubSegments() {
        var subAngleCount = SubAngleCount();
        var subAngle = Angle / subAngleCount;
        var subAngleLastIndex = subAngleCount - 1;
        Debug.Log(subAngle);
        Debug.Log(subAngleCount);
        return Enumerable.Range(0, subAngleCount).
            Select((index) => new CurveRouteSegment(
                index == subAngleLastIndex ? Angle - subAngle * subAngleLastIndex : subAngle,
                Radius,
                Permil
                ) as RouteSegment);
    }

    private int SubAngleCount() {
        var maxSubAngle = 180 * SubAngleLength / Math.PI / Radius;
        return (int)Math.Ceiling(Angle / maxSubAngle);
    }
}
