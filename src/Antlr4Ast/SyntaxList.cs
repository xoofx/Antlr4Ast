// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.
/*
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Antlr4Ast;

[DebuggerTypeProxy(typeof(SyntaxList<>.DebugListView)), DebuggerDisplay("Count = {Count}")]
public abstract class SyntaxList<T>: SyntaxNode, IList<T> where T : SyntaxNode
{
    private readonly List<T> _elements;

    public SyntaxList()
    {
        _elements = new List<T>();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_elements).GetEnumerator();
    }

    public void Add(T item)
    {
        _elements.Add(item);
    }

    public void Clear()
    {
        _elements.Clear();
    }

    public bool Contains(T item)
    {
        return _elements.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _elements.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _elements.Remove(item);
    }

    public int Count => _elements.Count;

    bool ICollection<T>.IsReadOnly => false;

    public int IndexOf(T item)
    {
        return _elements.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _elements.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _elements.RemoveAt(index);
    }

    public T this[int index]
    {
        get => _elements[index];
        set => _elements[index] = value;
    }

    private class DebugListView
    {
        private readonly SyntaxList<T> _collection;

        public DebugListView(SyntaxList<T> collection)
        {
            this._collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return _collection.ToArray();
            }
        }
    }
}
*/