using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDash : Skill
{
    [SerializeField] private float dashLength;
    [SerializeField] private float dashSec;

    public override IEnumerator _OnSkillActive()
    {
        Vector3 dir = target.transform.position - origin.transform.position;
        float curDashSec = dashSec;
        while(curDashSec > 0)
        {
            if(!isContinuable) yield break;
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