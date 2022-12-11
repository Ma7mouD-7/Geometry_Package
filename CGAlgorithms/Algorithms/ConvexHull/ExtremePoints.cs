using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count > 3)
            {
                for (int i = 0; i < points.Count; i++){
                    bool found = false;
                    for (int j = 0; j < points.Count; j++){
                        if (j == i) continue;
                        for (int k = 0; k < points.Count; k++){
                            if (k == i) continue;
                            for (int z = 0; z < points.Count; z++){
                                if (z == i) continue;
                                Enums.PointInPolygon status = HelperMethods.PointInTriangle(points[i], points[j], points[k], points[z]);
                                if (status == Enums.PointInPolygon.Inside || status == Enums.PointInPolygon.OnEdge){
                                    points.Remove(points[i]);
                                    i--;
                                    found = true;
                                    break;
                                }
                                if (found) break;
                            }
                            if (found) break;
                        }
                        if (found)  break;
                    }
                }
            }
            outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
