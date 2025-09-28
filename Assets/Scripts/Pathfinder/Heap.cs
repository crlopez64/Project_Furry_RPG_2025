using System.Collections;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    /// <summary>
    /// Add an item to the heap.
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    /// <summary>
    /// Update an item's priority in the list.
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    /// <summary>
    /// Revert the itemCount back to zero.
    /// </summary>
    public void Clear()
    {
        currentItemCount = 0;
    }

    /// <summary>
    /// Return if Heap has the item being asked of.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item)
    {
        if (item.HeapIndex < currentItemCount)
        {
            return Equals(items[item.HeapIndex], item);
        }
        return false;
    }

    /// <summary>
    /// Return count of the Heap.
    /// </summary>
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    /// <summary>
    /// Remove an item from the array and return the item.
    /// </summary>
    /// <returns></returns>
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /// <summary>
    /// Sort down within the heap.
    /// </summary>
    /// <param name="item"></param>
    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    
    /// <summary>
    /// Sort up within the heap.
    /// </summary>
    /// <param name="item"></param>
    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1)/2;
        while(true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    /// <summary>
    /// Swap two items.
    /// </summary>
    /// <param name="itemA"></param>
    /// <param name="itemB"></param>
    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable
{
    int HeapIndex{ get; set; }
}
