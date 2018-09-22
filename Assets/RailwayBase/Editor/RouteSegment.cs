using System.Collections.Generic;
using System;

[Serializable]
public class RouteSegment {
    public virtual IEnumerable<RouteSegment> ToSubSegments() { return new List<RouteSegment> { this }; }
}
