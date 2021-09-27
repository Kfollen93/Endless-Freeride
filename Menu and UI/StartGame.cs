using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject playerPrefab; // Accessed by Respawn script
    public GameObject sphereMotorPrefab; // Accessed by Respawn script
    [SerializeField] private GameObject canvasJoySticks;
    [SerializeField] private GameObject mainMenu;

    // This function represents Awake() in terms of logic, as this will be the first function called.
    public void StartButtonClick()
    {
        playerPrefab.SetActive(true);
        sphereMotorPrefab.SetActive(true);
        canvasJoySticks.SetActive(true);
        mainMenu.SetActive(false);
    }
}
