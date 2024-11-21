using System;
using UnityEngine;

namespace UI.Core
{
    [Serializable]
    public class LevelMedals
    {
        [SerializeField]
        private Sprite _medal;

        [SerializeField]
        private Vector2 _percentToMedal;

        public Sprite Medal => _medal;

        public Vector2 PercentToMedal => _percentToMedal;
    }
}