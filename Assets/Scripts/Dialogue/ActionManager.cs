using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public GameObject ActionButtonPrefab;
    public Transform ButtonContainer;
    public AudioSource AudioSource;
    public Animator Animator;
    private ActionNode currentNode;

    public void Initialize(ActionNode rootNode)
    {
        currentNode = rootNode;
        UpdateUI();
    }

    public void TraverseTo(ActionNode node)
    {
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
        if (node.ActionAudio != null && AudioSource != null)
        {
            AudioSource.clip = node.ActionAudio;
            AudioSource.Play();
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
}
