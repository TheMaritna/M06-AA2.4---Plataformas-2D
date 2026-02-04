using UnityEngine;
using System.Collections;

public class HelthSystem : MonoBehaviour
{
    [Header("References")]
    public PlayerRespawn respawn;


    public void Hit(float Damage)
    {
        EndGame();
    }
    public void EndGame()
    {
        Debug.Log("Player Die");
        respawn.Dead();
    }
}
