using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel
{
    /// <summary>
    /// The collection class for DataNode
    /// </summary>
    [Serializable]
    public sealed class DataNodeCollection : ICollection, IEnumerable
    {
        private List<DataNode> _values;
        private Dictionary<Guid, DataNode> _idsValues;
        private Dictionary<string, DataNode> _namesValues;
        private DataNode _owner;

        #region implements ICollection
        /// <summary>
        /// Gets the count of the objects in the collection
        /// </summary>
        public int Count
        {
            get { return _values.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the System.Collections.ICollection
        //     is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        ///  Gets an object that can be used to synchronize access to the System.Collections.ICollection.
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// Copy objects to array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < _values.Count; i++)
            {
                array.SetValue(_values[i], i + index);
            }
        }
        #endregion

        #region implements IEnumerator
        /// <summary>
        /// Gets the IEnumerator object
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }
        #endregion

        public DataNodeCollection(DataNode owner)
        {
            _values = new List<DataNode>();
            _idsValues = new Dictionary<Guid, DataNode>();
            _namesValues = new Dictionary<string, DataNode>();
            _owner = owner;
        }

        public DataNodeCollection(DataNode owner, DataNode[] nodes)
            : this(owner)
        {
            if (nodes == null)
                return;
            foreach (DataNode node in nodes)
            {
                node.ParentNode = _owner;
                _values.Add(node);
                _idsValues.Add(node.Id, node);
                _namesValues.Add(node.Name.ToLower(), node);
            }
        }

        public DataNodeCollection(DataNode owner, DataNodeCollection nodes)
            : this(owner)
        {
            foreach (DataNode node in nodes)
            {
                node.ParentNode = _owner;
                _values.Add(node);
                _idsValues.Add(node.Id, node);
                _namesValues.Add(node.Name.ToLower(), node);
            }
        }

        public void CopyTo(DataNode[] array, int index)
        {
            this._values.CopyTo(array, index);
        }

        public void Add(DataNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            node.ParentNode = _owner;
            _values.Add(node);
            _idsValues.Add(node.Id, node);
            _namesValues.Add(node.Name.ToLower(), node);
        }

        public void Clear()
        {
            this._values.Clear();
            this._idsValues.Clear();
            this._namesValues.Clear();
        }

        public void Remove(Guid id)
        {
            DataNode node = null;
            if (_idsValues.TryGetValue(id, out node))
            {
                _idsValues.Remove(id);
                _namesValues.Remove(node.Name.ToLower());
                _values.Remove(node);
            }
        }

        public void Remove(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            DataNode node = null;
            if (_namesValues.TryGetValue(name.ToLower(), out node))
            {
                _idsValues.Remove(node.Id);
                _namesValues.Remove(name.ToLower());
                _values.Remove(node);
            }
        }

        public DataNode this[string name]
        {
            get 
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                DataNode node = null;
                _namesValues.TryGetValue(name.ToLower(), out node);
                return node;
            }
        }

        public DataNode this[Guid id]
        {
            get 
            {
                DataNode node = null;
                _idsValues.TryGetValue(id, out node);
                return node;
            }
        }

        public DataNode this[int index]
        {
            get 
            {
                if (index < 0 || index >= _values.Count)
                    return null;
                else
                    return _values[index];
            }
        }

        public bool Contains(Guid id)
        {
            DataNode value = null;
            return _idsValues.TryGetValue(id, out value);
        }

        public bool Contains(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            DataNode value = null;
            return _namesValues.TryGetValue(name.ToLower(), out value);
        }
    }
}
