namespace MonoGameClusterFuck.Helpers
{
    public enum UIElementPositioEnEnum
    {
        None                = 0b00000000,
        CenterHorizontal    = 0b00000001,
        CenterVertical      = 0b00000010,
        Center              = 0b00000011,
        TopLeftCorner       = 0b00000100,
        TopRightCorner      = 0b00001000,
        TopCenter           = 0b00001100,
        BottomLeftCorner    = 0b00010000,
        BottomRightCorner   = 0b00100000,
        BottomCenter        = 0b00110000,
    }
}