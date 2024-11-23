using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "GamePlayConfig", menuName = "ScriptableObjects/GamePlay", order = 1)]
    public class GamePlayConfig : ScriptableObject
    {
        [SerializeField]
        private float _secondsToCheckWord = 1.5f;

        [SerializeField]
        private int _startHintCount = 10;

        [SerializeField]
        private int _coinCountForUnlockedWord = 1;

        [SerializeField]
        private int _hintCost = 1;


        public float SecondsToCheckWord => _secondsToCheckWord;
        public int StartHintCount => _startHintCount;
        public int CoinCountForUnlockedWord => _coinCountForUnlockedWord;
        public int HintCost => _hintCost;
    }
}