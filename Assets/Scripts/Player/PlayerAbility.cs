using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviourWithPause
{
    public float abilityCD;
    bool isAvailable = true;
    PlayerInput input;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    protected override void UpdateWithPause()
    {
        if (input.skillInput)
        {
            TryAbility();
        }
    }

    protected void TryAbility()
    {
        if (isAvailable == false)
        {
            return;
        }

        UseAbility();

        StartCoroutine(StartCooldown());
    }

    protected virtual void UseAbility()
    {
        
    }

    private IEnumerator StartCooldown()
    {
        isAvailable = false;

        yield return new WaitForSeconds(abilityCD);

        isAvailable = true;
    }

    public void MakeAbilityAvailable() {
        isAvailable = true;
    }
}
