using Asis_Batia.ViewModel;

namespace Asis_Batia.View;

public partial class RulesPage : ContentPage
{
	public RulesPage()
	{
		InitializeComponent();
		BindingContext = new RulesPageViewModel();
    }
}