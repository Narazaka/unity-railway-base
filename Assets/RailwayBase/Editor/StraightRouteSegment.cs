using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StraightRouteSegment : RouteSegment {
    public float Length { get; set; }
    public float Permil;
    public float Cant;

    public StraightRouteSegment(float length, float permil = 0, float cant = 0) {
        Length = length;
        Permil = permil;
        Cant = cant;
    }

    public override IEnumerable<RouteSegment> ToSubSegments() {
        return new List<RouteSegment> { this };
    }
}
