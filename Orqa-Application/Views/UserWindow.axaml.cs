using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orqa_Application.Views
{
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}