namespace Chaotic.Views;

public partial class AnimePage : ContentPage
{
	public AnimePage(AnimeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
