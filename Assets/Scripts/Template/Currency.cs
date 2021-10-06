using Template.Tools;

namespace Template
{
    public enum CurrencyType
    {
        Coin,
        Video,
        Gem,
    }

    public class Currency
    {
        public string Type;
        public PlayerPrefsProperty<int> Current;

        public Currency(string type, int defaultAmount)
        {
            Type = type;
            Current = new PlayerPrefsProperty<int>($"{Type}Amount", defaultAmount);
        }
    }
}