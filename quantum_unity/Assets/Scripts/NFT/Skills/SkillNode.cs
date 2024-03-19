using UnityEngine;
using System;

[Serializable]
public class SkillNode
{
    public string id;
    public string name;
    public int currentLevel = 0;
    public int maxLevel;
    public SkillNode[] nextNodes;

    public void Upgrade()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            CheckForUnlock();
        }
    }

    private void CheckForUnlock()
    {
        foreach (var node in nextNodes)
        {
            // Implement your condition for unlocking the next node.
            // For example, next node requires current node to be at least level 1.
            if (currentLevel >= 1)
            {
                // Unlock the node or do something
                Console.WriteLine(node.name + " is now unlocked!");
            }
        }
    }
}
