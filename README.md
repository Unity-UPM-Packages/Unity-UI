# 🎨 Unity UI Framework (com.thelegends.unity.ui)

**Unity UIs** is a powerful UI framework, specialized in managing UI screens and transitions in Unity3D. Designed specifically for Mobile games, this framework completely eliminates the heavy `Animator` system, utilizing `LitMotion` to provide smooth effects with **Zero-GC Allocation**.

---

## 🌟 Key Features
- **Composition-based Architecture:** Combine small motion components (Fade, Scale, Slide...) to create complex transitions without writing code.
- **Superior Performance:** Runs on `LitMotion`, saving battery, CPU, and avoiding lag spikes caused by Garbage Collection.
- **Flexibility:** Supports running motions in Parallel or Sequential mode.
- **Global UIs Included:** Provides ready-to-use common UIs like Loading Screen and Toast Notifications that can be called from anywhere.

---

## 📦 Installation

Since the package depends on `LitMotion`, make sure your project already has it installed.
Configure `manifest.json` in your project's `Packages` folder:

```json
{
  "dependencies": {
    "com.thelegends.unity.ui": "https://github.com/Unity-UPM-Packages/Unity-UI.git",
    "com.annulusgames.lit-motion": "2.0.1",
    "com.annulusgames.lit-motion.animation": "2.0.1"
  }
}
```

---

## 🏗 Core Architecture

The framework operates based on 2 main components:
1. **`UIScreenController`**: The brain of a screen. It collects and orchestrates all motions on that screen.
2. **`IUIMotion` (classes inheriting from `UIMotionBase`)**: Specific motion pieces. You can attach multiple Motions to the same GameObject.

Available Motions:
- `UIFadeMotion`: Fades in / fades out (CanvasGroup Alpha).
- `UIScaleMotion`: Scales up / scales down.
- `UISlideMotion`: Slides the screen from directions (Top, Bottom, Left, Right).
- `UITranslateMotion`: Translates by custom coordinates.
- `UIRotateMotion`: Rotates the screen.

---

## 🚀 Detailed Usage Guide

### 1. Create a basic UI Screen
1. Create a UI Panel/GameObject in your Canvas. Name it `MainMenuScreen`.
2. Attach the `UIScreenController` component to `MainMenuScreen`.
   *(By default, on awake, the controller will automatically call `HideImmediate()` to hide the screen).*

### 2. Add motions to the Screen
For example, if you want the `MainMenuScreen` to **slide up from the bottom while fading in**:
1. Attach the `UISlideMotion` component to `MainMenuScreen`.
   - Set `Direction`: `Bottom`
   - Set `Duration`: `0.5`, `Ease`: `OutBack`
2. Attach the `UIFadeMotion` component to `MainMenuScreen`.
   - Ensure `MainMenuScreen` has a `CanvasGroup` component.
   - Set `Duration`: `0.3`, `Ease`: `Linear`

### 3. Call Show / Hide from Code
In your UI control script (e.g., `GameManager` or `UIManager`), reference the `UIScreenController` and call the corresponding methods:

```csharp
using TheLegends.Base.UI;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    [SerializeField] private UIScreenController _mainMenuScreen;
    
    // Open the screen
    public void OpenMenu()
    {
        _mainMenuScreen.Show();
    }

    // Close the screen
    public void CloseMenu()
    {
        _mainMenuScreen.Hide();
    }
}
```

### 4. Listen to Lifecycle Events
`UIScreenController` provides Actions for you to catch events when the screen changes its state:

```csharp
private void Start()
{
    _mainMenuScreen.OnShowing += () => Debug.Log("Screen starting to show...");
    _mainMenuScreen.OnShown += () => Debug.Log("Screen fully shown!");
    
    _mainMenuScreen.OnHiding += () => Debug.Log("Screen starting to hide...");
    _mainMenuScreen.OnHidden += () => Debug.Log("Screen fully hidden!");
}
```

---

## ⚙️ Advanced Setup

### Transition Mode
In the Inspector of `UIScreenController`, you can select `Show Mode` and `Hide Mode`:
- **Parallel (Default):** All Motions attached to the GameObject will play at the same time.
- **Sequential:** Motions will play one after another sequentially. The execution order is determined by the `Order` field inside each Motion component (lower `Order` executes first).

### ⏱ Motion Preset (Sync timing & configuration)
If your project has many screens that need the same motion feel (e.g., all popups must open in `0.3s` with `OutBack` ease), instead of tweaking each one manually, you should use **UIMotionPreset**.

**How to create and use:**
1. Right-click in the Project window -> `Create > TheLegends > UI > Motion Preset`.
2. Name the file (e.g., `PopupMotionPreset`), configure `Duration`, `Ease`, `Delay`.
3. Select your Motion components (e.g., `UIScaleMotion`), drag the newly created Preset file into the `Preset (Optional)` slot.
4. Now, the component will **ignore** local Timing parameters and grab values directly from the Preset. Change the Preset once, and it applies to the entire UI!

---

## 🛠 Developers: How to create a new Custom Motion

The framework is highly extensible. If you need a specific effect (e.g., changing Text color, blurring a Material, page flipping...), you can easily create a new Motion by inheriting from `UIMotionBase`.

**Steps to implement:**
1. Create a new script inheriting from `TheLegends.Base.UI.UIMotionBase`.
2. Override the `CreateMotion()` method to create a `MotionHandle` using `LitMotion`.
3. Override `ResetToStart()` and `ResetToEnd()` to set up the default state without going through the Animation (crucial for `HideImmediate()` or `ShowImmediate()` methods).

**Example: Create a UI color change effect (UIColorMotion)**

```csharp
using System;
using LitMotion;
using TheLegends.Base.UI;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("TheLegends/UI/Motions/Color")]
public class UIColorMotion : UIMotionBase
{
    [SerializeField] private Graphic _target;
    [SerializeField] private Color _from = Color.white;
    [SerializeField] private Color _to = Color.green;

    // Return the LitMotion Handle so the Controller can await or cancel it
    protected override MotionHandle CreateMotion(bool reverse, Action onComplete)
    {
        Color startColor = reverse ? _to : _from;
        Color endColor = reverse ? _from : _to;

        // Note: Always pass Duration, MotionEase, and Delay from the base class
        return LMotion.Create(startColor, endColor, Duration)
            .WithEase(MotionEase)
            .WithDelay(Delay)
            .WithOnComplete(() => onComplete?.Invoke())
            .Bind(_target, (value, target) => target.color = value);
    }

    public override void ResetToStart() => _target.color = _from;
    public override void ResetToEnd() => _target.color = _to;
}
```

After saving the script, you can drop `UIColorMotion` into the same GameObject containing the `UIScreenController`, and it will automatically be collected and orchestrated along with other Motions without needing to modify the Core code!

---

## 🌐 Global UIs (Global Utilities)

The framework comes with 2 highly useful independent utilities for Mobile Games. You just need to drop the Prefab containing their Controllers into the initial Scene, then call them from anywhere.

### 1. Loading Screen (`UILoadingController`)
Displays a loading screen with a progress bar.

```csharp
using TheLegends.Base.UI;

// Method 1: Automatically simulate the progress bar over a fixed time
// (Great for fake loading to make the game feel premium)
UILoadingController.Show(time: 2.0f, onComplete: () => {
    Debug.Log("Simulated loading complete!");
});

// Method 2: Set the actual loading percentage (from 0 to 1) when loading Scenes/Bundles
UILoadingController.SetProgress(0.5f);

// Hide the loading screen
UILoadingController.Hide();
```

### 2. Toast Notification (`UIToatsController`)
Displays a small notification text that disappears automatically, excellent for alerts like "Not enough Gold", "Game Saved", etc.

```csharp
using TheLegends.Base.UI;

// Display a notification, auto-dismisses after 2 seconds, located in the middle of the screen
UIToatsController.Show("Not enough Gold!", duration: 2.0f, ToastPosition.MiddleCenter);
```

---

## 🛡 Best Practices
1. **Do not use `Update()` to check State:** Instead of `if (screen.State == UIScreenState.Visible)`, subscribe to `OnShown` and `OnHidden` events to execute logic (e.g., Play sound effect, enable Input).
2. **Always use `CanvasGroup` for `UIFadeMotion`:** Do not try to manually alter the Alpha of an Image. `CanvasGroup` is much more optimized for fading an entire UI hierarchy and has the ability to block interactions (`blocksRaycasts`).
3. **Hierarchy:** Try not to nest Canvases too deeply (Nested Canvas) unless absolutely necessary to optimize Draw Call overhead, while coupling with `LitMotion` for absolute performance.
