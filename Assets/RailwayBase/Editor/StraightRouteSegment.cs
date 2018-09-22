﻿using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StraightRouteSegment : RouteSegment {
    public float Length { get; set; }
    public float Permil;

    public StraightRouteSegment(float length, float permil = 0) {
        Length = length;
        Permil = permil;
    }

    public override IEnumerable<RouteSegment> ToSubSegments() {
        return new List<RouteSegment> { this };
    }
}
