# RCV (Required Components Validator)

## Description
### Introduction
RCV is a Unity package that helps developers create games using the Entity-Component architecture by eliminating the risk of hidden dependencies.

### Entity-Component architecture
Unity's GameObject hierarchy heavily implies Entity-Component to be the architecture of choice for Unity projects.  
In the E-C architecture, game objects are **entities** that get their functionality from attached **components**.  
For example, the Player entity may have a RigidBody2D component and a Moveable script component.  

### Entity-Component architecture in Unity
Many components can function independently from each other, e.g. a component that makes the camera focus on the entity.  
However, some components will be controlled from the entity. For example, the Player entity may want to temporarily disable the Moveable component to disable movement.  
This could be done by getting the component and disabling it:  
```cs 
gameObject.GetComponent<MoveableComponent>().enabled = false; 
```

### The issue with dynamical fetching of components
The issue arises from the fact we just created a hidden dependency in our code. If there is no script of type `MoveableComponent` attached to the Player entity, the entity will encounter a `NullReferenceException` when it attempts to run the above line of code.  
We could add a null check and throw an error if it's null, but besides the issue of having to manually do that each time, we still wouldn't know about the missing component until it's actually used. That could be at any time during runtime, maybe even in production.   
Unity Engine offers no way to systematically check that the dependencies are present before compile time, or even runtime.

### Validation of components with RCV
The RCV package adds a new class attribute: `RequiredCustomComponent`.  
Add the attribute for each required component to your entity class. Ensure that the `using ZDelas.Rcv.Runtime;` namespace reference is included at the top of the file.  
At any time, use the new RCV button in your Unity Editor menu toolbar to validate that all required components are present.  
If any are missing, you'll see an error pop-up in the console.  
Because RCV validates all the required components are present, `gameObject.GetComponent` can be safely used for any component listed using `RequiredCustomComponent`.

## Installation
To install, download the ZIP file from the [latest release](https://github.com/zvonimir-delas/rcv/releases/), open the Unity Package Manager and select installation from local file: [Unity guide](https://docs.unity3d.com/Manual/upm-ui-local.html).  
Alternatively, download the source code as a ZIP and follow the same instructions.

## Example usage
### The Player class with several required components
```cs
using ZDelas.Rcv.Runtime;

[RequiredCustomComponent(typeof(InputHandlerComponent))]
[RequiredCustomComponent(typeof(CollisionComponent))]
[RequiredCustomComponent(typeof(MoveableComponent))]
public class Player : MonoBehaviour
{
    void ExampleMethod()
    {
        // Can be safely called without the risk of the dependency missing
        gameObject.GetComponent<MoveableComponent>().enabled = false;
        // Can safely retrieve properties from the components
        var objectCollidedWithPlayer = gameObject.GetComponent<InputHandlerComponent>().currentlyCollidedObject;
    }
    // rest of the class implementation
}
```
### The menu button

![Image](https://i.ibb.co/DQwW9nX/2023-04-26-23-11-28-ANew-Legacy-Unity-Main-Menu-Windows-Mac-Linux-Unity-2022-1-14f1-DX11.png)

### The console output
The console output includes the number of verified components in the brackets.   

![Image](https://i.ibb.co/hKRynsH/2023-04-26-23-17-17-ANew-Legacy-Unity-Main-Menu-Windows-Mac-Linux-Unity-2022-1-14f1-DX11.png)

### Run validation on each change
To run the validation automatically on each change, add a method to your `Editor` folder with the attribute `EditorInit` and call `RcvActions.ValidateRequiredComponentsForAllClasses`.  
This will run the validation each time Unity recompiles your scripts.

## Inspiration
This package was inspired by Unity's `RequiredComponent` attribute which unfortunately lacks the ability to validate components, but opts to dynamically insert them instead - which is an issue if your components have configurable serialized fields that would silently stay unallocated.

## Wishlist
- [ ] Add a GUI option to automatically run on each recompile
- [ ] Add a GUI option to collapse Debug.Log messages into a single message
