using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orqa_Application.Controls
{
    public partial class WorkTableControl : UserControl
    {
        public WorkTableControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}