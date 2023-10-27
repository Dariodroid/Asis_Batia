using Asis_Batia.View;
using MauiPopup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asis_Batia.ViewModel
{
    public class PopupRulesPageViewModel
    {
        public ICommand ClosePopupPageCommand { get; set; }
        public ICommand AcceptPopupPageCommand { get; set; }

        public PopupRulesPageViewModel()
        {
            ClosePopupPageCommand = new Command(async () => await Close());
            AcceptPopupPageCommand = new Command(async () => await Accept());

        }

        public async Task Close()
        {
            PopupAction.ClosePopup(true);
            Application.Current.Quit();
        }
        public async Task Accept()
        {
            PopupAction.ClosePopup(false);
        }
    }
}
