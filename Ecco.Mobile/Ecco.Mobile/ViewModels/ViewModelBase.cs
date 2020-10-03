using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecco.Mobile.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public virtual void SubscribeAutoUpdates() { }
        public virtual void UnsubscribeAutoUpdates() { }
    }
}