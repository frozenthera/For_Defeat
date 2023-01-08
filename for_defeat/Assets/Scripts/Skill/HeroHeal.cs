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
        float leftHeal = totalHeal;
        while(leftHeal >= 0)
        {
            if(GameManager.Instance.hero.isInKnuckBack)
            {
                HealEffect.SetActive(false);        
                yield break;
            } 
            GameManager.Instance.hero.GetHeal(healPerTick);
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