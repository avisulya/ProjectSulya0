using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// Example presentation script for a node.  It updates an Image's color or sprite
/// based on the node's state (locked/available/completed).  Attach to the same
/// GameObject as <see cref="ProgressionNode"/> along with an Image component.
/// </summary>
[RequireComponent(typeof(ProgressionNode))]
[RequireComponent(typeof(Image))]

public class ProgressionNodeUI : MonoBehaviour, IPointerClickHandler
{
    public Color lockedColor = Color.gray;
    public Color availableColor = Color.white;
    public Color completedColor = Color.green;
    public Color highlightedColor = new Color(1f, 1f, 0.5f); // yellowish highlight

    public Sprite battleSprite;
    public Sprite rewardSprite;
    public Sprite shopSprite;
    public Sprite eventSprite;
    public Sprite bossSprite;

    private bool _isHighlighted = false;

    /// <summary>
    /// Fired when the player clicks a node that is currently available.
    /// Hook this up in the inspector to drive your level‑loading logic.
    /// </summary>
    public UnityEvent<ProgressionNode> onClicked;

    private Image _image;
    private ProgressionNode _node;

    void Awake()
    {
        _image = GetComponent<Image>();
        _node = GetComponent<ProgressionNode>();
    }

    public void Refresh(ProgressionNode node)
    {
        if (_node == null) _node = node;

        Color targetColor = lockedColor;
        if (node.completed)
        {
            targetColor = completedColor;
        }
        else if (!node.locked)
        {
            targetColor = availableColor;
        }

        // Apply highlight if set
        if (_isHighlighted && !node.locked)
        {
            targetColor = highlightedColor;
        }

        _image.color = targetColor;

        // Optionally swap sprite according to type
        switch (node.type)
        {
            case NodeType.Battle:
                _image.sprite = battleSprite;
                break;
            case NodeType.Reward:
                _image.sprite = rewardSprite;
                break;
            case NodeType.Shop:
                _image.sprite = shopSprite;
                break;
            case NodeType.Event:
                _image.sprite = eventSprite;
                break;
            case NodeType.Boss:
                _image.sprite = bossSprite;
                break;
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        _isHighlighted = highlighted;
        Refresh(_node);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_node != null && _node.available && !(_node.locked))
        {
            onClicked?.Invoke(_node);
        }
    }
}
