using System.Numerics;

namespace StainedGlass;

public struct MouseState {
    public Vector2 Position;
    public Vector2 RelativeDelta;
    public Vector2 Scroll;
    private int _buttons;

    public bool IsButtonDown(Mouse.Button button) {
        return (_buttons & (1 << ((int)button - 1))) != 0;
    }

    public void SetButtonState(Mouse.Button button, bool isDown) {
        int buttonBit = 1 << ((int)button - 1);
        if (isDown) {
            _buttons |= buttonBit;
        }
        else {
            _buttons &= ~buttonBit;
        }
    }

    internal void Clear() {
        Scroll = Vector2.Zero;
        RelativeDelta = Vector2.Zero;
    }
    
    internal void ClearAllButtons() {
        _buttons = 0;
    }
    
    internal void CopyFrom(MouseState source) {
        Position = source.Position;
        Scroll = source.Scroll;
        _buttons = source._buttons;
    }
}