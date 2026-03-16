using UnityEngine;
using System.Reflection;

public class MeleeAgentFixer : MonoBehaviour
{
    void Start()
    {
        AiAgent agent = GetComponent<AiAgent>();
        if (agent == null) return;

        // 1. НАЗНАЧАЕМ PLAYER TRANSFORM (это решает ошибку)
        if (agent.playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Character");
            if (player != null)
            {
                agent.playerTransform = player.transform;
                Debug.Log("Player assigned to melee enemy");
            }
            else
            {
                Debug.LogError("Player not found! Make sure it has 'Player' tag.");
            }
        }

        // 2. Даем время на инициализацию stateMachine
        Invoke(nameof(RemoveWeaponStates), 0.1f);
    }

    void RemoveWeaponStates()
    {
        AiAgent agent = GetComponent<AiAgent>();
        if (agent?.stateMachine == null) return;

        var statesField = typeof(AiStateMachine).GetField("states",
            BindingFlags.NonPublic | BindingFlags.Instance);

        if (statesField != null)
        {
            var states = (AiState[])statesField.GetValue(agent.stateMachine);

            // Удаляем ненужные состояния
            states[(int)AiStateId.FindWeapon] = null;
            states[(int)AiStateId.AttackTarget] = null;
            states[(int)AiStateId.FindAmmo] = null;

            Debug.Log("Weapon states removed for melee enemy");
        }
    }
}