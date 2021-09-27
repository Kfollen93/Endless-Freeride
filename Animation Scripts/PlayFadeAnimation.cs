using UnityEngine;
using UnityEngine.UI;

public class PlayFadeAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Button restartButton;
    private bool playAnim;

    private void Start()
    {
        Button btn = restartButton.GetComponent<Button>();
        btn.onClick.AddListener(PlayAnimationSetter);
    }

    public void Update()
    {
        if (playAnim)
        {
            anim.SetBool("playRespawn", true);
        }
        else
        {
            anim.SetBool("playRespawn", false);
        }

        playAnim = false;
    }

    private void PlayAnimationSetter()
    {
        playAnim = true;
    }
}
