using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T> : IList<T>, ICloneable where T : IComparable
{
    #region PrivateParameters
    private List<T> FList;

    public int Count => FList.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    public T this[int index] { get => FList[index]; set => throw new NotSupportedException("Can't set elements with [] in heap"); }

    public T min => FList[0];
    #endregion

    #region Public Fields

    #endregion


    #region Node Access Shortcuts
    private int GetParentIndex(int index) => (index - 1) / 2;
    private int GetLeftChildIndex(int index) => (2 * index) + 1;
    private int GetRightChildIndex(int index) => (2 * index) + 2;

    private bool IsMin(int index) => index == 0;
    private bool HasLeftChild(int index) => GetLeftChildIndex(index) < Count - 1;//Might make use of a size variable to make this neater.
    private bool HasRightChild(int index) => GetRightChildIndex(index) < Count - 1;

    private T GetParent(int atIndex) => FList[GetParentIndex(atIndex)];
    private T GetLeftChild(int atIndex) => FList[GetLeftChildIndex(atIndex)];
    private T GetRightChild(int atIndex) => FList[GetRightChildIndex(atIndex)];
    #endregion

    #region Constructors
    public MinHeap()
    {
        FList = new List<T>();
    }

    public MinHeap(T[] constructingArray)
    {
        FList = new List<T>(constructingArray);
        Heapify();
    }

    public MinHeap(List<T> constructingList)
    {
        FList = constructingList;
        Heapify();
    }
    #endregion

    #region Inherited Members
    public int IndexOf(T item)
    {
        int index = FList.BinarySearch(item);
        if (index >= 0)
        {
            return index;
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
        Heapify();
    }

    public void Add(T item)
    {
        FList.Add(item);
        Heapify(FList.Count - 1);
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
        //Start by verifying that the element is in the list.
        int index = FList.BinarySearch(item);
        if (index >= 0)
        {
            Swap(index, FList.Count - 1);
            FList.RemoveAt(FList.Count - 1);//This operation is O(n)
            ///resort list
            Heapify(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return FList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return FList.GetEnumerator();
    }

    public object Clone()
    {
        MinHeap<T> clone = new MinHeap<T>(FList);
        return clone;
    }
    #endregion

    #region Public Methods
    

    public T RemoveMin()
    {
        if (Count > 0)
        {
            T min = FList[0];
            Swap(0, Count - 1);
            FList.RemoveAt(Count - 1);
            FilterDown(0);
            return min; 
        }
        return default;
    }
    
    public List<T> ToList()
    {
        return FList;
    }

    public T[] ToArray()
    {
        return FList.ToArray();
    }

    public override string ToString()
    {
        String sb = "{";
        foreach(T item in FList)
        {
            sb += "\n { " + item.ToString() + "}";
        }

        return sb;
    }
    #endregion

    #region Private Members
    /// <summary>
    /// This method re-sorts the heap.
    /// </summary>
    private void Heapify()
    {
        for (int i = 0; i < Count; i++)
        {
            if (FList[i].CompareTo(GetParent(i)) < 0)
            {
                FilterUp(i);
            }
        }
    }

    /// <summary>
    /// This method re-sorts the sub-tree from the given index.
    /// </summary>
    /// <param name="index"></param>
    private void Heapify(int index)
    {
        //If the parent is greater, filter it up.
        if (GetParent(index).CompareTo(FList[index]) > 0)
        {
            FilterUp(index);
        }
        else
        {
            FilterDown(index);
        }
    }

    /// <summary>
    /// Just a helper method that swaps two indices in the array. 
    /// </summary>
    /// <param name="in1"></param>
    /// <param name="in2"></param>
    private void Swap(int startPos, int endPos)
    {
        T tmp = FList[startPos];
        FList[startPos] = FList[endPos];
        FList[endPos] = tmp;
    }

    /// <summary>
    /// Filters given index up to restore the tree definition. While the
    /// element in question is less than its parent.
    /// </summary>
    /// <param name="index"></param>
    private void FilterUp(int index)
    {
        while(!IsMin(index))//We want to keep going while the index in question has a parent
        {
            int parent = GetParentIndex(index);

            if(FList[index].CompareTo(FList[parent]) < 0)
            {
                Swap(index, parent);
                index = parent;
            }
            else
            {
                break;//FList[index] is in correct position.
            }
        }
    }

    private void FilterDown(int index)
    {
        //while the index has at least one child
        while (HasLeftChild(index) || HasRightChild(index))
        {
            T item = FList[index];
            T leftChild = GetLeftChild(index);//If it has any children, it should have a left child. But I need to test this to be sure.

            int leftIndex = GetLeftChildIndex(index);
            //check if it has one or two children.
            if (HasLeftChild(index) && HasRightChild(index))
            {
                T rightChild = GetRightChild(index);
                int rightIndex = GetRightChildIndex(index);

                if ((item.CompareTo(leftChild) < 0) && (item.CompareTo(rightChild) < 0))
                {
                    //If it's less than both break
                    break;
                }
                else
                {
                    //Swap it with whatever one is lower.
                    if (leftChild.CompareTo(rightChild) < 0)
                    {
                        Swap(index, leftIndex);
                        index = leftIndex;
                    }
                    else
                    {
                        Swap(index, rightIndex);
                        index = rightIndex;
                    }
                }
            }
            else
            {
                ///If it just has one swap only if it's lower, not that because
                ///of how items are added to the heap, that an item can only 
                ///have one child if that child is the last in the list. And
                ///that child has to be a 'left child'
                if(item.CompareTo(leftChild) < 0)
                {
                    Swap(index, leftIndex);
                    index = leftIndex;
                }
                else
                {
                    break; //Item is in the right place, break.
                }
            }
        }
    }
    #endregion
}
