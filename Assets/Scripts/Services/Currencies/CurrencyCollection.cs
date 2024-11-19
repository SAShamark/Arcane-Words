using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Services.Currencies
{
    [CreateAssetMenu(fileName = "CurrencyCollection", menuName = "ScriptableObjects/UI/Sprites/CurrencyCollection")]
    public class CurrencyCollection : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<CurrencyType, Sprite> _currencySprites;

        public Dictionary<CurrencyType, Sprite> CurrencySprites => _currencySprites;
    }
}