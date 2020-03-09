using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace cryptology_wpf_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentFileName;
        const string uaAlphabet = "абвгґдеєжзиіїклмнопрстуфхцчшщьюяАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        const string enAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?():;";
        int A;
        int B;
        int C = -1;
        public MainWindow()
        {
            InitializeComponent();
        }

        public enum Direction { Left, Right }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void CheckString(int mostProb, char letter, string text)
        {
            //int shift = letter - mostProb;
            //string result = Decryption(text, Direction.Right, Math.Abs(shift));
            //string[] words = result.Split(' ');

            //NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            //oDict.DictionaryFile = "en-US.dic";
            //oDict.Initialize();
            //NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();
            //oSpell.Dictionary = oDict;
            //if (oSpell.TestWord(words[0]) || oSpell.TestWord(words[1]) || oSpell.TestWord(words[2]))
            //{
            //    MessageBox.Show(shift.ToString() + " " + result);
            //}
        }
        private void PredictOutputButton_Click(object sender, RoutedEventArgs e)
        {
            string firstText = rawTextBox.Text.ToString();
            string finalText = finalTextBox.Text.ToString();

            Direction dir = Direction.Right;
            string result = "";

            if ((bool)secondTypeRadioButton.IsChecked)
            {
                if ((bool)englishCheckBox.IsChecked)
                {
                    var results = (from A in Enumerable.Range(1, 10)
                                   from B in Enumerable.Range(1, 10)
                                   let letters = "A: " + A.ToString() + " B:" + B.ToString()
                                   let str = Encpypt(firstText, dir,enAlphabet, A, B)
                                   where string.Equals(str, finalText)
                                   select letters + " " + str).ToArray();
                    result = string.Join("\n", results);
                }
                else
                {
                    var results = (from A in Enumerable.Range(1, 10)
                                   from B in Enumerable.Range(1, 10)
                                   let letters = "A: " + A.ToString() + " B:" + B.ToString()
                                   let str = Encpypt(firstText, dir,uaAlphabet, A, B)
                                   where string.Equals(str, finalText)
                                   select letters + " " + str).ToArray();
                    result = string.Join("\n", results);
                }
            }
            else if ((bool)thirdTypeRadioButton.IsChecked)
            {
                if ((bool)englishCheckBox.IsChecked)
                {
                    var results = (from A in Enumerable.Range(1, 10)
                                   from B in Enumerable.Range(1, 10)
                                   from C in Enumerable.Range(1, 10)
                                   let letters = "A: " + A.ToString() + " B:" + B.ToString() + " C:" + C.ToString()
                                   let str = Encpypt(firstText, dir,enAlphabet, A, B, C)
                                   where string.Equals(str, finalText)
                                   select letters + " " + str).ToArray();
                    result = string.Join("\n", results);
                }
                else
                {
                    var results = (from A in Enumerable.Range(1, 10)
                                   from B in Enumerable.Range(1, 10)
                                   from C in Enumerable.Range(1, 10)
                                   let letters = "A: " + A.ToString() + " B:" + B.ToString() + " C:" + C.ToString()
                                   let str = Encpypt(firstText, dir,uaAlphabet, A, B, C)
                                   where string.Equals(str, finalText)
                                   select letters + " " + str).ToArray();
                    result = string.Join("\n", results);
                }
            }
            else
            {
                if ((bool)englishCheckBox.IsChecked)
                {
                    const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int wordSize = int.Parse(keySizeField.Text);
                    string rawText = string.Concat(firstText.Split());
                    int textLength = rawText.Length;
                    var lettersProb = rawText.GroupBy(c => c).Select(c => new { Char = c.Key, Count = (double)(c.Count()) }).OrderByDescending(c => c.Count).ToDictionary(pair => pair.Char, pair => (pair.Count / textLength));
                    char[] arrayLetter = new char[wordSize];
                    arrayLetter = lettersProb.Select(k => k.Key).Take(wordSize).ToArray();
                    char[] englishLetter = { 'e', 'o', 'i', 's', 'n' };
                    result = rawText.Replace(arrayLetter[0], englishLetter[0]);
                    MessageBox.Show(result);

                    //result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet);
                }
                else
                {
                    result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), uaAlphabet);
                }
            }


            using (StreamWriter writer = new StreamWriter("../../attack.txt"))
            {
                writer.WriteLine(result);
            }
            MessageBox.Show("attack.txt\nSuccessfully saved!");

        }
        //encryption
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            string firstText = rawTextBox.Text.ToString();
            Direction dir = Direction.Right;
            string result = "";

            if ((bool)secondTypeRadioButton.IsChecked)
            {
                A = int.Parse(aForSecond.Text);
                B = int.Parse(bForSecond.Text);
                if ((bool)englishCheckBox.IsChecked)
                {
                    result = Encpypt(firstText, dir, enAlphabet, A, B);
                }
                else
                {
                    result = Encpypt(firstText, dir, uaAlphabet, A, B);
                }
            }
            else if ((bool)thirdTypeRadioButton.IsChecked)
            {
                A = int.Parse(aForThird.Text);
                B = int.Parse(bForThird.Text);
                C = int.Parse(cForThird.Text);
                if ((bool)englishCheckBox.IsChecked)
                {
                    result = Encpypt(firstText, dir, enAlphabet,A, B, C);
                }
                else
                {
                    result = Encpypt(firstText, dir,uaAlphabet, A, B, C);
                }
            }
            else
            {
                if ((bool)englishCheckBox.IsChecked)
                {
                    const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet).ToLower();
                }
                else
                {
                    const string defaultAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                    result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet).ToLower();
                }
            }
            finalTextBox.Text = result;
        }
        
        private string Encpypt(string rawText, Direction dir,string alphabet, int A, int B, int C = -1)
        {
            var fullAlfabet = alphabet;
            var letterQty = fullAlfabet.Length;
            var resultText = "";
            for (int i = 0; i < rawText.Length; i++)
            {
                int shiftNumb = C == -1 ? A * i + B : A * i*i+ B * i + C;
                var raw = rawText[i];
                var index = fullAlfabet.IndexOf(raw);
                if (index < 0)
                {
                    resultText += raw.ToString();
                }
                else
                {
                    var codeIndex = (letterQty + index + shiftNumb) % letterQty;
                    resultText += fullAlfabet[codeIndex];
                }
            }
            return resultText;
        }

        public void WriteToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(finalTextBox.Text);
            }
        }

        //decryption       
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string firstText = rawTextBox.Text.ToString();
            Direction dir = Direction.Right;
            string result;

            if ((bool)secondTypeRadioButton.IsChecked)
            {
                A = int.Parse(aForSecond.Text);
                B = int.Parse(bForSecond.Text);
                if ((bool)englishCheckBox.IsChecked)
                {
                    result = Decrypt(firstText, dir,enAlphabet, A, B);
                }
                else
                {
                    result = Decrypt(firstText, dir,uaAlphabet, A, B);
                }
            }
            else if ((bool)thirdTypeRadioButton.IsChecked)
            {
                A = int.Parse(aForThird.Text);
                B = int.Parse(bForThird.Text);
                C = int.Parse(cForThird.Text);
                if ((bool)englishCheckBox.IsChecked)
                {
                    result = Decrypt(firstText, dir, enAlphabet, A, B, C);
                }
                else
                {
                    result = Decrypt(firstText, dir,uaAlphabet, A, B, C);
                }
            }
            else
            {
                if ((bool)englishCheckBox.IsChecked)
                {
                    const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet, false).ToLower();

                }
                else
                {
                    const string defaultAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                    result = Vigenere(firstText.ToUpper(), keyFieldSlogan.Text.ToUpper(), defaultAlphabet, false).ToLower();
                }
            }
            finalTextBox.Text = result;
        }
        
      
        private string Decrypt(string rawText, Direction dir, string alp, int A, int B, int C = -1)
        {
            var fullAlfabet = alp;
            var letterQty = fullAlfabet.Length;
            var resultText = "";
            for (int i = 0; i < rawText.Length; i++)
            {
                int shiftNumb = C == -1 ? A * i  + B : A * i*i + B * i + C;
                var raw = rawText[i];
                var index = fullAlfabet.IndexOf(raw);
                if (index < 0)
                {
                    resultText += raw.ToString();
                }
                else
                {
                    var codeIndex = (letterQty + index - shiftNumb) % letterQty;
                    if (codeIndex<0)
                    {
                        codeIndex = letterQty + codeIndex;
                    }
                    resultText += fullAlfabet[codeIndex];
                }
            }
            return resultText;
        }

        //other buttons
        private void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            currentFileName = null;
            rawTextBox.Text = String.Empty;
            finalTextBox.Text = String.Empty;
        }
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentFileName == null)
            {
                currentFileName = "../../resultFile.txt";
            }
            WriteToFile(currentFileName);
            MessageBox.Show($"{currentFileName}\nSuccessfully saved!");
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                rawTextBox.Text = File.ReadAllText(openFileDialog.FileName);
            currentFileName = openFileDialog.FileName;
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This text Caesar Cipher encryption application was created by Ksenia Klakovych.");
        }

        private void PrintFileButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.ShowDialog();
        }
        //Vigenere
        //генерування повторюваного пароля

        private string GetRepeatKey(string s, int n)
        {
            var p = s;
            while (p.Length < n)
            {
                p += p;
            }

            return p.Substring(0, n);
        }
        private string Vigenere(string text, string password, string alph, bool encrypting = true)
        {
            string letters = alph;

            var gamma = GetRepeatKey(password, text.Length);
            var retValue = "";
            var q = letters.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = letters.IndexOf(text[i]);
                var codeIndex = letters.IndexOf(gamma[i]);
                if (letterIndex < 0)
                {
                    //якщо літера не знайдена, додаємо її в незмінному вигляді
                    retValue += text[i].ToString();
                }
                else
                {
                    retValue += letters[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                }
            }

            return retValue;
        }

        private void SwitchTextButton_Click(object sender, RoutedEventArgs e)
        {
            string first = rawTextBox.Text;
            rawTextBox.Text = finalTextBox.Text;
            finalTextBox.Text = first;
        }
        //    private static readonly ILog _log = LogManager.GetLogger(typeof(KasiskiAttack).Namespace);

        //    public static string AttackWithKeyLength(string cipherText, int keyLength)
        //    {
        //        var cipher;
        //        var allFreqScores = new List<Dictionary<char, int>>();
        //        for (var i = 1; i < keyLength + 1; i++)
        //        {
        //            var nthLetters = Kasiski.GetNthSubkeysLetters(i, keyLength, cipherText.ToUpper());

        //            var frequencyScore = new Dictionary<char, int>();
        //            foreach (var letter in Constants.Letters)
        //            {
        //                var decryptedText = cipher.Decipher(nthLetters, Char.ToString(letter));

        //                var score = Frequency.EnglishFreqMatchScore(decryptedText);
        //                frequencyScore.Add(letter, score);
        //            }

        //            //Sort the dictionary by factor ocurrences descending
        //            var sortedCommonFactors = (from entry in frequencyScore orderby entry.Value descending select entry)
        //                .Take(4)
        //                .ToDictionary(pair => pair.Key, pair => pair.Value);

        //            allFreqScores.Add(sortedCommonFactors);
        //        }
        //        //Print our possible keys
        //        var position = 1;
        //        var possibleKeys = new List<char[]>();
        //        foreach (var freq in allFreqScores)
        //        {
        //            _log.InfoFormat("Possible letters for letter {0} of the key: ", position);
        //            var possibleKeyOutput = new StringBuilder();
        //            var keyStore = new char[freq.Keys.Count];
        //            var keyPosition = 0;
        //            foreach (var keyPossible in freq.Keys)
        //            {
        //                possibleKeyOutput.AppendFormat("{0}\t", keyPossible);
        //                keyStore[keyPosition] = keyPossible;
        //                keyPosition++;
        //            }
        //            possibleKeys.Add(keyStore);
        //            _log.InfoFormat(possibleKeyOutput.ToString());
        //            position++;
        //        }

        //        var keyCombinations = Combinator.Combinations(possibleKeys);

        //        var keyFound = false;
        //        var key = string.Empty;

        //        //Go through the possible keys with the given freqency and determine if they produce
        //        //an English sentence.
        //        Parallel.ForEach(keyCombinations, (possibleKey, loopState) =>
        //        {
        //            //Stop processing if we find the possible key
        //            if (keyFound)
        //                loopState.Break();

        //            _log.DebugFormat("Attempting with key: {0}", possibleKey);

        //            var decryptAttempt = cipher.Decipher(cipherText, possibleKey.ToString());

        //            if (!English.IsEnglish(decryptAttempt)) return;

        //            _log.InfoFormat("Found Possible Decryption Key: {0}", possibleKey);
        //            keyFound = true;
        //            key = possibleKey.ToString();
        //        }
        //        );

        //        return key;
        //    }
        //}
        //public static class Constants
        //{
        //    public static readonly Regex NonlettersPattern = new Regex("[^A-Z]", RegexOptions.Compiled);

        //    /// <summary>
        //    ///     Number of Most Frequent Letters to extract during a Kasiski Analysis
        //    /// </summary>
        //    public const int NumMostFreqLetters = 4;

        //    /// <summary>
        //    ///     Maximum key length supported
        //    /// </summary>
        //    public const int MaxKeyLength = 16;

        //    /// <summary>
        //    ///     English Alphabet
        //    /// </summary>
        //    public const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //    /// <summary>
        //    ///     Frequent English Letters in order of most frequent
        //    ///     From: http://en.wikipedia.org/wiki/Letter_frequency
        //    /// </summary>
        //    public const string ETAOIN = "ETAOINSHRDLCUMWFGYPBVKJXQZ";

        //    /// <summary>
        //    ///     Freqency of English Letters
        //    ///     From: http://en.wikipedia.org/wiki/Letter_frequency
        //    /// </summary>
        //    public static Dictionary<char, double> EnglishLetterFreq = new Dictionary<char, double>()
        //    {
        //        {'E',12.70},
        //        {'T', 9.06},
        //        {'A', 8.17},
        //        {'O', 7.51},
        //        {'I', 6.97},
        //        {'N', 6.75},
        //        {'S', 6.33},
        //        {'H', 6.09},
        //        {'R', 5.99},
        //        {'D', 4.25},
        //        {'L', 4.03},
        //        {'C', 2.78},
        //        {'U', 2.76},
        //        {'M', 2.41},
        //        {'W', 2.36},
        //        {'F', 2.23},
        //        {'G', 2.02},
        //        {'Y', 1.97},
        //        {'P', 1.93},
        //        {'B', 1.29},
        //        {'V', 0.98},
        //        {'K', 0.77},
        //        {'J', 0.15},
        //        {'X', 0.15},
        //        {'Q', 0.10},
        //        {'Z', 0.07}
        //    };
        //}
        //public static class Combinator
        //{
        //    /// <summary>
        //    ///     Takes in the list of characters for each key position and returns all possible valid combinations
        //    /// </summary>
        //    /// <param name="letters"></param>
        //    /// <returns></returns>
        //    public static List<StringBuilder> Combinations(List<char[]> letters)
        //    {
        //        //Take the top most character array which will be processed at this level in the recursive call stack
        //        var myChars = letters[0];
        //        letters.RemoveAt(0);
        //        var myStringSize = letters.Count + 1;
        //        var output = new List<StringBuilder>();

        //        //This happens if we're at the deepest in the recursion (last letter)
        //        if (myStringSize == 1)
        //        {
        //            foreach (var c in myChars)
        //            {
        //                var newString = new StringBuilder(myStringSize);
        //                newString.Append(c);
        //                output.Add(newString);
        //            }
        //            return output;
        //        }

        //        //Take combinations returned by deeper in the stack and append this levels combinations
        //        foreach (var s in Combinations(letters))
        //        {
        //            foreach (var c in myChars)
        //            {
        //                var newString = new StringBuilder(myStringSize);
        //                newString.Append(c);
        //                newString.Append(s);
        //                output.Add(newString);
        //            }
        //        }
        //        return output;
        //    }
        //}
        //public static class Frequency
        //{
        //    /// <summary>
        //    ///     Returns an empty dictionary of all english letters with counts of zero
        //    /// </summary>
        //    /// <returns></returns>
        //    private static Dictionary<char, int> BuildNewFrequencyDictionary()
        //    {
        //        return Constants.Letters.ToDictionary(c => c, c => 0);
        //    }

        //    /// <summary>
        //    ///     Returns a dictionary containing characters and the count of occurances of those characters in the specified message
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static Dictionary<char, int> GetLetterCount(string message)
        //    {
        //        var output = BuildNewFrequencyDictionary();
        //        foreach (var c in message.ToUpper().Where(output.ContainsKey))
        //        {
        //            output[c] += 1;
        //        }

        //        return output;
        //    }

        //    /// <summary>
        //    ///     Returns the character frequency order by highest freqency and ETAOIN order
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static string GetFrequencyOrder(string message)
        //    {
        //        var output = new StringBuilder(26);

        //        //Get Letter Counts
        //        var letterFreqency = GetLetterCount(message);

        //        //Group Letters by common ount
        //        var commonFrequencies = letterFreqency.GroupBy(r => r.Value).OrderByDescending(key => key.Key)
        //              .ToDictionary(t => t.Key, t => t.Select(r => r.Key).ToList());

        //        var sortedCommonFrequencies = new Dictionary<int, List<char>>(commonFrequencies);

        //        //Make Letter groups follow ETAOIN format
        //        foreach (var commonFrequency in commonFrequencies)
        //        {
        //            var sorted = Constants.ETAOIN.Where(c => commonFrequency.Value.Contains(c)).ToList();
        //            sortedCommonFrequencies[commonFrequency.Key] = sorted;
        //        }

        //        //Go through now and build the output string
        //        foreach (var c in sortedCommonFrequencies.SelectMany(commonFrequency => commonFrequency.Value))
        //        {
        //            output.Append(c);
        //        }

        //        return output.ToString();
        //    }

        //    /// <summary>
        //    ///     Creates a match score based on the occurange number of the Top 6 Most Frequent & The Bottom 6 Least Frequent characters
        //    ///     in the English language when compared to the specified message.
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static int EnglishFreqMatchScore(string message)
        //    {
        //        var frequencyOrder = GetFrequencyOrder(message);

        //        var top6Freq = frequencyOrder.Substring(0, 6);
        //        var bottom6Freq = frequencyOrder.Substring(frequencyOrder.Length - 7, 6);

        //        var top6ETAOIN = Constants.ETAOIN.Substring(0, 6);
        //        var bottom6ETAOIN = Constants.ETAOIN.Substring(Constants.ETAOIN.Length - 7, 6);

        //        return top6Freq.Count(top6ETAOIN.Contains) + bottom6Freq.Count(bottom6ETAOIN.Contains);
        //    }
        //}
        //public static class Kasiski
        //{

        //    /// <summary>
        //    ///     Returns every Nth letter for each keyLength set of letters in text.
        //    /// </summary>
        //    /// <param name="n"></param>
        //    /// <param name="keyLength"></param>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static string GetNthSubkeysLetters(int n, int keyLength, string message)
        //    {
        //        //Apply RegEx to message to we're not looking at non-alpha characters
        //        var filteredMessage = Constants.NonlettersPattern.Replace(message, string.Empty);

        //        var outputBuffer = new StringBuilder();
        //        for (var i = n - 1; i < filteredMessage.Length; i += keyLength)
        //        {
        //            outputBuffer.Append(filteredMessage[i]);
        //        }

        //        return outputBuffer.ToString();
        //    }

        //    /// <summary>
        //    ///     Finds 3-5 character repeating sequences
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static Dictionary<string, List<int>> FindRepeatSequencesSpacings(string message)
        //    {
        //        var output = new Dictionary<string, List<int>>();

        //        //Apply RegEx to message to we're not looking at non-alpha characters
        //        var filteredMessage = Constants.NonlettersPattern.Replace(message.ToUpper(), string.Empty).ToUpper();

        //        //Sets the length of sequences we'll search for
        //        for (var i = 3; i < 6; i++)
        //        {
        //            //Sets our sequence start
        //            for (var j = 0; j < filteredMessage.Length - i; j++)
        //            {
        //                //Fing other matching sequences in the string
        //                var currentSequence = filteredMessage.Substring(j, i);

        //                var sequenceFoundPosition = filteredMessage.IndexOf(currentSequence, j + 1, StringComparison.Ordinal);
        //                while (sequenceFoundPosition > 0)
        //                {
        //                    //Calulate the Lenth Apart
        //                    var lengthApart = (sequenceFoundPosition + i) - (j + i);

        //                    //Mark the position we found it in and increment the number of times we saw it
        //                    if (!output.ContainsKey(currentSequence))
        //                        output.Add(currentSequence, new List<int>());
        //                    if (!output[currentSequence].Contains(lengthApart))
        //                        output[currentSequence].Add(lengthApart);

        //                    //Find the next instance
        //                    sequenceFoundPosition = filteredMessage.IndexOf(currentSequence, sequenceFoundPosition + 1, StringComparison.Ordinal);
        //                }
        //            }
        //        }
        //        return output;
        //    }

        //    /// <summary>
        //    ///     Returns a list of useful factors of num. By "useful" we mean factors less than MAX_KEY_LENGTH + 1.
        //    /// </summary>
        //    /// <param name="number"></param>
        //    /// <returns></returns>
        //    public static List<int> GetUsefulFactors(int number)
        //    {
        //        var output = new List<int>();

        //        for (var i = 2; i <= Constants.MaxKeyLength; i++)
        //        {
        //            if (number % i == 0)
        //                output.Add(i);
        //        }

        //        if (output.Contains(1))
        //            output.Remove(1);

        //        return output;
        //    }

        //    /// <summary>
        //    ///     
        //    /// </summary>
        //    /// <param name="sequenceFactors"></param>
        //    /// <returns></returns>
        //    public static Dictionary<int, int> GetMostCommonFactors(List<List<int>> sequenceFactors)
        //    {
        //        var output = new Dictionary<int, int>();

        //        foreach (var factor in sequenceFactors.SelectMany(seqFactor => seqFactor))
        //        {
        //            if (!output.ContainsKey(factor))
        //            {
        //                output.Add(factor, 1);
        //            }
        //            else
        //            {
        //                output[factor]++;
        //            }
        //        }

        //        return output;
        //    }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="cipherText"></param>
        //    /// <returns></returns>
        //    public static List<int> KasiskiExamination(string cipherText)
        //    {
        //        //First we get the sequence spacing
        //        var seqSpacing = FindRepeatSequencesSpacings(cipherText);

        //        //Find the Factors
        //        //Unrolled
        //        //var seqList = new List<List<int>>();
        //        //foreach (var seq in seqSpacing.Values)
        //        //{
        //        //    foreach (var spacing in seq)
        //        //    {
        //        //        seqList.Add(GetUsefulFactors(spacing));
        //        //    }
        //        //}

        //        var seqList = (from seq in seqSpacing.Values from spacing in seq select GetUsefulFactors(spacing)).ToList();

        //        //Find the most common factors
        //        var likelyKeyLengths = GetMostCommonFactors(seqList);

        //        //Sort the dictionary by factor ocurrences descending
        //        var sortedCommonFactors = (from entry in likelyKeyLengths orderby entry.Value descending select entry)
        //                 .Take(3)
        //                 .ToDictionary(pair => pair.Key, pair => pair.Value);

        //        //Take the top 3 and return those
        //        return sortedCommonFactors.Keys.ToList();

        //    }
        //}
        //public static class English
        //{
        //    private static readonly ILog _log = LogManager.GetLogger(typeof(English).Namespace);
        //    private const int WordPercentage = 40;
        //    private const int LetterPercentage = 70;

        //    public static readonly Dictionary<int, List<string>> SortedDictionary = new Dictionary<int, List<string>>();

        //    /// <summary>
        //    ///     Static Constructor
        //    /// 
        //    ///     Loads the dictionary into memory
        //    /// </summary>
        //    static English()
        //    {
        //        _log.Debug("Loading Dictionary...");
        //        using (var sr = new StreamReader(
        //            Assembly.GetExecutingAssembly().GetManifestResourceStream(
        //                "VigenereSolver.Library.Resources.dictionary.txt")))
        //        {
        //            while (sr.Peek() >= 0)
        //            {
        //                var word = sr.ReadLine();

        //                if (string.IsNullOrEmpty(word))
        //                    break;

        //                if (!SortedDictionary.ContainsKey(word.Length))
        //                {
        //                    SortedDictionary.Add(word.Length, new List<string>() { word });
        //                    continue;
        //                }

        //                SortedDictionary[word.Length].Add(word);
        //            }
        //        }
        //        _log.Debug("Dictionary Loaded!");
        //    }

        //    /// <summary>
        //    ///     Evaluates dictionary words against the specified message
        //    /// 
        //    ///     This works when the input message doesn't have spacing
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static double GetDictionaryWordsInStringRatio(string message)
        //    {
        //        var messageWords = message.ToUpper();

        //        //Order the dictionary by largest words first, so we'll start there
        //        foreach (var dict in SortedDictionary.OrderByDescending(k => k.Key))
        //        {
        //            foreach (var word in dict.Value)
        //            {
        //                messageWords = message.Replace(word, string.Empty);
        //            }
        //        }

        //        return (double)messageWords.Length / message.Length;
        //    }

        //    /// <summary>
        //    ///     The number of words in the string that appear in the loaded dictionary
        //    /// 
        //    ///     This works when the input message has spacing to denote words
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static double GetStringWordsInDictionaryRatio(string message)
        //    {
        //        var messageWords = message.ToUpper().Split(' ');
        //        var count = messageWords.Count(word => SortedDictionary.ContainsKey(word.Length) && SortedDictionary[word.Length].Contains(word));

        //        return (double)count / messageWords.Length;
        //    }


        //    /// <summary>
        //    ///     Determines if the specified message is an English string using a dictionary
        //    ///     and English patterns/sentence structure
        //    /// </summary>
        //    /// <param name="message"></param>
        //    /// <returns></returns>
        //    public static bool IsEnglish(string message)
        //    {
        //        var sw = new Stopwatch();
        //        var englishWordCount2 = GetStringWordsInDictionaryRatio(message) * 100;
        //        var letterCount = message.Length -
        //                          (message.Length - Constants.NonlettersPattern.Replace(message.ToUpper(), string.Empty).Length);

        //        var letterPercentage = ((double)letterCount / message.Length) * 100;

        //        return englishWordCount2 >= WordPercentage && letterPercentage >= LetterPercentage;
        //    }
        //}
    }
}
