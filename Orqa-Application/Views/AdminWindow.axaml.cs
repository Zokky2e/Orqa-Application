using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Orqa_Application.Services;
using Orqa_Application.ViewModels;
using ReactiveUI;

namespace Orqa_Application.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}