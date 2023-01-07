using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHeal : HeroSkill
{
    [SerializeField] private float totalHeal;
    [SerializeField] private float healPerTick;
    [SerializeField] private GameObject HealEffect;

    public override IEnumerator _OnSkillActive()
    {
        Debug.Log("Heal Start");
        HealEffect.SetActive(true);
        HealEffect.transform.position = origin.transform.position;
        HeroBehaviour HB = target.GetComponent<HeroBehaviour>();
        float leftHeal = totalHeal;
        while(leftHeal >= 0)
        {
            if(!isContinuable)
            {
                HealEffect.SetActive(false);        
                yield break;
            } 
            HB.GetHeal(healPerTick);
            leftHeal -= healPerTick;
            yield return new WaitForSeconds(.5f);
        }
        HealEffect.SetActive(false);
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}