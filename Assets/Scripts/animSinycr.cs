using UnityEngine;

public class animSinycr : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        anim.speed = PlayerTime.TIME;
    }
}
