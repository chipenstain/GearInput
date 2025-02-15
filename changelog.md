### 2022.1(unreleased) [07/06/22]
- First version of **GearInput**
- Fixing namings
- Fixing/Cleanups code style
- Change project's' structure
- Adding Rebind sistem Gen2 (without generation)
- Reorganise examples
- Converted to package

### 2018_H [06/09/18]
- Can now navigate right from slider selectables in rebind menu
- Rebind menu setting positions have been tweaked a little and should have better automatic navigation
- When rebind menu is rebuilt as pads are connected/disconnected, Selections are preserved. (Rebind menu can be 100% mouse free now!)
- Added example scenes showing Single player, Multiplayer, Touch Control, Canvas UI integration, and Custom UI 
- Fixed: ListenForSlotPress would ignore ResetInputs(KeyboardAndMouse) and return individual inputs from mouse or keyboard before reset wait was complete.

### 2018_G [06/08/18]
- Rebind screen now automatically scrolls to highlighted menu item when not using the mouse
- Made optimisations on code/functions that can run every frame
- Sinput.controls now ~definitely~ returns a copy of the controls list and not a reference to it. This can be modified without changing sinput's active control scheme
- Improved rebind menu settings editor
- String values that get be saved are now sanitised in the editor with SinputFileIO.SanitiseStringForSaving()
- Public Sinput properties/functions are now summarised so their descriptions should appear in code hints
- Fixed: when rebinding, newly added inputs won't overwrite/delete other controls when rebound, staying blank themselves
- Fixed: When rebinding, resetting a single control's inputs will properly remove UI elements of newly added inputs
- Sinput settings/bindings are saved to a file instead of player prefs on PC (player prefs are still used otherwise)

### 2018_F [06/04/18]
- Rebind menu is no longer completely rebuilt when; rebinding an input, deleting an input, adding an input, and resetting a control's inputs
- Starts listening for new input to bind immediately after adding a new input in the rebind screen
- Made rebind menu's 'highlighted' colours contrast more with the normal/unslelected colours
- Sinput ScriptableObjects now appear in their own 'Sinput' dropdown in the Asset 'Create' menu
- optimised cached control checks (should at least 3-6 times faster - that's a mental calculation, I'm too scared to actually look at the profiler yet D:)
- virtual inputs no longer have button states, they are just held or not and sinput controls determine press/release states instead. virtual input functions have been updated as needed.
- rebind menu now has settings which allow you to set up mouse sensitivity, control toggles, and smart control inversion and sensitivity
- readme updated, with a more explicit licence for Sinput added

### 2018_E [05/30/18]
- Sinput editor foldouts toggle when either label or icon is clicked instead of just icon
- Controls' toggle settings are now saved/loaded with custom binding
- Smart controls' scale and inversion settings are now saved/loaded with custom binding
- mouse sensitivity setting is now saved/loaded with custom binding
- Increased default wait time before GetButtonDownRepeating() starts repeating by an extra quarter second
- Added Standalone Sinput Module, to replace Standalone Input Modules and make Sinput button presses effect unity UI events
- Reverted to an older version of the Rebinder initialisation function to fix a bug (rebinding happened on active control list)
- Rebind menu no longer lists reported gamepads with "" (empty string) names. This fixes a bug with disconnected pads but will mean if a pad genuinely has no ID it can't be bound anymore

### 2018_D [05/28/18]
- Version names now have an underscore in them to make them easier to read. Big change, I know.
- Removed GetControls() - instead use Sinput.controls to get a copy of all the rebindable controls instead. (the old function gave a reference I mean thats just no good)
- SetControls() is gone too, it wasn't used (I think LoadControlScheme is gonna be the way to do it?)
- Improved initialisation process... somewhat
- can now set default inversion setting for smart controls in the editor
- Removed GetGamepads() - use Sinput.gamepads instead.
- Improved virtual inputs so setting axis value also sets button state, and setting button state sets axis value
- GetSlotPress() can now return virtual slots
- Added touchscreen button & joystick prefabs
- Some minor optimisations
- Now every function that takes a control or smart control as a parameter will log an error if that control/smart control isn't found
- Made mouse movement rebind check a lot less sensitive
- Newly created control scheme assets now have default controls & smart controls
- "Common Binding" is now "Common Mapping". A terminology change so gamepad mapping is distinct from binding/rebinding. (mapping = controller layout, binding = which input is linked to a control)
- Added common mapping for Rock Band Guitar. \m/ (-_-) \m/

### 2018C [05/25/18]
- A dialogue is displayed to confirm generating new Input Manager axis, and the changes update immediately in the editor
- positive/negative control settings for smart controls can now be selected from a pop-up instead of being typed out in the editor
- Changing a control's name in the editor will update any smart controls that reference it
- Virtual inputs now implemented, set with functions SetVirtualAxis(), SetDeltaPreference(), SetVirtualButton(), and/or SetVirtualButtonHeld() in SinputSystems.VirtualInputs
- Addev "virtual1" slot, I might add more virtual slots later once I've figured out how I want them to work
- ResetInputs() can now be used by slot, if you want to have only one device's inputs reset
- fixed issue where a couple of functions may have used the last frame's input data
- CurrentValueIsFramerateIndependent() is now PrefersDeltaUse(). Old function was too unweildly and harder to understand.
- UpdateGamepads() is now CheckGamepads(), so as not to confuse with other update functions
- Tidied up core Sinput class & update loop (moved axis-as-button-state stuff into control updates)
- Added SetInverted() to set whether a smart control (doesn't work with regular controls) is inverted, and GetInverted() too, because you probably wanna know if it's inverted.
- added mouseSensitivity setting
- Added SetScale() and GetScale() so you can set the 'sensitivity' of a smart control by slot (basically this just sets a multiplier)

### 2018B [05/23/18]
- Added Sinput editor menu
- Moved input settings generation to the editor menu
- Rebind UI scales with screen size
- GetVector() results now never have a magnitude greater than 1 by default
- Added ResetInputs() to force sinput to ignore new inputs for X amount of time
- moved my development notes to this file
- changed how control checks work, now they are all calculated no more than once per frame, the state of each control for each slot being cached
- added GetButtonDownRepeating() function for times when you want a button hold to trigger multiple pressed (good for scrolling menu selection)
- added readme file
- Changed default control scheme mouse inputs to be "look" inputs that work with gamepad or mouse
- Added GetAxisRaw()
- Added CurrentValueIsFrameRateIndependent(), tells you if you should avoid multiplying a controls GetAxis values by deltaTime for this frame.
- values from framerate independent inputs (eg mouse motion) are now no longer smoothed for smart control axis checks
- Control scheme editor foldouts are now animated, because it's pretty
- Controls & smart controls can now be re-ordered in the editor
- Control scheme editor UI appearance improvements
- Added SetToggle() & GetToggle() for making controls with toggle behaviour
- mouse movement and mouse scroll can now be set when rebinding
- Added partial name checks for common bindings, for if an exact name match can't be found
- Common bindings now had an 'isDefault' setting, if no name match can be found at all, this default binding will be used

### 2018A [05/17/18]
- First release