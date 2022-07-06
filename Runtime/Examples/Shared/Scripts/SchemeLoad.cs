using UnityEngine;

[AddComponentMenu("Ginput/Examples/SchemeLoad", 0)]
public class SchemeLoad : MonoBehaviour
{
	//this script just ensure the example control scheme is loaded, and not the default control scheme (which has no virtual inputs)
	public GinputSystems.ControlScheme controlScheme;

	private void Awake()
	{
		Ginput.LoadControlScheme(controlScheme, false);
	}
}
