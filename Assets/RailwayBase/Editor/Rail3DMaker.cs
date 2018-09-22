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
        foreach (var railObject in railObjects) {
            var baseObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // var leftRailObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // var rightRailObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseObj.name = "base-" + index;
            baseObj.transform.position = railObject.Center;
            baseObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, railObject.Direction);
            baseObj.transform.localScale = new Vector3(RailObject.BaseWidth, RailObject.BaseHeight, railObject.BaseLength);
            baseObj.transform.parent = container.transform;
            ++index;
        }
    }
}
