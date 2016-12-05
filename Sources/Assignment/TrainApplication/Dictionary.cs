using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public class DictionaryPair<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }

    public class Dictionary<T>
    {
        ILinkedList<DictionaryPair<T>>[] arr;
        readonly int size;

        public Dictionary(int size)
        {
            this.size = size;
            arr = new ILinkedList<DictionaryPair<T>>[size];
            var empty = new EmptyList<DictionaryPair<T>>();
            for (int i = 0; i < size; i++)
            {
                arr[i] = empty;
            }
        }

        public T this[string key]
        {
            get
            {
                int indexll = Math.Abs(key.GetHashCode() % size);
                var pair = arr[indexll].TryFind(p => p.Key == key);
                if (pair == null) throw new KeyNotFoundException();
                return pair.Value;
            }
            set
            {
                int indexll = Math.Abs(key.GetHashCode() % size);
                var pair = arr[indexll].TryFind(p => p.Key == key);
                if (pair == null) throw new KeyNotFoundException();
                pair.Value = value;

            }
        }

        public void Add(string key, T item)
        {
            int indexll = Math.Abs(key.GetHashCode() % size);
            var pair = arr[indexll].TryFind(p => p.Key == key);
            if (pair != null) throw new Exception($"key already in use key: {key}");

            arr[indexll] = new LinkedList<DictionaryPair<T>>
            {
                Head = new DictionaryPair<T> { Key = key, Value = item },
                Tail = arr[indexll]
            };
        }

        public bool KeyExists(string key)
        {
            int indexll = Math.Abs(key.GetHashCode() % size);
            return null != arr[indexll].TryFind(p => p.Key == key);
        }
    }
}
