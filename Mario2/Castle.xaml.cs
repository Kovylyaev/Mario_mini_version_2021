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

namespace Mario2
{
    /// <summary>
    /// Логика взаимодействия для Castle.xaml
    /// </summary>
    public partial class Castle : UserControl
    {
        public Castle(int x,int y)
        {
            InitializeComponent();
            Width = 500;
            Height = 400;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(x, y, 0, 0);
        }
    }
}
