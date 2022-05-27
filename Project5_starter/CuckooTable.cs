using System;
using System.Collections.Generic;

namespace Project5_starter
{
    public class CuckooTable<TKey, TValue> where TKey : IDoubleHashable
    {
        public KeyValuePair<TKey, TValue>[] _entries1 = new KeyValuePair<TKey, TValue>[10];
        public KeyValuePair<TKey, TValue>[] _entries2 = new KeyValuePair<TKey, TValue>[10];
        private int Count1 { get; set; }
        private int Count2 { get; set; }

        /// <summary>
        /// Gives the user the option to get or set a value
        /// </summary>
        /// <param name="k">The key portion of the desired item</param>
        /// <returns>The value of the corresponding key</returns>
        /// <exception cref="KeyNotFoundException">Not in the table</exception>
        public TValue this[TKey k]
        {
            get
            {
                for (int i = 0; i < _entries1.Length; i++) // For every item in _entries1
                {
                    if (_entries1[i].Key != null) // If that spot in the array is null (via the key)
                    {
                        if (_entries1[i].Key.Equals(k)) // If the key at position i is equal to the desired key
                        {
                            return _entries1[i].Value; // Return that corresponding value
                        }
                    }
                }

                for (int i = 0; i < _entries2.Length; i++) // For every item in _entries2
                {
                    if (_entries2[i].Key != null) // If that spot in the array is null (via the key)
                    {
                        if (_entries2[i].Key.Equals(k)) // If the key at position i is equal to the desired key
                        {
                            return _entries2[i].Value; // Return that corresponding value
                        }
                    }
                }

                throw new KeyNotFoundException(); // If we've gotten this far, the key is not in the table
            }
            set
            {
                for (int i = 0; i < _entries1.Length; i++) // For every item in _entries1
                {
                    if (_entries1[i].Key != null) // If that spot in the array is null (via the key)
                    {
                        if (_entries1[i].Key.Equals(k)) // If the key at position i is equal to the desired key
                        {
                            _entries1[i] = new KeyValuePair<TKey, TValue>(k, value); // Create a new KeyValuePair at that position with given variables
                            return;
                        }
                    }
                }

                for (int i = 0; i < _entries2.Length; i++) // For every item in _entries2
                {
                    if (_entries2[i].Key != null) // If that spot in the array is null (via the key)
                    {
                        if (_entries2[i].Key.Equals(k)) // If the key at position i is equal to the desired key
                        {
                            _entries2[i] = new KeyValuePair<TKey, TValue>(k, value); // Create a new KeyValuePair at that position with given variables
                            return;
                        }
                    }
                }

                throw new KeyNotFoundException(); // If we've gotten this far, the key is not in the table
            }
        }

        /// <summary>
        /// Checks to see if the key exists in one of the arrays
        /// </summary>
        /// <param name="k">The desired key</param>
        /// <returns>If it found the key or not</returns>
        public bool ContainsKey(TKey k)
        {
            for (int i = 0; i < _entries1.Length; i++) // For every item in _entries1
            {
                if (_entries1[i].Key != null) // If that spot in the array is null (via the key)
                {
                    if (_entries1[i].Key.Equals(k)) // If the key at position i is equal to the desired key
                    {
                        return true; // Then we've found it
                    }
                }
            }

            for (int i = 0; i < _entries2.Length; i++) // For every item in _entries1
            {
                if (_entries2[i].Key != null) // If that spot in the array is null (via the key)
                {
                    if (_entries2[i].Key.Equals(k)) // If the key at position i is equal to the desired key
                    {
                        return true; // Then we've found it
                    }
                }
            }
            return false; // If we've gotten this far, the key is not in the table
        }

        /// <summary>
        /// Rehashes (hence the name) the two arrays by increasing their size
        /// </summary>
        private void Rehash()
        {
            int len1 = _entries1.Length;
            int len2 = _entries2.Length;

            // Need to keep a record of the old arrays
            KeyValuePair<TKey, TValue>[] temp1 = _entries1; 
            KeyValuePair<TKey, TValue>[] temp2 = _entries2;

            // Resize both of the arrays
            _entries1 = new KeyValuePair<TKey, TValue>[(2*len1) + 1];
            _entries2 = new KeyValuePair<TKey, TValue>[(2*len2) + 1];

            for (int i = 0; i < temp1.Length; i++) // For every item in temp1
            {
                if (temp1[i].Key != null) // If that spot in the array is null (via the key)
                {
                    Add(temp1[i].Key, temp1[i].Value);
                }
            }            
            
            for (int i = 0; i < temp2.Length; i++) // For every item in temp2
            {
                if (temp2[i].Key != null) // If that spot in the array is null (via the key)
                {
                    Add(temp2[i].Key, temp2[i].Value);
                }
            }
        }

        /// <summary>
        /// Adds the key and the value (in the form of a KeyValuePair) via Displaced
        /// to the arrays while rehashing when necessary
        /// </summary>
        /// <param name="k">The key</param>
        /// <param name="v">The value</param>
        public void Add(TKey k, TValue v)
        {
            KeyValuePair<TKey, TValue>? displaced = null;
            KeyValuePair<TKey, TValue>? cur = new KeyValuePair<TKey, TValue>(k, v);
            bool finished = false;
            int curTable = 1;
            List<Tuple<string, int>> tried = new List<Tuple<string, int>>();

            while (!finished) // As long as we aren't finished (everything has been assigned)
            {
                if (tried.Contains(new Tuple<string, int>(cur.Value.Key.ToString(), curTable))) // If we've already tried this word
                {
                    Rehash(); // Then we need to rehash (increase the size)
                    Add(k, v); // And try to add it again
                    return;
                }
                displaced = Displaced(cur, curTable); // Add this new word to the table while keeping track of a possible value that was kicked out
                if (displaced.Value.Key == null) // If there wasn't a value that was kicked out
                {
                    finished = true; // Then we don't have to continue the loop anymore
                    if (curTable == 1) // If we are on the first table
                    {
                        Count1++; // Increase its size
                    }
                    else
                    {
                        Count2++; // Otherwise, increase the second table's size
                    }
                }
                else
                {
                    // Otherwise we know that there is a rogue value
                    tried.Add(new Tuple<string, int>(displaced.Value.Key.ToString(), curTable));
                    // Switch the focused table around
                    if (curTable == 1)
                    {
                        curTable = 2;
                    }
                    else
                    {
                        curTable = 1;
                    }
                    cur = displaced; // Now we want to focus on this value that was kicked out
                }
            }
        }

        /// <summary>
        /// Adds a KeyValuePair to the table
        /// </summary>
        /// <param name="kvp">The KeyValuePair to be added</param>
        /// <param name="table">The table to put it in</param>
        /// <returns>The value kicked out due to adding the new one</returns>
        public KeyValuePair<TKey, TValue>? Displaced(KeyValuePair<TKey, TValue>? kvp, int table)
        {
            KeyValuePair<TKey, TValue>? temp = new KeyValuePair<TKey, TValue>();
            KeyValuePair<TKey, TValue> cur = (KeyValuePair<TKey, TValue>)kvp;
            if (cur.Key != null) // If cur is not null
            {
                if (table == 1) // If we are on the first table
                {
                    int hash1 = cur.Key.Hash1; // Get the first hash code
                    int h = newHash1(hash1); // Convert it to an index
                    temp = _entries1[h]; // Get the value that's at that index
                    _entries1[h] = cur; // Then plug in the new value
                }
                else // Otherwise, we are on the second table
                {
                    int hash2 = cur.Key.Hash2; // Get the second hash code
                    int h = newHash2(hash2); // Convert it to an index
                    temp = _entries2[h]; // Get the value that's at that index
                    _entries2[h] = cur; // Then plug in the new value
                }

                if (((_entries1.Length / 3.0) <= Count1) || ((_entries2.Length / 3.0) <= Count2)) // If one of the arrays if over 1/3 of the way full
                {
                    Rehash(); // We know that we need to rehash
                }
            }

            return temp; // Return the value kicked out
        }

        /// <summary>
        /// Converts the hash code to an array index
        /// </summary>
        /// <param name="hash">The hash code</param>
        /// <returns>The new array friendly int</returns>
        public int newHash1(int hash)
        {
            return Math.Abs(hash) % _entries1.Length; // Converts the hash code to an array index
        }

        /// <summary>
        /// Converts the hash code to an array index
        /// </summary>
        /// <param name="hash">The hash code</param>
        /// <returns>The new array friendly int</returns>
        public int newHash2(int hash)
        {
            return Math.Abs(hash) % _entries2.Length; // Converts the hash code to an array index
        }
    }
}
