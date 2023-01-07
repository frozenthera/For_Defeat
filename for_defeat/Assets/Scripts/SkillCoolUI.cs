using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolUI : MonoBehaviour
{
    [SerializeField] private Image coolTime;
    [SerializeField] private Image shadow;
    [SerializeField] private string skillKind;

    private PlayerController player;
    private Dictionary<string, float> dict_cur = new Dictionary<string, float>();
    private Dictionary<string, float> dict_max = new Dictionary<string, float>();
    private Dictionary<string, bool> dict_active = new Dictionary<string, bool>();
    private float maxTime;
    private float curTime;

    private void Start()
    {
        player = GameManager.Instance.player;

        dict_cur.Add("Q", player.CurQCoolDown);
        dict_cur.Add("W", player.CurWCoolDown);
        dict_cur.Add("E", player.CurECoolDown);
        dict_cur.Add("R", player.CurRCoolDown);
        dict_cur.Add("Flash", player.CurFlashCoolDown);

        dict_max.Add("Q", player.MaxQCoolDown);
        dict_max.Add("W", player.MaxWCoolDown);
        dict_max.Add("E", player.MaxECoolDown);
        dict_max.Add("R", player.MaxRCoolDown);
        dict_max.Add("Flash", player.MaxFlashCoolDown);

        dict_active.Add("Q", player.IsQActive);
        dict_active.Add("W", player.IsWActive);
        dict_active.Add("E", player.IsEActive);
        dict_active.Add("R", player.IsRActive);
        dict_active.Add("Flash", player.IsFlashActive);


        maxTime = dict_max[skillKind];
        curTime = dict_cur[skillKind];
    }

    private void Update()
    {
        dict_cur["Q"] = player.CurQCoolDown;
        dict_cur["W"] = player.CurWCoolDown;
        dict_cur["E"] = player.CurECoolDown;
        dict_cur["R"] = player.CurRCoolDown;
        dict_cur["Flash"] = player.CurFlashCoolDown;

        dict_active["Q"] = player.IsQActive;
        dict_active["W"] = player.IsWActive;
        dict_active["E"] = player.IsEActive;
        dict_active["R"] = player.IsRActive;
        dict_active["Flash"] = player.IsFlashActive;

        curTime = dict_cur[skillKind];
        coolTime.GetComponent<Image>().fillAmount = curTime/maxTime;
        shadow.gameObject.SetActive(!dict_active[skillKind]);
    }
}
