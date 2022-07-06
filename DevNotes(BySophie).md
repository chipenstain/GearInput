# Sophie's Dev Notes:
## misc to-do for future versions:
TODO: get undo/redo to work with inspector changes for control schemes and common mappings
TODO: look into Sinput.smartcontrols - do I want that working how I have or should it be more like sinput.controls?
TODO: have control "groups" - controls & smartControls can be part of a group, using Sinput.SetGroup("modeName", activeOrNot) decides which controls can be used (others will just return 0/false)
TODO: support for multiple control schemes and saved custom control schemes
TODO: virtual slots for virtual inputs
TODO: make it so touchscreen controls can be repositioned and rescaled
TODO: other possible touch control prefabs; slider, dpad, swipe listener that can act as left/right/up/down buttons, pinch-zoom axis, and any other gestures I can think of

## Rebinding to-do;
TODO: if keyboard&mouse control are set to distinct, seperate them in the rebind menu
TODO: be able to cancel listening for a rebind
TODO: Find a way to deal with (or at least highlight) inputs that clash in the rebinding menu
TODO: Get rebind menu elements to have explicit navigation controls because the automatic ones are not exactly perfect
TODO: be able to bind modifier keys (and gamepad buttons), eg shift+a, LB+dpadLeft ?
TODO: don't move in the menu on the frame a binding is set with a button that would move the menu

## Documentation to-do;
TODO: video documentation with subtitles
TODO: Proper code docs with examples
TODO: Make more example scenes showing:
	- Button Toggle
	- delta preference
	- Virtual Inputs

## future feature wishlist/to-do
 - Figure out sub-frame inputs (if a button is pressed AND released between frames) - right now they are just lost
 - GetDisplayName(controlName, defaultDisplayName, slot) for user-facing text prompts (if 'any' slot, use whichever slot last returned a true/non-zero value)
 - GetDisplayIcon() like GetDisplayName() but returns an image for the button/key/whatever
 - support for networked controllers (eg recieving input from phone/tablet apps sending control inputs)
 - text-file input, check a text file that can be edited by extrenal stuff and set its own input values. could be good for custom controllers or stream input?
 - Multiple mice/keyboards
 - VR input
 - Microphone input
 - force feedback
 - find ways of supporting more gamepads?
 - have multiple slots for a single keyboard so two players can share?
 - common shortcut to bring up rebind menu, loads the rebind scene additively and ignores other sinput calls until the menu is closed