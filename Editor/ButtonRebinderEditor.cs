using GinputSystems.Rebinding.Gen2;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ButtonRebinder))]
[CanEditMultipleObjects]
public class ButtonRebinderEditor : Editor
{
	private int selectedControl;

	private SerializedProperty inputRebinderController;
	private SerializedProperty textComponent;
	private SerializedProperty idControl;
	private SerializedProperty inputIndex;
	private SerializedProperty deviceName;

	private void OnEnable()
	{
		inputRebinderController = serializedObject.FindProperty("inputRebinderController");
		textComponent = serializedObject.FindProperty("textComponent");
		idControl = serializedObject.FindProperty("idControl");
		inputIndex = serializedObject.FindProperty("inputIndex");
		deviceName = serializedObject.FindProperty("deviceName");

		selectedControl = idControl.intValue;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		if (inputRebinderController.objectReferenceValue == null)
		{
			inputRebinderController.objectReferenceValue = FindObjectOfType<GinputRebinderController>();
		}

		EditorGUILayout.PropertyField(textComponent);
		((ButtonRebinder)target).textComponent.text = Ginput.controls[idControl.intValue].inputs[inputIndex.intValue].GetDisplayName();

		List<string> controls = new();
		Ginput.LoadControlScheme("MainControlScheme", true);
		foreach (GinputSystems.Control control in Ginput.controls)
		{
			controls.Add(control.name);
		}
		selectedControl = EditorGUILayout.Popup("Control", selectedControl, controls.ToArray());
		idControl.intValue = selectedControl;

		EditorGUILayout.PropertyField(inputIndex);
		EditorGUILayout.PropertyField(deviceName);

		serializedObject.ApplyModifiedProperties();
	}
}
