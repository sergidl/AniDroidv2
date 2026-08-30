namespace Chaotic.Views;

public partial class AnimeListPage : ContentPage
{
	public AnimeListPage(AnimeListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
