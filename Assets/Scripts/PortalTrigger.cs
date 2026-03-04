using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
	[SerializeField] private string sceneName;

	private void OnTriggerEnter(Collider other)
	{
		// Check if the object entering is the player
		if (other.CompareTag("Player"))
		{
			// Load the specified scene
			SceneManager.LoadScene(sceneName);
		}
	}
}
