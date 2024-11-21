using System.Collections.Generic;
using UnityEngine;

namespace UI.Core
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "ScriptableObjects/Levels", order = 1)]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField]
        private TextAsset _levelsFileData;

        [SerializeField]
        private int _levelsOnPage = 9;
        
        [SerializeField]
        private List<LevelMedals> _levelMedals;

        public TextAsset LevelsFileData => _levelsFileData;

        public int LevelsOnPage => _levelsOnPage;

        public List<LevelMedals> LevelMedals => _levelMedals;
    }
}