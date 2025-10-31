using SDL3;

namespace StainedGlass;

/// <summary>
///     A basic wrapper around an SDL window. Up to the user to implement windowing functions not covered here.
/// </summary>
public unsafe struct SdlWindow : IDisposable {
    private readonly IntPtr _window;

    internal SdlWindow(IntPtr window) { _window = window; }
    
    public SdlWindow(ReadOnlySpan<char> title, int width, int height, SDL.WindowFlags flags) {
        _window = SDL.CreateWindow(title.ToString(), width, height, flags);
    }

    public readonly uint Id => SDL.GetWindowID(_window);
    
    public readonly bool IsDisposed => _window == IntPtr.Zero;
    public readonly SDL.WindowFlags Flags => SDL.GetWindowFlags(_window);

    public readonly SdlDisplay Display {
        get {
            var id = SDL.GetDisplayForWindow(_window);
            SDL.DisplayMode? mode = SDL.GetCurrentDisplayMode(id);

            return new SdlDisplay((SDL.DisplayMode)mode!);
        }
    }
    
    public ReadOnlySpan<char> Title {
        get => SDL.GetWindowTitle(_window);
        set => SDL.SetWindowTitle(_window, value.ToString());
    }
    
    public nint CreateVulkanSurface(nint vulkanInstance) {
        nint allocator = 0;
        bool success = SDL.VulkanCreateSurface(_window, vulkanInstance, allocator, out var surfacePointer);
        if (!success) {
            throw new Exception($"could not create vk surface: {SDL.GetError()}");
        }
        
        return new(surfacePointer);
    }

    public readonly void SetTransparency(float alpha) {
        var format = SDL.GetWindowPixelFormat(_window);
        SDL.GetWindowSize(_window, out var width, out var height);
        var shape = SDL.CreateSurface(width, height, format);
        SDL.ClearSurface(_window, 0, 0, 0, alpha);
        var success = SDL.SetWindowShape(_window, shape);
        if (!success) {
            throw new Exception($"could not set window shape: {SDL.GetError()}");
        }
        
        SDL.DestroySurface(shape);
    }

    public void Dispose() {
        SDL.DestroyWindow(_window);
    }
}