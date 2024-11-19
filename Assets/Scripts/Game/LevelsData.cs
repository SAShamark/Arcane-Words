using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class LevelsData
    {
        public List<string> Levels;
        public List<GameWord> GameWords;
        public Dictionary<string, List<GameWord>> LevelsGameWords;

        public LevelsData()
        {
            Levels = new List<string>();
            GameWords = new List<GameWord>();
            LevelsGameWords = new Dictionary<string, List<GameWord>>();
        }

        public LevelsData(List<string> levels, List<GameWord> allGameWords)
        {
            Levels = levels;
            GameWords = allGameWords;
            LevelsGameWords = new Dictionary<string, List<GameWord>>();
        }

        public void InitializeLevelsGameWords(Dictionary<string, List<GameWord>> levelsGameWords)
        {
            LevelsGameWords = levelsGameWords ?? new Dictionary<string, List<GameWord>>();
        }
    }
}