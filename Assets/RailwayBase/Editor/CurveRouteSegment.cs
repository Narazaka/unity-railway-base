using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class CurveRouteSegment : RouteSegment {
    public const float MaxSubSegmentLength = 3;
    public const float MaxSubSegmentAngle = 1;
    public const float MaxSubSegmentCant = 0.003f;
    public float Angle;
    public float Radius;
    public float Permil;
    public float StartCant;
    public float EndCant;

    public CurveRouteSegment(float angle, float radius, float permil = 0, float startCant = 0, float endCant = 0) {
        Angle = angle;
        Radius = radius;
        Permil = permil;
        StartCant = startCant;
        EndCant = endCant;
    }

    public override IEnumerable<RouteSegment> ToSubSegments() {
        var subSegmentCount = SubSegmentCount();
        var subSegmentLastIndex = subSegmentCount - 1;
        var subAngle = Angle / subSegmentCount;
        var subCant = (EndCant - StartCant) / subSegmentCount;
        var subCants = Enumerable.Range(0, subSegmentCount).Select(index => StartCant + index * subCant).ToList();
        subCants.Add(EndCant);
        return Enumerable.Range(0, subSegmentCount).
            Select((index) => new CurveRouteSegment(
                index == subSegmentLastIndex ? Angle - subAngle * subSegmentLastIndex : subAngle,
                Radius,
                Permil,
                subCants[index],
                subCants[index + 1]
                ) as RouteSegment);
    }

    private int SubSegmentCount() {
        return Math.Max(Math.Max(SubSegmentCountByLength(), SubSegmentCountByAngle()), SubSegmentCountByCant());
    }

    private int SubSegmentCountByLength() {
        var maxSubAngle = 180 * MaxSubSegmentLength / Math.PI / Radius;
        return (int)Math.Ceiling(Angle / maxSubAngle);
    }

    private int SubSegmentCountByAngle() {
        return (int)Math.Ceiling(Angle / MaxSubSegmentAngle);
    }

    private int SubSegmentCountByCant() {
        return (int)Math.Ceiling(Math.Abs(EndCant - StartCant) / MaxSubSegmentCant);
    }
}
