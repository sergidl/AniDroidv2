namespace Chaotic.Views;

public partial class CharacterPage : ContentPage
{
	public CharacterPage(CharacterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
