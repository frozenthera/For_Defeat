using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_Hero : MonoBehaviour
{
    [SerializeField] private Slider heroHPBar;
    private HeroBehaviour hero;
    private float maxHP;

    private void Start()
    {
        hero = GameManager.Instance.hero;
        maxHP = hero.HeroMaxHP;
        heroHPBar.value = 1f;
    }

    private void Update()
    {
        heroHPBar.value = hero.curHP/maxHP;
    }
}
