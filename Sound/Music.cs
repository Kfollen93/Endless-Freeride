using UnityEngine;
using TMPro;

/* Contains all music logic */
public class Music : MonoBehaviour
{
    public GameObject music;
    public TMP_Text musicMenuText;
    public TMP_Text musicGameText;
    [SerializeField] private AudioSource audioSrc;

    private bool musicStatus = true;

    private void Start()
    {
        musicMenuText.text = "Music: \nOn";
        musicGameText.text = "Music: \nOn";
    }

    // Using OnClick() for the UI Music Buttons to toggle
    public void ClickMusicButtons()
    {
        if (musicStatus)
        {
            musicMenuText.text = "Music: \nOff";
            musicGameText.text = "Music: \nOff";
            music.SetActive(false);
            musicStatus = false;
        }
        else
        {
            musicMenuText.text = "Music: \nOn";
            musicGameText.text = "Music: \nOn";
            music.SetActive(true);
            musicStatus = true;
        }
    }
}
