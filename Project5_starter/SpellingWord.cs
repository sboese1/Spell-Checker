/// <summary>
/// This class is home to a single word
/// </summary>

namespace Project5_starter
{
    public class SpellingWord : IDoubleHashable
    {
        string word { get; set; }

        public int Hash1 { get; set; }

        public int Hash2 { get; set; }

        public SpellingWord(string w)
        {
            word = w;
            Hash1 = GetHashCode();
            Hash2 = getHash2();
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// This is a hash function algorithm (specifically, polynomial hashing)
        /// used to convert it to hash code
        /// </summary>
        /// <returns>The converted hash code</returns>
        public int getHash2()
        {
            int k = 37; // Start with a prime number
            int hashCode = 0;

            foreach (char c in word) // For every character in the word
            {
                hashCode = hashCode * k + c; // Use this formula to convert it
            }
            return hashCode; // And return the result
        }

        /// <summary>
        /// Checks to see if the two items are equal
        /// </summary>
        /// <param name="obj">The object to be compared to</param>
        /// <returns>If the two are equal or not</returns>
        public override bool Equals(object obj)
        {
            string _words = obj.ToString(); // Convert the object to a string
            return word.Equals(_words); // And compare them
        }

        /// <summary>
        /// Returns the word
        /// </summary>
        /// <returns>The word</returns>
        public override string ToString()
        {
            return word;
        }
    }
}
