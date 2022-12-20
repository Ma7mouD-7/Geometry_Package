using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{


    public class Customize_Comparing : IComparer<Point>
    {
        

        public Point p;

        public Customize_Comparing(Point p)
        {
            this.p = p;
        }
        public int Compare(Point b, Point c)
        {
            
            Line segment = new Line(p, b);
            Enums.TurnType pos  = HelperMethods.CheckTurn(segment, c);
            if (pos == Enums.TurnType.Right)
                return 1;
            else if (pos == Enums.TurnType.Left)
                return -1;

            else
            {

                double pointB_distance = distance(this.p, b);
                double pointC_distance = distance(this.p, c);
                if (pointB_distance >= pointC_distance)
                    return 1;
                else
                    return -1;
            }
        }
        public double distance(Point p1, Point p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

        }
    }


    public class GrahamScan : Algorithm
    {


        // Helper Fuctions 
        public double distance(Point p1, Point p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

        }

        public int Min_idx(List<Point> points)
        {
            Point cur_point = points[0];
            int idx = 0;
           
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == cur_point.X)
                {
                    if (points[i].Y < cur_point.Y)
                    {
                        cur_point = points[i];
                        idx = i;
                    }
                }
                else if (points[i].X < cur_point.X)
                {
                    cur_point = points[i];
                    idx = i;
                }

            }
            return idx;
        }


        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            int sz = points.Count;

            if (sz==0)
                return;

            if(sz==1)
            {
                outPoints.Add(points[0]);
                return;
            }

            int min_idx = Min_idx(points);
            Point min_point = points[min_idx];
            Customize_Comparing sort_by_angle = new Customize_Comparing(min_point);

            List<Point> points_afterSort = points;
            points_afterSort.Sort(sort_by_angle);

            List<Point> hullPoints = new List<Point>();

            hullPoints.Add(min_point);

            hullPoints.Add(points_afterSort[1]);

            for (int i = 2; i < points_afterSort.Count; i++)
            {
                
                Point cur_to_add = points_afterSort[i];
                
                /*  Just like get last two element in your stack */
                /*  check dir if in your link else remove top element and check again */
                
                int lst = hullPoints.Count - 1;
                Point st = hullPoints[lst - 1];
                Point en = hullPoints[lst];

                
                Line segment= new Line(st, en);
                Enums.TurnType pos = HelperMethods.CheckTurn(segment, cur_to_add);
                if (pos == Enums.TurnType.Left)
                {
                    hullPoints.Add(cur_to_add);
                }
                else if (pos == Enums.TurnType.Right)
                {
                    

                    hullPoints.RemoveAt(hullPoints.Count - 1);  
                    i -= 1;

                    
                }
                else { 

                    double en_distace = distance(st, en);
                    double cur_to_add_distance = distance(st, cur_to_add);
                    if (cur_to_add_distance > en_distace)
                    {
                        hullPoints.RemoveAt(hullPoints.Count - 1);  
                        hullPoints.Add(cur_to_add);
                    }
                }
            }

           

            outPoints = hullPoints;
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
