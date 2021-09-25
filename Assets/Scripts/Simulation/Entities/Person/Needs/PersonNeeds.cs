using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonNeeds : MonoBehaviour
{
    public Dictionary<NeedType, Need> Needs;

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
        return Needs.Where(n => !exclude.Contains(n.Key)).OrderBy(n => n.Value.Weight).Last().Key;
    }

    public bool HasCriticalNeed()
    {
        return Needs.Any(n => n.Value.IsCritical);
    }

    public void SetNeeds(Dictionary<NeedType, Need> needs)
    {
        Needs = needs;
    }
}