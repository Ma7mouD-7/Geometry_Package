using CGUtilities;
using System.Collections.Generic;
using System.Linq;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public List<Point> divide(List<Point> points)
        {

            int sz = points.Count;
            if (sz == 1)
            {
                return points;
            }

            // just divide all current points to 2 set {(0 to i < (sz/2) )  | ( sz/2 to i < sz ) } 

            List<Point> lft_side = new List<Point>();
            List<Point> rt_side = new List<Point>();

            
            // left side
            int idx = 0;
            while(idx<sz/2)
                lft_side.Add(points[idx++]);

            while(idx<sz)
                rt_side.Add(points[idx++]);

            List<Point> curr_lft = divide(lft_side);
            List<Point> curr_rt = divide(rt_side);



            // recurse with rem points
            return merge(curr_lft, curr_rt);
        }


        public List<Point> merge(List<Point> lf, List<Point> rt)
        {

            
            List<Point> a = new List<Point>();
            List<Point> b = new List<Point>();

            Dictionary<Point, bool> map1 = new Dictionary<Point, bool>();
            Dictionary<Point, bool> map2 = new Dictionary<Point, bool>();

            for (int i = 0; i < lf.Count; ++i)
                map1[lf[i]] = false;
            for (int i = 0; i < rt.Count; ++i)
                map2[rt[i]] = false;


            for (int i = 0; i < lf.Count; ++i)
            {

                if (!map1[lf[i]])
                {
                    a.Add(lf[i]);
                    map1[lf[i]] = true;
                }

            }

            for (int i = 0; i < rt.Count; ++i)
            {
                if (!map2[rt[i]])
                {
                    b.Add(rt[i]);
                    map2[rt[i]] = true;
                }
            }
            

            int idx1 = 0, idx2 = 0, sz1 = a.Count, sz2 = b.Count;

            // search for right most point in left side points
            for (int i = 1; i < sz1; i++)
            {
                if (a[i].X > a[idx1].X)
                    idx1 = i;
                else if (a[i].X == a[idx1].X)
                {
                    if (a[i].Y > a[idx1].Y)
                        idx1 = i;
                }

            }

            // search for left most point in right side points

            for (int i = 1; i < sz2; i++)
            {
                if (b[i].X < b[idx2].X)
                    idx2 = i;
                else if (b[i].X == b[idx2].X)
                {
                    if (b[i].Y < b[idx2].Y)
                        idx2 = i;
                }

            }

            //  upper left && right 
            int upper_left = idx1, upper_right = idx2;
            bool found = false;
           //

            while (!found)
            {
                found = true;
                while (HelperMethods.CheckTurn(new Line(b[upper_right].X,
                           b[upper_right].Y, a[upper_left].X, a[upper_left].Y),
                           a[(upper_left + 1) % sz1]) == Enums.TurnType.Right)
                {
                    upper_left = (upper_left + 1) % sz1;
                    found = false;
                }
                if (found == true &&
                    (HelperMethods.CheckTurn(new Line(b[upper_right].X, b[upper_right].Y, a[upper_left].X, a[upper_left].Y),
                         a[(upper_left + 1) % sz1]) == Enums.TurnType.Colinear))
                    upper_left = (upper_left + 1) % sz1;

                while (HelperMethods.CheckTurn(new Line(a[upper_left].X, a[upper_left].Y, b[upper_right].X, b[upper_right].Y), b[(sz2 + upper_right - 1) % sz2]) == Enums.TurnType.Left)
                {
                    upper_right = (sz2 + upper_right - 1) % sz2;
                    found = false;

                }
                if (found == true && (HelperMethods.CheckTurn(new Line(a[upper_left].X, a[upper_left].Y, b[upper_right].X, b[upper_right].Y), b[(upper_right + sz2 - 1) % sz2]) == Enums.TurnType.Colinear))
                    upper_right = (upper_right + sz2 - 1) % sz2;


            }

            int lower_left = idx1, lower_right = idx2;
            found = false; 
            while (!found)
            {
                found = true;
                while (HelperMethods.CheckTurn(new Line(b[lower_right].X, b[lower_right].Y, a[lower_left].X, a[lower_left].Y), a[(lower_left + sz1 - 1) % sz1]) == Enums.TurnType.Left)
                {
                    lower_left = (lower_left + sz1 - 1) % sz1;
                    found = false;
                }

                if (found == true &&
                    (HelperMethods.CheckTurn(new Line(b[lower_right].X, b[lower_right].Y, a[lower_left].X, a[lower_left].Y),
                         a[(lower_left + sz1 - 1) % sz1]) == Enums.TurnType.Colinear))
                    lower_left = (lower_left + sz1 - 1) % sz1;

                while (HelperMethods.CheckTurn(new Line(a[lower_left].X, a[lower_left].Y, b[lower_right].X, b[lower_right].Y), b[(lower_right + 1) % sz2]) == Enums.TurnType.Right)
                {
                    lower_right = (lower_right + 1) % sz2;
                    found = false;
                }
                if (found == true && (HelperMethods.CheckTurn(new Line(a[lower_left].X, a[lower_left].Y, b[lower_right].X, b[lower_right].Y), b[(lower_right + 1) % sz2]) == Enums.TurnType.Colinear))
                    lower_right = (lower_right + 1) % sz2;
            }

            List<Point> out_points = new List<Point>();

            int ind = upper_left;
            if (!out_points.Contains(a[upper_left]))
                out_points.Add(a[upper_left]);

            while (ind != lower_left)
            {
                ind = (ind + 1) % sz1;

                if (!out_points.Contains(a[ind]))
                {
                    out_points.Add(a[ind]);

                }


            }

            ind = lower_right;
            if (!out_points.Contains(b[lower_right]))

                out_points.Add(b[lower_right]);

            while (ind != upper_right)
            {
                ind = (ind + 1) % sz2;

                if (!out_points.Contains(b[ind]))

                    out_points.Add(b[ind]);

            }




            return out_points;
        }

        
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {


           /*
            
                - sort
                - divide
                - merge
            
            */


            // sort by x and y 
            points = points.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            
            
            // to get only distinct points

            bool[] visited = new bool[points.Count];


            Dictionary<Point, bool> map =
                       new Dictionary<Point, bool>();

            List<Point> answer = divide(points);


            //init all keys with value false to avoid more than one same point
            for(int i = 0; i < answer.Count; ++i)
                map[answer[i]] = false;
            // to just add distinct points

            for (int i = 0; i < answer.Count; ++i)
            {


                if (map[answer[i]] == false)
                {
                    outPoints.Add(answer[i]);
                    map[answer[i]] = true;
                }
            }

            return;

        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}