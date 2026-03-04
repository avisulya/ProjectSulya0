using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
	[SerializeField] private string sceneName;

	private void OnTriggerEnter(Collider other)
	{
		// Player entered portal trigger; could show UI prompt here
	}

	private void OnTriggerStay(Collider other)
	{
		// When player is inside the trigger and presses E, change scene
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}
