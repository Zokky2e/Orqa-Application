using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orqa_Application.Controls
{
    public partial class UserCardControl : UserControl
    {
        public UserCardControl()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}