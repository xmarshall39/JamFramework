# Launchpad
The Jam Framework Launchpad is a scene asset and singleton management paradigm. Using a Launchpad scene means having a scene at build index 0 that must load before any other scenes can be initialized. The Launchpad contains and initializes any Singletons required for your game which keeps them around for the game's entire lifetime, without worrying about surrounding game context for valid references.

## How it Works
The Launchpad has the following qualities:
- Occupies build index 0
- Contains a root GameObject with a Launchpad component
- Stores all Singleton components on child gameobjects (automatically granted DoNotDestroyOnLoad)
- Loads the scene at build index 1 immidiately after initialization
- Optionally can be injected before playing any scene in the editor (QOL feature for testing)

Note: While the user should expect that all GameObjects in the Launchpad scene will be active at all times, disabling their gameobjects in the editor can avoid duplicate Awake() calls. For most Singletons, this is not an issue. However, it was for the Debug Menu and so we keep its whole hierarchy disabled initially and let the Launchpad handle when it Awake()s.