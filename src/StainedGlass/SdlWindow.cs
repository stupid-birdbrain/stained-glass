using SDL3;
using System.Runtime.CompilerServices;

namespace StainedGlass;

/// <summary>
///     A basic wrapped around an SDL window. Up to the user to implement windowing functions not covered here.
/// </summary>
public unsafe struct SdlWindow : IDisposable{
    private readonly IntPtr _window;
    private readonly nint _surface;

    public readonly IntPtr Surface {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _surface;
    }

    public SdlWindow(int width, int height, SDL.WindowFlags flags) {
        _window = SDL.CreateWindow("asd", width, height, flags);
    }

    public readonly uint Id => SDL.GetWindowID(_window);
    
    public readonly bool IsDisposed => _window == IntPtr.Zero;
    public readonly SDL.WindowFlags Flags => SDL.GetWindowFlags(_window);
    
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

    public void Dispose() {
        SDL.DestroyWindow(_window);
    }
}