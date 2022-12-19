using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {

        // Helper Functions 

        public int compare(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y < p2.Y)
                    return -1;
                return 1;
            }
            else if (p1.X < p2.X)
                return -1;
            return 1;
        }

        public bool Point_to_Point(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) <= Constants.Epsilon && Math.Abs(p1.Y - p2.Y) <= Constants.Epsilon)
                return true;
            return false;
        }

        public bool Point_in_segment(Line l, Point p)
        {
    
            if (p.X>= Math.Min(l.Start.X, l.End.X) && p.X <= Math.Max(l.Start.X, l.End.X))
                if (p.Y >= Math.Min(l.Start.Y, l.End.Y) && p.Y <= Math.Max(l.Start.Y, l.End.Y))
                    return true;
            return false;
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            points.Sort(compare);

            // need to make  sure all points that i get is distinct ?

            bool[] visited = new bool[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                visited[i] = false;
            }

            for(int i=0;i<points.Count;i++)
            {

                if (i > 0 && Point_to_Point(points[i], points[i - 1]))
                    continue;
                

                    for(int j=i+1;j<points.Count;j++)
                    {
                        if (Point_to_Point(points[i], points[j]))
                            continue;
                        if (j>0 && Point_to_Point(points[j], points[j - 1]))
                        continue;

                        Line segment = new Line(points[i], points[j]);

                        int lf = 0, rt = 0;

                        for(int k=0;k<points.Count;k++)
                        {
                            if(i==k||k==j)
                            {
                                continue;
                            }


                            Enums.TurnType pos = HelperMethods.CheckTurn(segment, points[k]);

                            if (pos == Enums.TurnType.Left)
                                lf++;
                            else if (pos == Enums.TurnType.Right)
                                rt++;
                            else
                            {
                                // can be already coliner or just fake point need to ignore
                                if (!Point_in_segment(segment, points[k]))
                                {
                                    lf++;
                                    rt++;

                                }

                            }

                        }


                        if(lf==0 || rt==0)
                        {

                            if (!visited[i])
                            {
                                outPoints.Add(points[i]);
                                visited[i] = true;
                            }

                            if (!visited[j])
                            {
                                outPoints.Add(points[j]);
                                visited[j] = true;
                            }
                        }

                    }


                
            }
           
            if (points.Count <3 )
            {
                outPoints = points;
            }
            

        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
