using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public class TwoDTree<T>
    {
        class Node
        {
            internal Vector2 Position { get; set; }
            internal T Value { get; set; }
            internal Node Left { get; set; }
            internal Node Right { get; set; }

            public bool ToLeft(int level, Vector2 pos) => level % 2 == 0 ? Position.X > pos.X : Position.Y > pos.Y;

            internal Node Add(Vector2 pos, T value, int level)
            {
                Func<Node,Node> _add = (Node direction) => direction == null ? new Node { Position = pos, Value = value } : direction.Add(pos, value, level + 1);

                if (ToLeft(level, pos))
                    Left = _add(Left);
                else
                    Right = _add(Right);


                return this;
            }

            internal T TryGet(Vector2 pos, int level)
            {
                if (pos.X == Position.X && pos.Y == Position.Y)
                    return Value;

                if (ToLeft(level, pos))
                    return Left == null ? default(T) : Left.TryGet(pos, level + 1);
                else
                    return Right == null ? default(T) : Right.TryGet(pos, level + 1);

            }
        }

        Node root;

        public TwoDTree(Vector2 pos, T value)
        {
            root = new Node { Position = pos, Value = value };
        }

        public void Add(Vector2 pos, T value) => root.Add(pos, value, 0);
    }

    public struct Vector2
    {
        public int X;
        public int Y;
    }
}
