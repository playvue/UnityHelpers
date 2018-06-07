using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public static class PostBuildHelper
{
	[PostProcessBuild]
	public static void OnBuildComplete(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget != BuildTarget.iOS)
		{
			return;
		}

		IncrementBuildNumber();
	}

	private static void IncrementBuildNumber()
	{
		// Load the PlayerSettings asset.
		var playerSettings = Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault();

		if (playerSettings != null)
		{

			string currentValue = PlayerSettings.iOS.buildNumber;
			int ver = 0;
			if (int.TryParse (currentValue, out ver)) {
				PlayerSettings.iOS.buildNumber = (ver + 1).ToString ();
			}

		}
	}
}