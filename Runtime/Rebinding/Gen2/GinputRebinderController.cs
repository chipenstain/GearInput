using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GinputSystems.Rebinding.Gen2
{
	[RequireComponent(typeof(GinputMonitor))]
	[AddComponentMenu("Ginput/Rebinding/Gen2/GinputRebinderController", 0)]
	public class GinputRebinderController : MonoBehaviour
	{
		private GinputMonitor inputMonitor;

		public Control[] controls;

		private bool rebinding = false;
		private int rebindingFrames = 0;
		private int rebindingControlIndex = -1;
		private int rebindingInputIndex = -1;
		private string rebindingDevice;
		private Text rebindInputText;

		private string[] recordedPads = new string[0];

		private void Awake()
		{
			inputMonitor = GetComponent<GinputMonitor>();
		}

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			Ginput.LoadControlScheme("MainControlScheme", true);
			controls = Ginput.controls;
		}

		private void Update()
		{
			bool gamepadsChanged = false;
			string[] gamepads = Ginput.gamepads;
			if (recordedPads.Length != gamepads.Length) gamepadsChanged = true;
			if (!gamepadsChanged)
			{
				for (int i = 0; i < recordedPads.Length; i++)
				{
					if (recordedPads[i].ToUpper() != gamepads[i].ToUpper()) gamepadsChanged = true;
				}
			}
			if (gamepadsChanged)
			{
				//connected gamepads have changed
				rebinding = false;

				recordedPads = Ginput.gamepads;
				Init();
				return;
			}


			if (!rebinding) return;
			rebindingFrames++;
			if (rebindingFrames < 5) return;

			//we're ready to swap out an input now
			if (inputMonitor.changeFound)
			{
				rebinding = false;
				rebindInputText.text = "?";
				//Debug.Log("CHANGE INPUT SETTING NOW!");


				InputDeviceType changedInputType = InputDeviceType.Keyboard;

				if (inputMonitor.changedKey != KeyCode.None)
				{
					//change was keyboard input
					//Debug.Log(inputMonitor.changedKey.ToString());
					changedInputType = InputDeviceType.Keyboard;
				}
				if (inputMonitor.changedPadAxis.padIndex >= 0)
				{
					//change was a gamepad axis
					//Debug.Log("Found change in gamepad " + (inputMonitor.changedPadAxis.padIndex+1).ToString() + " axis " + (inputMonitor.changedPadAxis.inputIndex).ToString());
					changedInputType = InputDeviceType.GamepadAxis;
				}
				if (inputMonitor.changedPadButton.padIndex >= 0)
				{
					//change was a gamepad button
					//Debug.Log("Found change in gamepad " + (inputMonitor.changedPadButton.padIndex+1).ToString() + " button " + inputMonitor.changedPadButton.inputIndex.ToString());
					changedInputType = InputDeviceType.GamepadButton;
				}
				if (inputMonitor.changedMouse != MouseInputType.None)
				{
					//change was mouse click
					//Debug.Log(inputMonitor.changedMouse.ToString());
					changedInputType = InputDeviceType.Mouse;
				}

				DeviceInput newInput = new DeviceInput(changedInputType);
				newInput.isCustom = true;
				newInput.deviceName = rebindingDevice;
				newInput.commonMappingType = CommonGamepadInputs.NOBUTTON;//don't remove this input when gamepads are unplugged/replugged

				if (changedInputType == InputDeviceType.Keyboard)
				{
					newInput.keyboardKeyCode = inputMonitor.changedKey;
				}

				int padIndex = -1;
				if (changedInputType == InputDeviceType.GamepadButton)
				{
					newInput.gamepadButtonNumber = inputMonitor.changedPadButton.inputIndex;
					newInput.displayName = "B" + inputMonitor.changedPadButton.inputIndex.ToString();

					padIndex = inputMonitor.changedPadButton.padIndex;
					List<int> slots = new List<int>();
					for (int g = 0; g < gamepads.Length; g++)
					{
						if (gamepads[g].ToUpper() == rebindingDevice.ToUpper()) slots.Add(g);
					}
					newInput.allowedSlots = slots.ToArray();
				}

				if (changedInputType == InputDeviceType.GamepadAxis)
				{
					GinputMonitor.GamepadStateChange axisChange = inputMonitor.changedPadAxis;

					newInput.gamepadAxisNumber = axisChange.inputIndex;
					newInput.displayName = "A" + axisChange.inputIndex.ToString();
					if (axisChange.axisMotionIsPositive) newInput.displayName += "+";
					if (!axisChange.axisMotionIsPositive) newInput.displayName += "-";

					newInput.invertAxis = false;
					if (!axisChange.axisMotionIsPositive) newInput.invertAxis = true;

					newInput.clampAxis = true;
					newInput.axisButtoncompareVal = 0.4f;
					newInput.defaultAxisValue = 0f;

					newInput.rescaleAxis = false;
					if (axisChange.restingValue != 0f)
					{
						newInput.rescaleAxis = true;
						if (axisChange.restingValue < 0f)
						{
							newInput.rescaleAxisMin = -1f;
							newInput.rescaleAxisMax = 1f;
						}
						else
						{
							newInput.rescaleAxisMin = 1f;
							newInput.rescaleAxisMax = -1f;
						}
					}

					padIndex = axisChange.padIndex;
					List<int> slots = new List<int>();
					for (int g = 0; g < gamepads.Length; g++)
					{
						if (gamepads[g].ToUpper() == rebindingDevice.ToUpper()) slots.Add(g);
					}
					newInput.allowedSlots = slots.ToArray();

				}

				if (changedInputType == InputDeviceType.GamepadAxis || changedInputType == InputDeviceType.GamepadButton)
				{
					//lets also set all other inputs on this control with matching allowed slots to be custom and remove common binding
					//this should preserve stuff with the same common binding from being ignored when re reload common bindings
					for (int i = 0; i < controls[rebindingControlIndex].inputs.Count; i++)
					{
						bool sameDevice = false;
						if (controls[rebindingControlIndex].inputs[i].commonMappingType != CommonGamepadInputs.NOBUTTON)
						{
							for (int k = 0; k < controls[rebindingControlIndex].inputs[i].allowedSlots.Length; k++)
							{
								if (controls[rebindingControlIndex].inputs[i].allowedSlots[k] == padIndex) sameDevice = true;
							}
						}
						if (sameDevice)
						{
							controls[rebindingControlIndex].inputs[i].isCustom = true;
							controls[rebindingControlIndex].inputs[i].commonMappingType = CommonGamepadInputs.NOBUTTON;
							controls[rebindingControlIndex].inputs[i].deviceName = rebindingDevice;
						}
					}
				}

				if (changedInputType == InputDeviceType.Mouse)
				{
					newInput.mouseInputType = inputMonitor.changedMouse;
				}

				controls[rebindingControlIndex].inputs[rebindingInputIndex] = newInput;

				rebindInputText.text = controls[rebindingControlIndex].inputs[rebindingInputIndex].GetDisplayName();
			}
		}

		public void BeginRebindInput(int controlIndex, int inputIndex, string deviceName, Text text)
		{
			if (rebinding) return;
			rebinding = true;
			rebindingFrames = 0;
			rebindingControlIndex = controlIndex;
			rebindingInputIndex = inputIndex;
			rebindInputText = text;
			rebindInputText.text = "...";
			rebindingDevice = deviceName;

			inputMonitor.SetListeningDevice(rebindingDevice);
		}
	}
}