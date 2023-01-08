using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehaviour : UnitBehaviour
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
    public float HeroMaxHP => maxHP;
    [SerializeField] public float curHP;


    public List<HeroSkill> skillList = new();

    public Animator heroAnim;
    public GameObject heroAttackCircle;
    public bool isInKnuckBack = false;
    public bool isInTrap = false;

    public enum HeroState
    {
        Idle,
        Move,
        Attack,
        Cast,
        Trapped,
        KnuckBack,
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

    [SerializeField] private float normalAttckGaugeGain;
    public float NormalAttackGaugeGain => normalAttckGaugeGain;
    private SpriteRenderer heroSprite;
    private void Awake()
    {
        GameManager.Instance.hero = this;
    }
    private void Start()
    {
        curHP = maxHP;

        IState idle = new HeroIdle(this);
        IState move = new HeroMove(this);
        IState attack = new HeroAttack(this);
        IState cast = new HeroCast(this);
        IState trapped = new HeroTrapped(this);
        IState knuckback = new HeroKnuckBack(this);

        dicState.Add(HeroState.Idle, idle);
        dicState.Add(HeroState.Move, move);
        dicState.Add(HeroState.Attack, attack);
        dicState.Add(HeroState.Cast, cast);
        dicState.Add(HeroState.Trapped, trapped);
        dicState.Add(HeroState.KnuckBack, knuckback);

        stateMachine = new StateMachine(dicState[HeroState.Move]);

        StartCoroutine(ESlashCD());
        StartCoroutine(EDashCD());
        StartCoroutine(EHealCD());
        StartCoroutine(EImmuneCD());

        foreach(var skill in skillList) skill.origin = gameObject;

        player = GameManager.Instance.player;

        heroAnim = GetComponent<Animator>();
        heroSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 moveVec = player.transform.position - transform.position;
        if(moveVec.x < 0)
        {
            heroSprite.flipX = false;
        }
        else
        {
            heroSprite.flipX = true;
        }

        if(!isInDelay && stateMachine.CurruentState != dicState[HeroState.KnuckBack] && !GameManager.Instance.hero.isInTrap)
        {
            if((player.transform.position - transform.position).magnitude <= heroRecogRad)
            {
                UpdateState(HeroState.Attack);
            }
            else if(isImmunable && curHP <= immuneThreshold)
            {
                UpdateState(HeroState.Cast, heroSKill.Immune);
                StartCoroutine(EImmuneCD());
            }
            else if(isSlashable && (stateMachine.CurruentState != dicState[HeroState.Trapped]))
            {
                UpdateState(HeroState.Cast, heroSKill.Slash);   
                StartCoroutine(ESlashCD());
            }
            else if(isDashable)
            {
                UpdateState(HeroState.Cast, heroSKill.Dash);
                StartCoroutine(EDashCD());
            }
            if(isHealable && curHP <= healThreshold && (stateMachine.CurruentState != dicState[HeroState.Trapped]))
            {
                UpdateState(HeroState.Cast, heroSKill.Heal);
                StartCoroutine(EHealCD());
            }
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

    public void UpdateState(HeroState type, GameObject obj)
    {
        ((HeroTrapped)dicState[HeroState.Trapped]).trapObject = obj.GetComponent<TrapObject>();
        stateMachine.SetState(dicState[type]);
    }

    public void UpdateState(HeroState type, Vector3 knuckBackDir, float sec)
    {
        HeroKnuckBack KB = ((HeroKnuckBack)dicState[HeroState.KnuckBack]);
        KB.KnuckBackDist = knuckBackDir;
        KB.KnuckBackTime = sec;
        stateMachine.SetState(dicState[type]);
    }

    public void GetHeal(float heal)
    {
        curHP += heal;
        Mathf.Clamp(curHP, 0, maxHP);
    }

    public void GetDamage(float damage)
    {   
        curHP -= damage;
        // Debug.Log(damage + ", " + curHP);
        if(curHP <= 0) HeroDie();
    }

    public void HeroDie()
    {
        GameManager.Instance.GameEnd("분노한 마왕에 의해 세상은 멸망했습니다");
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

    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.transform.CompareTag("Wall"))
        {
            if(isInKnuckBack) 
            {
                isInKnuckBack = false;
            }
        }
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, heroRecogRad);

    }
}