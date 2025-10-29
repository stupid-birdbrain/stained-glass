using SDL3;

namespace StainedGlass;

public readonly unsafe struct SdlDisplay(SDL.DisplayMode* displayMode) {
    private readonly SDL.DisplayMode* _displayMode = displayMode;

    public uint Id => _displayMode->DisplayID;
    public uint Width => (uint)_displayMode->W;
    public uint Height => (uint)_displayMode->H;
    public uint RefreshRate => (uint)_displayMode->RefreshRate;
}