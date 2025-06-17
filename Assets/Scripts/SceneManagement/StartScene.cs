using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
	public GameObject startButton;
	public GameObject loadingText;
	public GameObject loadingInterface;
	public Image loadingProgressBar;

	[SerializeField, Tooltip("Reference to the Azure Manager")] private AzureManager azureManager;
	[SerializeField, Tooltip("Reference to the Subtitle Manager")] private SettingsMenu settingsMenu;

	private enum StartScenes
    {
		HomeScene,
		LatheIntroduction,
		MillingIntroduction,
		GrinderIntroduction
    }

	[SerializeField, Tooltip("Select start scene")] StartScenes scene;

	/// <summary>
	/// OnAwake Start Coroutine to wait for azure connection
	/// </summary>
    private void Awake()
    {
		StartCoroutine(WaitForAzure());
        if (settingsMenu != null)
        {
			settingsMenu.ToggleSubtitles(false);
		}
	}

    /// <summary>
    /// Method that gets called when the learner clicks the start button.
    /// It hides the start button, calls the load scene and shows the load progress bar.
    /// </summary>
    public void StartSim()
	{
		ToggleStartButton(false);
        ToggleLoadingScreen(true);
		Debug.Log($"Scene to string: {scene.ToString()}");
		ScreenManager.Instance.LoadScene(scene.ToString(), false);

		if (settingsMenu != null)
		{
			settingsMenu.ToggleSubtitles(true);
		}
	}

	//The progress bar fill gets updated.
    private void Update()
    {
		float progress = ScreenManager.Instance.LoadingProgress();
		loadingProgressBar.fillAmount = progress;
	}

    public void ToggleStartButton(bool on)
	{
		startButton.SetActive(on);
	}

	public void ToggleLoadingScreen(bool on)
	{
		loadingInterface.SetActive(on);
	}

	private IEnumerator WaitForAzure()
    {
		loadingText.SetActive(true);

		yield return new WaitUntil(() => azureManager.HasLoaded == true);
		
		loadingText.SetActive(false);

		ToggleStartButton(true);
    }
}
