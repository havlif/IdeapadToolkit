using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace IdeapadToolkit.WinUI3.Services;
internal class NavigationService : INavigationService
{
    public Frame? NavFrame { get; set; }

    public bool CanGoBack
    {
        get
        {
            return this.NavFrame?.CanGoBack ?? false;
        }
    }
    public void GoBack()
    {
        NavFrame?.GoBack();
    }

    public void Navigate<TElement>(object? args, NavigationTransitionInfo? navigationTransitionInfo = null) where TElement : UIElement
    {
        if (NavFrame is null) return;
        NavFrame.Navigate(typeof(TElement), args, navigationTransitionInfo);
        NavFrame.BackStack.Clear();
    }

    public void Navigate(Type page, object? args, NavigationTransitionInfo? navigationTransitionInfo = null)
    {
        if (NavFrame is null) return;
        NavFrame.Navigate(page, args, navigationTransitionInfo);
        NavFrame.BackStack.Clear();
    }

}
