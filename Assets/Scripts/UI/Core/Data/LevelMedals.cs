using System;
using UnityEngine;

namespace UI.Core.Data
{
    [Serializable]
    public class LevelMedals
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private Vector2 _percentToMedal;

        public Sprite Sprite => _sprite;

        public Vector2 PercentToMedal => _percentToMedal;
    }
}