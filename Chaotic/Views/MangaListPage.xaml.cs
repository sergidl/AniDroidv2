namespace Chaotic.Views;

public partial class MangaListPage : ContentPage
{
	public MangaListPage(MangaListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
