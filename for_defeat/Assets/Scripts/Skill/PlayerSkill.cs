using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    [SerializeField] private float angerGaugeUsage;
    //스킬 데미지
    [SerializeField] protected float skilldamage;
    public GameObject origin;
    public GameObject target;

    //스킬 선딜레이
    [SerializeField] protected float BDelay;
    //스킬 후딜레이
    [SerializeField] protected float ADelay;
    public bool isContinuable = true;

    public virtual IEnumerator OnSkillActive(int skillIdx)
    {
        if(GameManager.Instance.player.CurAngerGauge < angerGaugeUsage) yield break;
        yield return StartCoroutine(EBDelay());
        GameManager.Instance.player.UseAngergauge(angerGaugeUsage);
        yield return StartCoroutine(_OnSkillActive());
        yield return StartCoroutine(EADelay());
        StartCoroutine(GameManager.Instance.player.ECoolDownDic[(PlayerController.EPlayerSkill)skillIdx]);
    }

    public abstract IEnumerator _OnSkillActive();

    protected IEnumerator EBDelay()
    {
        float curBDelay = BDelay;
        while(curBDelay >= 0)
        {
            if(!isContinuable) yield break;
            curBDelay -= Time.deltaTime;
            yield return null;
        }
    }

    protected IEnumerator EADelay()
    {
        origin.GetComponent<UnitBehaviour>().isInDelay = true;
        float curADelay = ADelay;
        while(curADelay >= 0)
        {
            if(!isContinuable) yield break;
            curADelay -= Time.deltaTime;
            yield return null;
        }
        origin.GetComponent<UnitBehaviour>().isInDelay = false;
    }
}
