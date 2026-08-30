namespace Chaotic.Views;

public partial class StudiosPage : ContentPage
{
	public StudiosPage(StudiosViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
