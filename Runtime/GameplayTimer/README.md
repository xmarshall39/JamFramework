# Gameplay Timer

## Intro
The Jam Framework gameplay timer is a simple baseline for setting up and displaying in-game timers. Like the ones you see in Mario or Sonic.

## GameplayTimer
A data-driven component that functions like a simple stopwatch. While running it records `ElapsedTime` in seconds every frame and it can report that time in various useful time formats. What follows are the public fields and functions it implements:
- `float ElapsedTime` - The current amount of time in seconds passed while the timer was updating
- `bool IsTimerRunning` - Is the timer updating
- `StartTimer()` - Begin/unpause the stopwatch by allowing ElapseTime to update
- `void PauseTimer()` - Prevents ElapsedTime from updating until StartTimer() is called
- `void RestartTimer()` - Starts the timer from and ElapsedTime of 0
- `int GetMinutes()` - Get elapsed time in minutes
- `int GetSeconds()` - Get elapsed time in seconds
- `int GetMilliseconds()` - Returns elapsed time in milliseconds
- `int GetRollingSeconds()` - Returns the number of elapsed seconds in the current minutes (between 0 and 60 exclusive)
- `int GetRollingMilliseconds()` - Returns the number of elapsed miliseconds in the current second (between 0 and 100 exclusive)
## GameplayTimerDisplay
This component uses a GameplayTimer to display `ElapsedTime` in various time formats. It can be configured with 3 options that determine how precice its displayed units are:
- ShowMinutes
- ShowSeconds
- ShowMilliseconds
So long as this component is active, the associated `text` will synchronize to the `timer`.
