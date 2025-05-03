namespace CheeseMods.AIHelicopterGunner;

public static class GunnerAIConfig
{
    public const float headSetFov = 110f;
    public const float halfHeadSetFov = headSetFov / 2f;
    public const float headsetResolution = 1200f;

    public const float headsetPixelSize = headSetFov / headsetResolution;
    public const float minTargetSizePixels = 3f;
    public const float minimumTargetSizeAngular = headsetPixelSize * minTargetSizePixels;
}
