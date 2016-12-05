using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public interface IQueue<T>
    {
        void EnQueue(T item);
        T Peek();
        T Dequeue();
        bool IsEmpty { get; }
        bool IsFull { get; }

    }
    public class FixedQueue<T> : IQueue<T>
    {
        int firstIndex = -1;
        int lastIndex = -1;
        readonly int size;
        readonly T[] arr;

        public bool IsEmpty { get; private set; } = true;
        public bool IsFull => firstIndex == (lastIndex + 1) % size && !IsEmpty;

        public FixedQueue(int size)
        {
            this.size = size;
            arr = new T[size];
        }

        public T Dequeue()
        {
            if (IsEmpty) throw new Exception("Empty Queue");
            var rvalue = arr[firstIndex];
            if (firstIndex == lastIndex) IsEmpty = true;
            firstIndex = (firstIndex + 1) % size;
            return rvalue;
        }

        public void EnQueue(T item)
        {
            if (IsFull) throw new Exception("Queue is already full");
            lastIndex = (lastIndex + 1) % size;
            arr[lastIndex] = item;
            IsEmpty = false;
            if (firstIndex == -1) firstIndex = 0;
        }

        public T Peek()
        {
            return arr[firstIndex];
        }
    }
    public class DynamicQueue<T> : IQueue<T>
    {
        ILinkedList<T> start = new EmptyList<T>();
        ILinkedList<T> end = new EmptyList<T>();
        public bool IsEmpty => start is EmptyList<T>;
        public bool IsFull => false;

        public T Dequeue()
        {
            var rvalue = (start as LinkedList<T>).Head;
            start = (start as LinkedList<T>).Tail;
            return rvalue;
        }

        public void EnQueue(T item)
        {
            //special case for empty queue
            if (IsEmpty)
            {
                end = new LinkedList<T> { Head = item, Tail = end };
                start = end;
            }
            else
            {
                (end as LinkedList<T>).Tail = new LinkedList<T> { Head = item, Tail = (end as LinkedList<T>).Tail };
                end = (end as LinkedList<T>).Tail;
            }
        }

        public T Peek()
        {
            if (!IsEmpty)
                return (start as LinkedList<T>).Head;
            else
                throw new Exception("Queue is empty");
        }
    }
}
