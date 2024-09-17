using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();
        }
        private TextBox _focusedTextBox;
        private bool _isShiftPressed = false;

        private void ShiftButton_Click(object sender, RoutedEventArgs e)
        {
            _isShiftPressed = !_isShiftPressed;

            // Opciono: ažurirajte izgled tastera na tastaturi da odražava trenutno stanje `Shift` tastera
            // Na primer, možete promeniti boju tastera ili tekst na njemu
            if (_isShiftPressed)
            {
                Shift.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                Shift.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _focusedTextBox = textBox;
                KeyboardGrid.Visibility = Visibility.Visible;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {


            


        }
        private void TableView_PreviewMouseDown(object sender,MouseButtonEventArgs e)
        {
            if(!isKeyboardOrTextBoxClicked(e.OriginalSource as DependencyObject))
            {
                KeyboardGrid.Visibility = Visibility.Collapsed;
            }
        }
        private bool isKeyboardOrTextBoxClicked(DependencyObject source)
        {
            if(source == KeyboardGrid || (source as FrameworkElement)?.Parent == KeyboardGrid)
            {
                return true;
            }
            if(source == SearchBox || source == IdBox || source == NazivBox)
            {
                return true;
            }
            if(VisualTreeHelper.GetParent(source) != null)
            {
                return isKeyboardOrTextBoxClicked(VisualTreeHelper.GetParent(source));
            }
            return false;
        }
    
        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {

        if (_focusedTextBox == null) return;

        if (sender is Button button)
        {
            string key = button.Content.ToString();
            int caretIndex = _focusedTextBox.CaretIndex;

            if (key == "Space")
            {
                _focusedTextBox.Text = _focusedTextBox.Text.Insert(caretIndex, " ");
                _focusedTextBox.CaretIndex = caretIndex + 1;
            }
            else if (key.Contains("Delete"))
            {
                if (_focusedTextBox.Text.Length > 0 && caretIndex > 0)
                {
                    _focusedTextBox.Text = _focusedTextBox.Text.Remove(caretIndex - 1, 1);
                    _focusedTextBox.CaretIndex = caretIndex - 1;
                }
            }else if(key == "Enter")
                {
                    KeyboardGrid.Visibility = Visibility.Collapsed;
                    _focusedTextBox = null;
                    return;
                }
            else
            {
                key = _isShiftPressed ? key.ToUpper() : key.ToLower();
                _focusedTextBox.Text = _focusedTextBox.Text.Insert(caretIndex, key);
                _focusedTextBox.CaretIndex = caretIndex + key.Length;
            }

            _focusedTextBox.Focus();
        }
    }
    
    }
}
