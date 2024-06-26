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
using System.Windows.Threading;

namespace Mario2
{
    /// <summary>
    /// Логика взаимодействия для Goomba.xaml
    /// </summary>
    public partial class Goomba : UserControl
    {
        public bool existance;
        public double target1;
        public double target2;
        public double current_target;
        public double current_position_x;
        public double current_position_y;
        DispatcherTimer timer =new DispatcherTimer();
        public int appearance_time;
        public Goomba(int x1,int y1,int x2)
        {
            
            InitializeComponent();
            Width = 50;
            Height = 50;
            existance = true;
            Visibility = Visibility.Visible;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(x1, y1, 0, 0);
            target1 = x2;
            target2 = x1;
            current_target = x2;
            current_position_x = x1;
            current_position_y = y1;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timertick);
            timer.Start();
        }
        public void SelfDestruct()
        {
            existance = false;
        }
        public void Go()
        {
            if (current_target < current_position_x)
            {
                if (current_position_x - current_target > 20)
                { 
                    Margin = new Thickness(current_position_x - 20, current_position_y, 0, 0);
                    change_appearance_left();
                }
                else
                {
                    if (current_target == target1)
                        current_target = target2;
                    else
                        current_target = target1;
                }
            }
            else
            {
                if (current_target-current_position_x > 20)
                {
                    Margin = new Thickness(current_position_x + 20, current_position_y, 0, 0);
                    change_appearance_right();
                }
                else
                {
                    if (current_target == target1)
                        current_target = target2;
                    else
                        current_target = target1;
                }
            }
            current_position_x = (int)Margin.Left;
        }
        private void Timertick(object sender, EventArgs e)
        {
            appearance_time++;
        }
        private void change_appearance_left()
        {
            if(appearance_time%2==0)
                texture.Source=new BitmapImage(new Uri(@"GSR2.png", UriKind.Relative));
            else
                texture.Source = new BitmapImage(new Uri(@"GSR1.png", UriKind.Relative));
        }
        private void change_appearance_right()
        {
            if (appearance_time % 2 == 0)
                texture.Source = new BitmapImage(new Uri(@"GSL2.png", UriKind.Relative));
            else
                texture.Source = new BitmapImage(new Uri(@"GSL1.png", UriKind.Relative));
        }
    }
}
