using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

public static class SetRail {

    static Route Route = new Route(new List<RouteSegment> {
        new StraightRouteSegment(800),
        new CurveRouteSegment(30, 400),
        new StraightRouteSegment(100, 20),
        new CurveRouteSegment(30, 400),
        new StraightRouteSegment(100, 20),
        new CurveRouteSegment(30, 400),
        new StraightRouteSegment(100, 20),
        new StraightRouteSegment(100, 20),
        new StraightRouteSegment(10000, 2),
    });

    [MenuItem("RailRoad/Make")]
    static void Set() {
        var railObjects = Route.ToSubSegments().ToRailObjects();
    }
}
