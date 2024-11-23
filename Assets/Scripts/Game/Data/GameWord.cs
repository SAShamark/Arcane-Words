using System;

namespace Game.Data
{
    [Serializable]
    public class GameWord
    {
        public string Word;
        public string Description;
        public string Category;

        public GameWord(string word, string description, string category)
        {
            Word = word;
            Description = description;
            Category = category;
        }
    }
}