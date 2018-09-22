using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class Route : ScriptableObject {
    [Header("segments")]
    public List<RouteSegment> RouteSegments = new List<RouteSegment>();

    public Route() : base() { }

    public Route(IEnumerable<RouteSegment> route) {
        RouteSegments = route.ToList();
    }

    public Route ToSubSegments() {
        return new Route(RouteSegments.SelectMany(segment => segment.ToSubSegments()));
    }

    public IList<RailObject> ToRailObjects(Vector3 currentPosition = default(Vector3), float currentAngle = 0) {
        var railObjects = new List<RailObject>();
        foreach (var segment in RouteSegments) {
            Debug.Log(currentPosition + " " + currentAngle);
            var railObject =
                segment is StraightRouteSegment ?
                new RailObject(
                    currentPosition,
                    currentAngle,
                    segment as StraightRouteSegment
                    ) :
                new RailObject(
                    currentPosition,
                    currentAngle,
                    segment as CurveRouteSegment
                    );
            currentPosition += railObject.MoveVector;
            if (segment is CurveRouteSegment) currentAngle += (segment as CurveRouteSegment).Angle;
            railObjects.Add(railObject);
            Debug.Log("move=" + railObject.MoveVector + " len=" + railObject.Length + " center=" + railObject.Center + " rot=" + railObject.Direction);
        }
        return railObjects;
    }
}
