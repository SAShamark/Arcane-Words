using System.Collections.Generic;
using Game.Data;

namespace Game
{
    public class WordsBuilder
    {
        private readonly List<GameWord> _gameWords;

        public WordsBuilder(List<GameWord> gameWords)
        {
            _gameWords = gameWords;
        }

        public Dictionary<string, List<GameWord>> BuildWordsByLevel(List<string> levels)
        {
            var wordsGroupedByLevel = new Dictionary<string, List<GameWord>>();

            foreach (string levelName in levels)
            {
                List<GameWord> validWordsForLevel = GetValidWordsForLevel(levelName);
                wordsGroupedByLevel.Add(levelName, validWordsForLevel);
            }

            return wordsGroupedByLevel;
        }

        private List<GameWord> GetValidWordsForLevel(string levelName)
        {
            Dictionary<char, int> levelLetterCounts = CountLetters(levelName);
            var validWords = new List<GameWord>();
            var seenWords = new HashSet<string>();

            foreach (GameWord gameWord in _gameWords)
            {
                if (CanBuildWordFromLetters(gameWord.Word, levelLetterCounts) && seenWords.Add(gameWord.Word))
                {
                    validWords.Add(gameWord);
                }
            }

            return validWords;
        }

        private Dictionary<char, int> CountLetters(string text)
        {
            var letterCounts = new Dictionary<char, int>();

            foreach (char letter in text)
            {
                if (!letterCounts.TryAdd(letter, 1))
                    letterCounts[letter]++;
            }

            return letterCounts;
        }

        private bool CanBuildWordFromLetters(string word, Dictionary<char, int> availableLetters)
        {
            Dictionary<char, int> wordLetterCounts = CountLetters(word);

            foreach (KeyValuePair<char, int> letter in wordLetterCounts)
            {
                if (!availableLetters.TryGetValue(letter.Key, out int availableCount) || availableCount < letter.Value)
                    return false;
            }

            return true;
        }
    }
}