using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GamePlayConfig", menuName = "ScriptableObjects/GamePlay", order = 1)]
    public class GamePlayConfig : ScriptableObject
    {
        [SerializeField]
        private float _secondsToCheckWord = 1.5f;

        public float SecondsToCheckWord => _secondsToCheckWord;
    }
}