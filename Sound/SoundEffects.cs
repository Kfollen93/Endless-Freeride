using UnityEngine;
using TMPro;

/* Contains all SFX logic */
public class SoundEffects : MonoBehaviour
{
    public GameObject soundEffects;
    public TMP_Text soundEffectsMenuText;
    public TMP_Text soundEffectsGameText;
    private bool sfxStatus = true;
    [SerializeField] private GameObject player;
    private Player playerScript;
    [SerializeField] private AudioSource audioSrcGlobal;
    private bool isSFXPlaying;

    private void Start()
    {
        playerScript = player.GetComponent<Player>();
        soundEffectsMenuText.text = "SFX: \nOn";
        soundEffectsGameText.text = "SFX: \nOn";
    }

    // Using OnClick() for the UI SFX Buttons to toggle
    public void ClickSFXButtons()
    {
        if (sfxStatus)
        {
            soundEffectsMenuText.text = "SFX: \nOff";
            soundEffectsGameText.text = "SFX: \nOff";
            soundEffects.SetActive(false);
            sfxStatus = false;
        }
        else
        {
            soundEffectsMenuText.text = "SFX: \nOn";
            soundEffectsGameText.text = "SFX: \nOn";
            soundEffects.SetActive(true);
            sfxStatus = true;
        }
    }

    private void Update()
    {
        if (playerScript.Grounded && !isSFXPlaying)
        {
            audioSrcGlobal.Play();
            isSFXPlaying = true;
        }
        else if (!playerScript.Grounded)
        {
            audioSrcGlobal.Stop();
            isSFXPlaying = false;
        }
    }
}
