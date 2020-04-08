using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Ecco.Mobile.ViewModels.Auth
{
    public class LoginViewModelBase : ViewModelBase
    {
        #region Fields

        private string email;
        private bool isInvalidEmail;
        private bool loading;

        #endregion

        #region Property

        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email == value)
                {
                    return;
                }

                this.email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password { get; set; }

        public bool IsInvalidEmail
        {
            get
            {
                return this.isInvalidEmail;
            }

            set
            {
                if (this.isInvalidEmail == value)
                {
                    return;
                }

                this.isInvalidEmail = value;
                OnPropertyChanged(nameof(IsInvalidEmail));
            }
        }

        public bool Loading
        {
            get
            {
                return loading;
            }
            set
            {
                loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        #endregion
    }
}
