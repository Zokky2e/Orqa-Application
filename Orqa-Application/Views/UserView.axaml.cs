using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Orqa_Application.ViewModels;
using System;

namespace Orqa_Application.Views
{
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();
        }
        public UserView(IServiceProvider services): this()
        {
            DataContext = services.GetRequiredService<UserViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}