namespace Templates.Tools
{
    public static class MathExtension
    {
        public static float Remap(float value, float from1, float to1, float from2, float to2) 
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        // public void SmoothFloat(ref float valueFrom, float to, float duration)
        // {
        //     StartCoroutine(SmoothFloat());
        //     IEnumerator SmoothFloat()
        //     {
        //         float timer = 0;
        //         while (timer < duration)
        //         {
        //             timer += Time.deltaTime;
        //             valueFrom = Mathf.MoveTowards(valueFrom, to, Time.deltaTime);
        //             yield return null;
        //         }
        //     }
        // }
    
        // private void CalculateByPercentage(float currentValue, float fromMin, float fromMax, float toMin, float toMax)
        // {
        //     // пример из 24 прототипа кольцо ножей
        //     // зависимость радиуса (х) в процентном соотношении (у)
        //     // x = текущий радиус; y = текущая заполненность кольца мечами в процентах;
        //     // по формуле (y1-y2)*x + (x2-x1)*y + (x1*y2 - x2*y1) = 0;
        //     // где y1 = 0%; y2 = 100%;
        //     // x1 = ringRadiusBorders.x; x2 = ringRadiusBorders.y;
        //     // y = knives.Count / maxKnivesCount;
        //     // тогда x = (- (x2-x1)*y - (x1*y2 - x2*y1)) / (y1 - y2);
        //
        //     float zeroPercent = 0;
        //     float fullPercent = 1;
        //     float minRadius = ringRadiusBorders.x;
        //     float maxRadius = ringRadiusBorders.y;
        //     float currKnivesFillPercent = (knives.Count / maxKnivesCount);
        //     float currentRadius = (- (maxRadius - minRadius) * currKnivesFillPercent - 
        //                            (minRadius * fullPercent - maxRadius * zeroPercent)) 
        //                           / (zeroPercent - fullPercent);
        //
        //     currentRingRadius = currentRadius;
        // }
    }
}