using Asis_Batia.ViewModel;
using MauiPopup.Views;

namespace Asis_Batia.View;

public partial class PopupRulesPage : BasePopupPage
{
	public PopupRulesPage()
	{
		InitializeComponent();
		BindingContext = new PopupRulesPageViewModel();
	}
}