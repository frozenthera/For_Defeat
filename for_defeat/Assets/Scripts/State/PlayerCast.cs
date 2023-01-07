using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCast : IState
{
    private PlayerController player;
    public int skillIdx;
    public PlayerCast(PlayerController player)
    {
        this.player = player;
    }

    public void OperateEnter()
    {
        player.playerAnim.SetBool("Attack", true);
        player.StartCoroutine(ECheckDone());
    }

    public void OperateExit()
    {
        player.playerAnim.SetBool("Attack", false);
    }

    public void OperateUpdate()
    {

    }

    private IEnumerator ECheckDone()
    {
        player.skillList[skillIdx].origin = player.gameObject;
        yield return player.StartCoroutine(player.skillList[skillIdx].OnSkillActive(skillIdx));
        player.UpdateState(PlayerController.EPlayerState.Wait);
    }
}
