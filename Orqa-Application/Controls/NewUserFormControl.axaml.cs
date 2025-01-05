using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orqa_Application.Controls
{
    public partial class NewUserFormControl : UserControl
    {
        public NewUserFormControl()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}