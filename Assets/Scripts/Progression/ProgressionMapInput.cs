using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProgressionMapInput : MonoBehaviour
{
    public ProgressionMapController controller;
    public float verticalThreshold = 2.0f; // Adjust based on your map scale
    
    private ProgressionNode _currentNode;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<ProgressionMapController>();

        // Initialize to starting node
        if (controller.map != null && controller.map.startNode != null)
        {
            _currentNode = controller.map.startNode;
        }

        UpdateNodeHighlight();
    }

    void Update()
    {
        if (_currentNode == null) return;

        // Spatial Navigation using WASD
        if (Input.GetKeyDown(KeyCode.W)) NavigateInDirection(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) NavigateInDirection(Vector2.down);
        if (Input.GetKeyDown(KeyCode.A)) NavigateInDirection(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) NavigateInDirection(Vector2.right);

        // E to select
        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectCurrentNode();
        }
    }

    /// <summary>
    /// Finds the nearest node in a specific world-space direction.
    /// </summary>
    void NavigateInDirection(Vector2 direction)
    {
        if (controller.map == null) return;

        ProgressionNode bestCandidate = null;
        float closestDistance = float.MaxValue;

        foreach (var node in controller.map.allNodes)
        {
            if (node == _currentNode) continue;

            // Calculate vector from current node to potential candidate
            Vector2 toCandidate = (Vector2)node.transform.position - (Vector2)_currentNode.transform.position;

            // Check if the candidate is generally in the direction pressed
            // Using Dot Product to ensure the node is in the correct hemisphere
            float dot = Vector2.Dot(toCandidate.normalized, direction);
            
            if (dot > 0.7f) // Roughly a 45-degree cone in that direction
            {
                float distance = toCandidate.sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestCandidate = node;
                }
            }
        }

        if (bestCandidate != null)
        {
            _currentNode = bestCandidate;
            UpdateNodeHighlight();
            Debug.Log($"Spatial Nav to: {_currentNode.name}");
        }
    }

    void SelectCurrentNode()
    {
        // Only allow selection if the node is actually available (unlocked)[cite: 5, 6]
        if (_currentNode != null && !_currentNode.locked)
        {
            controller.OnNodeSelected(_currentNode);
        }
    }

    void UpdateNodeHighlight()
    {
        // Unhighlight all nodes and highlight the current one[cite: 5]
        foreach (var node in controller.map.allNodes)
        {
            var ui = node.GetComponent<ProgressionNodeUI>();
            if (ui != null) ui.SetHighlighted(node == _currentNode);
        }
    }
}