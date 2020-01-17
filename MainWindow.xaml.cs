using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VMAsyncDemo VM = new VMAsyncDemo();
            DataContext = VM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

     
        private bool closeCompleted = false;

        private void FormFadeOut_Completed(object sender, EventArgs e)
        {
            closeCompleted = true;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!closeCompleted)
            {
                FormFadeOut.Begin();
                e.Cancel = true;
            }
        }

        private void BtnRun_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            var button = (Button)sender;
            if (button.IsEnabled)
            {
                Cursor = Cursors.Arrow;
            }
            else
            {
                Cursor = Cursors.Wait;
            }
        }
    }
}
