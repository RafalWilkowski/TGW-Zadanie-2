using UnityEngine;

public class CaptureScreen : MonoBehaviour
{
	[SerializeField] bool shootOnStart;
	[SerializeField] float shootInterval = 1f;
	[SerializeField] float delay = 0;
	int screenshotNumber;
	float startTime, lastScreenshotTime;
    // Start is called before the first frame update
    void Start()
    {
		if (shootOnStart)
		{
			ScreenCapture.CaptureScreenshot("D:/ScreenShot.png");
			Debug.Log("Screenshot taken!");

		}
		startTime = lastScreenshotTime = Time.time;
	}
	private void Update()
	{
		if (Time.time - startTime < delay) return;
		if (Time.time - lastScreenshotTime > shootInterval)
		{
			string screenshotName = string.Format("D:/ScreenShot{0:0000}.png", screenshotNumber);
			ScreenCapture.CaptureScreenshot(screenshotName);
			Debug.LogFormat("Screenshot {0} taken!", screenshotName);
			screenshotNumber++;
			lastScreenshotTime = Time.time;
		}
	}
}
 