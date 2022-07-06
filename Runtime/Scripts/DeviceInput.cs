using UnityEngine;

namespace GinputSystems
{
	public class DeviceInput
	{
		public InputDeviceType inputType;
		public string displayName;

		//custom bound stuff
		public bool isCustom = false;
		public string deviceName = "";

		public string GetDisplayName()
		{
			if (inputType == InputDeviceType.Keyboard)
			{
				return keyboardKeyCode.ToString();
			}
			if (inputType == InputDeviceType.Mouse)
			{
				return mouseInputType.ToString();
			}
			return displayName;
		}

		public DeviceInput(InputDeviceType type)
		{
			inputType = type;
		}

		//////////// ~ keyboard specific stuff ~ ////////////
		public KeyCode keyboardKeyCode; //keycode for if this input is controlled by a keyboard key

		//////////// ~ mouse specific stuff ~ ////////////
		public MouseInputType mouseInputType;

		//////////// ~ gamepad specific stuff ~ ////////////
		public int[] allowedSlots; //list of gamepad slots that this input is allowed to check (they will be ones with a matching name to the known binding
		public CommonGamepadInputs commonMappingType; //if this is set, this input is a preset/default
		public int gamepadButtonNumber; //button number for if this input is controlled by a gamepad button

		public int gamepadAxisNumber; //axis number for if this input is controlled by a gamepad axis
		public bool invertAxis;
		public bool clampAxis;
		public bool rescaleAxis;//for rescaling input axis from something else to 0-1
		public float rescaleAxisMin;
		public float rescaleAxisMax;

		//stuff for treating axis like a button
		public float axisButtoncompareVal; //axis button is 'pressed' if (axisValue [compareType] compareVal)

		//all GetAxis() checks will return default value until a measured change occurs, since readings before then can be wrong
		private bool useDefaultAxisValue = true;
		public float defaultAxisValue;
		private float measuredAxisValue = -54.321f;

		//////////// ~ virtual specific stuff ~ ////////////
		public string virtualInputID;

		public bool ButtonHeldCheck(InputDeviceSlot slot)
		{
			//keyboard key checks
			if (inputType == InputDeviceType.Keyboard)
			{
				if (slot == InputDeviceSlot.any || slot == InputDeviceSlot.keyboard || slot == InputDeviceSlot.keyboardAndMouse)
				{
					if (Input.GetKey(keyboardKeyCode)) return true;
					if (Input.GetKeyDown(keyboardKeyCode)) return true;
				}

				return false;
			}

			//gamepad button checks
			if (inputType == InputDeviceType.GamepadButton || inputType == InputDeviceType.GamepadAxis)
			{
				if (slot == InputDeviceSlot.keyboard || slot == InputDeviceSlot.mouse || slot == InputDeviceSlot.keyboardAndMouse) return false;

				//if checking any slot, call this function for each possible slot
				//this shouldn't happen anymore
				if (slot == InputDeviceSlot.any)
				{
					return ButtonHeldCheck(InputDeviceSlot.gamepad1) || ButtonHeldCheck(InputDeviceSlot.gamepad2) || ButtonHeldCheck(InputDeviceSlot.gamepad3) || ButtonHeldCheck(InputDeviceSlot.gamepad4) || ButtonHeldCheck(InputDeviceSlot.gamepad5) || ButtonHeldCheck(InputDeviceSlot.gamepad6) || ButtonHeldCheck(InputDeviceSlot.gamepad7) || ButtonHeldCheck(InputDeviceSlot.gamepad7) || ButtonHeldCheck(InputDeviceSlot.gamepad9) || ButtonHeldCheck(InputDeviceSlot.gamepad10) || ButtonHeldCheck(InputDeviceSlot.gamepad11) || ButtonHeldCheck(InputDeviceSlot.gamepad12) || ButtonHeldCheck(InputDeviceSlot.gamepad13) || ButtonHeldCheck(InputDeviceSlot.gamepad14) || ButtonHeldCheck(InputDeviceSlot.gamepad15) || ButtonHeldCheck(InputDeviceSlot.gamepad16);
				}

				int slotIndex = ((int)slot) - 1;

				//don't check slots without a connected gamepad
				if (Ginput.connectedGamepads <= slotIndex) return false;

				//make sure the gamepad in this slot is one this input is allowed to check (eg don't check PS4 pad bindings for an XBOX pad)
				bool allowInputFromThisPad = false;
				for (int i = 0; i < allowedSlots.Length; i++)
				{
					if (slotIndex == allowedSlots[i]) allowInputFromThisPad = true;
				}
				if (!allowInputFromThisPad) return false;

				//gamepad button check
				if (inputType == InputDeviceType.GamepadButton)
				{
					//get the keycode for the gamepad's slot/button
					string buttonString = string.Format("Joystick{0}Button{1}", (slotIndex + 1), gamepadButtonNumber);
					if (string.IsNullOrEmpty(buttonString)) return false;
					UnityGamepadKeyCode keyCode = (UnityGamepadKeyCode)System.Enum.Parse(typeof(UnityGamepadKeyCode), buttonString);

					//button check now
					if (Input.GetKey((KeyCode)(int)keyCode)) return true;
					if (Input.GetKeyDown((KeyCode)(int)keyCode)) return true;
				}
				return false;
			}


			//virtual device input checks
			if (inputType == InputDeviceType.Virtual)
			{
				if (slot == InputDeviceSlot.any || slot == InputDeviceSlot.virtual1)
				{
					return VirtualInputs.GetVirtualButton(virtualInputID);
				}
			}

			//mouseaxis button checks (these don't happen)
			if (inputType == InputDeviceType.Mouse)
			{
				if (slot != InputDeviceSlot.any && slot != InputDeviceSlot.mouse && slot != InputDeviceSlot.keyboardAndMouse) return false;

				KeyCode mouseKeyCode = KeyCode.None;
				if (mouseInputType == MouseInputType.Mouse0) mouseKeyCode = KeyCode.Mouse0;
				if (mouseInputType == MouseInputType.Mouse1) mouseKeyCode = KeyCode.Mouse1;
				if (mouseInputType == MouseInputType.Mouse2) mouseKeyCode = KeyCode.Mouse2;
				if (mouseInputType == MouseInputType.Mouse3) mouseKeyCode = KeyCode.Mouse3;
				if (mouseInputType == MouseInputType.Mouse4) mouseKeyCode = KeyCode.Mouse4;
				if (mouseInputType == MouseInputType.Mouse5) mouseKeyCode = KeyCode.Mouse5;
				if (mouseInputType == MouseInputType.Mouse6) mouseKeyCode = KeyCode.Mouse6;

				if (mouseKeyCode != KeyCode.None)
				{
					//clicky mouse input
					if (Input.GetKey(mouseKeyCode)) return true;
					if (Input.GetKeyDown(mouseKeyCode)) return true;
				}
				return false;
			}

			return false;
		}

		public float AxisCheck(InputDeviceSlot slot)
		{
			//keyboard checks
			if (inputType == InputDeviceType.Keyboard)
			{
				if (slot == InputDeviceSlot.any || slot == InputDeviceSlot.keyboard || slot == InputDeviceSlot.keyboardAndMouse)
				{
					if (Input.GetKey(keyboardKeyCode)) return 1f;
				}

				return 0f;
			}

			//gamepad button and axis checks
			if (inputType == InputDeviceType.GamepadButton || inputType == InputDeviceType.GamepadAxis)
			{
				if (slot == InputDeviceSlot.keyboard || slot == InputDeviceSlot.mouse || slot == InputDeviceSlot.keyboardAndMouse) return 0f;

				//if checking any slot, call this function for each possible slot
				if (slot == InputDeviceSlot.any)
				{
					float greatestV = 0f;
					for (int i = 1; i <= Ginput.connectedGamepads; i++)
					{
						greatestV = Mathf.Max(greatestV, Mathf.Abs(AxisCheck((InputDeviceSlot)i)));
					}
					return greatestV;
				}

				int slotIndex = ((int)slot) - 1;

				//don't check slots without a connected gamepad
				if (Ginput.connectedGamepads <= slotIndex) return 0f;

				//make sure the gamepad in this slot is one this input is allowed to check (eg don't check PS4 pad bindings for an XBOX pad)
				bool allowInputFromThisPad = false;
				for (int i = 0; i < allowedSlots.Length; i++)
				{
					if (slotIndex == allowedSlots[i]) allowInputFromThisPad = true;
				}

				if (!allowInputFromThisPad) return 0f;

				//button as axis checks
				if (inputType == InputDeviceType.GamepadButton)
				{
					string buttonString = string.Format("Joystick{0}Button{1}", (slotIndex + 1), gamepadButtonNumber);
					if (string.IsNullOrEmpty(buttonString)) return 0f;
					UnityGamepadKeyCode keyCode = (UnityGamepadKeyCode)System.Enum.Parse(typeof(UnityGamepadKeyCode), buttonString);

					//button check now
					if (Input.GetKey((KeyCode)(int)keyCode)) return 1f;
				}

				//gamepad axis check
				if (inputType == InputDeviceType.GamepadAxis)
				{
					string axisString = string.Format("J_{0}_{1}", (slotIndex + 1), gamepadAxisNumber);
					float axisValue = Input.GetAxisRaw(axisString);
					if (invertAxis) axisValue *= -1f;
					if (rescaleAxis)
					{
						//some gamepad axis are -1 to 1 or something when you want them as 0 to 1, EG; triggers on XBONE pad on OSX
						axisValue = Mathf.InverseLerp(rescaleAxisMin, rescaleAxisMax, axisValue);
					}

					if (clampAxis) axisValue = Mathf.Clamp01(axisValue);

					//we return every axis' default value unless we measure a change first
					//this prevents weird snapping and false button presses if the pad is reporting a weird value to start with
					if (useDefaultAxisValue)
					{
						if (measuredAxisValue != -54.321f)
						{
							if (axisValue != measuredAxisValue) useDefaultAxisValue = false;
						}
						else
						{
							measuredAxisValue = axisValue;
						}
						if (useDefaultAxisValue) axisValue = defaultAxisValue;
					}
					return axisValue;
				}
				return 0f;
			}

			//virtual device axis input checks
			if (inputType == InputDeviceType.Virtual)
			{
				if (slot == InputDeviceSlot.any || slot == InputDeviceSlot.virtual1)
				{
					return VirtualInputs.GetVirtualAxis(virtualInputID);
				}
				//return virtualAxisValue;
			}

			//mouseaxis button checks (these don't happen)
			if (inputType == InputDeviceType.Mouse)
			{
				if (slot != InputDeviceSlot.any && slot != InputDeviceSlot.mouse && slot != InputDeviceSlot.keyboardAndMouse) return 0f;

				switch (mouseInputType)
				{
					case MouseInputType.MouseHorizontal:
						return Input.GetAxisRaw("Mouse Horizontal") * Ginput.mouseSensitivity;
					case MouseInputType.MouseMoveLeft:
						return Mathf.Min(Input.GetAxisRaw("Mouse Horizontal") * Ginput.mouseSensitivity, 0f) * -1f;
					case MouseInputType.MouseMoveRight:
						return Mathf.Max(Input.GetAxisRaw("Mouse Horizontal") * Ginput.mouseSensitivity, 0f);
					case MouseInputType.MouseMoveUp:
						return Mathf.Max(Input.GetAxisRaw("Mouse Vertical") * Ginput.mouseSensitivity, 0f);
					case MouseInputType.MouseMoveDown:
						return Mathf.Min(Input.GetAxisRaw("Mouse Vertical") * Ginput.mouseSensitivity, 0f) * -1f;
					case MouseInputType.MouseVertical:
						return Input.GetAxisRaw("Mouse Vertical") * Ginput.mouseSensitivity;
					case MouseInputType.MouseScroll:
						return Input.GetAxisRaw("Mouse Scroll");
					case MouseInputType.MouseScrollUp:
						return Mathf.Max(Input.GetAxisRaw("Mouse Scroll"), 0f);
					case MouseInputType.MouseScrollDown:
						return Mathf.Min(Input.GetAxisRaw("Mouse Scroll"), 0f) * -1f;
					case MouseInputType.MousePositionX:
						return Input.mousePosition.x;
					case MouseInputType.MousePositionY:
						return Input.mousePosition.y;
					default:
						//it's a click type mouse input
						if (Input.GetKey((KeyCode)(System.Enum.Parse(typeof(KeyCode), mouseInputType.ToString())))) return 1f;
						break;
				}
			}
			return 0f;
		}
	}
}
