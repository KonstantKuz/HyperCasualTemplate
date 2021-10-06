using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Template
{
    public class PlayerWallet : Singleton<PlayerWallet>
    {
        [SerializeField] private int defaultCoins;
        public Action OnCurrencyChanged;
    
        private List<Currency> _currencies;
    
        private void Awake()
        {
            _currencies = new List<Currency>
            {
                new Currency(CurrencyType.Coin.ToString(), defaultCoins),
            };
            
            OnCurrencyChanged?.Invoke();
        }

        public void IncreaseCurrency(string type, int value)
        {
            ChangeCurrency(type, value);
        }

        public void DecreaseCurrency(string type, int value)
        {
            ChangeCurrency(type, -value);
        }

        private void ChangeCurrency(string type, int value)
        {
            int currencyValue = GetCurrencyCurrentValue(type);
            currencyValue += value;
            if (currencyValue < 0 && value < 0)
            {
                throw new Exception($"Player has no {type} currency. Check currency with HasCurrency(type, amount).");
            }

            GetCurrency(type).Current.Value += value;

            OnCurrencyChanged?.Invoke();
        }

        public bool HasCurrency(string type, int amount)
        {
            if (type == CurrencyType.Video.ToString())
            {
                return true;
            }

            return GetCurrencyCurrentValue(type) >= amount;
        }

        public int GetCurrencyCurrentValue(string type)
        {
            return GetCurrency(type).Current.Value;
        }
        
        private Currency GetCurrency(string type)
        {
            return _currencies.First(currency => currency.Type == type);
        }
    }
}