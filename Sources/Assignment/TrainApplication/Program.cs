using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainApplication;

namespace TrainApplication
{
    //public class BinaryTree<T>
    //{
    //    private int cLeft = 0;
    //    private int cRight = 0;

    //    public int Count => cLeft + cRight;

    //    public T Value { get; set; }
    //    public BinaryTree<T> Left { get; set; }
    //    public BinaryTree<T> Right { get; set; }

    //    public void Add(T item)
    //    {
    //        if(cLeft < cRight)
    //        {
    //            if (Left == null)
    //                Left = new BinaryTree<T> { Value = item };
    //            else
    //                Left.Add(item);
    //            cLeft++;
    //        }
    //        else
    //        {
    //            if (Right == null)
    //                Right = new BinaryTree<T> { Value = item };
    //            else
    //                Right.Add(item);
    //            cRight++;
    //        }
    //    }

    //    public void Print()
    //    {
    //        print(0);
    //    }

    //    private void print(int level)
    //    {            
    //        Left?.print(level + 1);
    //        Console.WriteLine($"{new String('\t', level)}{Value}");
    //        Right?.print(level + 1);
    //    }

    //    public BinaryTree<T> keepWhen(Predicate<T> p)
    //    {
    //        if (p(Value))
    //        {
    //            Left?.keepWhen(p);
    //            Right?.keepWhen(p);
    //            return this;
    //        }
    //        else
    //        {
    //            if (Left == null && Right == null)
    //                return null;
    //            else if (Left == null)
    //                return Right;
    //            else if(Right == null)
    //                return Left;
    //            else
    //            {
    //                MergeTrees(Left, Right);
    //            }
    //        }
                
    //    }

    //    public static BinaryTree<T> MergeTrees(BinaryTree<T> left, BinaryTree<T> right)
    //    {
    //        if (left == null) throw new ArgumentNullException(nameof(left));
    //        if (right == null) throw new ArgumentNullException(nameof(right));

    //        var baseTree = left.Count > right.Count ? left : right;
    //    }
    //}


    public class BalancedBinaryTree
    {
        class Node
        {
            internal int Value { get; set; }

            internal int CountLeft { get; set; } = 0;
            internal int CountRight { get; set; } = 0;
            internal Node Left { get; set; }
            internal Node Right { get; set; }

            internal void print(int v)
            {
                Left?.print(v + 1);
                Console.WriteLine(new String('\t', v)+Value);
                Right?.print(v + 1);
            }

            internal Node add(int value)
            {
                return Add(this, value);
            }

            internal Tuple<int, Node> PickAndDeleteLast()
            {
                if (Right == null)
                    return new Tuple<int, Node>(Value, Left);
                else
                {
                    var tup = this.Right.PickAndDeleteLast();
                    this.Right = tup.Item2;
                    this.CountRight--;
                    return new Tuple<int, Node>(tup.Item1, this);
                }
            }

            internal Tuple<int, Node> PickAndDeleteFirst()
            {
                if(Left==null)
                    return new Tuple<int, Node>(Value, Right);
                else
                {
                    var tup = this.Left.PickAndDeleteFirst();
                    this.Left = tup.Item2;
                    this.CountLeft--;
                    return new Tuple<int, Node>(tup.Item1, this);
                }
            }

            internal bool Exist(int value)
            {

                if (Value == value)
                    return true;
                else if (Value < value && Right != null)
                    return Right.Exist(value);
                return Left != null && Left.Exist(value);

            }

            internal Node Delete(int value)
            {
                if(Value == value)
                {
                    if (Left == null) return Right;
                    else if (Right == null) return Left;
                    else
                    {
                        if(CountLeft > CountRight)
                        {
                            var right = Right.PickAndDeleteFirst();
                            Value = right.Item1;
                            Right = right.Item2;
                            CountRight--;
                        }
                        else
                        {
                            var left = Left.PickAndDeleteLast();
                            Value = left.Item1;
                            Left = left.Item2;
                            CountLeft--;
                        }
                    }
                }
                //should togle shift for balancing
                #region shifting
                else if (value < Value)
                {
                    if (Right != null && CountRight > CountLeft)
                    {
                        var right = Right.PickAndDeleteFirst();
                        var currentValue = Value;
                        Value = right.Item1;
                        Right = right.Item2;
                        CountRight--;
                        Left = Left.add(currentValue);
                        Left = Left.Delete(value);
                    }
                    else
                    {
                        CountLeft--;
                        Left = Left.Delete(value);
                    }
                    
                }
                else
                {
                    if(Left!=null && CountLeft > CountRight)
                    {
                        var left = Left.PickAndDeleteLast();
                        var currentValue = Value;
                        Value = left.Item1;
                        Left = left.Item2;
                        CountLeft--;
                        Right = Right.add(currentValue);
                        Right = Right.Delete(value);
                    }
                    else
                    {
                        CountRight--;
                        Right = Right.Delete(value);
                    }
                    
                }
                #endregion
                return this;
            }
        }

        private Node root;

        public BalancedBinaryTree(int rootValue)
        {
            root = new Node { Value = rootValue };
        }

        public void Print()
        {
            root.print(0);
        }

        private static Node Add(Node r, int value)
        {
            if(r.Value > value)
            {
                if (r.CountRight < r.CountLeft)
                {
                    Tuple<int, Node> rTup = r.Left.PickAndDeleteLast();
                    r.Left = rTup.Item2;
                    r.CountLeft--;
                    var currentValue = r.Value;
                    if(value < rTup.Item1)
                    {
                        r.Value = rTup.Item1;
                        r = Add(r, currentValue);
                        return Add(r, value);
                    }
                    else
                    {
                        r.Value = value;
                        r = Add(r, currentValue);
                        return Add(r, rTup.Item1);
                    }
                    
                }

                if (r.Left == null)
                    r.Left = new Node { Value = value };
                else
                    r.Left = r.Left.add(value);
                r.CountLeft++;
            }
            else
            {
                if(r.CountRight > r.CountLeft)
                {
                    Tuple<int, Node> lTup = r.Right.PickAndDeleteFirst();
                    r.Right = lTup.Item2;
                    r.CountRight--;
                    var currentValue = r.Value;
                    if(value > lTup.Item1)
                    {
                        r.Value = lTup.Item1;
                        r = Add(r, currentValue);
                        return Add(r, value);
                    }
                    else
                    {
                        r.Value = value;
                        r = Add(r, currentValue);
                        return Add(r, lTup.Item1);
                    }
                    
                }

                if (r.Right == null)
                    r.Right = new Node { Value = value };
                else
                    r.Right = r.Right.add(value);
                r.CountRight++;
            }
            return r;
        }

        public void Add(int value)
        {
            if(!root.Exist(value))
                root = root.add(value);
            //otherwise just ignore for now
        }

        public bool Exist(int value) => root.Exist(value);

        public void Delete(int value)
        {
            if (root.Exist(value))
            {
                root = root.Delete(value);
            }
            //otherwise just ignore for now
        }

    }



    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            var t = new BalancedBinaryTree(0);
            for (int i = 63; i > 1; i--)
            {
                //t.Add(i);
                t.Add(rnd.Next(50));
                
            }
            t.Print();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 63; i > 1; i--)
            {
                //t.Add(i);
                t.Delete(rnd.Next(50));

            }
            t.Print();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();




            Console.Read();
        }
    }
}
