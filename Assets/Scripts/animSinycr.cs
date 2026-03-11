using UnityEngine;

public class animSinycr : MonoBehaviour
{
    private Animator anim;
    public bool isActive = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isActive)
            anim.speed = PlayerTime.TIME;
        else
            anim.speed = 1;
    }
}
