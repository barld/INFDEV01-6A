using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public interface IStack<T>
    {
        void Push(T item);
        T Peek();
        T Pop();
        bool IsEmpty { get; }
        bool IsFull { get; }
    }
    public class FixedStack<T> : IStack<T>
    {
        private readonly T[] arr;
        private readonly int size;
        private int currentIndex = -1;

        public bool IsEmpty => currentIndex == -1;
        public bool IsFull => currentIndex == size - 1;

        public FixedStack(int size)
        {
            arr = new T[size];
            this.size = size;
        }
        public T Peek() => arr[currentIndex];
        public T Pop() => arr[currentIndex--];
        /// <summary>
        /// could throw exeption if stack is full
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item) => arr[++currentIndex] = item;
    }
    public class DynamicStack<T> : IStack<T>
    {
        private ILinkedList<T> list = new EmptyList<T>();
        public bool IsEmpty => list is EmptyList<T>;
        public bool IsFull => false;

        public T Peek()
        {
            if (list is LinkedList<T>)
                return (list as LinkedList<T>).Head;
            else
                throw new FieldAccessException("stack is empty");
        }

        public T Pop()
        {
            if (list is LinkedList<T>)
            {
                T rValue = (list as LinkedList<T>).Head;
                list = (list as LinkedList<T>).Tail;
                return rValue;
            }
            else
                throw new FieldAccessException("stack is empty");
        }

        public void Push(T item) =>
            list = new LinkedList<T> { Head = item, Tail = list };
    }
}
