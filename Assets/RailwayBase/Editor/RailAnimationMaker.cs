using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class RailAnimationMaker {
    // 高低差を考慮して求めるが結果中点の座標の高さ成分は使わない
    public static void MakeAnimationCenterXY(IList<RailObject> railObjects) {
        var centerPoints = new List<Vector3>();
        var startIndex = 0;
        var endIndex = 0;
        // 方向については計算方法が違う
        while (endIndex < railObjects.Count) {
            if (endIndex >= startIndex) {
                var startPosition = railObjects[startIndex].StartPosition;
                var endRailObject = railObjects[endIndex];
                var startCrossPoint = GetCrossPoint(endRailObject.StartPosition, endRailObject.EndPosition, startPosition, RailObject.WheelBetweenWidth);
                if (startCrossPoint != null) { // endIndexの直線上にstartからの橋渡しが可能なら
                    centerPoints.Add((startPosition + (Vector3)startCrossPoint) / 2);
                    ++startIndex;
                }
            }
            var endPosition = railObjects[endIndex].EndPosition;
            var startRailObject = railObjects[startIndex - 1];
            // startIndexの直線上に橋渡しを求める
            var endCrossPoint = GetCrossPoint(startRailObject.StartPosition, startRailObject.EndPosition, endPosition, RailObject.WheelBetweenWidth);
            if (endCrossPoint != null) { // nullでないようにしたいが後回し
                centerPoints.Add((endPosition + (Vector3)endCrossPoint) / 2);
            }
            ++endIndex;
        }
    }

    static Vector3? GetCrossPoint(Vector3 start, Vector3 end, Vector3 center, float radius) {
        if (start.x == end.x) {
            var root = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(center.x - start.x, 2));
            var targetZ = center.z + root;
            if ((start.z < targetZ && end.z < targetZ) || (start.z > targetZ && end.z > targetZ)) {
                targetZ = center.z - root;
            }
            if ((start.z < targetZ && end.z < targetZ) || (start.z > targetZ && end.z > targetZ)) {
                return null;
            }
            return new Vector3(start.x, 0, (float)targetZ);
        } else {
            var xdiff = start.x - end.x;
            var xdiff2 = Math.Pow(xdiff, 2);
            var zdiff = start.z - end.z;
            var zdiff2 = Math.Pow(zdiff, 2);
            var x1z2x2z1 = (start.x * end.z - end.x * start.z);
            var a = xdiff2 + zdiff2;
            var b =
                -center.x * xdiff2
                - center.z * xdiff * zdiff
                + zdiff * x1z2x2z1;
            var c =
                (center.x * center.x + center.y * center.y) * xdiff2
                - 2 * center.z * x1z2x2z1 * xdiff
                + Math.Pow(x1z2x2z1, 2)
                - Math.Pow(radius, 2) * xdiff2;
            var root = Math.Sqrt(Math.Pow(b, 2) - 4 * a * c);
            var targetX = (-b + root) / 2 / a;
            if ((start.x < targetX && end.x < targetX) || (start.x > targetX && end.x > targetX)) {
                targetX = (-b - root) / 2 / a;
            }
            if ((start.x < targetX && end.x < targetX) || (start.x > targetX && end.x > targetX)) {
                return null;
            }
            var targetZ = (zdiff * targetX + x1z2x2z1) / xdiff;
            return new Vector3((float)targetX, 0, (float)targetZ);
        }
    }
}
