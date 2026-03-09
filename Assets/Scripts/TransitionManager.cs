using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [SerializeField] private Animator anim;
    [SerializeField] private Image transitionImage;

    [SerializeField] private Sprite startSprite;
    [SerializeField] private float transitionTime = 1f;

    private bool isTransitioning = false;

    private void Awake()
    {
        instance = this;
        transitionImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        transitionImage.sprite = startSprite;
        anim.transform.rotation = Quaternion.Euler(0, 0, 180);
        anim.SetTrigger("Open");
        StartCoroutine(Open());
    }

    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
            StartCoroutine(Close(sceneName));
    }

    IEnumerator Close(string sceneName)
    {
        isTransitioning = true;

        anim.transform.rotation = Quaternion.Euler(0, 0, 0);

        transitionImage.sprite = startSprite;

        anim.SetTrigger("Close");
        AudioManager.instance.PlaySFX("Close", 1);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Open()
    {
        AudioManager.instance.PlaySFX("Open", 1);

        yield return new WaitForSeconds(transitionTime);

        transitionImage.sprite = null;

        isTransitioning = false;
    }
}