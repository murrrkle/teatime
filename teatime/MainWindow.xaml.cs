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

        private static Planet addThisPlanet;
        private static int addThisPlanetType;
        private static int addThisPlanetOrbit;
        private static int addThisPlanetDirection;
        private static int addThisPlanetLevel;
        private static int initialPositionLeft;
        private static int initialPositionTop;
        private static DispatcherTimer holdTimer;


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
            InitializePlanetPoints();

            holdTimer = new DispatcherTimer(); // determines when to make planet bigger while mouse button is held down
            orbitTimer = new DispatcherTimer(); // syncs planet orbits
            idleTimer = new DispatcherTimer(); // plays an idle animation every x milliseconds

            addThisPlanetType = Planet.SMALL;
            addThisPlanetOrbit = PlanetPoint.SHORT;
            addThisPlanetDirection = PlanetPoint.UP;
            addThisPlanet = new Planet(addThisPlanetType, addThisPlanetOrbit, addThisPlanetDirection);
            addThisPlanetLevel = 0;

            mainWindow.MouseMove += MainWindow_MouseMove;
            boxy.MouseDown += Boxy_MouseDown;
            boxy.MouseUp += Boxy_MouseUp;

            idleTimer.Interval = TimeSpan.FromMilliseconds(1000);

            holdTimer.Interval = TimeSpan.FromSeconds(1);
            holdTimer.Tick += HoldTimer_Tick;

        }

        private void Boxy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(0));
            t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            cursor.RenderTransform = t;
           

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
            else
            {
                List<Planet> toRemove = new List<Planet>();
                foreach (var u in mainCanvas.Children)
                {
                    if (u is Planet)
                    {
                        toRemove.Add((Planet)u);
                    }
                }
                foreach (Planet i in toRemove)
                    mainCanvas.Children.Remove(i);
            }
        }
        public Canvas GetCanvas()
        {
            return this.mainCanvas;
        }
        public static void ChoosePlanet(int orbitType, int direction)
        {
            addThisPlanetOrbit = orbitType;
            addThisPlanetDirection = direction;
            addThisPlanet = new Planet(Planet.SMALL, addThisPlanetOrbit, addThisPlanetDirection);

            initialPositionLeft = 0;
            initialPositionTop = 0;

            switch (direction)
            {
                case PlanetPoint.UP:
                    if(orbitType == PlanetPoint.SHORT)
                    {
                        initialPositionTop = 221;
                    }
                    else if (orbitType == PlanetPoint.MED)
                    {
                        initialPositionTop = 154;
                    }
                    else if (orbitType == PlanetPoint.LONG)
                    {
                        initialPositionTop = 70;
                    }
                    initialPositionLeft = 620;
                    break;
                case PlanetPoint.RIGHT:
                    if (orbitType == PlanetPoint.SHORT)
                    {
                        initialPositionLeft = 750;
                    }
                    else if (orbitType == PlanetPoint.MED)
                    {
                        initialPositionLeft = 816;
                    }
                    else if (orbitType == PlanetPoint.LONG)
                    {
                        initialPositionLeft = 901;
                    }
                    initialPositionTop = 350;
                    break;
                case PlanetPoint.DOWN:
                    if (orbitType == PlanetPoint.SHORT)
                    {
                        initialPositionTop = 480;
                    }
                    else if (orbitType == PlanetPoint.MED)
                    {
                        initialPositionTop = 546;
                    }
                    else if (orbitType == PlanetPoint.LONG)
                    {
                        initialPositionTop = 632;
                    }
                    initialPositionLeft = 620;
                    break;
                case PlanetPoint.LEFT:
                    if (orbitType == PlanetPoint.SHORT)
                    {
                        initialPositionLeft = 491;
                    }
                    else if (orbitType == PlanetPoint.MED)
                    {
                        initialPositionLeft = 423;
                    }
                    else if (orbitType == PlanetPoint.LONG)
                    {
                        initialPositionLeft = 338;
                    }
                    initialPositionTop = 350;
                    break;

            }

            ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(addThisPlanet);
            Canvas.SetLeft(addThisPlanet, initialPositionLeft);
            Canvas.SetTop(addThisPlanet, initialPositionTop);

            holdTimer.Start();

//            ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(addThisPlanet);
            
        }

        private static void HoldTimer_Tick(object sender, EventArgs e)
        {
            if (addThisPlanetLevel == 0)
            {
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Remove(addThisPlanet);
                addThisPlanet = new Planet(Planet.MED, addThisPlanetOrbit, addThisPlanetDirection);
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(addThisPlanet);


                switch (addThisPlanetDirection)
                {
                    case PlanetPoint.UP:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 221;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 154;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 70;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.RIGHT:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 750;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 816;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 901;
                        }
                        initialPositionTop = 350;
                        break;
                    case PlanetPoint.DOWN:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 480;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 546;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 632;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.LEFT:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 491;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 423;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 338;
                        }
                        initialPositionTop = 350;
                        break;
                }

                Canvas.SetLeft(addThisPlanet, initialPositionLeft);
                Canvas.SetTop(addThisPlanet, initialPositionTop);

                addThisPlanetLevel = 1;
            }
            else if (addThisPlanetLevel == 1)
            {
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Remove(addThisPlanet);
                addThisPlanet = new Planet(Planet.LARGE, addThisPlanetOrbit, addThisPlanetDirection);
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(addThisPlanet);
                switch (addThisPlanetDirection)
                {
                    case PlanetPoint.UP:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 221;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 154;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 70;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.RIGHT:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 750;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 816;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 901;
                        }
                        initialPositionTop = 350;
                        break;
                    case PlanetPoint.DOWN:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 480;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 546;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 632;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.LEFT:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 491;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 423;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 338;
                        }
                        initialPositionTop = 350;
                        break;
                }


                Canvas.SetLeft(addThisPlanet, initialPositionLeft);
                Canvas.SetTop(addThisPlanet, initialPositionTop);

                addThisPlanetLevel = 2;
            }
            else if (addThisPlanetLevel == 2)
            {
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Remove(addThisPlanet);
                addThisPlanet = new Planet(Planet.SMALL, addThisPlanetOrbit, addThisPlanetDirection);
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(addThisPlanet);
                switch (addThisPlanetDirection)
                {
                    case PlanetPoint.UP:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 221;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 154;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 70;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.RIGHT:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 750;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 816;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 901;
                        }
                        initialPositionTop = 350;
                        break;
                    case PlanetPoint.DOWN:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 480;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 546;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 632;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.LEFT:
                        if (addThisPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 491;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 423;
                        }
                        else if (addThisPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 338;
                        }
                        initialPositionTop = 350;
                        break;
                }

                Canvas.SetLeft(addThisPlanet, initialPositionLeft);
                Canvas.SetTop(addThisPlanet, initialPositionTop);

                addThisPlanetLevel = 0;
            }
        }

        public static void AddPlanet(int orbitType)
        {
            holdTimer.Stop();
            addThisPlanetLevel = 0;
            addThisPlanet.BeginOrbit();
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
            PourTea(e);

            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-pour.png", UriKind.Relative));
            boxy_status = POURING;
            
        }

        private void PourTea(MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            teaPouring = true;
            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(-45));
            t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            cursor.RenderTransform = t;
            // play sound of pouring
            //pouring animation that loops
        }

        public void UpdatePlanetPosition(Planet p)
        {
            int leftOrbitOffset = 0;
            int topOrbitOffset = 0;
            switch (p.Orbit)
            {
                case PlanetPoint.SHORT:
                    leftOrbitOffset = -10;
                    topOrbitOffset = 0;
                    break;
                case PlanetPoint.MED:
                    leftOrbitOffset = -10;
                    topOrbitOffset = 0;
                    break;
                case PlanetPoint.LONG:
                    leftOrbitOffset = -5;
                    topOrbitOffset = 0;
                    break;
            }


            Canvas.SetLeft(p, 620 + leftOrbitOffset + p.radius * Math.Cos(p.degrees * (Math.PI / 180)));
            Canvas.SetTop(p, 345 + topOrbitOffset + p.radius * Math.Sin(p.degrees * (Math.PI / 180)));
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
        private void InitializePlanetPoints()
        {
            btn_s1.Type = PlanetPoint.SHORT;
            btn_s2.Type = PlanetPoint.SHORT;
            btn_s3.Type = PlanetPoint.SHORT;
            btn_s4.Type = PlanetPoint.SHORT;

            btn_m1.Type = PlanetPoint.MED;
            btn_m2.Type = PlanetPoint.MED;
            btn_m3.Type = PlanetPoint.MED;
            btn_m4.Type = PlanetPoint.MED;

            btn_l1.Type = PlanetPoint.LONG;
            btn_l2.Type = PlanetPoint.LONG;
            btn_l3.Type = PlanetPoint.LONG;
            btn_l4.Type = PlanetPoint.LONG;

            btn_s1.Direction = PlanetPoint.UP;
            btn_s2.Direction = PlanetPoint.RIGHT;
            btn_s3.Direction = PlanetPoint.DOWN;
            btn_s4.Direction = PlanetPoint.LEFT;

            btn_m1.Direction = PlanetPoint.UP;
            btn_m2.Direction = PlanetPoint.RIGHT;
            btn_m3.Direction = PlanetPoint.DOWN;
            btn_m4.Direction = PlanetPoint.LEFT;

            btn_l1.Direction = PlanetPoint.UP;
            btn_l2.Direction = PlanetPoint.RIGHT;
            btn_l3.Direction = PlanetPoint.DOWN;
            btn_l4.Direction = PlanetPoint.LEFT;
        }
    }
}
