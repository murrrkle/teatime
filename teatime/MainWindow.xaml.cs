using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace teatime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer orbitTimer;
        private DispatcherTimer idleTimer;
        private Vector planets;
        private Int32 holdTime;

        private const int IDLE = 0;
        private const int POURING = 1;
        private const int DRINKING = 2;
        private const int HAPPY = 3;
        private int boxy_status = IDLE;

        private bool started = false;

        private bool teaPouring = false;

        public MainWindow()
        {
            InitializeComponent();
            HideElements(); 

            orbitTimer = new DispatcherTimer(); // syncs planet orbits
            idleTimer = new DispatcherTimer(); // plays an idle animation every x milliseconds
            planets = new Vector();
          
            holdTime = 0;

            mainWindow.MouseMove += MainWindow_MouseMove;
            boxy.MouseDown += Boxy_MouseDown;
            boxy.MouseUp += Boxy_MouseUp;

            idleTimer.Interval = TimeSpan.FromMilliseconds(1000);
            BindGuideMouseEvents();

        }

        private void Boxy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(0));
            t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            cursor.RenderTransform = t;

            //holdTime = holdTime - (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            //TODO: drinking animation
            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-drink.png", UriKind.Relative));
            boxy_status = DRINKING;

            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-happy.png", UriKind.Relative));
            boxy_status = HAPPY;

            if (!started)
            {
                MakeAllVisible();
                
            }

            // reset all planets
        }

        

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            
            Point mousePos = e.GetPosition(this);
            TranslateTransform transform = new TranslateTransform
            {
                X = mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4,
                Y = mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4
            };
            this.cursor.RenderTransform = transform;
                
        }

        private void Boxy_MouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);

            // holdTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            //clickTimer.Start();

            PourTea(mousePos);

            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-pour.png", UriKind.Relative));
            boxy_status = POURING;
            
        }

        private void PourTea(Point mousePos)
        {
            teaPouring = true;
            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(-45));
            t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            cursor.RenderTransform = t;
            // play sound of pouring
            //pouring animation that loops
        }

        private void UpdatePlanets()
        {

        }

        private class Planet : Image
        {

        }
        private void MakeAllVisible()
        {
            space.Visibility = Visibility.Visible;
            orbits.Visibility = Visibility.Visible;

            btn_s1.Visibility = Visibility.Visible;
            btn_s2.Visibility = Visibility.Visible;
            btn_s3.Visibility = Visibility.Visible;
            btn_s4.Visibility = Visibility.Visible;

            btn_m1.Visibility = Visibility.Visible;
            btn_m2.Visibility = Visibility.Visible;
            btn_m3.Visibility = Visibility.Visible;
            btn_m4.Visibility = Visibility.Visible;

            btn_l1.Visibility = Visibility.Visible;
            btn_l2.Visibility = Visibility.Visible;
            btn_l3.Visibility = Visibility.Visible;
            btn_l4.Visibility = Visibility.Visible;
            started = true;
        }
        private void HideElements()
        {
            space.Visibility = Visibility.Hidden;
            orbits.Visibility = Visibility.Hidden;

            btn_s1.Visibility = Visibility.Hidden;
            btn_s2.Visibility = Visibility.Hidden;
            btn_s3.Visibility = Visibility.Hidden;
            btn_s4.Visibility = Visibility.Hidden;

            btn_m1.Visibility = Visibility.Hidden;
            btn_m2.Visibility = Visibility.Hidden;
            btn_m3.Visibility = Visibility.Hidden;
            btn_m4.Visibility = Visibility.Hidden;

            btn_l1.Visibility = Visibility.Hidden;
            btn_l2.Visibility = Visibility.Hidden;
            btn_l3.Visibility = Visibility.Hidden;
            btn_l4.Visibility = Visibility.Hidden;
        }

        private void BindGuideMouseEvents()
        {
            btn_s1.MouseEnter += Btn_s1_MouseEnter;
            btn_s2.MouseEnter += Btn_s2_MouseEnter;
            btn_s3.MouseEnter += Btn_s3_MouseEnter;
            btn_s4.MouseEnter += Btn_s4_MouseEnter;

            btn_m1.MouseEnter += Btn_m1_MouseEnter;
            btn_m2.MouseEnter += Btn_m2_MouseEnter;
            btn_m3.MouseEnter += Btn_m3_MouseEnter;
            btn_m4.MouseEnter += Btn_m4_MouseEnter;

            btn_l1.MouseEnter += Btn_l1_MouseEnter;
            btn_l2.MouseEnter += Btn_l2_MouseEnter;
            btn_l3.MouseEnter += Btn_l3_MouseEnter;
            btn_l4.MouseEnter += Btn_l4_MouseEnter;

            btn_s1.MouseLeave += Btn_s1_MouseLeave;
            btn_s2.MouseLeave += Btn_s2_MouseLeave;
            btn_s3.MouseLeave += Btn_s3_MouseLeave;
            btn_s4.MouseLeave += Btn_s4_MouseLeave;

            btn_m1.MouseLeave += Btn_m1_MouseLeave;
            btn_m2.MouseLeave += Btn_m2_MouseLeave;
            btn_m3.MouseLeave += Btn_m3_MouseLeave;
            btn_m4.MouseLeave += Btn_m4_MouseLeave;

            btn_l1.MouseLeave += Btn_l1_MouseLeave;
            btn_l2.MouseLeave += Btn_l2_MouseLeave;
            btn_l3.MouseLeave += Btn_l3_MouseLeave;
            btn_l4.MouseLeave += Btn_l4_MouseLeave;

        }

        private void Btn_l4_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_l4.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_l3_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_l3.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_l2_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_l2.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_l1_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_l1.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_m4_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_m4.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_m3_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_m3.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_m2_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_m2.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_m1_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_m1.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_s4_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_s4.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_s3_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_s3.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_s2_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_s2.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_s1_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_s1.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void Btn_l3_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_l3.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_l4_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_l4.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_l2_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_l2.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_l1_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_l1.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_m4_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_m4.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_s4_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_s4.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_s3_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_s3.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_m2_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_m2.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_m3_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_m3.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_m1_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_m1.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_s2_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_s2.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }

        private void Btn_s1_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_s1.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }
    }
}
