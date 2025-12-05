using System.Numerics;

namespace StainedGlass;

public struct Mouse {
    public enum Button : byte {
        LeftButton = 1,
        MiddleButton = 2,
        RightButton = 3,
    }
    
    private MouseState _currentState;
    private MouseState _previousState;
    
    public Vector2 GetPosition() => _currentState.Position;
    public Vector2 GetScrollDelta() => _currentState.Scroll;
    public Vector2 GetRelativeDelta() => _currentState.RelativeDelta;
    
    public bool IsButtonDown(Button button) => _currentState.IsButtonDown(button);
    public bool IsButtonUp(Button button) => !_currentState.IsButtonDown(button);

    public bool WasButtonPressed(Button button) {
        bool wasDownPrev = _previousState.IsButtonDown(button);
        bool isDownCurrent = _currentState.IsButtonDown(button);
        return isDownCurrent && !wasDownPrev;
    }

    public bool WasButtonReleased(Button button) {
        bool wasDownPrev = _previousState.IsButtonDown(button);
        bool isDownCurrent = _currentState.IsButtonDown(button);
        return !isDownCurrent && wasDownPrev;
    }
    
    public void Update() {
        _previousState.CopyFrom(_currentState);
        _currentState.Clear();
    }

    public void SetPosition(Vector2 newPosition) {
        _currentState.Position = newPosition;
    }

    public  void SetButtonState(Button button, bool isDown) {
        _currentState.SetButtonState(button, isDown);
    }

    public  void AddScrollDelta(Vector2 delta) {
        _currentState.Scroll += delta;
    }

    public void AddRelativeDelta(Vector2 delta) {
        _currentState.RelativeDelta += delta;
    }
    
    internal void Clear() {
        _currentState = default;
        _previousState = default;
    }
}