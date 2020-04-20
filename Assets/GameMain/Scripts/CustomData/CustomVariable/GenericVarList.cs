using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

namespace StarForce
{
    public abstract class VarList<T> : VarList, IList<T>
    {
        private List<T> m_Value;

        protected VarList()
        {
            m_Value = new List<T>();
        }

        protected VarList(List<T> value)
        {
            m_Value = value;
        }

        public override object GetValue()
        {
            return m_Value;
        }

        public override void SetValue(object value)
        {
            m_Value = (List<T>) value;
        }

        public override void Reset()
        {
            m_Value = default(List<T>);
        }

        public override Type Type
        {
            get { return m_Value.GetType(); }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            m_Value.Add(item);
        }

        public void Clear()
        {
            m_Value.Clear();
        }

        public bool Contains(T item)
        {
            return m_Value.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Value.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return m_Value.Remove(item);
        }

        public int Count
        {
            get { return m_Value.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int IndexOf(T item)
        {
            return m_Value.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_Value.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Value.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return m_Value[index]; }
            set { m_Value[index] = value; }
        }
    }
}