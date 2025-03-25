using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode
{
    public string ActionName { get; private set; }
    public List <ActionNode> Children { get; private set; }
    public ActionNode Parent { get; private set; }
    public AudioClip ActionAudio { get; private set; }
    public string AnimationStateName { get; private set; }

    public ActionNode(string actionName, AudioClip audio = null, string animationState = null)
    {
        ActionName = actionName;
        ActionAudio = audio;
        AnimationStateName = animationState;
        Children = new List<ActionNode>();
    }

    public void AddChild(ActionNode child)
    {
        child.Parent = this;
        Children.Add(child);
    }
}
