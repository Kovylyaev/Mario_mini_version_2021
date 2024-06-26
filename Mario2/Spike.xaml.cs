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
    /// Логика взаимодействия для Spike.xaml
    /// </summary>
    public partial class Spike : UserControl
    {
        public bool existance;
        public Spike(int x, int y)
        {
            InitializeComponent();
            Width = 50;
            Height = 50;
            existance = true;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            texture.Source= new BitmapImage(new Uri(@"Spike.png", UriKind.Relative));
            Margin = new Thickness(x, y, 0, 0);
        }
        public void SelfDestruct()
        {
            existance = false;
        }
    }
}
