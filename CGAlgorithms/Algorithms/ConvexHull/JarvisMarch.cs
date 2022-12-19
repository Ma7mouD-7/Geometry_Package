using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
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
        public bool Point_in_segment(Line l, Point p)
        {

            if (p.X >= Math.Min(l.Start.X, l.End.X) && p.X <= Math.Max(l.Start.X, l.End.X))
                if (p.Y >= Math.Min(l.Start.Y, l.End.Y) && p.Y <= Math.Max(l.Start.Y, l.End.Y))
                    return true;
            return false;
        }
        public bool Point_to_Point(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) <= Constants.Epsilon && Math.Abs(p1.Y - p2.Y) <= Constants.Epsilon)
                return true;
            return false;
        }

        public double distance(Point p1, Point p2)
        { 
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
             
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {


            points.Sort(compare);




            // getting min index 

            Point temp = points[0];
            int idx = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y == temp.Y)
                {
                    if (points[i].X < temp.X)
                    {
                        temp = points[i];
                        idx = i;
                    }
                }
                else if (points[i].Y < temp.Y)
                {
                    temp = points[i];
                    idx = i;
                }

            }

            int min_idx = idx , cur_idx = idx;
            Point cur_point = points[idx];

            List<Point> ans = new List<Point>();
            ans.Add(cur_point);


            int sz = points.Count;
            // collect 
            while (true)
            {

                int nxt_idx = (cur_idx + 1) % sz;

                while (Point_to_Point(cur_point, points[nxt_idx]) && nxt_idx != cur_idx)
                    nxt_idx = (nxt_idx + 1) % sz;
                if (nxt_idx == cur_idx)
                    break;
                Point next_point = points[nxt_idx];


                Line segment = new Line(cur_point, next_point);
                // left most so all [ right | coliner ] to me
                for (int i = 0; i < sz; i++)
                {
                    Point temper = points[i];
                    
                    Enums.TurnType pos = HelperMethods.CheckTurn(segment, temper);
                    if (pos == Enums.TurnType.Right)
                    {
                        segment.End = temper;
                        nxt_idx = i;
                        next_point = temper;
                    }
                    else if (pos == Enums.TurnType.Colinear)
                    {
                       
                        double cur_dist = distance(cur_point, next_point);
                        double temp_dist = distance(cur_point, temper);
                        if (temp_dist > cur_dist)
                        {
                            segment.End = temper;
                            nxt_idx = i;
                            next_point = temper;
                        }
                    }
                }

                if (nxt_idx == min_idx)
                    break;

                
                ans.Add(next_point);
                cur_point = next_point;
                cur_idx = nxt_idx;

            }
            outPoints = ans;

        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
