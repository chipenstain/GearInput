using UnityEditor;

[CustomEditor(typeof(UnityEngine.EventSystems.StandaloneGinputModule))]
[CanEditMultipleObjects]
public class StandaloneGinputModuleEditor : Editor
{

	private UnityEngine.EventSystems.StandaloneGinputModule ssm;

	public override void OnInspectorGUI()
	{
		ssm = this.target as UnityEngine.EventSystems.StandaloneGinputModule;

		ssm.m_GinputUpButton = EditorGUILayout.TextField("Up Control", ssm.m_GinputUpButton);
		ssm.m_GinputDownButton = EditorGUILayout.TextField("Down Control", ssm.m_GinputDownButton);
		ssm.m_GinputLeftButton = EditorGUILayout.TextField("Left Control", ssm.m_GinputLeftButton);
		ssm.m_GinputRightButton = EditorGUILayout.TextField("Right Control", ssm.m_GinputRightButton);
		ssm.m_GinputSubmitButton = EditorGUILayout.TextField("Submit Control", ssm.m_GinputSubmitButton);
		ssm.m_GinputCancelButton = EditorGUILayout.TextField("Cancel Control", ssm.m_GinputCancelButton);


		ssm.forceModuleActive = EditorGUILayout.Toggle("Force Module Active", ssm.forceModuleActive);
	}

}

