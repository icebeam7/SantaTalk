﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SantaTalk.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navPage = new NavigationPage(new Views.MainPage())
            {
                BarBackgroundColor = Color.FromHex("#301536"),
                BarTextColor = Color.Wheat
            };

            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
