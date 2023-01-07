using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    //스킬 스프라이트
    [SerializeField] protected Sprite skillSprite;
    //스킬 이름
    [SerializeField] protected string skillname;
    public GameObject origin;
    public GameObject target;
    //스킬의 범위 적용 형식 ex) 박스, 원형, 부채꼴...
    public enum AttackType
    {
        Box,
        Circle,
        Arc
    }
    [SerializeField] protected AttackType attackType;
    //스킬 데미지
    [SerializeField] protected float skilldamage;
    //스킬 선딜레이
    [SerializeField] protected float BDelay;
    //스킬 후딜레이
    [SerializeField] protected float ADelay;
    
    public bool isContinuable = true;

    public virtual IEnumerator OnSkillActive()
    {
        yield return StartCoroutine(EBDelay());
        yield return StartCoroutine(_OnSkillActive());
        yield return StartCoroutine(EADelay());
    }

    public abstract IEnumerator _OnSkillActive();
    public abstract float CalcDamage();
    
    public void CalcRange()
    {
        switch(attackType)
        {
            case AttackType.Box:

                    break;
            case AttackType.Circle:

                    break;
            case AttackType.Arc:
                    
                    break;

            default:
            
                    break;
        }
    }

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