namespace Chaotic.Views;

public partial class UserInfoPage : ContentPage
{
	public UserInfoPage(UserInfoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
