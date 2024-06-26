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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double uskorenie_vniz = 0;
        int chetchik_dla_smeni_shagov = 0;
        bool A = false, D = false, stoit_li_na_bloke = false;
        int kontrol_dvoinogo_prishka = 0, malenkie_shagi_vlevo = 0, malenkie_shagi_vpravo = 0, n;   //n - номер ближайшего блока
        List<Brick> blocks = new List<Brick>();
        List<Goomba> goombas = new List<Goomba>();
        List<Spike> spikes = new List<Spike>();
        List<Castle> castle = new List<Castle>();
        Random rnd = new Random();
        bool game_going = true;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 17; i++)
            {
                Brick tmp = new Brick(i * 50, 364);
                tmp.pol_li_eto = true;
                blocks.Add(tmp);
            }
            castle.Add(new Castle(5050,0));
            grid.Children.Add(castle[0]);
            Fon.Source = new BitmapImage(new Uri(@"Fon_dla_Mario.jpg", UriKind.Relative));
            generate_level();
            for (int i = 0; i < goombas.Count; i++)
            {
                grid.Children.Add(goombas[i]);
            }
            for (int i = 0; i < spikes.Count; i++)
            {
                grid.Children.Add(spikes[i]);
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                grid.Children.Add(blocks[i]);
            }


            //a.Source = new BitmapImage(new Uri(@"block.png",UriKind.Relative));

            Beg_Mario.Source = new BitmapImage(new Uri(@"Mario_stoit.png", UriKind.Relative));
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Tick += new EventHandler(timerTick);
            timer.Start();

            DispatcherTimer existance_check = new DispatcherTimer();
            existance_check.Interval = new TimeSpan(0, 0, 1);
            existance_check.Tick += new EventHandler(check_if_exists);
            existance_check.Start();
        }


        public void check_if_exists(object sender, EventArgs e)
        {
            if (game_going == true)
            { 
                for (int i = 0; i < blocks.Count; i++)
                {
                    if (blocks[i].existance == false)
                    {
                        blocks[i].Visibility = Visibility.Collapsed;
                        grid.Children.Remove(blocks[i]);
                        blocks.Remove(blocks[i]);
                    }
                }
                for (int i = 0; i < goombas.Count; i++)
                {
                    if (goombas[i].existance == false)
                    {
                        goombas[i].Visibility = Visibility.Collapsed;
                        grid.Children.Remove(goombas[i]);
                        goombas.Remove(goombas[i]);
                    }
                    else
                    {
                        goombas[i].Go();
                    }
                }
            }
        }


        private void timerTick(object sender, EventArgs e)
        {
            int z;
            int y;
            if(Beg_Mario.Margin.Top>= 270)
            {
                game_going = false;
                MessageBox.Show("Adventure", "???",MessageBoxButton.OK,MessageBoxImage.Question);
                this.Close();
            }
            if(Beg_Mario.Margin.Left>=castle[0].Margin.Left)
            {               
                MessageBoxResult answer = MessageBox.Show("Вы победили!", "Победа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                if (answer == MessageBoxResult.OK)
                {
                    game_going = false;
                    this.Close();
                }
            }
            if (game_going == true)
            {
                chetchik_dla_smeni_shagov += 5;

                if (stoit_li_na_bloke)
                {
                    uskorenie_vniz = 0;
                    kontrol_dvoinogo_prishka = 0;
                }
                else
                {
                    uskorenie_vniz += 0.4;
                }
                Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left, Beg_Mario.Margin.Top + uskorenie_vniz, 0, 0);


                if (Beg_Mario.Margin.Top >= 400 - 136)
                {
                    Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left, 400 - 136, 0, 0);
                    kontrol_dvoinogo_prishka = 0;
                    stoit_li_na_bloke = true;
                }

                if (A)
                {
                    int x = find_closest_brick();
                    y = find_closest_goomba();
                    z = find_closest_spike();
                    //blocks[x].texture.Source= new BitmapImage(new Uri(@"target_brick.jpg", UriKind.Relative));
                    if (chetchik_dla_smeni_shagov % 50 == 0)
                    {
                        Beg_Mario.Source = new BitmapImage(new Uri(@"Shag vlevo_1.png", UriKind.Relative));
                        if (chetchik_dla_smeni_shagov % 100 == 0)
                        {
                            Beg_Mario.Source = new BitmapImage(new Uri(@"Shag vlevo_2.png", UriKind.Relative));
                        }
                    }
                    if (!(Beg_Mario.Margin.Left - 5 <= blocks[x].Margin.Left + 50 && Beg_Mario.Margin.Left >= blocks[x].Margin.Left + 25 && Beg_Mario.Margin.Top + 102 >= blocks[x].Margin.Top && Beg_Mario.Margin.Top <= blocks[x].Margin.Top + 50))
                    {
                        Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left - 2.5, Beg_Mario.Margin.Top, 0, 0);
                    }
                    if (spikes.Count > 0 && (Beg_Mario.Margin.Left - 5 <= spikes[z].Margin.Left + 50 && Beg_Mario.Margin.Left >= spikes[z].Margin.Left + 25 && Beg_Mario.Margin.Top + 102 >= spikes[z].Margin.Top && Beg_Mario.Margin.Top <= spikes[z].Margin.Top + 50))
                    {
                        death_sreen();
                    }
                    if (goombas.Count > 0 && (Beg_Mario.Margin.Left - 5 <= goombas[y].Margin.Left + 50 && Beg_Mario.Margin.Left >= goombas[y].Margin.Left + 25 && Beg_Mario.Margin.Top + 102 >= goombas[y].Margin.Top && Beg_Mario.Margin.Top <= goombas[y].Margin.Top + 50))
                    {
                        death_sreen();
                    }
                    if (Beg_Mario.Margin.Left < 0)
                    {
                        Beg_Mario.Margin = new Thickness(0, Beg_Mario.Margin.Top, 0, 0);
                    }
                }

                if (D)
                {
                    int x = find_closest_brick();
                    z = find_closest_spike();
                    y = find_closest_goomba();
                    //blocks[x].texture.Source = new BitmapImage(new Uri(@"target_brick.jpg", UriKind.Relative));
                    if (chetchik_dla_smeni_shagov % 50 == 0)
                    {
                        Beg_Mario.Source = new BitmapImage(new Uri(@"Shag vpravo_1.png", UriKind.Relative));
                        if (chetchik_dla_smeni_shagov % 100 == 0)
                        {
                            Beg_Mario.Source = new BitmapImage(new Uri(@"Shag vpravo_2.png", UriKind.Relative));
                        }
                    }
                    if (spikes.Count > 0 && Beg_Mario.Margin.Left + 92.5 + 2.5 >= spikes[z].Margin.Left && Beg_Mario.Margin.Left + 92.5 + 2.5 <= spikes[z].Margin.Left + 25 && Beg_Mario.Margin.Top + 98 >= spikes[z].Margin.Top && Beg_Mario.Margin.Top <= spikes[z].Margin.Top + 50)
                    {
                        death_sreen();
                    }
                    if (goombas.Count > 0 && Beg_Mario.Margin.Left + 92.5 + 2.5 >= goombas[y].Margin.Left && Beg_Mario.Margin.Left + 92.5 + 2.5 <= goombas[y].Margin.Left + 25 && Beg_Mario.Margin.Top + 98 >= goombas[y].Margin.Top && Beg_Mario.Margin.Top <= goombas[y].Margin.Top + 50)
                    {
                        death_sreen();
                    }
                    if (!(Beg_Mario.Margin.Left + 92.5 + 2.5 >= blocks[x].Margin.Left && Beg_Mario.Margin.Left + 92.5 + 2.5 <= blocks[x].Margin.Left + 25 && Beg_Mario.Margin.Top + 98 >= blocks[x].Margin.Top && Beg_Mario.Margin.Top <= blocks[x].Margin.Top + 50))
                    {
                        Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left + 2.5, Beg_Mario.Margin.Top, 0, 0);
                    }
                    if (Beg_Mario.Margin.Left > 236)
                    {
                        Beg_Mario.Margin = new Thickness(236, Beg_Mario.Margin.Top, 0, 0);
                        castle[0].Margin = new Thickness(castle[0].Margin.Left - 2.5, castle[0].Margin.Top, 0, 0);
                        for (int i = 0; i < blocks.Count(); i++)
                        {
                            blocks[i].Margin = new Thickness(blocks[i].Margin.Left - 2.5, blocks[i].Margin.Top, 0, 0);
                            if (blocks[i].Margin.Left < -blocks[i].Width)
                            {
                                if (blocks[i].pol_li_eto)
                                {
                                    blocks[i].Margin = new Thickness(797, blocks[i].Margin.Top, 0, 0);
                                }
                                else
                                {
                                    blocks[i].SelfDestruct();
                                }
                            }
                        }
                        for (int i = 0; i < goombas.Count(); i++)
                        {
                            goombas[i].Margin = new Thickness(goombas[i].Margin.Left - 2.5, goombas[i].Margin.Top, 0, 0);
                            goombas[i].target1 -= 2.5;
                            goombas[i].target2 -= 2.5;
                            goombas[i].current_target -= 2.5;
                            goombas[i].current_position_x -= 2.5;
                            if (goombas[i].Margin.Left < -goombas[i].Width)
                            {
                                goombas[i].SelfDestruct();
                            }
                        }
                        for(int i = 0; i < spikes.Count(); i++)
                        {
                            spikes[i].Margin = new Thickness(spikes[i].Margin.Left - 2.5, spikes[i].Margin.Top, 0, 0);                           
                            if (spikes[i].Margin.Left < -spikes[i].Width)
                            {
                                spikes[i].SelfDestruct();
                            }
                        }
                    }
                }
                
                if (!A && !D)
                {
                    Beg_Mario.Source = new BitmapImage(new Uri(@"Mario_stoit.png", UriKind.Relative));
                    malenkie_shagi_vlevo = 0;
                    malenkie_shagi_vpravo = 0;
                }

                n = find_closest_brick();
                z = find_closest_spike();
                y = find_closest_goomba();
                //blocks[n].texture.Source = new BitmapImage(new Uri(@"target_brick.jpg", UriKind.Relative));
                if (Beg_Mario.Margin.Left + 55 >= blocks[n].Margin.Left && Beg_Mario.Margin.Left <= blocks[n].Margin.Left + 45 && Beg_Mario.Margin.Top <= blocks[n].Margin.Top + 50 && Beg_Mario.Margin.Top >= blocks[n].Margin.Top)
                {
                    uskorenie_vniz *= -1;
                    if (Beg_Mario.Margin.Top < blocks[n].Margin.Top + 50)
                    {
                        Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left, blocks[n].Margin.Top + 51, 0, 0);
                    }
                }
                if (spikes.Count>0&&Beg_Mario.Margin.Left + 55 >= spikes[z].Margin.Left && Beg_Mario.Margin.Left <= spikes[z].Margin.Left + 45 && Beg_Mario.Margin.Top <= spikes[z].Margin.Top + 50 && Beg_Mario.Margin.Top >= spikes[z].Margin.Top)
                {
                    death_sreen();
                }
                else
                {
                    if (spikes.Count > 0 && Beg_Mario.Margin.Left + 55 >= spikes[z].Margin.Left && Beg_Mario.Margin.Left <= spikes[z].Margin.Left + 50 && Beg_Mario.Margin.Top + 98 >= spikes[z].Margin.Top && Beg_Mario.Margin.Top + 98 <= spikes[z].Margin.Top + 50)
                    {
                        death_sreen();
                    }
                    if (goombas.Count > 0 && Beg_Mario.Margin.Left + 55 >= goombas[y].Margin.Left && Beg_Mario.Margin.Left <= goombas[y].Margin.Left + 50 && Beg_Mario.Margin.Top + 98 >= goombas[y].Margin.Top && Beg_Mario.Margin.Top + 98 <= goombas[y].Margin.Top + 50)
                    {
                        goombas[y].texture.Source = new BitmapImage(new Uri(@"Squahed.png",UriKind.Relative));
                        //goombas[y].SelfDestruct();
                        grid.Children.Remove(goombas[y]);
                        goombas.Remove(goombas[y]);
                        uskorenie_vniz = -6;
                    }
                    if (Beg_Mario.Margin.Left + 55 >= blocks[n].Margin.Left && Beg_Mario.Margin.Left <= blocks[n].Margin.Left + 50 && Beg_Mario.Margin.Top + 98 >= blocks[n].Margin.Top && Beg_Mario.Margin.Top + 98 <= blocks[n].Margin.Top + 50)
                    {
                        if (Beg_Mario.Margin.Top + 98 > blocks[n].Margin.Top)
                        {
                            Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left, blocks[n].Margin.Top - 98, 0, 0);
                        }
                        stoit_li_na_bloke = true;
                    }
                    else
                    {
                        stoit_li_na_bloke = false;
                    }
                }

                //if (Beg_Mario.Margin.Left + 58 >= blocks[n].Margin.Left && Beg_Mario.Margin.Left <= blocks[n].Margin.Left + 50 && Beg_Mario.Margin.Top + 98 >= blocks[n].Margin.Top)
                //{
                //    //Beg_Mario.Margin = new Thickness(Beg_Mario.Margin.Left, blocks[n].Margin.Top - 98, 0, 0);
                //    stoit_li_na_bloke = true;
                //    uskorenie_vniz = 0; 
                //}           
                //else
                //{
                //    stoit_li_na_bloke = false;
                //}
            }
        }


        public double calculate_distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }


        public int find_closest_brick()
        {
            int n = 0;
            double min = calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, blocks[0].Margin.Left + 25, blocks[0].Margin.Top + 25);
            for (int i = 1; i < blocks.Count; i++)
            {
                if (!blocks[i].pol_li_eto && calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, blocks[i].Margin.Left + 25, blocks[i].Margin.Top + 25) < min)
                {
                    min = calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, blocks[i].Margin.Left + 25, blocks[i].Margin.Top + 25);
                    n = i;
                }
            }
            return n;
        }

        public int find_closest_goomba()
        {
            if (goombas.Count > 0)
            {
                int n = 0;
                double min = calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, blocks[0].Margin.Left + 25, blocks[0].Margin.Top + 25);
                for (int i = 1; i < goombas.Count; i++)
                {
                    if (calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, goombas[i].Margin.Left + 25, goombas[i].Margin.Top + 25) < min)
                    {
                        min = calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, blocks[i].Margin.Left + 25, blocks[i].Margin.Top + 25);
                        n = i;
                    }
                }
                return n;
            }
            return 0;
        }
        public int find_closest_spike()
        {
            int n = 0;
            if (spikes.Count > 0)
            {
                double min = calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, spikes[0].Margin.Left + 25, spikes[0].Margin.Top + 25);
                for (int i = 1; i < spikes.Count; i++)
                {
                    if (calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, spikes[i].Margin.Left + 25, spikes[i].Margin.Top + 25) < min)
                    {
                        min = calculate_distance(Beg_Mario.Margin.Left + 50, Beg_Mario.Margin.Top + 50, spikes[i].Margin.Left + 25, spikes[i].Margin.Top + 25);
                        n = i;
                    }
                }
                return n;
            }
            return 0;
        }
        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                A = true;
                malenkie_shagi_vlevo++;
                if (malenkie_shagi_vlevo < 2)
                {
                    Beg_Mario.Source = new BitmapImage(new Uri(@"Shag vlevo_1.png", UriKind.Relative));
                }
            }
            if (e.Key == Key.D)
            {
                D = true;
                malenkie_shagi_vpravo++;
                if (malenkie_shagi_vpravo < 2)
                {
                    Beg_Mario.Source = new BitmapImage(new Uri(@"Shag vpravo_1.png", UriKind.Relative));
                }
            }
            if (e.Key == Key.Space)
            {
                kontrol_dvoinogo_prishka++;
                if (kontrol_dvoinogo_prishka <= 2)
                {
                    uskorenie_vniz = -10;
                    stoit_li_na_bloke = false;
                }
            }
        }
        private void Window_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                A = false;
            }
            if (e.Key == Key.D)
            {
                D = false;
            }
        }
        public void death_sreen()
        {
            game_going = false;
            Beg_Mario.Visibility = Visibility.Hidden;
            MessageBox.Show("Вы поиграли","Поражение",MessageBoxButton.OK,MessageBoxImage.Exclamation);            
        }
        public void generate_level()
        {
            blocks.Add(new Brick(100, 100));
            blocks.Add(new Brick(300, 200));

            goombas.Add(new Goomba(300, 314, 150));

            spikes.Add(new Spike(390, 0));
            spikes.Add(new Spike(390, 50));
            spikes.Add(new Spike(390, 100));
            spikes.Add(new Spike(390, 150));
            spikes.Add(new Spike(390, 200));


            spikes.Add(new Spike(630, 100));
            spikes.Add(new Spike(630, 150));
            spikes.Add(new Spike(630, 200));
            spikes.Add(new Spike(630, 250));
            spikes.Add(new Spike(630, 300));

            blocks.Add(new Brick(525, 164));

            for (int i = 900; i <= 1950; i += 50)
            {
                spikes.Add(new Spike(i, 314));
            }

            blocks.Add(new Brick(850, 314));
            blocks.Add(new Brick(850, 264));
            blocks.Add(new Brick(850, 214));

            blocks.Add(new Brick(2000, 314));
            blocks.Add(new Brick(2000, 264));
            blocks.Add(new Brick(2000, 214));
            blocks.Add(new Brick(2000, 164));

            blocks.Add(new Brick(900, 250));
            blocks.Add(new Brick(950, 250));
            blocks.Add(new Brick(1150, 175));
            blocks.Add(new Brick(1350, 240));
            blocks.Add(new Brick(1600, 150));
            blocks.Add(new Brick(1900, 250));

            /*for (int i = 2200; i <= 2900; i += 50)
            { 
                blocks.Add(new Brick(i, 125));
            }*/
            goombas.Add(new Goomba(2200,314,2400));
            blocks.Add(new Brick(2450,314));
            goombas.Add(new Goomba(2500,314, 2600));
            blocks.Add(new Brick(2650,314));
            goombas.Add(new Goomba(2700,314, 2900));

            /*spikes.Add(new Spike(2250, 314));
            spikes.Add(new Spike(2450, 314));
            spikes.Add(new Spike(2650, 314));
            spikes.Add(new Spike(2850, 314));
            */

            blocks.Add(new Brick(2950, 314));
            blocks.Add(new Brick(2950, 264));
            blocks.Add(new Brick(2950, 214));
            blocks.Add(new Brick(2950, 164));
            blocks.Add(new Brick(2950, 114));

            blocks.Add(new Brick(2850, 244));
            blocks.Add(new Brick(3050, 244));

            spikes.Add(new Spike(3000,314));
            spikes.Add(new Spike(3050, 314));
            spikes.Add(new Spike(3100, 314));
            spikes.Add(new Spike(3150, 314));
            spikes.Add(new Spike(3200, 314));
            spikes.Add(new Spike(3250, 314));
            spikes.Add(new Spike(3300, 314));
            spikes.Add(new Spike(3350, 314));
            spikes.Add(new Spike(3400, 314));
            spikes.Add(new Spike(3450, 314));

            blocks.Add(new Brick(3350, 264));
            blocks.Add(new Brick(3500, 314));
            blocks.Add(new Brick(3550, 314));
            blocks.Add(new Brick(3500, 264));
            blocks.Add(new Brick(3550, 264));

            for (int i = 3600; i <= 4650; i += 50)
            {
                spikes.Add(new Spike(i, 314));

            }
            blocks.Add(new Brick(4700, 314));
            blocks.Add(new Brick(4700, 264));
            blocks.Add(new Brick(4700, 214));
            blocks.Add(new Brick(4700, 164));

            blocks.Add(new Brick(3750, 204));
            blocks.Add(new Brick(3800, 204));
            blocks.Add(new Brick(3850, 204));
            spikes.Add(new Spike(3800, 154));

            blocks.Add(new Brick(4100, 124));
            blocks.Add(new Brick(4150, 124));
            blocks.Add(new Brick(4200, 124));
            blocks.Add(new Brick(4250, 124));
            goombas.Add(new Goomba(4150, 74, 4250));

            blocks.Add(new Brick(4450,204));
            
        }
    
    }
}
