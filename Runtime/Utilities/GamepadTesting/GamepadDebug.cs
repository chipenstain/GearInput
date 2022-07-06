using System;
using System.Collections.Generic;
using UnityEngine;

namespace GinputSystems.Examples
{
	//not actually an example of sinput stuff (this class is almost exclusively regular input). but lets keep the code hints free from nonsense :)
	[AddComponentMenu("Ginput/Examples/GamepadDebug", 0)]
	public class GamepadDebug : MonoBehaviour
	{
		struct GamepadOutput
		{
			public List<TextMesh> axisDisplay;
			public TextMesh buttonDisplay;
			public TextMesh gamepadName;
		}
		List<GamepadOutput> outputDisplays;

		private void Start()
		{
			float displayX = 0;
			float displayY = 0;

			outputDisplays = new List<GamepadOutput>();

			for (int i = 0; i < Ginput.MAXCONNECTEDGAMEPADS; i++)
			{
				displayY = 0;

				NewTextMesh("Slot: " + i.ToString(), displayX, displayY + 2.5f);

				GamepadOutput newGamepadOutput = new()
				{
					axisDisplay = new List<TextMesh>(),
					gamepadName = NewTextMesh("joystick name", displayX - 0.5f, displayY + 0.5f)
				};
				newGamepadOutput.gamepadName.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
				newGamepadOutput.gamepadName.anchor = TextAnchor.LowerRight;
				newGamepadOutput.gamepadName.characterSize *= 0.8f;

				for (int k = 0; k < Ginput.MAXAXISPERGAMEPAD; k++)
				{
					displayY -= 1f;
					newGamepadOutput.axisDisplay.Add(NewTextMesh("Axis" + (k + 1).ToString(), displayX, displayY));
				}

				displayY -= 1.5f;
				newGamepadOutput.buttonDisplay = NewTextMesh("Buttons Held:", displayX, 1f);

				outputDisplays.Add(newGamepadOutput);

				displayX += 4.5f;
			}
		}

		private TextMesh NewTextMesh(string initText, float x, float y)
		{
			GameObject go = new();
			go.transform.parent = transform;
			go.transform.localPosition = new Vector3(x, y, 0f);
			go.transform.localScale = Vector3.one;
			go.transform.localEulerAngles = Vector3.zero;
			go.name = "Text";
			go.AddComponent<TextMesh>().text = initText;
			go.GetComponent<TextMesh>().characterSize *= 0.5f;
			return go.GetComponent<TextMesh>();
		}

		private void Update()
		{
			for (int i = 0; i < outputDisplays.Count; i++)
			{
				if (i < Input.GetJoystickNames().Length)
				{
					outputDisplays[i].gamepadName.text = "\"" + Input.GetJoystickNames()[i] + "\"";
				}
				else
				{
					outputDisplays[i].gamepadName.text = "Not connected";
				}

				for (int k = 0; k < outputDisplays[i].axisDisplay.Count; k++)
				{
					outputDisplays[i].axisDisplay[k].text = "Axis" + (k + 1).ToString() + ": " + Input.GetAxisRaw(string.Format("J_{0}_{1}", i + 1, k + 1));
				}
				List<int> buttonsPressed = new();
				for (int k = 0; k < Ginput.MAXBUTTONSPERGAMEPAD; k++)
				{
					if (Input.GetKey((KeyCode)(int)(GinputSystems.UnityGamepadKeyCode)Enum.Parse(typeof(GinputSystems.UnityGamepadKeyCode), string.Format("Joystick{0}Button{1}", i + 1, k))))
					{
						buttonsPressed.Add(k);
					}
				}
				if (buttonsPressed.Count == 0)
				{
					outputDisplays[i].buttonDisplay.text = "Buttons:\nNone";
				}
				else
				{
					outputDisplays[i].buttonDisplay.text = "Buttons:\n";
					for (int k = 0; k < buttonsPressed.Count; k++)
					{
						outputDisplays[i].buttonDisplay.text += buttonsPressed[k].ToString() + ", ";
					}
				}
			}
		}
	}
}