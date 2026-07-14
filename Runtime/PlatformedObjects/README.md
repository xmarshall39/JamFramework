# Platformed Objects
These refer to a set of components used to modify the behavior of a gameobject depending on the Platformed

## PlatformedGameObject
- This component controls the activeState of child gameobjects depending on the platform
- A "quit game" button on the main menu that only appears on PC, but is disabled on mobile and web

## Platformed Image
- A component that works in tandem with the Image component, swapping out the target sprite depending on the target platform
- A UI sprite for player input that shows a button/mouse on PC and a tap icon on mobile

## Platformed Transform
- A component that modifies the transform properties on a gameobject depending on the platform.
- Example: A UI element that should appear slightly offset on mobile resolutions