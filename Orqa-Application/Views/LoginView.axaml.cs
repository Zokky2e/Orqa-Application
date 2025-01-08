using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Orqa_Application.ViewModels;
using System;

namespace Orqa_Application.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
         }
        public LoginView(IServiceProvider services): this()
        {
            DataContext = services.GetRequiredService<LoginViewModel>();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}