using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI backCount;

    public void ActualizarTexto(string texto)
    {
        backCount.text = texto;
    }
}