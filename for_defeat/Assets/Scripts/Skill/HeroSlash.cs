using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSlash : HeroSkill
{
    public GameObject SlashObject;
    [SerializeField] private float angerGaugeGain;
    public override IEnumerator _OnSkillActive()
    {
        GameObject go = Instantiate(SlashObject, origin.transform.position, Quaternion.identity);
        go.GetComponent<SlashObject>().origin = origin;
        go.GetComponent<SlashObject>().target = target;
        go.GetComponent<SlashObject>().damage = CalcDamage();
        go.GetComponent<SlashObject>().angerGaugeGain = angerGaugeGain;
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}