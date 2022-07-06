using GinputSystems;
using UnityEditor;
using UnityEngine;

public class GinputEditorMenus
{

	[MenuItem("Tools/GearInput/Generate Unity Inputs")]
	static void GenerateInputSettings()
	{
		int joystickNumber = Ginput.MAXCONNECTEDGAMEPADS;
		int axisNumber = Ginput.MAXAXISPERGAMEPAD;

		string inputManagerAssetLocation = "ProjectSettings/InputManager.asset";

		System.IO.File.Delete(inputManagerAssetLocation);

		System.IO.StreamWriter sr = System.IO.File.CreateText(inputManagerAssetLocation);

		sr.WriteLine("%YAML 1.1");
		sr.WriteLine("%TAG !u! tag:unity3d.com,2011:");
		sr.WriteLine("--- !u!13 &1");
		sr.WriteLine("InputManager:");
		sr.WriteLine("  m_Axes:");

		//need input axis for all possible gamepad axis
		for (int j = 1; j <= joystickNumber; j++)
		{
			for (int a = 1; a <= axisNumber; a++)
			{

				sr.WriteLine("  - serializedVersion: 3");
				sr.WriteLine("    m_Name: J_" + j.ToString() + "_" + a.ToString());
				sr.WriteLine("    descriptiveName: ");
				sr.WriteLine("    descriptiveNegativeName: ");
				sr.WriteLine("    negativeButton: ");
				sr.WriteLine("    positiveButton: ");
				sr.WriteLine("    altNegativeButton: ");
				sr.WriteLine("    altPositiveButton: ");
				sr.WriteLine("    gravity: 0");
				sr.WriteLine("    dead: 0.19");
				sr.WriteLine("    sensitivity: 1");
				sr.WriteLine("    snap: 0");
				sr.WriteLine("    invert: 0");
				sr.WriteLine("    type: 2");
				sr.WriteLine("    axis: " + (a - 1).ToString());
				sr.WriteLine("    joyNum: " + j.ToString());
			}
		}

		//need these for mouse inputs
		sr.WriteLine("  - serializedVersion: 3");
		sr.WriteLine("    m_Name: Mouse Horizontal");
		sr.WriteLine("    descriptiveName: ");
		sr.WriteLine("    descriptiveNegativeName: ");
		sr.WriteLine("    negativeButton: ");
		sr.WriteLine("    positiveButton: ");
		sr.WriteLine("    altNegativeButton: ");
		sr.WriteLine("    altPositiveButton: ");
		sr.WriteLine("    gravity: 1000");
		sr.WriteLine("    dead: 0");
		sr.WriteLine("    sensitivity: 1");
		sr.WriteLine("    snap: 0");
		sr.WriteLine("    invert: 0");
		sr.WriteLine("    type: 1");
		sr.WriteLine("    axis: 0");
		sr.WriteLine("    joyNum: 0");

		sr.WriteLine("  - serializedVersion: 3");
		sr.WriteLine("    m_Name: Mouse Vertical");
		sr.WriteLine("    descriptiveName: ");
		sr.WriteLine("    descriptiveNegativeName: ");
		sr.WriteLine("    negativeButton: ");
		sr.WriteLine("    positiveButton: ");
		sr.WriteLine("    altNegativeButton: ");
		sr.WriteLine("    altPositiveButton: ");
		sr.WriteLine("    gravity: 1000");
		sr.WriteLine("    dead: 0");
		sr.WriteLine("    sensitivity: 1");
		sr.WriteLine("    snap: 0");
		sr.WriteLine("    invert: 0");
		sr.WriteLine("    type: 1");
		sr.WriteLine("    axis: 1");
		sr.WriteLine("    joyNum: 0");

		sr.WriteLine("  - serializedVersion: 3");
		sr.WriteLine("    m_Name: Mouse Scroll");
		sr.WriteLine("    descriptiveName: ");
		sr.WriteLine("    descriptiveNegativeName: ");
		sr.WriteLine("    negativeButton: ");
		sr.WriteLine("    positiveButton: ");
		sr.WriteLine("    altNegativeButton: ");
		sr.WriteLine("    altPositiveButton: ");
		sr.WriteLine("    gravity: 1000");
		sr.WriteLine("    dead: 0");
		sr.WriteLine("    sensitivity: 1");
		sr.WriteLine("    snap: 0");
		sr.WriteLine("    invert: 0");
		sr.WriteLine("    type: 1");
		sr.WriteLine("    axis: 2");
		sr.WriteLine("    joyNum: 0");


		//and annoyingly, we need these so any StandaloneInputModule doesn't blow it's mind when it tries to use unity's crummy input system for UI events
		sr.WriteLine("  - serializedVersion: 3");
		sr.WriteLine("    m_Name: Horizontal");
		sr.WriteLine("    descriptiveName: ");
		sr.WriteLine("    descriptiveNegativeName: ");
		sr.WriteLine("    negativeButton: left");
		sr.WriteLine("    positiveButton: d");
		sr.WriteLine("    altNegativeButton: a");
		sr.WriteLine("    altPositiveButton: right");
		sr.WriteLine("    gravity: 1000");
		sr.WriteLine("    dead: 0.001");
		sr.WriteLine("    sensitivity: 1000");
		sr.WriteLine("    snap: 0");
		sr.WriteLine("    invert: 0");
		sr.WriteLine("    type: 0");
		sr.WriteLine("    axis: 0");
		sr.WriteLine("    joyNum: 0");

		sr.WriteLine("  - serializedVersion: 3");
		sr.WriteLine("    m_Name: Vertical");
		sr.WriteLine("    descriptiveName: ");
		sr.WriteLine("    descriptiveNegativeName: ");
		sr.WriteLine("    negativeButton: down");
		sr.WriteLine("    positiveButton: up");
		sr.WriteLine("    altNegativeButton: s");
		sr.WriteLine("    altPositiveButton: w");
		sr.WriteLine("    gravity: 1000");
		sr.WriteLine("    dead: 0.001");
		sr.WriteLine("    sensitivity: 1000");
		sr.WriteLine("    snap: 0");
		sr.WriteLine("    invert: 0");
		sr.WriteLine("    type: 0");
		sr.WriteLine("    axis: 0");
		sr.WriteLine("    joyNum: 0");

		sr.Close();

		AssetDatabase.Refresh();

		EditorUtility.DisplayDialog("Ginput", "Input Manager settings have been generated.", "OK");
	}

	[MenuItem("Tools/GearInput/Select Control Scheme")]
	static void SelectControlScheme()
	{
		//select main control scheme
		Object mainControlScheme = Resources.Load("MainControlScheme");
		if (mainControlScheme != null)
		{
			Selection.activeObject = mainControlScheme;
			return;
		}

		//couldn't find control scheme, creating a new one
		Debug.Log("Control scheme named \"MainControlScheme\" not found, so creating a new one with default controls.");
		ControlScheme controlScheme = (ControlScheme)ScriptableObject.CreateInstance(typeof(ControlScheme));

		AssetDatabase.CreateAsset(controlScheme, "Assets/Plugins/Ginput/Resources/MainControlScheme.asset");

		//now select the new control scheme
		mainControlScheme = Resources.Load("MainControlScheme");
		if (mainControlScheme != null)
		{
			Selection.activeObject = mainControlScheme;
			return;
		}
	}

	[MenuItem("Tools/GearInput/Legacy/Rebind Menu Settings", priority = 9999)]
	static void SelectRebindMenuSettings()
	{
		//select main control scheme
		Object mainRebindSettings = Resources.Load("RebindMenuSettings");
		if (mainRebindSettings != null)
		{
			Selection.activeObject = mainRebindSettings;
			return;
		}

		//couldn't find control scheme, creating a new one
		Debug.Log("Rebind menu settings named \"MainControlScheme\" not found, so creating a new one.");
		GinputSystems.Rebinding.RebindMenuSettings newRebindMenuSettings = (GinputSystems.Rebinding.RebindMenuSettings)ScriptableObject.CreateInstance(typeof(GinputSystems.Rebinding.RebindMenuSettings));

		AssetDatabase.CreateAsset(newRebindMenuSettings, "Assets/Plugins/Ginput/Resources/RebindMenuSettings.asset");

		//now select the new control scheme
		mainRebindSettings = Resources.Load("RebindMenuSettings");
		if (mainRebindSettings != null)
		{
			Selection.activeObject = mainRebindSettings;
			return;
		}
	}
}
