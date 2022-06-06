using System.Windows;
using System.Windows.Input;

namespace HospitalPatientRecords.MVVM.View;

public partial class MessageWindow : Window
{
    public MessageWindow()
    {
        InitializeComponent();
    }
    
    private void Logo_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }
    
    private void Close_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.Close();
    }

    private void Ok_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}