using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
	[SerializeField] private string sceneName;

	private void OnTriggerEnter(Collider other)
	{
		// Player entered portal trigger; show UI popup
	}

	private void OnTriggerStay(Collider other)
	{
		// While the player is inside the triggered collider, press E to teleport to the scene
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}
