using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple manager that keeps track of all nodes and the "start" node of the map.
/// Attach to a top‑level GameObject in your map scene.
/// </summary>
public class ProgressionMap : MonoBehaviour
{
    [Tooltip("The first node players start from; it will be made available automatically.")]
    public ProgressionNode startNode;

    /// <summary>
    /// All nodes found in the scene. You can use this list for serialization or
    /// to reset the map state.
    /// </summary>
    [HideInInspector]
    public List<ProgressionNode> allNodes = new List<ProgressionNode>();

    void OnValidate()
    {
        // keep a cached collection so that the editor can show them if needed
        allNodes.Clear();
        allNodes.AddRange(FindObjectsOfType<ProgressionNode>());
    }

    void Start()
    {
        // Build parent references from children relationships
        foreach (var node in allNodes)
        {
            node.parents.Clear();
        }
        foreach (var node in allNodes)
        {
            foreach (var child in node.children)
            {
                if (child != null && !child.parents.Contains(node))
                    child.parents.Add(node);
            }
        }

        if (startNode != null)
        {
            startNode.SetAvailable(true);
        }
    }

    /// <summary>
    /// Marks every node as not completed and locked (useful when restarting a run).
    /// </summary>
    public void ResetMap()
    {
        foreach (var node in allNodes)
        {
            node.locked = true;
            node.completed = false;
            // force refresh
            var ui = node.GetComponent<ProgressionNodeUI>();
            if (ui != null) ui.Refresh(node);
        }
        if (startNode != null)
            startNode.SetAvailable(true);
    }
}
