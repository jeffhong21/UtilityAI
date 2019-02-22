using System;
using System.Collections.Generic;


namespace AtlasAI.DataStructures
{
    public class BinaryHeap<T>
    {
        //
        // Fields
        //
        private int _count;
        private T[] _heap;
        private IComparer<T> _comparer;

        //
        // Properties
        //
        public int capacity{
            get { return _heap.Length; }
        }

        /// <summary>
        ///   Returns the number of items in the queue.
        /// </summary>
        public int count{
            get { return _count; }
        }

        /// <summary>
        ///   Returns true if there is an element at the head of the queue, i.e. if the queue is not
        ///   empty.
        /// </summary>
        public bool hasNext{
            get { return _count > 0; }
        }

        public T[] heap{
            get { return _heap; }
        }

        //
        // Constructors
        //
        public BinaryHeap(int capacity, IComparer<T> comparer)
        {
            _heap = new T[capacity];
            _count = 0;
            _comparer = comparer;
        }


        //
        // Methods
        //
        private int GetLeftChildIndex(int parentIndex) { return 2 * parentIndex + 1; }
        private int GetRightChildIndex(int parentIndex) { return 2 * parentIndex + 2; }
        private int GetParentIndex(int childIndex) { return (childIndex - 1) / 2; }

        private bool HasLeftChild(int index) { return GetLeftChildIndex(index) < count; }
        private bool HasRightChild(int index) { return GetRightChildIndex(index) < count; }
        private bool HasParent(int index) { return GetParentIndex(index) >= 0; }

        private T LeftChild(int index) { return _heap[GetLeftChildIndex(index)]; }
        private T RightChild(int index) { return _heap[GetRightChildIndex(index)]; }
        private T Parent(int index) { return _heap[GetParentIndex(index)]; }



        public void DebugHeap(bool pause = false)
        {
            string debug = "";
            for (int i = 0; i < _heap.Length; i++){
                debug += string.Format(" {0} |", _heap[i]);
            }

            debug += string.Format("  Count: {0} |  Capacity {1}", count, capacity);

            UnityEngine.Debug.Log(debug);
            if (pause) UnityEngine.Debug.Break();
        }


        public T Peek()
        {
            if (count == 0) throw new Exception();
            return _heap[0];
        }


        public void Add(T item)
        {
            //  Ensure there is capacity.
            Resize();
            //  Add element to the last spot.
            _heap[count] = item;
            //  Increase the count.
            _count++;
            HeapifyUp();
        }


        public T Remove()
        {
            if (count == 0) throw new Exception();
            T temp = _heap[0];
            //  Take last element of array and move it to the first element.
            _heap[0] = _heap[count - 1];
            //  Shrink the size of the array.
            _count--;

            HeapifyDown();
            return temp;
        }


        public T Remove(T item)
        {
            int index = Array.IndexOf(heap, item);

            T temp = _heap[index];

            //UnityEngine.Debug.LogFormat("Index {0} | {1}", index, temp);
            //UnityEngine.Debug.Break();

            //  If item was the last one.
            if(index == count - 1){
                _heap[count] = default(T);
                _count--;
                return temp;
            }


            Swap(index, count-1);
            _heap[count-1] = default(T);
            _count--;
            int parent = index >> 1;

            if (parent > 0 && _comparer.Compare(_heap[index], _heap[parent]) < 0)
                HeapifyUp();
            else
                HeapifyDown();

            return temp;
            //throw new NotImplementedException();
        }


        private void HeapifyDown()
        {
            int index = 0;

            while (HasLeftChild(index))
            {
                //  Checking which child is smaller.  Starting with the left child first.
                int smallerChildIndex = GetLeftChildIndex(index);
                //  If right child is smaller than left, than the smallerChild index is the right.
                if (HasRightChild(index) && _comparer.Compare(RightChild(index), LeftChild(index)) == -1)
                {
                    smallerChildIndex = GetRightChildIndex(index);
                }
                //  If item is less than both childs, than exit out of loop.
                if (_comparer.Compare(_heap[index], _heap[smallerChildIndex]) == -1 ||
                    _comparer.Compare(_heap[index], _heap[smallerChildIndex]) == 0 )
                {
                    break;
                }

                //  If not, than swap with the smaller child.
                else{
                    Swap(index, smallerChildIndex);
                }

                //  Now we start over with the smaller child.
                index = smallerChildIndex;
            }
        }





        private void HeapifyUp()
        {
            //  Start with the last element.
            int index = count - 1;

            //  Walk up as long as there is a parent item and parent is bigger than current item
            while (HasParent(index) && _comparer.Compare(Parent(index), _heap[index]) == 1)
            {
                //  Swap parent index.
                Swap(GetParentIndex(index), index);
                //  Walk up the tree.
                index = GetParentIndex(index);
            }
        }





        private void Swap(int indexOne, int indexTwo)
        {
            T temp = _heap[indexOne];
            _heap[indexOne] = _heap[indexTwo];
            _heap[indexTwo] = temp;
        }


        private void Resize()
        {
            if(count == capacity){
                T[] newArray = _heap;
                Array.Resize(ref newArray, _heap.Length + 1);
                _heap = newArray;
            }
        }



        //private void HeapifyDown(int i)
        //{
        //    while (true)
        //    {
        //        int smallest = i;
        //        int left = i << 1;
        //        int right = (i << 1) | 1;
        //
        //        if (left <= _count)
        //        {
        //            var cmp = _comparer.Compare(_heap[left], _heap[i]);
        //            if (cmp < 0 || cmp == 0 && _heap[left].Handle.Order < _heap[i].Handle.Order)
        //                smallest = left;
        //        }
        //
        //        if (right <= _count)
        //        {
        //            var cmp = _comparer.Compare(_heap[right], _heap[smallest]);
        //            if (cmp < 0 || cmp == 0 && _heap[right].Handle.Order < _heap[smallest].Handle.Order)
        //                smallest = right;
        //        }
        //
        //        if (smallest == i)
        //            return;
        //
        //        Swap(i, smallest);
        //        i = smallest;
        //    }
        //}
        //
        //private void HeapifyUp(int i)
        //{
        //    if (i < 1)
        //        return;
        //
        //    int parent = i >> 1;
        //    while (parent > 0)
        //        if (_comparer.Compare(_heap[i], _heap[parent]) < 0)
        //        {
        //            Swap(parent, i);
        //            i = parent;
        //            parent = parent >> 1;
        //        }
        //        else
        //            break;
        //}
    }
}

