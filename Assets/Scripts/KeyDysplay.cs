using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class KeyDisplay : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI KeyText;
    public TextMeshProUGUI ActionText;

    [Header("Sprites")]
    public Sprite Keyboard;
    public Sprite Controller;

    [Header("Texto manual")]
    public string keyboardKey = "Z";
    public string controllerKey = "A";
    public string actionText = "Saltar";

    private SpriteRenderer sp;
    private bool usingController;

    private InputSystem_Actions input;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        input = PlayerData.DATA.PC.input;

        ActionText.text = actionText;

        SubscribeToActions();
        UpdateUI();
    }

    void SubscribeToActions()
    {
        foreach (var map in input.asset.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.performed += ctx => DetectDevice(ctx);
            }
        }
    }

    void DetectDevice(InputAction.CallbackContext ctx)
    {
        var device = ctx.control.device;

        if (device is Gamepad)
            usingController = true;
        else if (device is Keyboard || device is Mouse)
            usingController = false;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (usingController)
        {
            KeyText.text = controllerKey;
            sp.sprite = Controller;
        }
        else
        {
            KeyText.text = keyboardKey;
            sp.sprite = Keyboard;
        }
    }
}