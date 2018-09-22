using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class CurveRouteSegment : RouteSegment {
    public int Angle;
    public int Radius;
    public int Permil;

    public CurveRouteSegment(int angle, int radius, int permil = 0) {
        Angle = angle;
        Radius = radius;
        Permil = permil;
    }

    public override IEnumerable<RouteSegment> ToSubSegments() {
        return Enumerable.Range(0, Angle).
            Select((_) => new CurveRouteSegment(1, Radius, Permil) as RouteSegment);
    }
}
