namespace SKTClientHires2022;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Khmer UI Bold.ttf", "OpenSansRegular");
			});

		return builder.Build();
	}
}
