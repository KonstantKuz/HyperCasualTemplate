using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Templates
{
    public class PlayerWallet : Singleton<PlayerWallet>
    {
        [SerializeField] private int defaultCoins;
        public Action OnCurrencyChanged;
    
        private Dictionary<string, Currency> _currencies;

        private Dictionary<string, Currency> Currencies
        {
            get
            {
                if (_currencies == null)
                {
                    InitializeCurrencies();
                }

                return _currencies;
            }
        }
    
        private void Awake()
        {
            OnCurrencyChanged?.Invoke();
        }

        private void InitializeCurrencies()
        {
            _currencies = new Dictionary<string, Currency>();
            AddNewCurrencyType(CurrencyType.Coin.ToString(), defaultCoins);
            AddNewCurrencyType(CurrencyType.Gem.ToString(), 0);
        }
        
        private void AddNewCurrencyType(string type, int defaultAmount)
        {
            Currencies.Add(type, new Currency(type, defaultAmount));
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
            if (IsVideoCurrencyRequested(type))
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
            if (IsVideoCurrencyRequested(type))
            {
                ExOnVideoCurrencyRequested();
            }

            if (IsNewCurrencyTypeRequested(type))
            {
                AddNewCurrencyType(type, 0);
            }
            
            return Currencies[type];
        }
        
        private bool IsNewCurrencyTypeRequested(string type)
        {
            return !Currencies.ContainsKey(type);
        }

        private bool IsVideoCurrencyRequested(string type)
        {
            return type == CurrencyType.Video.ToString();
        }

        private void ExOnVideoCurrencyRequested()
        {
            throw new Exception(
                "If the price of the product is a video, " +
                "then the required number of videos for the purchase of the product " +
                "should automatically decrease after each viewing of the video for this product. Implemented in Universal Shop Tab.");
        }
    }
}