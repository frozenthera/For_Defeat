using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    //스킬 스프라이트
    [SerializeField] protected Sprite skillSprite;
    //스킬 이름
    [SerializeField] protected string skillname;
    
    //스킬의 범위 적용 형식 ex) 박스, 원형, 기타 메쉬...

    //스킬 데미지
    [SerializeField] protected float skilldamage;
    //스킬 선딜레이
    [SerializeField] protected float BDelay;
    //스킬 후딜레이
    [SerializeField] protected float ADelay;
 
    public abstract void OnSkillActive();
    public abstract float CalcDamage();

}