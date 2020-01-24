using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T> : IList<T>, ICloneable where T : IComparable<T>
{
    #region PrivateParameters
    private List<T> FList;
    private IComparer<T> FComparer = null;
    private bool FUseObjectsComparison = true;
    private int size;

    public int Count => FList.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    public T this[int index] { get => FList[index]; set => throw new NotSupportedException("Can't set elements with [] in heap"); }
    #endregion

    #region Public Fields

    #endregion


    #region Node Access Shortcuts
    private int GetParentIndex(int index) => (index - 1) / 2;
    private int GetLeftChildIndex(int index) => (2 * index) + 1;
    private int GetRightChildIndex(int index) => (2 * index) + 2;

    private bool IsMin(int index) => index == 0;
    private bool HasLeftChild(int index) => GetLeftChildIndex(index) > Count - 1;//Might make use of a size variable to make this neater.
    private bool HasRightChild(int index) => GetRightChildIndex(index) > Count - 1;

    private T GetParent(int atIndex) => FList[GetParentIndex(atIndex)];
    #endregion


    #region Inherited Members
    public int IndexOf(T item)
    {
        if (FList.Contains(item))
        {
            return FList.BinarySearch(item);
        }
        else return -1;
    }

    public void Insert(int index, T item)
    {
        throw new NotSupportedException("Heap does not support inserting");
    }

    public void RemoveAt(int index)
    {
        FList.RemoveAt(index);
    }

    public void Add(T item)
    {
        throw new NotImplementedException();//TODO: Implement this.
    }

    public void Clear()
    {
        FList.Clear();
    }

    public bool Contains(T item)
    {
        return FList.BinarySearch(item) >= 0;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        FList.CopyTo(array);
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();//TODO: Implement this.
    }

    public IEnumerator<T> GetEnumerator()
    {
        return FList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();//TODO: Dunno what to do with this actually.
    }

    public object Clone()
    {
        throw new NotImplementedException();//TODO: Implement this.
    } 
    #endregion
}
