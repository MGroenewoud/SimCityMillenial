using UnityEngine;

public class Need
{
    public float NextDecay;
    public int Weight = 0;
    public bool IsCritical => Weight > GameSettings.CriticalNeedFactor;

    private float DecayRate;
    

    public Need(float decayRate)
    {
        DecayRate = decayRate;
        NextDecay = Time.time + DecayRate;
    }

    public void Decay()
    {
        Weight++;
        NextDecay = Time.time + DecayRate;
    }
}

public enum NeedType
{
    None = 0,
    Home = 1,
    Shopping = 2,
    Entertainment = 3,
}