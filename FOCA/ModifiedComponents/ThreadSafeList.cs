using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOCA.ModifiedComponents
{
    /// <summary>
    /// Esta lista por un lado hace que las operaciones de Add, Remove, Contains
    /// y acceso a elementos sean ThreadSafe.
    /// Por otro lado proporciona una funcion GetEnumerator que devuelve un enumerador
    /// de una copia de la lista de este modo se evita que surgan excepciones cuando
    /// se modifica la lista mientras se esta recorriendo con foreach.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed class ThreadSafeList<T> : IDisposable, IList<T>
    {
        [NonSerialized]
        private List<T> m_Inner;

        [NonSerialized]
        private ReaderWriterLockSlim m_Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public ThreadSafeList()
        {
            m_Inner = new List<T>();
        }

        public ThreadSafeList(IEnumerable<T> collection)
        {
            m_Inner = new List<T>(collection);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            // instead of returning an usafe enumerator,
            // we wrap it into our thread-safe class
            return new SafeEnumerator<T>(ref m_Inner, m_Lock);
        }

        // To be actually thread-safe, our collection
        // must be locked on all other operations
        // For example, this is how Add() method should look
        public void Add(T item)
        {
            while (m_Lock.RecursiveReadCount > 0)
            {
                Thread.Sleep(200);
                if (m_Lock.RecursiveReadCount > 0) m_Lock.ExitReadLock();
            }

            m_Lock.EnterWriteLock();

            try
            {
                m_Inner.Add(item);
            }
            finally
            {
                m_Lock.ExitWriteLock();
            }
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            m_Lock.EnterReadLock();
            try
            {
                return m_Inner.IndexOf(item);
            }
            finally
            {
                m_Lock.ExitReadLock();
            }
        }

        public void Insert(int index, T item)
        {
            while (m_Lock.RecursiveReadCount > 0)
            {
                Thread.Sleep(200);
                if (m_Lock.RecursiveReadCount > 0) m_Lock.ExitReadLock();
            }

            m_Lock.EnterWriteLock();
            try
            {
                m_Inner.Insert(index, item);
            }
            finally
            {
                m_Lock.ExitWriteLock();
            }
        }

        public void RemoveAt(int index)
        {
            while (m_Lock.RecursiveReadCount > 0)
            {
                Thread.Sleep(200);
                if (m_Lock.RecursiveReadCount > 0) m_Lock.ExitReadLock();
            }

            m_Lock.EnterWriteLock();
            try
            {
                m_Inner.RemoveAt(index);
            }
            finally
            {
                m_Lock.ExitWriteLock();
            }
        }

        public T this[int index]
        {
            get
            {
                m_Lock.EnterReadLock();
                try
                {
                    return m_Inner[index];
                }
                finally
                {
                    m_Lock.ExitReadLock();
                }
            }
            set
            {
                m_Lock.EnterWriteLock();
                try
                {
                    m_Inner[index] = value;
                }
                finally
                {
                    m_Lock.ExitWriteLock();
                }
            }
        }

        #endregion

        #region ICollection<T> Members


        public void Clear()
        {
            m_Lock.EnterWriteLock();
            try
            {
                m_Inner.Clear();
            }
            finally
            {
                m_Lock.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            m_Lock.EnterReadLock();
            try
            {
                return m_Inner.Contains(item);
            }
            finally
            {
                m_Lock.ExitReadLock();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Lock.EnterReadLock();
            try
            {
                m_Inner.CopyTo(array, arrayIndex);
            }
            finally
            {
                m_Lock.ExitReadLock();
            }
        }

        public int Count
        {
            get
            {
                m_Lock.EnterReadLock();
                try
                {
                    return m_Inner.Count;
                }
                finally
                {
                    m_Lock.ExitReadLock();
                }
            }
        }

        public bool Remove(T item)
        {
            while (m_Lock.RecursiveReadCount > 0)
            {
                Thread.Sleep(200);
                if (m_Lock.RecursiveReadCount > 0) m_Lock.ExitReadLock();
            }

            m_Lock.EnterWriteLock();
            try
            {
                return m_Inner.Remove(item);
            }
            finally
            {
                m_Lock.ExitWriteLock();
            }
        }

        #endregion

        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SafeEnumerator<T>(ref m_Inner, m_Lock);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool dispose)
        {
            m_Lock.Dispose();
        }
        #endregion

        #region ICollection<T> Members


        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion
    }

    [Serializable]
    public sealed class SafeEnumerator<T> : IEnumerator<T>, IDisposable
    {
        // this is the (thread-unsafe)
        // enumerator of the underlying collection
        private readonly IEnumerator<T> m_Inner;

        // this is the object we shall lock on.
        [NonSerialized]
        private readonly ReaderWriterLockSlim m_Lock;
        private bool AlreadyDisposed = false;
        [NonSerialized]
        private Thread CreationThread;

        public SafeEnumerator(ref List<T> lst, ReaderWriterLockSlim m_Lock)
        {
            CreationThread = Thread.CurrentThread;
            this.m_Lock = m_Lock;
            m_Lock.EnterReadLock();
            m_Inner = lst.GetEnumerator();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (!AlreadyDisposed)
            {
                try
                {
                    m_Lock.ExitReadLock();
                }
                catch { }
                AlreadyDisposed = true;
            }
        }
        #endregion

        //Finalizer para asegurar la ejecución del Dispose
        ~SafeEnumerator()
        {
            Dispose();
        }

        #region Implementation of IEnumerator

        // we just delegate actual implementation
        // to the inner enumerator, that actually iterates
        // over some collection

        public bool MoveNext()
        {

            bool b = m_Inner.MoveNext();
            //Cuando llega al final de la enumeración llama a dispose
            if (!b)
                Dispose();
            return b;

        }

        public void Reset()
        {
            m_Inner.Reset();
        }

        public T Current
        {
            get { return m_Inner.Current; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion
    }
}
