/*
MIT License

Copyright (c) 2025 Playvue Games LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
		if (buildTarget == BuildTarget.iOS){
			IncrementBuildNumber();
		} else if (buildTarget == BuildTarget.Android){
			IncrementBundleVersion();
		} else {
			return;
		}
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

	private static void IncrementBundleVersion()
	{
		// Load the PlayerSettings asset.
		var playerSettings = Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault();

		if (playerSettings != null)
		{
			int currentValue = PlayerSettings.Android.bundleVersionCode;
			PlayerSettings.Android.bundleVersionCode = currentValue + 1;
		}
	}
}
