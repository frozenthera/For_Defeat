using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{

    private PlayerController player;

    [SerializeField] private float heroSpeed;
    public float HeroSpeed => heroSpeed;
    
    [SerializeField] private float heroNormalBDelay;
    public float HeroNormalBDelay => heroNormalBDelay;
    [SerializeField] private float heroNormalADelay;
    public float HeroNormalADelay => heroNormalADelay;

    [SerializeField] private float heroNormalAttackDamage;
    public float HeroNormalAttackDamage => heroNormalAttackDamage;

    [SerializeField] private float heroRecogRad;
    public float HeroRecogRad => heroRecogRad;

    private bool isSlashable;
    public bool IsSlashable => isSlashable;
    [SerializeField] private float heroSlashCD;

    private bool isDashable;
    [SerializeField] private float heroDashCD;
    
    private bool isHealable;
    [SerializeField] private float heroHealCD;
    [SerializeField] private float healThreshold;

    public bool isImmune = false;
    private bool isImmunable;
    [SerializeField] private float heroImmuneCD;
    [SerializeField] private float immuneThreshold;


    [SerializeField] private float maxHP;
    [SerializeField] public float curHP;

    
    public List<Skill> skillList = new();

    public enum HeroState
    {
        Idle,
        Move,
        Attack,
        Cast,
        Length
    }

    public enum heroSKill
    {
        Slash,
        Dash,
        Heal,
        Immune,
        Length
    }

    public StateMachine stateMachine;

    private Dictionary<HeroState, IState> dicState = new Dictionary<HeroState, IState>();

    private void Start()
    {
        IState idle = new HeroIdle(this);
        IState move = new HeroMove(this);
        IState attack = new HeroAttack(this);
        IState cast = new HeroCast(this);

        dicState.Add(HeroState.Idle, idle);
        dicState.Add(HeroState.Move, move);
        dicState.Add(HeroState.Attack, attack);
        dicState.Add(HeroState.Cast, cast);

        stateMachine = new StateMachine(dicState[HeroState.Move]);

        StartCoroutine(ESlashCD());
        StartCoroutine(EDashCD());

        foreach(var skill in skillList) skill.origin = gameObject;

        player = GameManager.Instance.player;
    }

    private void Update()
    {
        if(isSlashable)
        {
            UpdateState(HeroState.Cast, heroSKill.Slash);   
            StartCoroutine(ESlashCD());
        }
        else if(isDashable)
        {
            UpdateState(HeroState.Cast, heroSKill.Dash);
            StartCoroutine(EDashCD());
        }
        else if(isHealable && curHP <= healThreshold)
        {
            UpdateState(HeroState.Cast, heroSKill.Heal);
            StartCoroutine(EHealCD());
        }
        else if(isImmunable && curHP <= immuneThreshold)
        {
            UpdateState(HeroState.Cast, heroSKill.Immune);
            StartCoroutine(EImmuneCD());
        }
        stateMachine.DoOperateUpdate();
    }

    public void UpdateState(HeroState type)
    {
        stateMachine.SetState(dicState[type]);
    }

    public void UpdateState(HeroState type, heroSKill skillType)
    {
        ((HeroCast)dicState[HeroState.Cast]).skillIdx = (int)skillType;
        stateMachine.SetState(dicState[type]);
    }

    public void GetHeal(float heal)
    {
        curHP += heal;
        Mathf.Clamp(curHP, 0, maxHP);
    }

    public IEnumerator SpeedBuff(float percent, float time)
    {
        float BSpeed = heroSpeed;
        heroSpeed *= (1 + percent);
        float curTime = time;
        while(curTime >= 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }
        heroSpeed = BSpeed;
    }

    public IEnumerator ImmuneBuff(float time)
    {
        isImmune = true;
        float curImmuneTime = time;
        while(curImmuneTime >= 0)
        {
            curImmuneTime -= Time.deltaTime;
            yield return null;    
        }
        isImmune = false;
    }

    //용사 스킬 쿨다운 관리
    private IEnumerator ESlashCD()
    {
        isSlashable = false;
        float curSlashCD = heroSlashCD;
        while(curSlashCD >= 0)
        {
            curSlashCD -= Time.deltaTime;
            yield return null;
        }
        isSlashable = true;
    }

    private IEnumerator EDashCD()
    {
        isDashable = false;
        float curDashCD = heroDashCD;
        while(curDashCD >= 0)
        {
            curDashCD -= Time.deltaTime;
            yield return null;
        }
        isDashable = true;
    }

    private IEnumerator EHealCD()
    {
        isHealable = false;
        float curHealCD = heroHealCD;
        while(curHealCD >= 0)
        {
            curHealCD -= Time.deltaTime;
            yield return null;
        }
        isHealable = true;
    }

    private IEnumerator EImmuneCD()
    {
        isImmunable = false;
        float curImmuneCD = heroImmuneCD;
        while(curImmuneCD >= 0)
        {
            curImmuneCD -= Time.deltaTime;
            yield return null;
        }
        isImmunable = true;
    }
}