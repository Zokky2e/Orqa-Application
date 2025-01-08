using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Orqa_Application.ViewModels;
using System;

namespace Orqa_Application.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(IServiceProvider services) : this()
    {
        DataContext = services.GetRequiredService<MainViewModel>();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}