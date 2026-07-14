## Intro
This component hosts methods that control high-level functionality of the main menu and things that interact with other Jam Framework systems

## Main Menu Template
The MainMenu scene template provides basic, undecorated main menu essentials. From it, you can play the game, quit, view settings and credits. This exists for you to stylize and extend as desired, while removing some of the repetitive boilerplate.

The template itself is controlled using buttons hooked up to a [Slideshow](https://github.com/xmarshall39/JamFramework/wiki/Slideshow) component to swap between pages and the [SceneManager](https://github.com/xmarshall39/JamFramework/wiki/Scene-Manager) to mock a transition into gameplay.