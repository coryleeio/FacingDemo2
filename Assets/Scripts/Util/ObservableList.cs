using System;
using System.Collections;
using System.Collections.Generic;

namespace Gamepackage
{

    public class ObservableList<T> : IList<T>
    {
        protected List<T> InternalList = new List<T>();

        public event Action OnChange;

        public IEnumerator<T> GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return InternalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            InternalList.Insert(index, item);

            if (OnChange != null)
                OnChange();
        }

        public void RemoveAt(int index)
        {
            InternalList.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return InternalList[index]; }
            set { InternalList[index] = value; }
        }

        public void Add(T item)
        {
            InternalList.Add(item);

            if (OnChange != null)
                OnChange();
        }

        public void Clear()
        {
            InternalList.Clear();

            if (OnChange != null)
                OnChange();
        }

        public bool Contains(T item)
        {
            return InternalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            InternalList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InternalList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            if (InternalList.Remove(item))
            {
                if (OnChange != null)
                    OnChange();

                return true;
            }

            return false;

        }
    }
}

