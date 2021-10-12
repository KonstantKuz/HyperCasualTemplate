namespace Templates.Tools
{
    public static class MathExtension
    {
        public static float Remap(float value, float from1, float to1, float from2, float to2) 
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static bool IsInRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }
    }
}