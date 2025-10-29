using SDL3;
using System.Diagnostics;

namespace StainedGlass;

/// <summary>
///     Contains info related to the SDL library.
/// </summary>
public sealed unsafe class SdlCtx : IDisposable {
    public readonly int Version;
    private readonly string _platform;
    
    public ReadOnlySpan<char> Platform => _platform.AsSpan();

    public SdlCtx(SDL.InitFlags flags) {
        var init = SDL.Init(flags);
        if(!init) {
            throw new Exception($"sdl init failed. {SDL.GetError()}");
        }

        Version = SDL.GetVersion();
        _platform = SDL.GetPlatform();
        SDL.SetLogOutputFunction((_, category, priority, message) => {
#if DEBUG
            if (priority >= SDL.LogPriority.Error) {
                throw new Exception($"{priority} [{category}]: {message}");
            }
#endif
            
            Console.WriteLine($"{priority} [{category}]: {message}");
        }, 0);
    }
    
    public bool PollEvent(out SDL.Event message) {
        var poll = SDL.PollEvent(out var evt);
        message = evt;
        return poll;
    }

    public string[] GetVulkanExtensions() {
        var extensionNames = SDL.VulkanGetInstanceExtensions(out _);
        Debug.Assert(extensionNames != null);
        
        for (int i = 0; i < extensionNames.Length; i++) {
            extensionNames[i] = extensionNames[i];
        }

        return extensionNames.ToArray();
    }

    public void Dispose() {
        SDL.Quit();
    }
}