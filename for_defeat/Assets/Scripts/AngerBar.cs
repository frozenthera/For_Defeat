using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AngerBar : MonoBehaviour
{
    [SerializeField] private Slider angerBar;
    private PlayerController player;
    private float maxAnger;
    private void Start()
    {
        player = GameManager.Instance.player;
        maxAnger = player.MaxAngerGauge;
    }
    
    private void Update()
    {
        angerBar.value = player.CurAngerGauge / maxAnger;
        Debug.Log(player.CurAngerGauge / maxAnger);
    }
}
