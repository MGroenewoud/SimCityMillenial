using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonNeeds : MonoBehaviour
{
    public Dictionary<NeedType, Need> Needs;

    void Awake()
    {
        Needs = new Dictionary<NeedType, Need>();
        Needs.Add(NeedType.Home, new Need(NeedType.Home));
        Needs.Add(NeedType.Shopping, new Need(NeedType.Shopping));
        Needs.Add(NeedType.Entertainment, new Need(NeedType.Entertainment));
    }

    void Update()
    {
        foreach(var need in Needs)
        {
            if(need.Value.NextDecay > Time.time)
                need.Value.Decay();
        }
    }

    public NeedType GetBiggestNeed(HashSet<NeedType> exclude)
    {
        return Needs.Where(n => !exclude.Contains(n.Value.Type)).Select(n => n.Value).OrderBy(n => n.Weight).Last().Type;
    }

    public bool HasCriticalNeed()
    {
        return Needs.Any(n => n.Value.IsCritical);
    }
}