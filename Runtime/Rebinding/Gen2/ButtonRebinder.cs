using UnityEngine;
using UnityEngine.UI;

namespace GinputSystems.Rebinding.Gen2
{
	[AddComponentMenu("Ginput/Rebinding/Gen2/ButtonRebinder", 0)]
	public class ButtonRebinder : MonoBehaviour
	{
		[SerializeField] private GinputRebinderController inputRebinderController;
		public Text textComponent;

		[SerializeField] private int idControl;
		[SerializeField] private int inputIndex;
		[SerializeField] private string deviceName;

		private void Start()
		{
			if (textComponent == null)
			{
				textComponent = GetComponentInChildren<Text>();
			}

			GetComponent<Button>().onClick.AddListener(delegate { inputRebinderController.BeginRebindInput(idControl, inputIndex, deviceName, textComponent); });
		}
	}
}