using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GinputSystems.Touch.TouchButton))]
[CanEditMultipleObjects]
public class GinputTouch_ButtonEditor : Editor
{

	private GinputSystems.Touch.TouchButton btn;

	public void OnSceneGUI()
	{
		btn = this.target as GinputSystems.Touch.TouchButton;
		Handles.color = Color.green;
		if (btn.collisionRadius < 0f)
		{
			Handles.color = Color.red;
		}
		Handles.DrawWireDisc(btn.transform.position, btn.transform.forward, btn.collisionRadius);


	}

}

[CustomEditor(typeof(GinputSystems.Touch.TouchStick))]
[CanEditMultipleObjects]
public class GinputTouch_StickEditor : Editor
{

	private GinputSystems.Touch.TouchStick stick;

	public void OnSceneGUI()
	{
		stick = this.target as GinputSystems.Touch.TouchStick;
		Handles.color = Color.green;
		if (stick.collisionRadius < 0f)
		{
			Handles.color = Color.red;
		}
		Handles.DrawWireDisc(stick.transform.position, stick.transform.forward, stick.collisionRadius);

	}

}
