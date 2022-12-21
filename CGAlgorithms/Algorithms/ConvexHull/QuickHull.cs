using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {


        public double postion_of_point(Point A, Point B, Point P)
        {
            double ans = (B.X - A.X) * (P.Y - A.Y) - (B.Y - A.Y) * (P.X - A.X);
            if (ans > 0)
                return 1;
            else if (ans == 0)
                return 0;
            else
                return -1;
        }


       
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {


            List<Point> convexhull = new List<Point>();

            if (points.Count < 3)
            {
                outPoints = points;
                return ;
            }

            int idx_minPoint = -1, idx_maxPoint = -1;
            double minX = double.MaxValue;
            double maxX = double.MinValue;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                    idx_minPoint = i;
                }

                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                    idx_maxPoint = i;
                }
            }


            Point A = points[idx_minPoint];
            Point B = points[idx_maxPoint];

            convexhull.Add(A);
            convexhull.Add(B);

            points.Remove(A);
            points.Remove(B);

            List<Point> leftSet = new List<Point>();
            List<Point> rightSet = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {

                Point p = points[i];

                if (postion_of_point(A, B, p) == -1)
                    leftSet.Add(p);
                else if (postion_of_point(A, B, p) == 1)
                    rightSet.Add(p);
            }

            solve_Hull(A, B, rightSet, convexhull);
            solve_Hull(B, A, leftSet, convexhull);


            outPoints = convexhull;
        }
        public double Distance(Point A, Point B, Point C)
        {
            double ABx = B.X - A.X;
            double ABy = B.Y - A.Y;
            double num = ABx * (A.Y - C.Y) - ABy * (A.X - C.X);
            if (num < 0)
                num = -num;
            return num;
        }


        public void solve_Hull(Point A, Point B, List<Point> set, List<Point> hull)
        {

            int insertPos = hull.IndexOf(B);

            if (set.Count == 0)
                return;
            if (set.Count == 1)
            {
                Point p = set[0];
                set.Remove(p);
                hull.Insert(insertPos, p);
                return;

            }


            double dist = int.MinValue;
            int idx_furthest = -1;
            for (int i = 0; i < set.Count(); i++)
            {
                Point p = set[i];
                double distance = Distance(A, B, p);
                if (distance > dist)
                {
                    dist = distance;
                    idx_furthest = i;
                }
            }
            Point P = set[idx_furthest];
            set.Remove(set[idx_furthest]);
            hull.Insert(insertPos, P);

            List<Point> leftSetAP = new List<Point>();

            for (int i = 0; i < set.Count(); i++)
            {
                Point M = set[i];
                if (postion_of_point(A, P, M) == 1)
                {
                    leftSetAP.Add(M);
                }
            }

            List<Point> leftSetPB = new List<Point>();
            for (int i = 0; i < set.Count(); i++)
            {
                Point M = set[i];
                if (postion_of_point(P, B, M) == 1)
                {
                    leftSetPB.Add(M);
                }
            }
            solve_Hull(A, P, leftSetAP, hull);
            solve_Hull(P, B, leftSetPB, hull);

        }


        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
