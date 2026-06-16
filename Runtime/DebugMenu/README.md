# Debug Menu
The Jam Framework debug menu is built using the [UImGui Extended](https://github.com/yCatDev/uimgui-extended) plugin. You can read more about imgui [here](https://github.com/ocornut/imgui). The Debug Menu is an extensible window of different tabs used for... anything you can dream of.
This [interactive web ImGui demo](https://pthom.github.io/imgui_explorer/) has everything you need to understand how to create ImGui interfaces and use all of its features

## Setup
The debug menu is automatically provided and availalbe in the Launchpad template. If you're not using the template, do the following:
- Create a new gameobject with a Debug Menu component
- On that same object, Add a UImGui component. 
- Configure the UImGui component using the DefaultStyle, DefaultShader, and DefaultCursorShape objects located in `Packages > UImGui Extended > Resources`

## Activation
The debug menu has it's visibility toggled by the tilde \` key and by default is present in all builds of the game.

## Creating New tabs
Each debug menu tab has it's properties and behaivor defined in its own script, and to create new ones you'll need to do the same.
- Create a new class that inherits from `DebugMenuCategory`
- Implement the mandatory overrides
- `CategoryName()` defines the name used to represent this tab. Using an exisitng name will extend a tab without editing the original script
- `Draw(bool p_open)` this is where you put your ImGui calls
- Once your script is ready, add it as a component to your debug menu. Preferably on a child gameobject.
- Then, add that component to the Debug Menu component's `Debug Menu Categories` list