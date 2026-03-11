#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ProgressionNodeGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        var node = GetComponent<ProgressionNode>();
        if (node == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);

        if (node.children != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var child in node.children)
            {
                if (child != null)
                    Gizmos.DrawLine(transform.position, child.transform.position);
            }
        }
    }
}
#endif