using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
//public static class BuildUtil
//{
//    private static readonly string[] EnableScenes = FindEnabledEditorScenes();
//    [MenuItem("MyTools/Windows Build With Postprocess")]
//    public static void BuildGame()
//    {
//        // Get filename.
//        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
//        string[] levels = new string[] { "Assets/Scene.unity" };
//        PlayerSettings.SplashScreen.showUnityLogo = false;


//        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
//        // Build player.
//        var s = BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);

//        // Copy a file from the project folder to the build folder, alongside the built game.
//        // FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

//        // Run the game (Process class from System.Diagnostics).
//        Process proc = new Process();
//        proc.StartInfo.FileName = path + "/BuiltGame.exe";
//        proc.Start();
//    }
//    private static string[] FindEnabledEditorScenes()
//    {
//        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
//    }
//}

public class ProjectBuilder : EditorWindow
{

	[Flags] private enum BuildStandalonePlatform { Win = 1 << 0, Mac = 1 << 1, Linux = 1 << 2, All = ~0 }
	private static string buildPath;
	private static ProjectBuilder window;
	private static string productName;
	private static string productFolder = "/Build-1.1";
	private static bool architecture_x64 = false;
	private static bool buildAndPlay = false;
	private static bool zipContent = false;
	private static bool showDirectory = false;

	private static BuildStandalonePlatform buildTarget = BuildStandalonePlatform.All;
	[MenuItem("HuntroxUtils/Project/ProjectBuilder", false, 0)]
	public static void ShowWindow()
	{
		window = GetWindow<ProjectBuilder>("ProjectBuilder");
		window.minSize = new Vector2(360, 250);
		window.Show();
	}
	private void OnEnable()
	{
		Load();
		DrawToolbar();
		DrawBuilSettings();
		DrawButtons();
	}

	private void Load()
	{
		productName = PlayerSettings.productName;
		buildTarget = (BuildStandalonePlatform)EditorPrefs.GetInt("buildTarget", (int)BuildStandalonePlatform.All);
		buildPath = EditorPrefs.GetString("lastBuildPath", Path.GetFullPath(".") + Path.DirectorySeparatorChar + @"Build");
		productFolder = EditorPrefs.GetString("productFolder", "/Build-1.0");
		architecture_x64 = EditorPrefs.GetBool("architecture64", true);
		buildAndPlay = EditorPrefs.GetBool("buildAndPlay", true);
		zipContent = EditorPrefs.GetBool("zipContent", true);
		showDirectory = EditorPrefs.GetBool("showDirectory", true);
	}

	private void DrawBuilSettings()
	{
		VisualElement Container = new VisualElement { name = "Standalone" };
		Box box = new Box { style = { marginRight = 10 } };


		TextField productNameField = new TextField("productName")
		{
			name = "GameNameField",
			value = productName
		};
		TextField BuildFolderField = new TextField("BuildFolder")
		{
			name = "BuildFolderField",
			value = productFolder,
		};
		BuildFolderField.UnregisterValueChangedCallback(evt =>
		{
			productFolder = evt.newValue;
			EditorPrefs.SetString("productFolder", evt.newValue);
		});

		Toggle x64Toggle = new Toggle("x64") { value = architecture_x64 };
		Toggle buildAndPlayToggle = new Toggle("Build and Play (only Windows)") { value = buildAndPlay };
		Toggle zipContentToggle = new Toggle("Zip Build Folder") { value = zipContent };
		Toggle showDirectoryToggle = new Toggle("Open Build Folder") { value = showDirectory };

		x64Toggle.RegisterValueChangedCallback(evt =>
		{
			architecture_x64 = evt.newValue;
			EditorPrefs.SetBool("architecture64", evt.newValue);
		});
		buildAndPlayToggle.RegisterValueChangedCallback(evt =>
		{
			buildAndPlay = evt.newValue;
			EditorPrefs.SetBool("buildAndPlay", evt.newValue);
		});
		zipContentToggle.RegisterValueChangedCallback(evt =>
		{
			zipContent = evt.newValue;
			EditorPrefs.SetBool("zipContent", evt.newValue);
		});

		showDirectoryToggle.RegisterValueChangedCallback(evt =>
		{
			showDirectory = evt.newValue;
			EditorPrefs.SetBool("showDirectory", evt.newValue);
		});

		Foldout foldout = new Foldout { text = "Build Settings", value = true };

		EnumFlagsField targetBuildPlatforms = new EnumFlagsField("Build Target", buildTarget)
		{
			value = buildTarget,
		};
		targetBuildPlatforms.RegisterValueChangedCallback(evt =>
		{
			buildTarget = (BuildStandalonePlatform)evt.newValue;
			EditorPrefs.SetInt("buildTarget", (int)((BuildStandalonePlatform)evt.newValue));
		});

		box.Add(targetBuildPlatforms);
		box.Add(x64Toggle);
		box.Add(productNameField);
		box.Add(BuildFolderField);
		box.Add(zipContentToggle);
		box.Add(buildAndPlayToggle);
		box.Add(showDirectoryToggle);


		foldout.Add(box);
		Container.Add(foldout);
		rootVisualElement.Add(Container);
	}

	private void DrawButtons()
	{
		Button button = new Button(Compress) { text = "Compress" };
		Button buildBtn = new Button(PerformBuild) { text = "Build",style = {minHeight=64,marginLeft =10, marginRight=10 } };
		//rootVisualElement.Add(button);
		Button revealButton =new Button(() => EditorUtility.RevealInFinder(buildPath + "/")) { text = "Open Build Path", style = { marginLeft = 10, marginRight = 10 } };
		rootVisualElement.Add(revealButton);
		rootVisualElement.Add(buildBtn);
	}

	private void DrawToolbar()
	{
		Toolbar toolbar = new Toolbar();

		ToolbarButton BuildBtn = new ToolbarButton(PerformBuild)
		{
			text = "Build"
		};

		TextField buildPathField = new TextField
		{
			value = buildPath,
			style =
			{
				minWidth=200,
				maxWidth=300,
			},

		};
		buildPathField.RegisterValueChangedCallback(evt =>
		{
			buildPath = evt.newValue;
			EditorPrefs.SetString("lastBuildPath", evt.newValue);
		});
		ToolbarButton buildPathBtn = new ToolbarButton(() =>
		{
			buildPath = EditorUtility.SaveFolderPanel("Choose Build Location", "", "");
			buildPathField.value = buildPath;
		})
		{
			text = "Build Path: "
		};
		toolbar.Add(buildPathBtn);
		toolbar.Add(buildPathField);
		toolbar.Add(new ToolbarSpacer { name = "spacer1", flex = true });
		rootVisualElement.Add(toolbar);
	}

	private void PerformBuild()
	{
		if (buildTarget == 0)
		{
			Debug.Log("Select Target Platform First");
			return;
		}
		BuildOptions buildOptions = BuildOptions.None;
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		string fileName;
		var stopwatch = System.Diagnostics.Stopwatch.StartNew();
		try
		{
			if (!stopwatch.IsRunning)
				stopwatch.Start();

			BuildTarget build_target = BuildTarget.NoTarget;
			if (buildTarget.HasFlag(BuildStandalonePlatform.Win))
			{
				build_target = (architecture_x64) ? BuildTarget.StandaloneWindows64 : BuildTarget.StandaloneWindows;
				string path = buildPath + productFolder + "(Win)/";
				Directory.CreateDirectory(path);
				fileName = path + productName + ".exe";

				PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
				buildPlayerOptions.scenes = GetAllEnabledEditorScenes();
				buildPlayerOptions.target = build_target;
				buildPlayerOptions.options = buildOptions;
				buildPlayerOptions.locationPathName = fileName;
				GenericBuild(build_target, buildPlayerOptions);
			}
			if (buildTarget.HasFlag(BuildStandalonePlatform.Linux))
			{
				build_target = BuildTarget.StandaloneLinux64;
				string path = buildPath + productFolder + "(Linux)/";
				Directory.CreateDirectory(path);
				fileName = path + productName + ".x86_64";
				PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
				buildPlayerOptions.scenes = GetAllEnabledEditorScenes();
				buildPlayerOptions.target = build_target;
				buildPlayerOptions.options = buildOptions;
				buildPlayerOptions.locationPathName = fileName;
				GenericBuild(build_target, buildPlayerOptions);
			}
			if (buildTarget.HasFlag(BuildStandalonePlatform.Mac))
			{
				build_target = BuildTarget.StandaloneOSX;
				string path = buildPath + productFolder + "(Mac)/";
				Directory.CreateDirectory(path);
				fileName = path + productName;
				PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
				buildPlayerOptions.scenes = GetAllEnabledEditorScenes();
				buildPlayerOptions.target = build_target;
				buildPlayerOptions.options = buildOptions;
				buildPlayerOptions.locationPathName = fileName;
				GenericBuild(build_target, buildPlayerOptions);

			}
		}catch(Exception e)
		{
			Debug.Log(e.Message);
		}
		finally
		{
			stopwatch.Stop();
			Debug.Log("=====================================================");
			Debug.Log("Build Time: " + stopwatch.Elapsed.TotalSeconds + "s");
		}

		if (zipContent)
			Compress();
		if(showDirectory)
			EditorUtility.RevealInFinder(buildPath+ "/");
		Debug.Log("Finished");

		if (buildAndPlay)
		{
			Debug.Log("Launching the game...");
			var proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = buildPath+ productFolder+"(Win)"+ "/"+productName+".exe";
			proc.Start();
		}
	}

	private void Compress()
	{
		Load();
		Debug.Log("Compressing...");
		List<Action> actions = new List<Action>();

		if (buildTarget.HasFlag(BuildStandalonePlatform.Win))
			actions.Add(CompressDirectory(buildPath, productFolder + "(Win)"));
		if (buildTarget.HasFlag(BuildStandalonePlatform.Linux))
			actions.Add(CompressDirectory(buildPath, productFolder + "(Linux)"));
		if (buildTarget.HasFlag(BuildStandalonePlatform.Mac))
			actions.Add(CompressDirectory(buildPath, productFolder + "(Mac)"));

		for (int i = 0; i < actions.Count; i++)
			actions[i].Invoke();
		Debug.Log("Compressing Finshed");
	}

	private void GenericBuild(BuildTarget build_target, BuildPlayerOptions buildPlayerOptions)
	{

		var stopwatch = System.Diagnostics.Stopwatch.StartNew();
		try
		{
			if (!stopwatch.IsRunning)
				stopwatch.Start();
			Debug.Log("StartBuilding...");
			Debug.Log("Target: "+buildPlayerOptions.target);
			PlayerSettings.SplashScreen.showUnityLogo = false; //desperate attempt to get rid of that logo :'(
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(build_target), build_target);
			var resu = BuildPipeline.BuildPlayer(buildPlayerOptions);

			BuildSummary summary = resu.summary;
			if (summary.result == BuildResult.Succeeded)
			{

				
				Debug.Log(buildPlayerOptions.target + " Build succeeded");
				Debug.Log(buildPlayerOptions.target + " Size: " + (summary.totalSize / 1e+6) + " mb");
			}

			if (summary.result == BuildResult.Failed)
			{
				Debug.Log(buildPlayerOptions.target + " Build was failed");
				Debug.Log(summary.result);
			}
		}
		catch (Exception e)
		{
			Debug.Log(buildPlayerOptions.target + " Build was failed");
			Debug.Log("==============");
			Debug.Log(e.Message);
		}
		finally
		{
			stopwatch.Stop();
			Debug.Log(buildPlayerOptions.target + " Build Time: " + stopwatch.Elapsed.TotalSeconds + "s");
		}
	}

	private Action CompressDirectory(string inputPath, string filename,bool osx =false)
		=> new Action(() =>
		{
			if (osx)
			{
				string startPath = Path.GetDirectoryName(inputPath + filename + "/" + productName + ".app/Contents/");
				string zipPath = Path.Combine(Path.GetDirectoryName(inputPath + filename + "/"), Path.GetDirectoryName(inputPath + filename + "/") + ".zip");
				if (Directory.Exists(startPath))
				{
					if (File.Exists(zipPath))
						File.Delete(zipPath);
					ZipFile.CreateFromDirectory(startPath, zipPath, System.IO.Compression.CompressionLevel.Optimal, true);
				}
				else
				{
					Debug.Log($"Directory '{startPath}' Does Not Exist!");
				}
			}
			else
			{
				string startPath = Path.GetDirectoryName(inputPath + filename + "/");
				string zipPath = Path.Combine(startPath, startPath + ".zip");
				string il2cppFolder = Path.GetDirectoryName(inputPath + filename + "/"+$"{productName}_BackUpThisFolder_ButDontShipItWithYourGame"+"/");
				if (Directory.Exists(il2cppFolder))
				{
					var dirtrytInfo = new DirectoryInfo(il2cppFolder);

					foreach (FileInfo file in dirtrytInfo.GetFiles())
					{
						file.Delete();
					}
					foreach (DirectoryInfo dir in dirtrytInfo.GetDirectories())
					{
						dir.Delete(true);
					}
					Directory.Delete(il2cppFolder);
				}
				if (Directory.Exists(startPath))
				{
					if (File.Exists(zipPath))
						File.Delete(zipPath);
					ZipFile.CreateFromDirectory(startPath, zipPath, System.IO.Compression.CompressionLevel.Optimal, true);

				}
				else
				{
					Debug.Log($"Directory '{startPath}' Does Not Exist!");
				}
			}

		});

	private string[] GetAllEnabledEditorScenes() => (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();

}