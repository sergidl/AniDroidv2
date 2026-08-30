namespace Chaotic.Views;

public partial class MangaPage : ContentPage
{
	public MangaPage(MangaViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
