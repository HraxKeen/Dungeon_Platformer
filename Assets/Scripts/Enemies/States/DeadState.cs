using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;
    const float i_dropChance = 1f / 4f;
    
    public DeadState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        OnEnemyJustDied();

        GameObject.Instantiate(stateData.deathBloodParticles, entity.aliveGO.transform.position, stateData.deathBloodParticles.transform.rotation);
        GameObject.Instantiate(stateData.deathChunkParticles, entity.aliveGO.transform.position, stateData.deathChunkParticles.transform.rotation);
        soundEffects.sfxInstance.Audio.PlayOneShot(soundEffects.sfxInstance.dSound);

        entity.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void OnEnemyJustDied()
    {
        if(Random.Range(0f, 1f) <= i_dropChance)
        {
            GameObject.Instantiate(stateData.dropItem, entity.aliveGO.transform.position, stateData.dropItem.transform.rotation);
            Debug.Log("Item!!");
        }
        
    }

}

