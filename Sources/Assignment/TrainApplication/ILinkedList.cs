using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrainApplication
{
    public static class LinkedListExtensions
    {
        public static T TryFind<T>(this ILinkedList<T> l, Predicate<T> p)
        {
            while(!(l is EmptyList<T>))
            {
                if (p((l as LinkedList<T>).Head))
                    return (l as LinkedList<T>).Head;
                l = (l as LinkedList<T>).Tail;
            }
            return default(T);
        }
    }

    public interface ILinkedList<T> { }
    
    public class LinkedList<T> : ILinkedList<T>
    {
        public T Head { get; set; }
        public ILinkedList<T> Tail { get; set; }
    }
    public class EmptyList<T> : ILinkedList<T>
    {

    }
}
