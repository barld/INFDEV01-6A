using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainApplication;

namespace TrainApplication
{

    public class BalancedBinaryTree<T, U> where T : IComparable
    {
        struct KeyValue
        {
            public T Key { get; set; }
            public U Value { get; set; }
        }

        class Node
        {
            internal KeyValue KeyValue { get; set; }

            internal int CountLeft { get; set; } = 0;
            internal int CountRight { get; set; } = 0;
            internal Node Left { get; set; }
            internal Node Right { get; set; }

            internal void print(int v)
            {
                Left?.print(v + 1);
                Console.WriteLine($"{new String('\t', v)} {KeyValue.Key}: {KeyValue.Value}");
                Right?.print(v + 1);
            }

            internal Node add(T key, U value)
            {
                return Add(this, key, value);
            }

            internal Tuple<KeyValue, Node> PickAndDeleteLast()
            {
                if (Right == null)
                    return new Tuple<KeyValue, Node>(KeyValue, Left);
                else
                {
                    var tup = this.Right.PickAndDeleteLast();
                    this.Right = tup.Item2;
                    this.CountRight--;
                    return new Tuple<KeyValue, Node>(tup.Item1, this);
                }
            }

            internal Tuple<KeyValue, Node> PickAndDeleteFirst()
            {
                if(Left==null)
                    return new Tuple<KeyValue, Node>(KeyValue, Right);
                else
                {
                    var tup = this.Left.PickAndDeleteFirst();
                    this.Left = tup.Item2;
                    this.CountLeft--;
                    return new Tuple<KeyValue, Node>(tup.Item1, this);
                }
            }

            internal bool Exist(T key)
            {

                if (KeyValue.Key.CompareTo(key) == 0)
                    return true;
                else if (KeyValue.Key.CompareTo(key) < 0 && Right != null)
                    return Right.Exist(key);
                return Left != null && Left.Exist(key);

            }

            internal Node Delete(T key)
            {
                if(KeyValue.Key.CompareTo(key) == 0)
                {
                    if (Left == null) return Right;
                    else if (Right == null) return Left;
                    else
                    {
                        if(CountLeft > CountRight)
                        {
                            var right = Right.PickAndDeleteFirst();
                            KeyValue = right.Item1;
                            Right = right.Item2;
                            CountRight--;
                        }
                        else
                        {
                            var left = Left.PickAndDeleteLast();
                            KeyValue = left.Item1;
                            Left = left.Item2;
                            CountLeft--;
                        }
                    }
                }
                //should togle shift for balancing
                #region shifting
                else if (KeyValue.Key.CompareTo(key) > 0)
                {
                    if (Right != null && CountRight > CountLeft)
                    {
                        var right = Right.PickAndDeleteFirst();
                        var currentValue = KeyValue;
                        KeyValue = right.Item1;
                        Right = right.Item2;
                        CountRight--;
                        Left = Left.add(currentValue.Key, currentValue.Value);
                        Left = Left.Delete(key);
                    }
                    else
                    {
                        CountLeft--;
                        Left = Left.Delete(key);
                    }
                    
                }
                else
                {
                    if(Left!=null && CountLeft > CountRight)
                    {
                        var left = Left.PickAndDeleteLast();
                        var currentValue = KeyValue;
                        KeyValue = left.Item1;
                        Left = left.Item2;
                        CountLeft--;
                        Right = Right.add(currentValue.Key, currentValue.Value);
                        Right = Right.Delete(key);
                    }
                    else
                    {
                        CountRight--;
                        Right = Right.Delete(key);
                    }
                    
                }
                #endregion
                return this;
            }
        }

        private Node root;

        public BalancedBinaryTree(T key, U value)
        {
            root = new Node { KeyValue = new KeyValue { Key = key, Value = value } };
        }

        public void Print()
        {
            root.print(0);
        }

        private static Node Add(Node r, T key, U value)
        {
            if(r.KeyValue.Key.CompareTo(key) > 0)
            {
                if (r.CountRight < r.CountLeft)
                {
                    Tuple<KeyValue, Node> rTup = r.Left.PickAndDeleteLast();
                    r.Left = rTup.Item2;
                    r.CountLeft--;
                    var currentValue = r.KeyValue;
                    if(key.CompareTo(rTup.Item1.Key) < 0)
                    {
                        r.KeyValue = rTup.Item1;
                        r = Add(r, currentValue.Key, currentValue.Value);
                        return Add(r, key, value);
                    }
                    else
                    {
                        r.KeyValue = new KeyValue { Key = key, Value = value };
                        r = Add(r, currentValue.Key, currentValue.Value);
                        return Add(r, rTup.Item1.Key, rTup.Item1.Value);
                    }
                    
                }

                if (r.Left == null)
                    r.Left = new Node { KeyValue = new KeyValue { Key = key, Value = value } };
                else
                    r.Left = r.Left.add(key, value);
                r.CountLeft++;
            }
            else
            {
                if(r.CountRight > r.CountLeft)
                {
                    Tuple<KeyValue, Node> lTup = r.Right.PickAndDeleteFirst();
                    r.Right = lTup.Item2;
                    r.CountRight--;
                    var currentValue = r.KeyValue;
                    if(key.CompareTo(lTup.Item1.Key) > 0)
                    {
                        r.KeyValue = lTup.Item1;
                        r = Add(r, currentValue.Key, currentValue.Value);
                        return Add(r, key, value);
                    }
                    else
                    {
                        r.KeyValue = new KeyValue { Key = key, Value = value };
                        r = Add(r, currentValue.Key, currentValue.Value);
                        return Add(r, lTup.Item1.Key, lTup.Item1.Value);
                    }
                    
                }

                if (r.Right == null)
                    r.Right = new Node { KeyValue = new KeyValue { Key = key, Value = value } };
                else
                    r.Right = r.Right.add(key, value);
                r.CountRight++;
            }
            return r;
        }

        public void Add(T key, U value)
        {
            if(!root.Exist(key))
                root = root.add(key, value);
            //otherwise just ignore for now
        }

        public bool Exist(T key) => root.Exist(key);

        public void Delete(T key)
        {
            if (root.Exist(key))
            {
                root = root.Delete(key);
            }
            //otherwise just ignore for now
        }

    }



    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            var t = new BalancedBinaryTree<int,string>(0, "name");
            for (int i = 200; i > 1; i--)
            {
                //t.Add(i);
                t.Add(rnd.Next(10000), RandomNameGenerator.NameGenerator.GenerateFirstName(RandomNameGenerator.Gender.Male));
                
            }
            t.Print();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //for (int i = 63; i > 1; i--)
            //{
            //    //t.Add(i);
            //    t.Delete(rnd.Next(50));

            //}
            //t.Print();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();




            Console.Read();
        }
    }
}
