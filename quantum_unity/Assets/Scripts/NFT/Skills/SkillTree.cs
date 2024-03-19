using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public List<SkillNode> nodes = new List<SkillNode>();

    void Start()
    {
        // Initialize your skill tree here
    }

    public void UpgradeNode(string nodeId)
    {
        var node = nodes.Find(n => n.id == nodeId);
        if (node != null)
        {
            node.Upgrade();
        }
    }
}
