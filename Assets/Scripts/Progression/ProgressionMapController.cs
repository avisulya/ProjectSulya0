using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Example behaviour you can attach to the map's root object.  It listens for
/// clicks from the node UI and dispatches whatever game logic you need (load
/// a battle scene, open a shop window, etc.).  In a real game you'd likely
/// replace or expand this with your own flow controller.
/// </summary>
public class ProgressionMapController : MonoBehaviour
{
    public ProgressionMap map;

    void Start()
    {
        if (map == null)
            map = GetComponent<ProgressionMap>();
    }

    /// <summary>
    /// Hook this up to the node prefab's <c>ProgressionNodeUI.onClicked</c> event
    /// in the inspector so that every node automatically informs the controller
    /// when it's selected.
    /// </summary>
    public void OnNodeSelected(ProgressionNode node)
    {
        if (node == null || !node.available)
            return;

        Debug.Log($"Node clicked: {node.name} ({node.type})");

        // example behaviour: mark the node completed and switch scenes
        node.Complete();

        switch (node.type)
        {
            case NodeType.Battle:
                // load a battle scene (assumes you have scenes named accordingly)
                SceneManager.LoadScene("BattleScene");
                break;
            case NodeType.Shop:
                // open shop UI or load shop scene
                break;
            case NodeType.Reward:
                // give reward then advance
                break;
            case NodeType.Boss:
                SceneManager.LoadScene("BossScene");
                break;
            case NodeType.Event:
                // show event popup
                break;
        }
    }
}
