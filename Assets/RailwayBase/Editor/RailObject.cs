using System;
using UnityEngine;

public class RailObject {
    public const float RailBetweenWidth = 1.067f;
    public const float BaseWidth = 3.2f;
    public const float RailWidth = 0.05f;
    public const float RailHeight = 0.15f;
    public const float BaseHeight = 0.2f;

    const double DegreeToRadian = Math.PI / 180;

    public Vector3 MoveVector;
    public Vector3 Center;
    /// <summary>
    /// 高さ変化を考慮しない長さ
    /// </summary>
    public float PlaneLength;
    public float Length;
    public float LeftLength;
    public float RightLength;
    public float BaseLength;
    public Vector3 Direction;
    public Vector3 StartDirection;
    public Vector3 EndDirection;
    public float StartAngle;
    public float EndAngle;

    public RailObject(Vector3 start, float startAngle, StraightRouteSegment segment) {
        PlaneLength = segment.Length;
        MoveVector =
            Quaternion.Euler(0, startAngle, 0) *
            new Vector3(0, segment.Permil * 0.001f, 1) *
            segment.Length;
        Center = start + MoveVector / 2;
        Length = MoveVector.magnitude;
        Direction = StartDirection = EndDirection = MoveVector.normalized;
        LeftLength = Length;
        RightLength = Length;
        BaseLength = Length;
        StartAngle = EndAngle = startAngle;
    }

    public RailObject(Vector3 start, float startAngle, CurveRouteSegment segment) {
        var middleAngle = startAngle + segment.Angle / 2.0f;
        var startDirection =
            Quaternion.Euler(0, startAngle, 0) *
            new Vector3(0, segment.Permil * 0.001f, 1);
        var endDirection =
            Quaternion.Euler(0, startAngle + segment.Angle, 0) *
            new Vector3(0, segment.Permil * 0.001f, 1);
        var direction = (startDirection + endDirection) / 2;
        var directionMagnitude = direction.magnitude;
        PlaneLength = StraightLength(segment.Radius, segment.Angle);
        var railOffset = StraightWidthOffsetLength(segment.Angle, RailBetweenWidth / 2.0f);
        if (segment.Angle >= 0) railOffset *= -1;
        var baseOffset = StraightWidthOffsetLength(segment.Angle, BaseWidth / 2.0f);

        MoveVector = direction * PlaneLength;
        Center = start + MoveVector / 2;
        Length = directionMagnitude * PlaneLength;
        Direction = MoveVector.normalized;
        StartDirection = startDirection.normalized;
        EndDirection = endDirection.normalized;
        LeftLength = directionMagnitude * (PlaneLength - railOffset);
        RightLength = directionMagnitude * (PlaneLength + railOffset);
        BaseLength = directionMagnitude * (PlaneLength + baseOffset);
        StartAngle = startAngle;
        EndAngle = startAngle + segment.Angle;
    }

    public Vector3 StartPosition { get { return Center - MoveVector / 2; } }
    public Vector3 EndPosition { get { return Center + MoveVector / 2; } }

    /// <summary>
    /// 曲線相当直線の長さを求める
    /// </summary>
    /// <param name="radius">曲率半径</param>
    /// <param name="angle">曲がる角度</param>
    /// <returns></returns>
    private float StraightLength(float radius, float angle) {
        return (float)(2 * Math.Sin(Math.Abs(angle) / 2.0 * DegreeToRadian) * radius);
    }

    /// <summary>
    /// 曲線相当直線の幅によるオフセットを求める
    /// </summary>
    /// <param name="angle">曲がる角度</param>
    /// <param name="width">中心からの幅</param>
    /// <returns></returns>
    private float StraightWidthOffsetLength(float angle, float width) {
        return (float)(Math.Tan(Math.Abs(angle) / 2.0 * DegreeToRadian) * width);
    }
}
