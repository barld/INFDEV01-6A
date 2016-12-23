using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint
{
    public class TwoDTree
    {
        class Node
        {
            internal Vector2 Position { get; set; }
            internal Node Left { get; set; }
            internal Node Right { get; set; }

            public bool ToLeft(int level, Vector2 pos) => level % 2 == 0 ? Position.X > pos.X : Position.Y > pos.Y;

            internal Node Add(Vector2 pos, int level)
            {
                Func<Node, Node> _add = (Node direction) => direction == null ? new Node { Position = pos } : direction.Add(pos, level + 1);

                if (ToLeft(level, pos))
                    Left = _add(Left);
                else
                    Right = _add(Right);


                return this;
            }

            internal void PutPointsBetweenInList(Vector2 leftTop, Vector2 rigthBottum, List<Vector2> list, int level = 0)
            {
                if  (
                    leftTop.X < Position.X && leftTop.Y < Position.Y &&
                    rigthBottum.X > Position.X && rigthBottum.Y > Position.Y
                    )
                {
                    list.Add(Position);
                }
                if(ToLeft(level, leftTop))
                {
                    Left?.PutPointsBetweenInList(leftTop, rigthBottum, list, level + 1);
                }
                if(!ToLeft(level, rigthBottum))
                {
                    Right?.PutPointsBetweenInList(leftTop, rigthBottum, list, level + 1);
                }
            }
        }

        Node root;

        public TwoDTree(Vector2 pos)
        {
            root = new Node { Position = pos };
        }

        public void Add(Vector2 pos) => root.Add(pos, 0);

        public IEnumerable<Vector2> GetPointsBetween(Vector2 leftTop, Vector2 rigthBottum)
        {
            List<Vector2> l = new List<Vector2>();
            root.PutPointsBetweenInList(leftTop, rigthBottum, l);
            return l;
        }

        public IEnumerable<Vector2> GetAllInDistanceOfPoint(Vector2 startPoint, float distance)
        {
            var leftTop = startPoint - new Vector2(distance, distance);
            var rightBottum = startPoint + new Vector2(distance, distance);

            var pointsInSquere = this.GetPointsBetween(leftTop, rightBottum);

            return pointsInSquere.Where(point => Vector2.Distance(point, startPoint) < distance);

        }
    }
}
