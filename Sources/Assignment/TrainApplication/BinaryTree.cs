using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public class BinaryTree<key, value> where key: IComparable
    {
        class Node
        {
            internal key Key { get; set; }
            internal value Value { get; set; }
            internal Node Left { get; set; }
            internal Node Right { get; set; }
        }

        private Node root;

        public value Search(key key)
        {
            return search(key, this.root);
        }

        public void Add(key key, value value)
        {
            root = add(key, value, root);
        }

        private Node add(key key, value value, Node root)
        {
            if (root == null) return new Node { Key = key, Value = value };
            if (root.Key.CompareTo(key) < 0)
            {
                root.Left = add(key, value, root.Left);
                return root;
            }
            else
            {
                root.Right = add(key, value, root.Right);
                return root;
            }
        }

        private value search(key key, Node root)
        {
            if (root == null) throw new ArgumentOutOfRangeException();
            if (root.Key.CompareTo(key) == 0) return root.Value;
            if (root.Key.CompareTo(key) < 0) return search(key, root.Right);
            else return search(key, root.Left);
        }
    }
}
