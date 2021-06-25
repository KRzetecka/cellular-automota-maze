using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CAWindowsForms
{
    /*public class SortedMultiSet
    {
        private SortedDictionary<Tuple<int,int>, int> _dict;

        public SortedMultiSet()
        {
            _dict = new SortedDictionary<Tuple<int, int>, int>(new djikstraSortComparer());
        }

        public SortedMultiSet(Tuple<int, int> items) : this()
        {
            Add(items);
        }

        public bool isEmpty()
        {
            if (_dict.Count == 0) return true;
            return false;
        }

        public bool Contains(Tuple<int, int> item)
        {
            return _dict.ContainsKey(item);
        }

        public void Add(Tuple<int, int> item)
        {
            if (_dict.ContainsKey(item))
                _dict[item]++;
            else
                _dict[item] = 1;

        }

        public void Remove(Tuple<int, int> item)
        {
            if (!_dict.ContainsKey(item))
                throw new ArgumentException();
            if (--_dict[item] == 0)
                _dict.Remove(item);
        }

        // Return the last value in theSortedMultiSet
        public Tuple<int, int> Peek()
        {
            if (!_dict.Any())
                throw new NullReferenceException();
            return _dict.Last().Key;
        }

        // Return the last value in theSortedMultiSet and remove it.
        public Tuple<int, int> Pop()
        {
            Tuple<int, int> item = Peek();
            Remove(item);
            return item;
        }
    }*/
    //=================================================
    /*
    public class SortedMultiSet<T> : IEnumerable<T>
    {
        private SortedDictionary<T, int> _dict;

        public SortedMultiSet()
        {
            _dict = new SortedDictionary<T, int>();
            
        }

        public SortedMultiSet(IEnumerable<T> items) : this()
        {
            Add(items);
        }
        
        public bool isEmpty()
        {
            if (_dict.Count == 0) return true;
            return false;
        }

        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        public void Add(T item)
        {
            if (_dict.ContainsKey(item))
                _dict[item]++;
            else
                _dict[item] = 1;

        }

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Remove(T item)
        {
            if (!_dict.ContainsKey(item))
                throw new ArgumentException();
            if (--_dict[item] == 0)
                _dict.Remove(item);
        }

        // Return the last value in theSortedMultiSet
        public T Peek()
        {
            if (!_dict.Any())
                throw new NullReferenceException();
            return _dict.Last().Key;
        }

        // Return the last value in theSortedMultiSet and remove it.
        public T Pop()
        {
            T item = Peek();
            Remove(item);
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var kvp in _dict)
                for (int i = 0; i < kvp.Value; i++)
                    yield return kvp.Key;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class djikstraSortComparer : IComparer<Tuple<int, int>>
    {
        public int Compare(Tuple<int, int> x, Tuple<int, int> y)
        {
            int left = x.Item1;
            int right = y.Item1;

            if (left < right) return -1;
            else return 1;
        }
    }*/
    //================================================================
    /*
    public class SortedMultiSet<T> : ICollection<T>
    {
        private readonly Dictionary<T, int> data;

        public SortedMultiSet()
        {
            data = new Dictionary<T, int>();
        }

        private SortedMultiSet(Dictionary<T, int> data)
        {
            this.data = data;
        }

        public void Add(T item)
        {
            int count = 0;
            data.TryGetValue(item, out count);
            count++;
            data[item] = count;
        }

        public void Clear()
        {
            data.Clear();
        }

        public SortedMultiSet<T> Except(SortedMultiSet<T> another)
        {
            SortedMultiSet<T> copy = new SortedMultiSet<T>(new Dictionary<T, int>(data));
            foreach (KeyValuePair<T, int> kvp in another.data)
            {
                int count;
                if (copy.data.TryGetValue(kvp.Key, out count))
                {
                    if (count > kvp.Value)
                    {
                        copy.data[kvp.Key] = count - kvp.Value;
                    }
                    else
                    {
                        copy.data.Remove(kvp.Key);
                    }
                }
            }
            return copy;
        }

        public SortedMultiSet<T> Intersection(SortedMultiSet<T> another)
        {
            Dictionary<T, int> newData = new Dictionary<T, int>();
            foreach (T t in data.Keys.Intersect(another.data.Keys))
            {
                newData[t] = Math.Min(data[t], another.data[t]);
            }
            return new SortedMultiSet<T>(newData);
        }

        public bool Contains(T item)
        {
            return data.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (KeyValuePair<T, int> kvp in data)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    array[arrayIndex] = kvp.Key;
                    arrayIndex++;
                }
            }
        }

        public IEnumerable<T> Mode()
        {
            if (!data.Any())
            {
                return Enumerable.Empty<T>();
            }
            int modalFrequency = data.Values.Max();
            return data.Where(kvp => kvp.Value == modalFrequency).Select(kvp => kvp.Key);
        }

        public int Count
        {
            get
            {
                return data.Values.Sum();
            }
        }
        public bool isEmpty()
        {
            if (data.Count == 0) return true;
            return false;
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            int count;
            if (!data.TryGetValue(item, out count))
            {
                return false;
            }
            count--;
            if (count == 0)
            {
                data.Remove(item);
            }
            else
            {
                data[item] = count;
            }
            return true;
        }
        public T Pop()
        {
            T item = Peek();
            Remove(item);
            return item;
        }
        public T Peek()
        {
            if (!data.Any())
                throw new NullReferenceException();
            return data.Last().Key;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return newSortedMultiSetEnumerator<T>(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return newSortedMultiSetEnumerator<T>(this);
        }

        private classSortedMultiSetEnumerator<T> : IEnumerator<T>
        {
            publicSortedMultiSetEnumerator(SortedMultiSet<T>SortedMultiSet)
            {
                this.multiset =SortedMultiSet;
                baseEnumerator =SortedMultiSet.data.GetEnumerator();
                index = 0;
            }

            private readonly SortedMultiSet<T>SortedMultiSet;
            private readonly IEnumerator<KeyValuePair<T, int>> baseEnumerator;
            private int index;

            public T Current
            {
                get
                {
                    return baseEnumerator.Current.Key;
                }
            }

            public void Dispose()
            {
                baseEnumerator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return baseEnumerator.Current.Key;
                }
            }

            public bool MoveNext()
            {
                KeyValuePair<T, int> kvp = baseEnumerator.Current;
                if (index < (kvp.Value - 1))
                {
                    index++;
                    return true;
                }
                else
                {
                    bool result = baseEnumerator.MoveNext();
                    index = 0;
                    return result;
                }
            }

            public void Reset()
            {
                baseEnumerator.Reset();
            }
        }
    }
    */
    //==================================================
    public class SortedMultiSet<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        //Constructors
        public SortedMultiSet() : this((IComparer<T>)null) { }
        public SortedMultiSet(IComparer<T> comparer)
        {
            _itemComparer = new ItemComparer(comparer);
            _set = new SortedSet<Item>(_itemComparer);
            _count = 0;
            _version = 0;
        }
        public SortedMultiSet(IEnumerable<T> collection) : this(collection, (IComparer<T>)null) { }
        public SortedMultiSet(IEnumerable<T> collection, IComparer<T> comparer)
            : this(comparer)
        {
            foreach (T item in collection)
            {
                Add(item);
            }
        }
        public SortedMultiSet(SortedMultiSet<T> other)
        {
            _itemComparer = other._itemComparer;
            _set = new SortedSet<Item>(other._set, _itemComparer);
            _count = other._count;
            _version = 0;
        }

        //IEnumerable
        public Enumerator GetEnumerator()
        {
            return new SortedMultiSet<T>.Enumerator(this);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new SortedMultiSet<T>.Enumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SortedMultiSet<T>.Enumerator(this);
        }

        //ICollection
        public void Add(T item)
        {
            _version++;
            _count++;

            Item x = Find(item);

            if (x == null)
            {
                _set.Add(new Item(item));
            }
            else
            {
                x.Value++;
            }
        }

        public void Clear()
        {
            _version++;
            _count = 0;
            _set.Clear();
        }

        public bool Contains(T item)
        {
            return _set.Contains(new Item(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _count; }
        }
        public bool isEmpty()
        {
            if (_set.Count == 0) return true;
            return false;
        }
        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            Item x = Find(item);

            if (x != null)
            {
                _version++;
                _count--;

                if (x.Value < 2)
                {
                    _set.Remove(x);
                }
                else
                {
                    x.Value--;
                }

                return true;
            }

            return false;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        //new

        public T Min
        {
            get
            {
                Item item = _set.Min;
                return item != null ? item.Key : default(T);
            }
        }

        public T Max
        {
            get
            {
                Item item = _set.Max;
                return item != null ? item.Key : default(T);
            }
        }

        public int CountSingle(T item)
        {
            var found = Find(item);
            return found != null ? found.Value : 0;
        }

        public bool RemoveAll(T item)
        {
            Item x = Find(item);

            if (x != null)
            {
                _version++;
                _count -= x.Value;
                _set.Remove(x);

                return true;
            }

            return false;
        }

        public IEnumerable<T> Reverse()
        {
            foreach (Item item in _set.Reverse())
            {
                for (int i = 0; i < item.Value; i++)
                {
                    yield return item.Key;
                }
            }
        }

        public IEnumerable<T> SubSet(T lower, T upper)
        {
            var lowerItem = new Item(lower);
            var upperItem = new Item(upper);
            if (_itemComparer.Compare(lowerItem, upperItem) <= 0)
            {
                foreach (Item item in _set.GetViewBetween(new Item(lower), new Item(upper)))
                {
                    for (int i = 0; i < item.Value; i++)
                    {
                        yield return item.Key;
                    }
                }
            }
        }

        public int SubSetCount(T lower, T upper)
        {
            int ret = 0;
            var lowerItem = new Item(lower);
            var upperItem = new Item(upper);
            if (_itemComparer.Compare(lowerItem, upperItem) <= 0)
            {
                ret = _set.GetViewBetween(lowerItem, upperItem).Count;
            }

            return ret;
        }

        private Item Find(T key)
        {
            var item = new Item(key);
            var view = _set.GetViewBetween(item, item);
            return view.FirstOrDefault();
        }

        private SortedSet<Item> _set;
        private ItemComparer _itemComparer;
        private int _count = 0;
        private int _version = 0;
        private object _syncRoot;



        private class Item
        {
            public Item(T key)
            {
                Key = key;
                Value = 1;
            }
            public T Key;
            public int Value;
        }

        private class ItemComparer : IComparer<Item>
        {
            public ItemComparer(IComparer<T> comparer)
            {
                _comparer = comparer;
                if (_comparer == null)
                {
                    _comparer = Comparer<T>.Default;
                }
            }

            public int Compare(Item x, Item y)
            {
                return _comparer.Compare(x.Key, y.Key);
            }

            private IComparer<T> _comparer;
        }

        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            public Enumerator(SortedMultiSet<T>SortedMultiSet)
            {
                _multiSet =SortedMultiSet;
                _enumerator = _multiSet._set.GetEnumerator();
                _currentCount = 0;
                _version = _multiSet._version;
            }

            public T Current
            {
                get
                {
                    CheckVersion();
                    return _enumerator.Current != null ? _enumerator.Current.Key : default(T);
                }
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                CheckVersion();
                Item item = _enumerator.Current;
                if (item != null && _currentCount < item.Value - 1)
                {
                    _currentCount++;
                    return true;
                }

                _currentCount = 0;
                return _enumerator.MoveNext();
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Reset()
            {
                CheckVersion();
                ((IEnumerator)_enumerator).Reset();
                _currentCount = 0;
            }

            private void CheckVersion()
            {
                if (_version != _multiSet._version)
                {
                    throw new InvalidOperationException("");
                }
            }

            private SortedMultiSet<T> _multiSet;
            private SortedSet<Item>.Enumerator _enumerator;
            private int _currentCount;
            private int _version;
        }
    }
}
