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
    /// Логика взаимодействия для Brick.xaml
    /// </summary>
    public partial class Brick : UserControl
    {
        public bool pol_li_eto = false;
        public bool existance;
        public Brick(int x, int y)
        {
            InitializeComponent();
            Width = 50;
            Height = 50;
            existance = true;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(x, y, 0, 0);
        }
        public void SelfDestruct()
        {
            existance = false;
        }
    }
}
