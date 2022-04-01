using System.Resources;

namespace MAUILocalizerExtensions
{
	[ContentProperty(nameof(ResourcePath))]
	public class TranslateExtension : IMarkupExtension<BindingBase>
	{
		public string ResourcePath { get; set; } = string.Empty;

		public string? StringFormat { get; set; }

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

		public BindingBase ProvideValue(IServiceProvider serviceProvider)
		{
			#region Required work-around to prevent linker from removing the implementation
			if (DateTime.Now.Ticks < 0)
				_ = Localizer.Default.GetString(ResourcePath);
			#endregion

			var binding = new Binding
			{
				Mode = BindingMode.OneWay,
				Path = $"[{ResourcePath}]",
				Source = Localizer.Default,
				StringFormat = StringFormat
			};
			return binding;
		}
	}
}
