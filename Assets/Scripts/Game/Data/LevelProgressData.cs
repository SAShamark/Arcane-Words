using System.Collections.Generic;

namespace Game.Data
{
    public class LevelProgressData
    {
        public string Word { get; private set; }
        public float GameTime { get; private set; }
        public List<string> UnlockedWords { get; private set; }
        public List<string> WordsWithHint { get; private set; }

        public LevelProgressData(string word, List<string> unlockedWords, List<string> wordsWithHint, float gameTime)
        {
            Word = word;
            GameTime = gameTime;
            UnlockedWords = unlockedWords;
            WordsWithHint = wordsWithHint;
        }
    }
}