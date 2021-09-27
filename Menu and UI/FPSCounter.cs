using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text averageFPSDisplay;
    private int framesPassed = 0;
    private float fpsTotal = 0f;
    private float fps;
    private bool fpsDisplayed;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        DisplayAverageFPS();
    }

    private void DisplayAverageFPS()
    {
        fps = 1 / Time.unscaledDeltaTime;
        fpsTotal += fps;
        framesPassed++;
        averageFPSDisplay.text = "" + (fpsTotal / framesPassed);
    }

    // Using OnClick() for the UI FPS Button to toggle
    public void ClickFPSButton()
    {
        if (!fpsDisplayed)
        {
            averageFPSDisplay.gameObject.SetActive(true);
            fpsDisplayed = true;
        }
        else
        {
            averageFPSDisplay.gameObject.SetActive(false);
            fpsDisplayed = false;
        }
    }
}
