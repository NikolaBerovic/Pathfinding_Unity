using System.Collections.Generic;
using System;

///<summary>Priority queue - generic list with min binary heap</summary>
public class PriorityQueue<T> where T : IComparable<T>
{
    // List of items in our queue
    List<T> _data;

    // number of items currently in queue
    public int Count { get { return _data.Count; }}


    public PriorityQueue()
    {
        _data = new List<T>();
    }

    ///<summary>Add an item to the queue and sort using a min binary heap</summary>
    public void Enqueue(T item)
    {
        _data.Add(item);
        int childindex = _data.Count - 1;

        while (childindex > 0)
        {
            //find the parent position in the heap
            int parentindex = (childindex - 1) / 2;

            //if parent and child are already sorted, stop sorting
            if (_data[childindex].CompareTo(_data[parentindex]) >= 0)
            {
                break;
            }

            //otherwise, swap parent and child
            T tmp = _data[childindex];
            _data[childindex] = _data[parentindex];
            _data[parentindex] = tmp;

            //move up one level in the heap and repeat until sorted
            childindex = parentindex;
        }
    }

    ///<summary>Remove an item from queue and keep it sorted using a min binary heap</summary>
    public T Dequeue()
    {
        int lastindex = _data.Count - 1;

        T frontItem = _data[0];

        //replace the first item with the last item
        _data[0] = _data[lastindex];
        _data.RemoveAt(lastindex);

        lastindex--;

        //start at the beginning of the queue to sort the binary heap
        int parentindex = 0;

        //sort using min binary heap
        while (true)
        {
            //choose the left child
            int childindex = parentindex * 2 + 1;

            //if there is no left child, stop sorting
            if (childindex > lastindex)
            {
                break;
            }

            //the right child
            int rightchild = childindex + 1;

            //if the value of the right child is less than the left child, switch to the right branch of the heap
            if (rightchild <= lastindex && _data[rightchild].CompareTo(_data[childindex]) < 0)
            {
                childindex = rightchild;
            }

            //if the parent and child are already sorted, then stop sorting
            if (_data[parentindex].CompareTo(_data[childindex]) <= 0)
            {
                break;
            }

            //if not, then swap the parent and child
            T tmp = _data[parentindex];
            _data[parentindex] = _data[childindex];
            _data[childindex] = tmp;

            //move down the heap onto the child's level and repeat until sorted
            parentindex = childindex;

        }

        //return the original first item
        return frontItem;
    }

    ///<summary>Look at the first item without dequeuing</summary>
    public T Peek()
    {
        T frontItem = _data[0];
        return frontItem;
    }

    ///<summary>Does data list contain an item</summary>
    public bool Contains(T item)
    {
        return _data.Contains(item);
    }

    ///<summary>Returns data as list</summary>
    public List<T> ToList()
    {
        return _data;
    }

}
