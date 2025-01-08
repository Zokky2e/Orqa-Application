using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Orqa_Application.Services;
using Orqa_Application.ViewModels;
using ReactiveUI;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Orqa_Application.Views
{
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
        }
        public AdminView(IServiceProvider services) : this()
        {
            DataContext = services.GetRequiredService<AdminViewModel>();
            //this. += (s, e) =>
            //{
            //    (DataContext as AdminViewModel)?.GetWorkPositions();
            //};
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}