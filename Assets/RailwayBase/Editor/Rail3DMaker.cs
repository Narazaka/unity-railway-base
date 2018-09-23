using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Rail3DMaker {
    public static void Make3DRail(IEnumerable<RailObject> railObjects, string containerName = "railroad") {
        var container = GameObject.Find(containerName);
        if (container == null) container = new GameObject(containerName);
        var children = new List<GameObject>();
        for (var i = 0; i < container.transform.childCount; ++i) {
            var child = container.transform.GetChild(i).gameObject;
            children.Add(child);
        }
        foreach (var child in children) {
            GameObject.DestroyImmediate(child);
        }
        var index = 0;
        var railHeightPosition = Vector3.up * (RailObject.BaseHeight + RailObject.RailHeight) / 2;
        var leftRailPosition = railHeightPosition + Vector3.left * RailObject.RailBetweenWidth / 2;
        var rightRailPosition = railHeightPosition + Vector3.right * RailObject.RailBetweenWidth / 2;
        foreach (var railObject in railObjects) {
            var segmentObj = new GameObject("segment-" + index);
            segmentObj.transform.parent = container.transform;
            segmentObj.transform.position = railObject.Center + Vector3.up * railObject.CenterCantHeight;
            segmentObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, railObject.Direction) * railObject.CantRotation;
            var baseObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseObj.name = "base";
            baseObj.transform.parent = segmentObj.transform;
            baseObj.transform.localScale = new Vector3(RailObject.BaseWidth, RailObject.BaseHeight, railObject.BaseLength);
            baseObj.transform.localPosition = Vector3.zero;
            baseObj.transform.localRotation = Quaternion.identity;
            var leftRailObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftRailObj.name = "left-rail";
            leftRailObj.transform.parent = segmentObj.transform;
            leftRailObj.transform.localScale = new Vector3(RailObject.RailWidth, RailObject.RailHeight, railObject.LeftLength);
            leftRailObj.transform.localPosition = leftRailPosition;
            leftRailObj.transform.localRotation = Quaternion.identity;
            var rightRailObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightRailObj.name = "right-rail";
            rightRailObj.transform.parent = segmentObj.transform;
            rightRailObj.transform.localScale = new Vector3(RailObject.RailWidth, RailObject.RailHeight, railObject.RightLength);
            rightRailObj.transform.localPosition = rightRailPosition;
            rightRailObj.transform.localRotation = Quaternion.identity;
            ++index;
        }
    }
}
