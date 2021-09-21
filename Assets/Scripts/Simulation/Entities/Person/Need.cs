using UnityEngine;

public class Need
{
    public NeedType Type;
    public int Weight = 0;

    private float DecayRate;
    private float NextDecay;

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
        if (Time.time > NextDecay)
        {
            Weight++;
            NextDecay = Time.time + DecayRate;
        }
    }
}

public enum NeedType
{
    Home = 0,
    Shopping = 1,
    Entertainment = 2,
}