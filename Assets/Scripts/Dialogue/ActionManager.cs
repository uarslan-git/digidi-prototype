using System.Collections;
using System.Collections.Generic;
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

    //public ActionTreeBuilder ActionTreeBuilder;


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
        print(IsOwner);
        if (!IsOwner) return;
        print("playing action");
        if (node.ClipId != -1 && AudioSource != null)
        {
            print("trying to play audio");
            PlayAudioRpc(1);
            //AudioSource.clip = ActionTreeBuilder.audioClips[node.ClipId];
            //AudioSource.Play();
        }

        if (!string.IsNullOrEmpty(node.AnimationStateName) && Animator != null)
        {
            Animator.SetTrigger(node.AnimationStateName);
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
        //AudioSource.clip = ActionTreeBuilder.audioClips[clipId];
        //AudioSource.Play();
        print("test");
    }
}
