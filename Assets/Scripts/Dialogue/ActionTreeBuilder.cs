using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTreeBuilder : MonoBehaviour
{
    public List<AudioClip> audioClips;

    public ActionNode RootNode { get; private set; }

    void Awake()
    {
        RootNode = new ActionNode("Root");
        var szene0Node = new ActionNode("Beginnen", 0, "Szene0Trigger");

        RootNode.AddChild(szene0Node);

        var szene3aNode = new ActionNode("Reaktion adäquat", 1, "Szene3aTrigger");
        var szene3bNode = new ActionNode("Reaktion unverständlich", 2, "Szene3bTrigger");
        var szene3cdNode = new ActionNode("Reaktion inadäquat", 3, "Szene3cdTrigger");
        var szene3eNode = new ActionNode("Reaktion ohne Beschwerde", 4, "Szene3eTrigger");

        szene0Node.AddChild(szene3aNode);
        szene0Node.AddChild(szene3bNode);
        szene0Node.AddChild(szene3cdNode);
        szene0Node.AddChild(szene3eNode);

        var szene5Node = new ActionNode("Abschied", 5, "Szene5Trigger");

        szene3aNode.AddChild(szene5Node);
        szene3bNode.AddChild(szene5Node);
        szene3cdNode.AddChild(szene5Node);
        szene3eNode.AddChild(szene5Node);
    }
}
