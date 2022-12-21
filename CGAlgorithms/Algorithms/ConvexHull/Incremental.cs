using CGUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {

        public int compare(Point p1, Point p2 )  
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

        

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
             
            int sz = points.Count;

            if (sz < 3)
            {
                outPoints = points;
                return;
            }

            // first sort by X left most
            points.Sort(compare);


            int [] nxt = new int[sz];
            int [] prv = new int[sz];

            int idx = 1;
            
            for (; idx < points.Count && Point_to_Point(points[0], points[idx]);)
                ++idx;
            
           

            // init 

            nxt[0] = idx;
            prv[0] = idx;
            nxt[idx] = 0;
            prv[idx] = 0;

            int cur_holded = idx;
            for (idx = idx + 1; idx < points.Count; idx++)
            {
                Point new_point = points[idx];
                
                if (Point_to_Point(new_point, points[cur_holded]))
                    continue;



                if (new_point.Y >= points[cur_holded].Y)
                {
                    nxt[idx] = nxt[cur_holded];
                    prv[idx] = cur_holded;
                }
                else
                {
                    nxt[idx] = cur_holded;
                    prv[idx] = prv[cur_holded];
                }
                nxt[prv[idx]] = idx;
                prv[nxt[idx]] = idx;

                
                while (true)
                {
                    Line seg = new Line(new_point, points[nxt[idx]]);
                    Point next_point = points[nxt[nxt[idx]]]; 
                    Enums.TurnType turn = HelperMethods.CheckTurn(seg, next_point);

                    
                    if (turn != Enums.TurnType.Left)
                    {
                       
                        nxt[idx] = nxt[nxt[idx]];
                        prv[nxt[idx]] = idx;
                      
                        if (turn == Enums.TurnType.Colinear)
                            break;
                    }
                    else
                        break;
                }

              
                while (true)
                {
                    Line seg = new Line(new_point, points[prv[idx]]);
                    Point next_point = points[prv[prv[idx]]];
                    Enums.TurnType turn = HelperMethods.CheckTurn(seg, next_point);
                    if (turn != Enums.TurnType.Right)
                    {
                        
                        prv[idx] = prv[prv[idx]];
                        nxt[prv[idx]] = idx;
                        if (turn == Enums.TurnType.Colinear)
                            break;
                    }
                    else break;
                }

                
                cur_holded = idx;
            }

       
            int current = 0;
            while (true)
            {
                outPoints.Add(points[current]);
                current = nxt[current];
                if (current == 0)
                    break;
            }
            return;




        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
