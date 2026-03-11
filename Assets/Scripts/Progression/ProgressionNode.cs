using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProgressionNode : MonoBehaviour
{
    [Header("Node data")]
    public NodeType type;

    [Tooltip("If true the node cannot be interacted with or seen as available.")]
    public bool locked = true;

    [Tooltip("This level has been finished.")]
    public bool completed = false;

    [Tooltip("Nodes that can be visited after this one is completed.")]
    public List<ProgressionNode> children = new List<ProgressionNode>();

    [HideInInspector]
    public List<ProgressionNode> parents = new List<ProgressionNode>();

    /// <summary>
    /// Returns true if this node is not locked (automatically available).
    /// </summary>
    public bool available { get { return !locked; } }

    // internal reference to the UI helper if one is attached
    private ProgressionNodeUI _ui;

    void Awake()
    {
        _ui = GetComponent<ProgressionNodeUI>();
        UpdateVisualState();
    }

    public void SetAvailable(bool value)
    {
        // Setting available to true means unlocking; false means locking
        locked = !value;
        UpdateVisualState();
    }

    public void Complete()
    {
        if (completed)
            return;

        completed = true;
        UpdateVisualState();
        UnlockChildren();
    }

    private void UnlockChildren()
    {
        foreach (var child in children)
        {
            // make child available if all of its parents are completed
            if (child.CanBeUnlocked())
                child.SetAvailable(true);
        }
    }

    /// <summary>
    /// Determines whether this node should unlock based on its parents' completion.
    /// Note: this simple implementation assumes all parents are connected via the
    /// editor using a bidirectional relationship.
    /// </summary>
    public bool CanBeUnlocked()
    {
        // A node is unlocked when none of the nodes that list it as a child remain incompleted.
        ProgressionNode[] allNodes = FindObjectsOfType<ProgressionNode>();
        foreach (var n in allNodes)
        {
            if (n.children.Contains(this) && !n.completed)
                return false;
        }
        return true;
    }

    private void UpdateVisualState()
    {
        if (_ui != null)
            _ui.Refresh(this);
    }
}
