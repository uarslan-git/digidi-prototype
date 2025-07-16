using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class XROriginTutorialCanvas : NetworkBehaviour
{
    public Canvas tutorialCanvas;

    private bool isLocalPlayer = false;
    private bool aButtonPrev = false;
    private bool canToggle = false;

    void Start()
    {
        bool isQuest = Application.platform == RuntimePlatform.Android;
        if (tutorialCanvas != null)
            tutorialCanvas.enabled = isQuest;
    }

    public void HideCanvas()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.enabled = false;
            CanvasGroup cg = tutorialCanvas.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = tutorialCanvas.gameObject.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        // Disable the pre-join camera (only relevant in your specific scene setup)
        var preJoinCamera = GameObject.Find("Camera");
        if (preJoinCamera != null && preJoinCamera != this.gameObject)
            preJoinCamera.SetActive(false);

        bool isQuest = Application.platform == RuntimePlatform.Android;

        if (isQuest)
        {
            isLocalPlayer = true;
            if (tutorialCanvas != null)
                tutorialCanvas.enabled = true;
            canToggle = true;
        }
        else
        {
            isLocalPlayer = false;
            if (tutorialCanvas != null)
                tutorialCanvas.enabled = false;
            canToggle = false;
        }
    }

    void Update()
    {
    }

    public void OnConfirmClicked()
    {
        if (tutorialCanvas != null && canToggle)
            tutorialCanvas.enabled = !tutorialCanvas.enabled;
    }
}

