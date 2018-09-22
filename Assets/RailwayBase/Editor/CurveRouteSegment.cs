using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class CurveRouteSegment : RouteSegment {
    public const float MaxSubSegmentLength = 3;
    public const float MaxSubSegmentAngle = 1;
    public float Angle;
    public float Radius;
    public float Permil;

    public CurveRouteSegment(float angle, float radius, float permil = 0) {
        Angle = angle;
        Radius = radius;
        Permil = permil;
    }

    public override IEnumerable<RouteSegment> ToSubSegments() {
        var subAngleCount = SubSegmentCount();
        var subAngle = Angle / subAngleCount;
        var subAngleLastIndex = subAngleCount - 1;
        return Enumerable.Range(0, subAngleCount).
            Select((index) => new CurveRouteSegment(
                index == subAngleLastIndex ? Angle - subAngle * subAngleLastIndex : subAngle,
                Radius,
                Permil
                ) as RouteSegment);
    }

    private int SubSegmentCount() {
        return Math.Max(SubSegmentCountByLength(), SubSegmentCountByAngle());
    }

    private int SubSegmentCountByLength() {
        var maxSubAngle = 180 * MaxSubSegmentLength / Math.PI / Radius;
        return (int)Math.Ceiling(Angle / maxSubAngle);
    }

    private int SubSegmentCountByAngle() {
        return (int)Math.Ceiling(Angle / MaxSubSegmentAngle);
    }
}
