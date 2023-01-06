using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_Player : MonoBehaviour
{
    [SerializeField] private Slider playerHPBar;
    private PlayerController player;
    private float maxHP;
    void Start()
    {
        player = GameManager.Instance.player;
        maxHP = player.PlayerMaxHP;
        playerHPBar.value = 1f;
    }

    void Update()
    {
        playerHPBar.value = player.PlayerCurHP/maxHP;
    }
}
