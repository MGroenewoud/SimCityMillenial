using UnityEngine;

public class Need
{
    public NeedType Type;
    public float NextDecay;
    public int Weight = 0;
    public bool IsCritical => Weight > GameSettings.CriticalNeedFactor;

    private float DecayRate;
    

    public Need(NeedType type)
    {
        Type = type;

        switch (Type)
        {
            case NeedType.Home:
                DecayRate = GameSettings.HomeDecayRate;
                break;
            case NeedType.Shopping:
                DecayRate = GameSettings.FoodDecayRate;
                break;
            case NeedType.Entertainment:
                DecayRate = GameSettings.EntertainmentDecayRate;
                break;
        }
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
    Home = 0,
    Shopping = 1,
    Entertainment = 2,
}