using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// This file handles of the interactions with the user. It controls the file handling as well as
/// the different one edit methods.
/// </summary>

namespace Project5_starter
{
    public partial class UserInterface : Form
    {
        CuckooTable<SpellingWord, BigInteger> words = new CuckooTable<SpellingWord, BigInteger>();

        public UserInterface()
        {
            InitializeComponent();

            try
            {
                var lines = File.ReadAllLines("wordCounts.txt"); // Read in the text file

                foreach (var line in lines) // For every line in the text file
                {
                    string[] split = line.Split('\t'); // Split it up into the word and count
                    SpellingWord s = new SpellingWord(split[0]); // Create a new word
                    words.Add(s, long.Parse(split[1])); // And add it to the words cuckoo table
                }
            }
            catch { MessageBox.Show("There was an error loading the file"); }
        }

        private void uxCheckButton_Click(object sender, EventArgs e)
        {
            uxResultingText.Text = ""; // Clear the resulting textbox
            string text = uxWord.Text.Trim(); // Get the text from the textbox
            text = text.ToLower(); // Convert it all to lowercase
            if (text == "") // If the user didn't enter in a word
            {
                MessageBox.Show("Enter a word"); 
                return; // Then don't do anything
            }
            foreach (char c in text) // For every character in the word
            {
                if (!Char.IsLetter(c)) // If it's not a letter
                {
                    MessageBox.Show("Invalid letter in the word");
                    return; // Don't do anything
                }
            }
            SpellingWord s = new SpellingWord(text); // Create a new word
            if (words.ContainsKey(s)) // If the entered word is already a valid word
            {
                uxResultingText.Text = text; // Then we're done
            }
            else
            {
                List<string> list = new List<string>();
                // Call all of the methods for the possible one edit away
                list.Add(Insert(text));
                list.Add(Removal(text));
                list.Add(Replace(text));
                list.Add(Split(text));
                list.Add(Swap(text));
                list = list.Where(str => !string.IsNullOrWhiteSpace(str)).ToList(); // Get rid of all the empty entries

                string result = Max(list); // Find the word with the most occurances
                if (result.Length == 0) // If the word does not exist 
                {
                    uxResultingText.Text = "No corrections found";
                }
                else
                {
                    uxResultingText.Text = result; // Otherwise, display it in the textbox
                }
            }
        }

        /// <summary>
        /// Checks to see if the one edit away is a missing letter
        /// </summary>
        /// <param name="s">The entered word</param>
        /// <returns>The possible change</returns>
        public string Insert(string s)
        {
            List<string> list = new List<string>();
            StringBuilder copy = new StringBuilder();

            for (int i = 0; i < s.Length+1; i++) // For every letter in the string
            {
                for (int j = 0; j < 26; j++) // For every letter in the alphabet
                {
                    copy.Clear(); // Clear the StringBuilder
                    copy.Append(s); // Always start off the default string
                    char letter = (char)(j+97); // Convert the int to a letter (char)
                    copy = copy.Insert(i, letter.ToString()); // Insert that letter into the string
                    SpellingWord sp = new SpellingWord(copy.ToString()); // Make a new word from it
                    if (words.ContainsKey(sp)) // If the new word is in the table
                    {
                        list.Add(copy.ToString()); // Then add it to the list
                    }
                }
            }

            return Max(list); // Return the word with the most occurances
        }

        /// <summary>
        /// Checks to see if the one edit away is an extra letter
        /// </summary>
        /// <param name="s">The entered word</param>
        /// <returns>The possible change</returns>
        public string Removal(string s)
        {
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();

            for (int i = 0; i < s.Length; i++) // For every letter in the string
            {
                sb.Clear(); // Clear the StringBuilder
                sb.Append(s); // Add the default string to it
                sb.Remove(i, 1); // And remove a character
                SpellingWord sp = new SpellingWord(sb.ToString()); // Make a new word from it
                if (words.ContainsKey(sp)) // If the new word is in the table 
                {
                    list.Add(sb.ToString()); // Then add it to the list
                }
            }

            return Max(list); // Return the word with the most occurances
        }

        /// <summary>
        /// Checks to see if the one edit away is a letter replacement
        /// </summary>
        /// <param name="s">The entered word</param>
        /// <returns>The possible change</returns>
        public string Replace(string s)
        {
            List<string> list = new List<string>();
            StringBuilder copy = new StringBuilder();

            for (int i = 0; i < s.Length; i++) // For every character in the string
            {
                for (int j = 0; j < 26; j++) // For every letter
                {
                    copy.Clear(); // Clear the StingBuilder
                    copy.Append(s); // Make copy the default string
                    char letter = (char)(j + 97); // Convert the int to a letter
                    copy = copy.Remove(i, 1); // Remove a character at position i
                    copy = copy.Insert(i, letter.ToString()); // Insert it the new letter into the string
                    SpellingWord sp = new SpellingWord(copy.ToString()); // Make a new word from it
                    if (words.ContainsKey(sp)) // If the new word is in the table
                    {
                        list.Add(copy.ToString()); // Then add it to the list
                    }
                }
            }

            return Max(list); // Return the word with the most occurances
        }

        /// <summary>
        /// Checks to see if the one edit away is a swap
        /// </summary>
        /// <param name="s">The entered word</param>
        /// <returns>The possible change</returns>
        public string Swap(string s)
        {
            List<string> list = new List<string>();
            StringBuilder copy = new StringBuilder();

            for (int i = 0; i < s.Length - 1; i++) // For every letter in s (except the last one)
            {
                copy.Clear(); // Clear the StringBuilder
                copy.Append(s); // Make copy the default string
                char firstLetter = s[i]; // First letter is at i
                char secondLetter = s[i+1]; // Second letter is at i + 1
                copy = copy.Remove(i, 1); // Remove a letter at i
                copy = copy.Remove(i, 1); // Remove another letter at i
                copy = copy.Insert(i, firstLetter.ToString()); // Insert it back into the string
                copy = copy.Insert(i, secondLetter.ToString()); // Insert the other back into the string

                SpellingWord str = new SpellingWord(copy.ToString()); // Make a new word from it
                if (words.ContainsKey(str)) // If the new word is in the table
                {
                    list.Add(copy.ToString()); // Then add it to the list
                }

                // Undo the swap we did earlier
                copy = copy.Remove(i, 1); // Remove a letter at i
                copy = copy.Remove(i, 1); // Remove another letter at i
                copy = copy.Insert(i, firstLetter.ToString()); // Insert it back into the string
                copy = copy.Insert(i, secondLetter.ToString()); // Insert the other back into the string
            }

            return Max(list); // Return the word with the most occurances
        }

        /// <summary>
        /// Checks to see if the one edit away is a swapping of letters
        /// </summary>
        /// <param name="s">The entered word</param>
        /// <returns>The possible change</returns>
        public string Split(string s)
        {
            List<string> list = new List<string>();
            string copy = s;

            for (int i = 0; i < s.Length; i++) // For every character in the string
            {
                copy = s; // Make copy the default string
                copy = copy.Insert(i, " "); // Insert a space into the string at position i
                string[] pieces = copy.Split(' '); // Split the word by a space
                SpellingWord sp1 = new SpellingWord(pieces[0]); // Create a word from the first half of the array
                SpellingWord sp2 = new SpellingWord(pieces[1]); // Create a wprd from the second half of the array 
                if (words.ContainsKey(sp1) && words.ContainsKey(sp2)) // If the new word is in the table
                {
                    list.Add(copy); // Then add it to the list
                }
            }

            return Max(list); // Return the word with the most occurances
        }

        /// <summary>
        /// Finds the word with the most occurances
        /// </summary>
        /// <param name="list">The list of possible words</param>
        /// <returns>The word with the most occurances</returns>
        public string Max(List<string> list)
        {
            string max = "";
            if (list.Count > 0) // If there is at least one thing in the list
            {
                max = list[0]; // Set max to the first item
                if (list.Count > 1) // If there is at least two things in the list
                {
                    foreach (string str in list) // For each item in the list
                    {
                        string[] pieces = str.Split(' '); // Split the word if there is a space
                        SpellingWord word1;
                        SpellingWord word2;

                        if (pieces.Length > 1) // If there is more than one thing in pieces
                        {
                            word1 = new SpellingWord(pieces[0]); // Make a new word from it
                            word2 = new SpellingWord(pieces[1]); // Make a new word from it
                        }
                        else
                        {
                            word1 = new SpellingWord(str); // Otherwise, make a new word from the new string
                            word2 = new SpellingWord(max); // As well as the current max (most occurances) word
                        }

                        if (words[word1] > words[word2]) // If the first word has more occurances than the second word
                        {
                            max = str; // Then we have found a new max word
                        }
                    }
                }
            }

            return max; // Return the word with the most occurances
        }
    }
}
