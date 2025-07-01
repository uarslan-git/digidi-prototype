using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : NetworkBehaviour
{
    // Allows manual triggering of the tutorial for Editor debugging
    public void ShowTutorial()
    {
        Debug.Log("ShowTutorial() called");
        InitializeTutorial();
    }

    // Editor-only: Press 'T' to show the tutorial in Play Mode
#if UNITY_EDITOR
    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("[TutorialManager] 'T' pressed in Editor. Showing tutorial.");
            ShowTutorial();
        }
    }
#endif
    [Header("UI References")]
    [SerializeField] private Canvas tutorialCanvas;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Tutorial Settings")]
    [SerializeField] private string tutorialMessage = "Welcome to VR! \n\n" +
        "- Use your right controller's trigger to interact with objects\n" +
        "- Use your left controller's stick to move around\n" +
        "- Press the start button below when you're ready!";

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Always show tutorial for all users
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.enabled = true;
            Debug.Log("Tutorial Canvas enabled");
            
            if (tutorialText != null)
                tutorialText.text = tutorialMessage;
            

            if (startButton != null)
            {
                startButton.onClick.AddListener(() => {
                    tutorialCanvas.enabled = false;
                    Debug.Log("Tutorial dismissed by user");
                    // You can trigger any additional tutorial completion logic here
                });
            }
        }
        else
        {
            Debug.LogError("Tutorial Canvas not assigned in TutorialManager!");
        }
    }
}
