namespace BDOMultiTool;

internal sealed record PortraitSettings(string FaceTextureFolder)
{
	public static PortraitSettings Default => new PortraitSettings(PortraitReplacerService.DefaultFaceTextureFolder);
}

