﻿using System;
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
using System.Windows.Media.Animation;
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
        private DispatcherTimer idleTimer;// plays an idle animation every x milliseconds

        private static Point lastReportedCursorLocation;

        // These variables help determine the planet to be added
        private static Planet placeholderPlanet;
        private static int placeholderPlanetType;
        private static int placeholderPlanetOrbit;
        private static int placeholderPlanetDirection;
        private static int placeholderPlanetSize; // the current size of the planet, changed during different levels of hold length
        private static int initialPositionLeft; // initial left coordinate of planet to be added
        private static int initialPositionTop; // initial top coordinate of planet to be added
        private static DispatcherTimer holdTimer; // determines when to make the planet bigger while button is held down (tea is pouring)


        //Storyboard Animations
        private Storyboard tea_drink;
        public Storyboard tea_pour;
        public Storyboard idle_bounce;


        // has the main attraction started yet? 
        private bool started = false;
        private bool started2 = false;
        private bool teaPouring = false;

        // mediaplayer for boxy's sounds and pouring sound
        private MediaPlayer player;



        public MainWindow()
        {
            InitializeComponent();
            HideElements();
            InitializePlanetPoints(); // Planet Points are locations where tea can be poured to make a planet, i.e. locations where the button(tea) is enabled

            holdTimer = new DispatcherTimer();
            idleTimer = new DispatcherTimer();

            // initializing the placeholder planet's attributes so nothing is null, hopefully
            placeholderPlanetType = Planet.SMALL;
            placeholderPlanetOrbit = PlanetPoint.SHORT;
            placeholderPlanetDirection = PlanetPoint.UP;
            placeholderPlanet = new Planet(placeholderPlanetType, placeholderPlanetOrbit, placeholderPlanetDirection);
            placeholderPlanetSize = 0;

            // Binding events
            mainWindow.MouseMove += MainWindow_MouseMove;
            boxy.MouseDown += Boxy_MouseDown;
            boxy.MouseUp += Boxy_MouseUp;

            // Setting up timers
            idleTimer.Interval = TimeSpan.FromMilliseconds(1000);

            holdTimer.Interval = TimeSpan.FromSeconds(1);
            holdTimer.Tick += HoldTimer_Tick;

            idleTimer.Interval = TimeSpan.FromMilliseconds(1500);
            idleTimer.Tick += IdleTimer_Tick;

            //Storyboard Animations
            tea_drink = this.Resources["teaDrink"] as Storyboard;
            tea_drink.Completed += Tea_drink_Completed;

            tea_pour = this.Resources["TeaPour"] as Storyboard;
            tea_pour.Completed += Tea_pour_Completed;

            idle_bounce = this.Resources["idleAnim"] as Storyboard;

            //init for mediaplayer
            player = new MediaPlayer();
        }

        private void IdleTimer_Tick(object sender, EventArgs e)
        {
            idle_bounce.Begin();
        }

        private void PlayPourSound()
        {
            player.Open(new Uri(@"../../sounds/other/pouring.mp3", UriKind.RelativeOrAbsolute));
            player.MediaEnded += Player_MediaEnded;
            player.Play();
        }

        private void PlayDrinkSound()
        {
            player.Open(new Uri(@"../../sounds/other/drinking.mp3", UriKind.RelativeOrAbsolute));
            player.MediaEnded += Player_MediaEnded;
            player.Play();
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            player.Stop();
            player.Position = TimeSpan.Zero;
            if (teaPouring)
            {
                player.Play();
            }
        }

        private void Tea_pour_Completed(object sender, EventArgs e)
        {
            if (teaPouring)
                tea_pour.Begin();
        }

        private void Tea_drink_Completed(object sender, EventArgs e)
        {

            // change to this after drinking animation is done
            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-happy.png", UriKind.Relative));

            if (!started)
            {
                MakeAllVisible();

            }
            // reset all planets
            else
            {
                idleTimer.Stop();
                started2 = false;
                List<Planet> toRemove = new List<Planet>();
                foreach (var u in mainCanvas.Children)
                {
                    if (u is Planet)
                    {
                        toRemove.Add((Planet)u);
                    }
                }
                foreach (Planet i in toRemove)
                {
                    i.StopTimer();
                    mainCanvas.Children.Remove(i);
                }
            }
        }

        private void Boxy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            teaPouring = false;
            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-drink.png", UriKind.Relative));
            player.Stop();
            player.Position = TimeSpan.Zero;
            PlayDrinkSound();

        }

        //return the canvas so static methods can do things to it
        public Canvas GetCanvas()
        {
            return this.mainCanvas;
        }

        // choosing the location/size of a planet to add
        public static void ChoosePlanet(Point mousePos, int orbitType, int direction)
        {
            PourTea(mousePos);
            ((MainWindow)App.Current.MainWindow).teaPouring = true;
            ((MainWindow)App.Current.MainWindow).PlayPourSound();

            placeholderPlanetOrbit = orbitType;
            placeholderPlanetDirection = direction;
            placeholderPlanet = new Planet(Planet.SMALL, placeholderPlanetOrbit, placeholderPlanetDirection); // always start small

            initialPositionLeft = 0;
            initialPositionTop = 0;

            // change the location to spawn the planet in depending on the PlanetPoint where tea is being poured
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


            // spawn in the placeholder planet
            ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(placeholderPlanet);
            Canvas.SetLeft(placeholderPlanet, initialPositionLeft);
            Canvas.SetTop(placeholderPlanet, initialPositionTop);

            // tea is being poured...
            holdTimer.Start();

//            ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(addThisPlanet);
            
        }

        private static void HoldTimer_Tick(object sender, EventArgs e)
        {
            if (placeholderPlanetSize == 0) // the planet was either just spawned in or was at maximum size
            {
                // remove the previous instance of the placeholder to make a bigger one
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Remove(placeholderPlanet); 
                placeholderPlanet = new Planet(Planet.MED, placeholderPlanetOrbit, placeholderPlanetDirection);
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(placeholderPlanet);

                // make sure it's in the same planetPoint
                switch (placeholderPlanetDirection)
                {
                    case PlanetPoint.UP:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 221;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 154;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 70;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.RIGHT:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 750;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 816;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 901;
                        }
                        initialPositionTop = 350;
                        break;
                    case PlanetPoint.DOWN:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 480;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 546;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 632;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.LEFT:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 491;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 423;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 338;
                        }
                        initialPositionTop = 350;
                        break;
                }

                Canvas.SetLeft(placeholderPlanet, initialPositionLeft);
                Canvas.SetTop(placeholderPlanet, initialPositionTop);

                //prepare for next tick if applicable
                placeholderPlanetSize = 1;
            }
            else if (placeholderPlanetSize == 1)
            {
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Remove(placeholderPlanet);
                placeholderPlanet = new Planet(Planet.LARGE, placeholderPlanetOrbit, placeholderPlanetDirection);
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(placeholderPlanet);
                switch (placeholderPlanetDirection)
                {
                    case PlanetPoint.UP:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 221;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 154;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 70;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.RIGHT:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 750;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 816;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 901;
                        }
                        initialPositionTop = 350;
                        break;
                    case PlanetPoint.DOWN:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 480;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 546;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 632;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.LEFT:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 491;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 423;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 338;
                        }
                        initialPositionTop = 350;
                        break;
                }


                Canvas.SetLeft(placeholderPlanet, initialPositionLeft);
                Canvas.SetTop(placeholderPlanet, initialPositionTop);

                placeholderPlanetSize = 2;
            }
            else if (placeholderPlanetSize == 2)
            {
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Remove(placeholderPlanet);
                placeholderPlanet = new Planet(Planet.SMALL, placeholderPlanetOrbit, placeholderPlanetDirection);
                ((MainWindow)App.Current.MainWindow).GetCanvas().Children.Add(placeholderPlanet);
                switch (placeholderPlanetDirection)
                {
                    case PlanetPoint.UP:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 221;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 154;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 70;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.RIGHT:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 750;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 816;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 901;
                        }
                        initialPositionTop = 350;
                        break;
                    case PlanetPoint.DOWN:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionTop = 480;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionTop = 546;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionTop = 632;
                        }
                        initialPositionLeft = 620;
                        break;
                    case PlanetPoint.LEFT:
                        if (placeholderPlanetOrbit == PlanetPoint.SHORT)
                        {
                            initialPositionLeft = 491;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.MED)
                        {
                            initialPositionLeft = 423;
                        }
                        else if (placeholderPlanetOrbit == PlanetPoint.LONG)
                        {
                            initialPositionLeft = 338;
                        }
                        initialPositionTop = 350;
                        break;
                }

                Canvas.SetLeft(placeholderPlanet, initialPositionLeft);
                Canvas.SetTop(placeholderPlanet, initialPositionTop);

                placeholderPlanetSize = 0;
            }
        }

        // tea is not being poured anymore. lock in the planet and let it start orbiting
        public static void AddPlanet(int orbitType)
        {
            holdTimer.Stop();
            ((MainWindow)App.Current.MainWindow).teaPouring = false;
            ((MainWindow)App.Current.MainWindow).player.Stop();
            ((MainWindow)App.Current.MainWindow).player.Position = TimeSpan.Zero;
            placeholderPlanetSize = 0;
            placeholderPlanet.BeginOrbit();
            if (((MainWindow)App.Current.MainWindow).started2 == false)
            {
                ((MainWindow)App.Current.MainWindow).idleTimer.Start();
                ((MainWindow)App.Current.MainWindow).started2 = true;
            }
        }

        // keeps the Button/tea at the cursor's location
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            lastReportedCursorLocation = e.GetPosition(this);
            //Debug.WriteLine(lastReportedCursorLocation);
            Point mousePos = e.GetPosition(this);
            TranslateTransform transform = new TranslateTransform
            {
                X = mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4,
                Y = mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4
            };
            this.cursor.RenderTransform = transform;
                
        }

        // Pouring tea for Boxy
        private void Boxy_MouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            teaPouring = true;
            PourTea(mousePos);
            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-pour.png", UriKind.Relative));
            PlayPourSound();
        }

        // Change the tea cursor
        private static void PourTea(Point mousePos)
        {
            ((MainWindow)App.Current.MainWindow).teaPouring = true;
            Image cursor = ((MainWindow)App.Current.MainWindow).cursor;

            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(-45));
            //t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            t.Children.Add(new TranslateTransform(lastReportedCursorLocation.X - Canvas.GetLeft(cursor) - cursor.Width / 4, lastReportedCursorLocation.Y - Canvas.GetTop(cursor) - cursor.Height / 4));

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


            Canvas.SetLeft(p, 620 + leftOrbitOffset + p.Radius * Math.Cos(p.Degrees * (Math.PI / 180)));
            Canvas.SetTop(p, 345 + topOrbitOffset + p.Radius * Math.Sin(p.Degrees * (Math.PI / 180)));
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
