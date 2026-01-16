# Unity Layered UI Panel Service

A Unity package that provides a layered UI panel management system with animation support. This service automatically handles canvas layer organization, panel instantiation, and smooth animations for UI panels.

## Features

- 🎨 Automatic canvas layer management
- 🎬 Built-in animation support (Instant and Slide)
- 🔄 Panel lifecycle management
- 📦 Service-based architecture for easy integration
- ⚡ Simple API for opening and closing panels

## Dependencies

This package requires the following dependencies:

### 1. Unity Service System
Add the following package via Unity Package Manager:

```
https://github.com/lourenco-pedro/UnityServiceSystem.git
```

**Installation:**
1. Open Unity Package Manager (Window > Package Manager)
2. Click the "+" button
3. Select "Add package from git URL..."
4. Paste: `https://github.com/lourenco-pedro/UnityServiceSystem.git`

### 2. DOTween
Download and import DOTween from the official website:

**Download:** https://dotween.demigiant.com/download.php

**Installation:**
1. Download the `.unitypackage` file from the link above
2. In Unity, go to Assets > Import Package > Custom Package
3. Select the downloaded DOTween package
4. Import all files

## Installation

Add this package to your Unity project via Package Manager:

```
https://github.com/lourenco-pedro/UnityLayeredUIPanelService.git
```

Or clone/download this repository into your project's `Packages` folder.

## Getting Started

### 1. Create Your Panel

Create a new UI panel by inheriting from `BaseLayeredPanel`:

```csharp
using ppl.ServiceManagement.LayeredUIService;
using UnityEngine;

public class MyPanel : BaseLayeredPanel
{
    // Your panel-specific logic here
    
    public override void Setup(int panelId)
    {
        base.Setup(panelId);
        // Initialize your panel here
    }
}
```

**Important:** Your panel GameObject must have the following components:
- `CanvasGroup`
- `Slide` (animation component)
- `Instant` (animation component)

These components are automatically required by the `BaseLayeredPanel` class.

### 2. Create a Panel Prefab

1. Create a new UI GameObject in your scene
2. Add your panel script component to it
3. Configure the UI elements as needed
4. Save it as a prefab
5. The `Slide` and `Instant` components will be automatically added if missing

### 3. Register the Service

Register the `LayeredUIService` with your service manager:

```csharp
using ppl.ServiceManagement;
using ppl.ServiceManagement.LayeredUIService.Implementation;

public class GameInitializer : MonoBehaviour
{
    private async void Start()
    {
        await ServiceContainer.AddService<ILayeredUIService, LayeredUIService>();
    }
}
```

### 4. Use the Service

#### Opening a Panel

```csharp
using ppl.ServiceManagement;
using ppl.ServiceManagement.LayeredUIService;

public class UIController : MonoBehaviour
{
    [SerializeField] private MyPanel _myPanelPrefab;
    
    private int _currentPanelId;
    
    public void OpenPanel()
    {
        ServiceContainer.UseService<ILayeredUIService>((service)=>{

            // Open with instant animation
            _currentPanelId = service.UseCanvas(_myPanelPrefab);

            // Or open with slide animation
            _currentPanelId = service.UseCanvas(_myPanelPrefab, BaseLayeredPanel.EntranceType.Slide);
        });
    }
}
```

#### Opening a Panel on a Higher Layer

For modals or overlays that should appear above all other panels:

```csharp
public void OpenModalPanel()
{
    ServiceContainer.UseService<ILayeredUIService>((service)=>{
        _currentPanelId = service.UseHigherCanvas(_myPanelPrefab);
    });
}
```

#### Closing a Panel

```csharp
public void ClosePanel()
{
    ServiceContainer.UseService<ILayeredUIService>((service)=>{
        service.Close(_currentPanelId);
    });
}
```

#### Getting a Panel Reference

```csharp
public void GetPanel()
{
    ServiceContainer.UseService<ILayeredUIService>((service)=>{
        var panel = service.GetPanel(_currentPanelId);
         if (panel != null)
        {
            // Do something with the panel
            var myPanel = panel as MyPanel;
        }
    });
}
```

## API Reference

### ILayeredUIService

The main service interface for managing UI panels.

#### Methods

- `int UseCanvas(ILayeredPanel ipanel, BaseLayeredPanel.EntranceType entranceType = BaseLayeredPanel.EntranceType.Instant)`
  - Opens a panel on the highest active canvas layer
  - Returns: Panel ID for future reference
  - Parameters:
    - `ipanel`: The panel to instantiate
    - `entranceType`: Animation type (Instant or Slide)

- `int UseHigherCanvas(ILayeredPanel ipanel)`
  - Opens a panel on a new, higher canvas layer
  - Returns: Panel ID for future reference
  - Use this for modal dialogs or overlays

- `BaseLayeredPanel GetPanel(int panelId)`
  - Retrieves a panel by its ID
  - Returns: The panel instance, or null if not found

- `void Close(int panelId)`
  - Closes and destroys the specified panel
  - Automatically plays the reverse animation

### BaseLayeredPanel

Base class for all UI panels in the system.

#### Properties

- `Transform Transform`: The panel's transform
- `GameObject GameObject`: The panel's GameObject
- `BaseLayeredPanel Panel`: Reference to itself
- `int PanelId`: Unique identifier assigned by the service

#### Methods

- `virtual void Setup(int panelId)`: Called when the panel is created
- `void Close(Action onClose = null)`: Closes the panel with animation

#### Animation Types

```csharp
public enum EntranceType
{
    Instant,  // Fade in/out
    Slide     // Slide animation with fade
}
```

## Animation System

The package includes two built-in animations:

### Instant Animation
- Simple fade in/out effect
- Fast and lightweight
- Default animation type

### Slide Animation
- Slides from off-screen with fade effect
- Configurable direction and duration
- Uses DOTween for smooth motion

You can extend the animation system by creating new classes that inherit from `BaseAnimation`.

## Best Practices

1. **Always store panel IDs** when opening panels so you can close them later
2. **Use `UseHigherCanvas`** for modal dialogs that should block interaction with panels beneath
3. **Override `Setup`** method in your panels to initialize them with data
4. **Clean up event listeners** in your panels when they're closed

## Troubleshooting

### Panel doesn't animate
- Ensure DOTween is properly imported
- Check that `Slide` and `Instant` components are attached to your panel prefab
- Verify that `CanvasGroup` component is present

### Panel doesn't appear
- Make sure the service is registered before use
- Check that your panel inherits from `BaseLayeredPanel`
- Verify the prefab is assigned in the inspector

### Missing service error
- Ensure Unity Service System is installed
- Register `LayeredUIService` on game initialization
- Check that you're using the correct service interface (`ILayeredUIService`)

## License

EasyUI is a free software; you can redistribute it and/or modify it under the terms of the MIT license. See LICENSE for details.