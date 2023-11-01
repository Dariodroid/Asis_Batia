using Asis_Batia.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asis_Batia.ViewModel
{
    public class RulesPageViewModel : BaseViewModel
    {
        public ICommand NextPageCommand { get; set; }
        public RulesPageViewModel()
        {
            NextPageCommand = new Command(async () => await NextPage());
        }
        private async Task NextPage()
        {
            try
            {
                await Shell.Current.GoToAsync("/FormPrin", true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cerrar");
                return;
            }
        }
    }
}
