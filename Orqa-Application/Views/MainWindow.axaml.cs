using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Orqa_Application.Services;
using Orqa_Application.ViewModels;
using System;

namespace Orqa_Application.Views;

public partial class MainWindow : Window
{
    private readonly NavigationService _navigationService;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(IServiceProvider services) : this()
    {
        DataContext = services.GetRequiredService<MainViewModel>();
        _navigationService = services.GetRequiredService<NavigationService>();
        _navigationService.ViewChanged += OnViewChanged;
    }

    private void OnViewChanged(object view)
    {
        if (view is UserControl userControl)
        {
            Content = userControl;
        }
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}