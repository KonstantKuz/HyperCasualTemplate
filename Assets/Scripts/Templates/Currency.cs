using Templates.Tools;

namespace Templates
{
    public enum CurrencyType
    {
        Coin,
        Video,
        Gem,
    }

    public class Currency
    {
        public readonly string Type;
        public readonly PlayerPrefsProperty<int> Current;

        public Currency(string type, int defaultAmount)
        {
            Type = type;
            Current = new PlayerPrefsProperty<int>($"{Type}Amount", defaultAmount);
        }
    }
}