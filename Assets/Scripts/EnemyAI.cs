using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    // Update is called once per frame
    private void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        timer = 2f;
    }
}
