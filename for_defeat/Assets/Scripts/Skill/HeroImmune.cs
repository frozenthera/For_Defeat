using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroImmune : HeroSkill
{
    [SerializeField] private float buffTime;
    [SerializeField] private float speedBuffPercent;
    public override IEnumerator _OnSkillActive()
    {
        HeroBehaviour HB = origin.GetComponent<HeroBehaviour>();
        HB.StartCoroutine(HB.SpeedBuff(speedBuffPercent, buffTime));
        HB.StartCoroutine(HB.ImmuneBuff(buffTime));
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}