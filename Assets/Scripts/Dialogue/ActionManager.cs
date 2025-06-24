using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class ActionManager : NetworkBehaviour
{
    public GameObject ActionButtonPrefab;
    public Transform ButtonContainer;
    public AudioSource AudioSource;
    public Animator Animator;
    private ActionNode currentNode;

    public ActionTreeBuilder ActionTreeBuilder;


    public void Initialize(ActionNode rootNode)
    {
        currentNode = rootNode;
        UpdateUI();
    }

    public void TraverseTo(ActionNode node)
    {
        print("traversing");
        PlayAction(node);
        currentNode = node;
        UpdateUI();
    }

    public void TraverseBack()
    {
        if (currentNode.Parent != null)
        {
            currentNode = currentNode.Parent;
            UpdateUI();
        }
    }

    private void PlayAction(ActionNode node)
    {
        //print(IsOwner);
        if (!IsOwner) return;
        //print("playing action");
        if (node.ClipId != -1 && AudioSource != null)
        {
            //print("trying to play audio");
            PlayAudioRpc(node.ClipId);
            //AudioSource.clip = ActionTreeBuilder.audioClips[node.ClipId];
            //AudioSource.Play();
        }

        if (!string.IsNullOrEmpty(node.AnimationStateName) && Animator != null)
        {
            Animator.CrossFade(node.AnimationStateName, 0.35f);  // Smooth transition over 0.25 seconds
        }
    }

    private void UpdateUI()
    {
        // Clear old buttons
        foreach (Transform child in ButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each child node
        foreach (var child in currentNode.Children)
        {
            var buttonObject = Instantiate(ActionButtonPrefab, ButtonContainer);
            var buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = child.ActionName;

            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => TraverseTo(child));
        }

        // Add a back button
        if (currentNode.Parent != null)
        {
            var backButtonObject = Instantiate(ActionButtonPrefab, ButtonContainer);
            var backButtonText = backButtonObject.GetComponentInChildren<TMP_Text>();
            backButtonText.text = "Back";

            var backButton = backButtonObject.GetComponent<Button>();
            backButton.onClick.AddListener(TraverseBack);
        }
    }

[Rpc(SendTo.Everyone)]
private void PlayAudioRpc(int clipId)
{
    AudioSource.clip = ActionTreeBuilder.audioClips[clipId];
    AudioSource.Play();
    StartCoroutine(WaitForAudioEnd());
}

private IEnumerator WaitForAudioEnd()
{
    while (AudioSource.isPlaying)
    {
        yield return null;
    }
    // Reset to idle state after audio finishes with smooth transition
    if (Animator != null)
    {
        Animator.CrossFade("Idle", 0.05f);  // Smooth transition over 0.25 seconds
    }
}
}
