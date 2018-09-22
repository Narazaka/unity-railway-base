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

    public RailObject(Vector3 start, float length, float angle, float permil) {
        PlaneLength = length;
        MoveVector =
            new Vector3(
                (float)Math.Sin(angle * DegreeToRadian),
                permil * 0.001f,
                (float)Math.Cos(angle * DegreeToRadian)
                ) * length;
        Center = start + MoveVector / 2;
        Length = MoveVector.magnitude;
        Direction = MoveVector.normalized;
        LeftLength = Length;
        RightLength = Length;
        BaseLength = Length;
    }

    public RailObject(Vector3 start, float radius, float angle, float addAngle, float permil) {
        var direction =
            new Vector3(
                (float)Math.Sin((angle + addAngle / 2.0) * DegreeToRadian),
                permil * 0.001f,
                (float)Math.Cos((angle + addAngle / 2.0) * DegreeToRadian)
                );
        var directionMagnitude = direction.magnitude;
        PlaneLength = StraightLength(radius, addAngle);
        var railOffset = StraightWidthOffsetLength(addAngle, RailBetweenWidth / 2.0f);
        if (addAngle >= 0) railOffset *= -1;
        var baseOffset = StraightWidthOffsetLength(addAngle, BaseWidth / 2.0f);

        MoveVector = direction * PlaneLength;
        Center = start + MoveVector / 2;
        Length = directionMagnitude * PlaneLength;
        Direction = MoveVector.normalized;
        LeftLength = directionMagnitude * (PlaneLength - railOffset);
        RightLength = directionMagnitude * (PlaneLength + railOffset);
        BaseLength = directionMagnitude * (PlaneLength + baseOffset);
    }

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
