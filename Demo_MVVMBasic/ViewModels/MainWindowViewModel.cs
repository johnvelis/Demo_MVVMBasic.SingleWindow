using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

using Demo_MVVMBasic.Models;
using Demo_MVVMBasic.Data;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Demo_MVVMBasic.Views;
using System.Net.NetworkInformation;

namespace Demo_MVVMBasic
{
    class MainWindowViewModel : ObservableObject
    {
        public ICommand ButtonSellCommand { get; set; }
        public ICommand ButtonBuyCommand { get; set; }
        public ICommand ButtonAddCommand { get; set; }
        public ICommand ButtonEditCommand { get; set; }
        public ICommand ButtonDeleteCommand { get; set; }
        public ICommand ButtonQuitCommand { get; set; }

        private Widget _selectedWidget;
        private Widget _widgetToAdd;
        private Widget _widgetToEdit;
        private string _addWidgetFeedback;
        private string _edidWidgetFeedback;






        public ObservableCollection<Widget> Widgets { get; set; }

        public Widget SelectedWidget
        {
            get { return _selectedWidget; }
            set
            {
                _selectedWidget = value;
                OnPropertyChanged(nameof(SelectedWidget));
            }
        }

        public Widget WidgetToAdd
        {
            get { return _widgetToAdd; }
            set
            {
                _widgetToAdd = value;
                OnPropertyChanged(nameof(WidgetToAdd));
            }
        }

        public Widget WidgetToEdit
        {
            get { return _widgetToEdit; }
            set
            {
                _widgetToEdit = value;
                OnPropertyChanged(nameof(WidgetToEdit));
            }
        }

        public string AddWidgetFeedback
        {
            get { return _addWidgetFeedback; }
            set
            {
                _addWidgetFeedback = value;
                OnPropertyChanged(nameof(AddWidgetFeedback));
            }
        }

        public string EditWidgetFeedback
        {
            get { return _edidWidgetFeedback; }
            set
            {
                _edidWidgetFeedback = value;
                OnPropertyChanged(nameof(EditWidgetFeedback));
            }
        }


        public MainWindowViewModel()
        {
            Widgets = new ObservableCollection<Widget>(WidgetData.GetAllWidgets());

            if (Widgets.Any()) SelectedWidget = Widgets[0];

            ButtonSellCommand = new RelayCommand(new Action<object>(SellWidgets));
            ButtonBuyCommand = new RelayCommand(new Action<object>(BuyWidgets));
            ButtonAddCommand = new RelayCommand(new Action<object>(AddWidget));
            ButtonEditCommand = new RelayCommand(new Action<object>(EditWidget));
            ButtonDeleteCommand = new RelayCommand(new Action<object>(DeleteWidget));
            ButtonQuitCommand = new RelayCommand(new Action<object>(QuitWidget));

            WidgetToAdd = new Widget();
        }

        public void SellWidgets(object parameter)
        {
            int.TryParse((string)parameter, out int quantity);
            SelectedWidget.CurrentInventory -= quantity;
        }

        public void BuyWidgets(object parameter)
        {
            int.TryParse((string)parameter, out int quantity);
            SelectedWidget.CurrentInventory += quantity;
        }

        public void AddWidget(object parameter)
        {
            //
            // TODO - add code to validate user input
            //

            string commandParameter = parameter.ToString();

            if (commandParameter == "SAVE")
            {
                if (WidgetToAdd != null)
                {
                    Widgets.Add(WidgetToAdd);
                    AddWidgetFeedback = "New Widget Added";
                    SelectedWidget = WidgetToAdd;
                }
            }
            else if (commandParameter == "CANCEL")
            {
                AddWidgetFeedback = "New Widget Canceled";
            }
            else
            {
                throw new ArgumentException($"{commandParameter} is not a valid command parameter for the adding widgets.");
            }
            WidgetToAdd = new Widget();

        }

        public void EditWidget(object parameter)
        {
            //
            // TODO - add code to validate user input
            //

            string commandParameter = parameter.ToString();
            if (commandParameter == "LOADWIDGET")
            {
                //
                // Copy method uses makes a Shallow Copy
                //
                WidgetToEdit = SelectedWidget.Copy();
            }
            else if (commandParameter == "SAVE")
            {
                if (WidgetToAdd != null)
                {
                    Widgets.Remove(SelectedWidget);
                    Widgets.Add(WidgetToEdit);
                    AddWidgetFeedback = "Widget Updated";
                }
            }
            else if (commandParameter == "CANCEL")
            {
                AddWidgetFeedback = "Widget Update Canceled";
            }
            else
            {
                throw new ArgumentException($"{commandParameter} is not a valid command parameter for the adding widgets.");
            }
            WidgetToEdit = new Widget();
        }

        public void DeleteWidget(object parameter)
        {
            if (SelectedWidget != null)
            {
                string widgetName = SelectedWidget.Name;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the {widgetName} widgets from inventory?", "Delete Widgets", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Widgets.Remove(SelectedWidget);
                        MessageBox.Show($"{widgetName} Widgets Deleted", "Delete Widgets");

                        if (Widgets.Any()) SelectedWidget = Widgets[0];
                        break;

                    case MessageBoxResult.No:
                        MessageBox.Show($"{widgetName} Widgets Deletion Canceled", "Delete Widgets");
                        break;
                }
            }
        }

        public void QuitWidget(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
