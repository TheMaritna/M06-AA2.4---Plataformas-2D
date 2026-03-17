using UnityEngine;
using System.Collections.Generic;

public class ChangeBlock : MonoBehaviour
{
    public List<GameObject> states = new List<GameObject>();

    public int currentState = 0;

    void Start()
    {
        UpdateState();
    }
    private void Update()
    {
        if (PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Jump.WasPressedThisFrame())
        {
            NextState();
        }
    }

    public void NextState()
    {
        if (states.Count == 0) return;

        currentState++;

        if (currentState >= states.Count)
            currentState = 0;

        UpdateState();
    }

    void UpdateState()
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i] != null)
                states[i].SetActive(i == currentState);
        }
    }
}