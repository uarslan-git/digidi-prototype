using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitializer : MonoBehaviour
{
    public ActionTreeBuilder TreeBuilder;
    public ActionManager ActionManager;

    void Start()
    {
        Debug.Log(TreeBuilder.RootNode.ActionName);
        ActionManager.Initialize(TreeBuilder.RootNode);
    }
}
