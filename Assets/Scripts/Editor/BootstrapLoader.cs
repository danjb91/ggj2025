using UnityEngine;
using UnityEngine.SceneManagement;

public static class BootstrapLoader
{
	private static int bootstrapSceneID = 0;
	private static int bootSceneIndex = -1;
	
	#if UNITY_EDITOR
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void BootstrapSceneCheck()
	{
		// Check if bootstrap is loaded
		if (IsBootstrapLoaded()) return;
		
		// Bootstrap is not loaded
		// Store the wanted scene index
		bootSceneIndex = SceneManager.GetActiveScene().buildIndex;

		// Load bootstrap scene
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
	#endif

	private static bool IsBootstrapLoaded()
	{
		// Go through currently loaded scenes
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Debug.Log(SceneManager.GetSceneAt(0).path);
			// Bootstrap scene is at build index 0
			if (SceneManager.GetSceneAt(i).buildIndex == bootstrapSceneID)
				return true;
		}

		return false;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	public static async void InitializeBootstrap()
	{
		try
		{
			// Bootstraps loaded, now load the wanted scene
			await SceneManager.LoadSceneAsync(bootSceneIndex, LoadSceneMode.Additive);
			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(bootSceneIndex));
		}
		catch (System.Exception e)
		{
			Debug.LogError($"[CRITICAL] Could not initialize\n{e.Message}");
		}
	}
}
