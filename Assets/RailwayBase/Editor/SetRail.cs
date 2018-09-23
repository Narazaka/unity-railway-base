using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

public static class SetRail {

    static Route Route = new Route(new List<RouteSegment> {
        new StraightRouteSegment(30),
        new CurveRouteSegment(6, 400, 0, 0, 0.1f),
        new CurveRouteSegment(18, 200, 0, 0.1f, 0.1f),
        new CurveRouteSegment(6, 400, 0, 0.1f, 0),
        new StraightRouteSegment(20, 5),
        new CurveRouteSegment(-6, 400, 10, 0, -0.1f),
        new CurveRouteSegment(-18, 250, 10, -0.1f, -0.1f),
        new CurveRouteSegment(-6, 400, 15, -0.1f, 0),
        new StraightRouteSegment(25, 20),
        new CurveRouteSegment(5, 600, 10, 0, 0.05f),
        new CurveRouteSegment(35, 400, 5, 0.05f, 0.05f),
        new CurveRouteSegment(5, 600, 0, 0.05f, 0),
        new StraightRouteSegment(10, -5),
        new StraightRouteSegment(10, -15),
    });

    [MenuItem("RailRoad/Make")]
    static void Set() {
        var railObjects = Route.ToSubSegments().ToRailObjects();
        Rail3DMaker.Make3DRail(railObjects);
    }
}
