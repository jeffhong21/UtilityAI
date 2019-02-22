using System;
using System.Collections.Generic;

namespace AtlasAI.Utilities
{
    public class BinaryHeapExample<T>
    {

        private int capacity = 10;
        private int size = 0;

        private int[] items;


        int GetLeftChildIndex(int parentIndex) { return 2 * parentIndex + 1; }
        int GetRightChildIndex(int parentIndex) { return 2 * parentIndex + 2; }
        int GetParentIndex(int childIndex) { return (childIndex - 1) / 2; }

        bool HasLeftChild(int index) { return GetLeftChildIndex(index) < size; }
        bool HasRightChild(int index) { return GetRightChildIndex(index) < size; }
        bool HasParent(int index) { return GetParentIndex(index) >= 0; }

        int LeftChild(int index) { return items[GetLeftChildIndex(index)]; }
        int RightChild(int index) { return items[GetRightChildIndex(index)]; }
        int Parent(int index) { return items[GetParentIndex(index)]; }


        void Swap(int indexOne, int indexTwo)
        {
            int temp = items[indexOne];
            items[indexOne] = items[indexTwo];
            items[indexTwo] = temp;
        }

        //void EnsureExtractCapacity(){
        //    if(size == capacity){
        //        items = Arrays.copyOf(items, capacity * 2);
        //        capacity *= 2;
        //    }
        //}

        public int Peek()
        {
            if (size == 0) throw new Exception();
            return items[0];
        }

        //  Extracts the minimum element and removes it from array.  This is a Remove
        public int Poll()
        {
            if (size == 0) throw new Exception();
            int item = items[0];
            //  Take last element of array and move it to the first element.
            items[0] = items[size - 1];
            //  Shrink the size of the array.
            size--;

            HeapifyDown();
            return item;
        }

        public void Add(int item)
        {
            //  Ensure there is capacity.
            //EnsureExtractCapacity();

            //  Add element to the last spot.
            items[size] = item;
            //  increase size.
            size++;
            HeapifyUp();
        }

        public void HeapifyDown()
        {
            int index = 0;

            while (HasLeftChild(index))
            {
                //  Checking which child is smaller.  Starting with the left child first.
                int smallerChildIndex = GetLeftChildIndex(index);
                //  If right child is smaller than left, than the smallerChild index is the right.
                if (HasRightChild(index) && RightChild(index) < LeftChild(index))
                {
                    smallerChildIndex = GetRightChildIndex(index);
                }
                //  If item is less than both childs, than exit out of loop.
                if (items[index] < items[smallerChildIndex])
                {
                    break;
                }
                //  If not, than swap with the smaller child.
                else
                {
                    Swap(index, smallerChildIndex);
                }

                //  Now we start over with the smaller child.
                index = smallerChildIndex;
            }

        }

        public void HeapifyUp()
        {
            //  Start with the last element.
            int index = size - 1;
            //  Walk up as long as there is a parent item and as long as parent item is bigger than current item
            while (HasParent(index) && Parent(index) > items[index])
            {
                //  Swap parent index.
                Swap(GetParentIndex(index), index);
                //  Walk up the tree.
                index = GetParentIndex(index);
            }
        }
    }
}

