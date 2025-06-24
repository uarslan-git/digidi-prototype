using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : NetworkBehaviour
{
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

        // Only show tutorial for the owner of this object
        if (!IsOwner)
        {
            if (tutorialCanvas != null)
                tutorialCanvas.enabled = false;
            return;
        }

        // Only show tutorial for Quest/XR users
        if (gameObject.name.Contains("XR Origin"))
        {
            InitializeTutorial();
        }
        else
        {
            if (tutorialCanvas != null)
                tutorialCanvas.enabled = false;
        }
    }

    private void InitializeTutorial()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.enabled = true;
            
            if (tutorialText != null)
                tutorialText.text = tutorialMessage;

            if (startButton != null)
            {
                startButton.onClick.AddListener(() => {
                    tutorialCanvas.enabled = false;
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
