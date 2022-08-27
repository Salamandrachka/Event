using System;
using System.Collections;
using System.Collections.Generic;

namespace ArrayEvent
{  
    public class CustomArray<T> : IEnumerable<T>
    {
        private IEnumerable<T> array;
        private int first;
        private int length;

        /// <param name="sender">CustomArray parameter </param>
        /// <param name="e">ArrayEventArgs parameter</param>
        public delegate void ArrayHandler(object sender, ArrayEventArgs<T> e);
        /// <summary>
        /// Event that invokes when array element was changed 
        /// </summary>
        public event ArrayHandler OnChangeElement;

        /// <summary>
        /// Event that invokes when index of changed element equal to value 
        /// </summary>
        public event ArrayHandler OnChangeEqualElement;

        /// <summary>
        /// Should return first index of array
        /// </summary>
        public int First
        {
            get => first;
            private set => first = value;
        }


        /// <summary>
        /// Should return last index of array
        /// </summary>
        public int Last
        {
            get => first + Length - 1;
        }


        /// <summary>
        /// Should return length of array
        /// <exception cref="ArgumentException">Thrown when value was smaller than 0</exception>
        /// </summary>
        public int Length
        {
            get => length;
            private set
            {
                if (value <= 0) throw new ArgumentException("Value < 0");
                length = value;
            }
        }


        /// <summary>
        /// Should return array 
        /// </summary>
        public T[] Array
        {
            get => (T[]) array;
        }



        /// <summary>
        /// Constructor with first index and length
        /// </summary>
        /// <param name="first">First Index</param>
        /// <param name="length">Length</param>         
        public CustomArray(int first, int length)
        {
            First = first;
            Length = length;
            array = new T[length];
        }

        /// <summary>
        /// Constructor with first index and collection  
        /// </summary>
        /// <param name="first">First Index</param>
        /// <param name="list">Collection</param>
        ///  <exception cref="ArgumentException">Thrown when list is null</exception>
        /// <exception cref="NullReferenceException">Thrown when count is smaler than 0</exception>
        public CustomArray(int first, IEnumerable<T> list)
        {
            if (list == null) throw new NullReferenceException("List is null");
            List<T> collection = list as List<T>;
            if (collection.Count == 0) throw new ArgumentException("Count < 0");
            array = list;
            Length = collection.Count;
            First = first;
        }

        /// <summary>
        /// Constructor with first index and params
        /// </summary>
        /// <param name="first">First Index</param>
        /// <param name="list">Params</param>
        ///  <exception cref="ArgumentNullException">Thrown when list is null</exception>
        /// <exception cref="ArgumentException">Thrown when list without elements </exception>
        public CustomArray(int first, params T[] list)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (list.Length == 0) throw new ArgumentException("List is empty");
            array = list;
            Length = list.Length;
            First = first;
        }


        /// <summary>
        /// Indexer with get and set  
        /// </summary>
        /// <param name="item">Int index</param>        
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when index out of array range</exception>
        /// <exception cref="ArgumentNullException">Thrown in set  when value passed in indexer is null</exception>
        public T this[int item]
        {
            get
            {
                if (item <= First || item >= First + Length) throw new ArgumentException("Index out of array range");
                T[] a = array as T[];
                return a[item - First];
            }
            set
            {
                if (item <= First || item >= First + Length) throw new ArgumentException("Index out of array range");
                if (value == null) throw new ArgumentNullException("value");
                T[] a = array as T[];
                T oldValue = a[item - First];
                a[item - First] = value;

                if (!value.Equals(oldValue))
                {
                    if (OnChangeElement != null) OnChangeElement.Invoke(this, new ArrayEventArgs<T>(item, "The value has been changed", value));

                    if (OnChangeEqualElement != null && value.Equals(item))
                    {
                        OnChangeEqualElement.Invoke(this, new ArrayEventArgs<T>(item, "The value has been changed", value));
                    }

                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the CustomArray.
        /// </summary>        
        public IEnumerator<T> GetEnumerator()
        {
            return array.GetEnumerator() as IEnumerator<T>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
