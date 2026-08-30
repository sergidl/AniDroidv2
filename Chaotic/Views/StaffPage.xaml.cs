namespace Chaotic.Views;

public partial class StaffPage : ContentPage
{
	public StaffPage(StaffViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
