using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Publicly accessed by OnClick from Inspector
    public void QuitButtonClick()
    {
        Application.Quit();
    }
}
