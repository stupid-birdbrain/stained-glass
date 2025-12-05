using SDL3;

namespace StainedGlass;

public struct KeyboardState {
    private const int key_buffer_size = 16;
    public const int MAX_KEYS = 256;
    private KeyBuffer _keys;
    
    public bool this[int index] {
        get => _keys.IsKeyDown((SDL.Scancode)index);
        set => _keys.SetKeyDown((SDL.Scancode)index, value);
    }

    public bool IsKeyDown(SDL.Scancode scancode) => _keys.IsKeyDown(scancode);
    public bool IsKeyUp(SDL.Scancode scancode) => !_keys.IsKeyDown(scancode);

    public void SetKeyState(SDL.Scancode scancode, bool isDown) {
        _keys.SetKeyDown(scancode, isDown);
    }
    
    internal void Clear() {
        _keys.Clear();
    }

    internal void CopyFrom(KeyboardState source) {
        _keys.CopyFrom(source._keys);
    }

    private unsafe struct KeyBuffer {
        private fixed ulong _buffer[key_buffer_size];

        public bool IsKeyDown(SDL.Scancode scancode) {
            int index = (int)scancode / 64;
            int bit = (int)scancode % 64;
            if (index < 0 || index >= key_buffer_size) return false;
            return (_buffer[index] & (1UL << bit)) != 0;
        }

        public void SetKeyDown(SDL.Scancode scancode, bool down) {
            int index = (int)scancode / 64;
            int bit = (int)scancode % 64;
            if (index is < 0 or >= key_buffer_size) return;

            if (down) {
                _buffer[index] |= (1UL << bit);
            }
            else {
                _buffer[index] &= ~(1UL << bit);
            }
        }

        public void Clear() {
            for (int i = 0; i < key_buffer_size; i++) {
                _buffer[i] = 0;
            }
        }

        public void CopyFrom(KeyBuffer source) {
            for (int i = 0; i < key_buffer_size; i++) {
                _buffer[i] = source._buffer[i];
            }
        }
    }
}