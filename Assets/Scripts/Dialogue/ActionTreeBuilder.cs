using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTreeBuilder : MonoBehaviour
{
    public AudioClip Szene0Audio;
    public AudioClip Szene3aAudio;
    public AudioClip Szene3bAudio;
    public AudioClip Szene3cdAudio;
    public AudioClip Szene3eAudio;
    public AudioClip Szene5Audio;

    public ActionNode RootNode { get; private set; }

    void Awake()
    {
        RootNode = new ActionNode("Root");
        var szene0Node = new ActionNode("Szene 0", Szene0Audio, "Szene0Trigger");

        RootNode.AddChild(szene0Node);

        var szene3aNode = new ActionNode("Szene 3a", Szene3aAudio, "Szene3aTrigger");
        var szene3bNode = new ActionNode("Szene 3b", Szene3bAudio, "Szene3bTrigger");
        var szene3cdNode = new ActionNode("Szene 3c_d", Szene3cdAudio, "Szene3cdTrigger");
        var szene3eNode = new ActionNode("Szene 3e", Szene3eAudio, "Szene3eTrigger");

        szene0Node.AddChild(szene3aNode);
        szene0Node.AddChild(szene3bNode);
        szene0Node.AddChild(szene3cdNode);
        szene0Node.AddChild(szene3eNode);

        var szene5Node = new ActionNode("Szene 5", Szene5Audio, "Szene5Trigger");

        szene3aNode.AddChild(szene5Node);
        szene3bNode.AddChild(szene5Node);
        szene3cdNode.AddChild(szene5Node);
        szene3eNode.AddChild(szene5Node);
    }
}
