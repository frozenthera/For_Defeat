using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHeal : Skill
{
    [SerializeField] private float totalHeal;
    [SerializeField] private float healPerTick;
    public override IEnumerator _OnSkillActive()
    {
        HeroBehaviour HB = target.GetComponent<HeroBehaviour>();
        float leftHeal = totalHeal;
        while(leftHeal >= 0)
        {
            if(!isContinuable) yield break;
            HB.GetHeal(healPerTick);
            leftHeal -= healPerTick;
            yield return new WaitForSeconds(.5f);
        }
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}