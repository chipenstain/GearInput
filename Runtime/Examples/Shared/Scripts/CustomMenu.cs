using UnityEngine;

[AddComponentMenu("Ginput/Examples/CustomMenu", 0)]
public class CustomMenu : MonoBehaviour
{
	public CustomMenuItem currentMenuItem;
	public Transform cam;
	public Transform cursor;

	private void Start()
	{
		currentMenuItem.highlighted = true;
	}

	private void Update()
	{
		if (Ginput.GetButtonDownRepeating("Up"))
		{
			//highlight item above
			currentMenuItem.highlighted = false;
			currentMenuItem = currentMenuItem.itemAbove;
			currentMenuItem.highlighted = true;
		}
		if (Ginput.GetButtonDownRepeating("Down"))
		{
			//highlight item below
			currentMenuItem.highlighted = false;
			currentMenuItem = currentMenuItem.itemBelow;
			currentMenuItem.highlighted = true;
		}
		if (Ginput.GetButtonDown("Submit"))
		{
			//select this item
			currentMenuItem.Select();
			Ginput.ResetInputs();
		}

		cam.SetPositionAndRotation(Vector3.Lerp(cam.position, currentMenuItem.camTargetPos.position, Time.deltaTime * 4f), Quaternion.Slerp(cam.rotation, currentMenuItem.camTargetPos.rotation, Time.deltaTime * 4f));

		cursor.position = currentMenuItem.cursorTarget.position;
	}
}
