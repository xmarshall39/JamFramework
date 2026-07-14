# Slideshow
## Intro
The slideshow component is a simple tool that enforces mutually exclusive activeState's across a collection of gameobjects. In other words, among the gameobjects in a Slideshow component's  `pages` list, only one may be active at a time. This is good for any menu with multiple pages. Later, we may want to include a method for custom in/out transitions. For now, it'll be nice and simple.

## Public Members
- `void SetPage(int pageIndex)` - Sets the currently active page using its index in the `pages` array
- `voiid SetPage(string pageName)` - Sets the currently active page using the name of a give page gameobject