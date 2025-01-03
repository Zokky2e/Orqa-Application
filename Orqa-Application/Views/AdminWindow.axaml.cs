using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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