using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace XFAzureHubs.Views
{
    public partial class SecondPage : ContentPage
    {
        public string Type { get; set; }
        public string ID { get; set; }

        public SecondPage(string type, string id)
        {
            ID = id;
            Type = type;

            InitializeComponent();

            BindingContext = this;

            
        }
    }
}
