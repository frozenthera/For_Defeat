using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDash : HeroSkill
{
    [SerializeField] private float dashLength;
    [SerializeField] private float dashSec;

    public override IEnumerator _OnSkillActive()
    {
        Vector3 dir = target.transform.position - origin.transform.position;
        float curDashSec = dashSec;
        while(curDashSec > 0)
        {
            if(GameManager.Instance.hero.isInKnuckBack) 
            {
                yield break;
            }
            origin.transform.position += dir.normalized * dashLength / dashSec * Time.deltaTime;
            curDashSec -= Time.deltaTime;
            yield return null;
        }
    }

    public override float CalcDamage()
    {
        return 0;
    }
}