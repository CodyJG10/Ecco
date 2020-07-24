using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.ViewModels.Home;
using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardListView : ContentPage
    {
        private ConnectionModel _swipedCard;

        private CardListViewModel ViewModel { get { return BindingContext as CardListViewModel; } }

        public CardListView()
        {
            InitializeComponent();
            InitFilterComboBox();

            ConnectionsList.DataSource.SortDescriptors.Add(new SortDescriptor()
            {
                PropertyName = "Name",
                Direction = ListSortDirection.Ascending,
            });
            ConnectionsList.RefreshView();

            ConnectionsList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "Name",
                KeySelector = (object card) =>
                {
                    var item = (card as ConnectionModel);
                    return item.Card.Card.FullName.ToArray()[0].ToString();
                },
                Comparer = new GroupByFirstCharComparer()
            });

        }

        private void InitFilterComboBox()
        {
            List<string> filters = new List<string>();
            filters.Add("Cardholder");
            filters.Add("Service Category ");
            FilterComboBox.ComboBoxSource = filters;
        }

        public void Refresh()
        { 
            (BindingContext as CardListViewModel).RefreshCommand.Execute(null);
        }

        private void ButtonDeleteCard_Clicked(object sender, EventArgs e)
        {
            var popupLayout = new SfPopupLayout();
            popupLayout.PopupView.AnimationMode = AnimationMode.Zoom;
            popupLayout.PopupView.ShowHeader = false;

            var templateView = new DataTemplate(() =>
            {
                var popupContent = new Label
                {
                    Text = "Are you sure you want to delete this card?",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18,
                };

                return popupContent;
            });

            var footerTemplateView = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                };

                var yesButton = new SfButton()
                {
                    Text = "Yes",
                    WidthRequest = 65d,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                var noButton = new SfButton()
                {
                    Text = "No",
                    WidthRequest = 65d,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };

                noButton.Clicked += (s, a) =>
                {
                    popupLayout.IsOpen = false;
                };
                yesButton.Clicked += (s, a) =>
                {
                    (BindingContext as CardListViewModel).DeleteConnectionCommand.Execute(_swipedCard);
                    popupLayout.IsOpen = false;
                    (BindingContext as CardListViewModel).RefreshCommand.Execute(null);
                };

                stackLayout.Children.Add(yesButton);
                stackLayout.Children.Add(noButton);

                return stackLayout;
            });

            popupLayout.PopupView.ContentTemplate = templateView;
            popupLayout.PopupView.FooterTemplate = footerTemplateView;

            popupLayout.Show();
        }

        private void ConnectionsList_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            _swipedCard = e.ItemData as ConnectionModel;
        }

        #region Filtering 

        private void CardholderNameSearchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ConnectionsList.DataSource != null)
            {
                switch(FilterComboBox.SelectedIndex)
                {
                    case 0:
                        ConnectionsList.DataSource.Filter = FilterByCardholder;
                        break;
                    case 1:
                        ConnectionsList.DataSource.Filter = FilterByService;
                        break;
                }

                ConnectionsList.DataSource.RefreshFilter();
            }
        }

        private bool FilterByCardholder(object obj)
        {
            if (FilterInput.Text == null)
                return true;

            var connection = obj as ConnectionModel;
            if (connection.Card.Card.FullName.ToLower().Contains(FilterInput.Text.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FilterByService(object obj)
        {
            if (FilterInput.Text == null)
                return true;

            string GetTitle(int id)
            {
                foreach (var field in typeof(ServiceTypes).GetFields())
                {
                    if ((int)field.GetValue(null) == id)
                    {
                        var attribute = field.GetCustomAttribute<ServiceInfo>();
                        return attribute.Title;
                    }
                }
                return null;
            }

            var connection = obj as ConnectionModel;
            string query = FilterInput.Text.ToLower();
            string cardServiceTypeTitle = GetTitle(connection.Card.Card.ServiceType).ToLower();
            if (cardServiceTypeTitle.Contains(query))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (FilterComboBox.SelectedIndex == 0)
            {
                //Cardholder
                FilterInput.Placeholder = "Cardholder Name";
            }
            if (FilterComboBox.SelectedIndex == 1)
            {
                //Service Category
                FilterInput.Placeholder = "Service Category";
            }
        }

        #endregion

        private void ConnectionsList_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            var groupcount = ConnectionsList.DataSource.Groups.Count;
            for (int i = 0; i < groupcount; i++)
            {
                Label label = new Label();
                GroupResult group = ConnectionsList.DataSource.Groups[i];
                label.Text = group.Key.ToString();
                IndexPanelGrid.Children.Add(label, 0, i);
                var labelTapped = new TapGestureRecognizer()
                {
                    Command = new Command<object>(OnTapped),
                    CommandParameter = label
                };
                label.GestureRecognizers.Add(labelTapped);
            }
        }

        private Label previousLabel;
        private void OnTapped(object obj)
        {
            if (previousLabel != null)
            {
                previousLabel.TextColor = Color.DimGray;
            }
            var label = obj as Label;
            var text = label.Text;
            label.TextColor = Color.Red;
            for (int i = 0; i < ConnectionsList.DataSource.Groups.Count; i++)
            {
                var group = ConnectionsList.DataSource.Groups[i];
                
                if ((group.Key != null && group.Key.Equals(text)))
                {
                    ConnectionsList.LayoutManager.ScrollToRowIndex(ConnectionsList.DataSource.DisplayItems.IndexOf(group), true);
                }
            }
            previousLabel = label;
        }
    }
}