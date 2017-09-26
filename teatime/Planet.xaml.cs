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
    /// Interaction logic for Planet.xaml
    /// </summary>
    public partial class Planet : UserControl
    {
        
        private DispatcherTimer timer; // timer that governs when to update the planet's position

        public int Size { get; }
        public int Orbit { get; }
        public int Direction { get; }
        public int Radius { get; }
        public int Degrees { get; set; }

        // planet size constants
        public const int SMALL = 0;
        public const int MED = 1;
        public const int LARGE = 2;

        private MediaPlayer sound1;
        private MediaPlayer sound2;
        private MediaPlayer sound3;
        private MediaPlayer sound4;

        private int soundLibrary_size;
        private int soundLibrary_direction;

        //
        private int degreesDelta;

        public Planet(int PlanetType, int orbitType, int direction)
        {
            InitializeComponent();
            this.IsHitTestVisible = false; // should not be able to interact with planets once made
            degreesDelta = 1;
            Degrees = 0;
            Radius = 1;
            Size = PlanetType;
            Orbit = orbitType;

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;

            

            //choose appropriate image
            switch (PlanetType)
            {
                case SMALL:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-small.png", UriKind.Relative));
                    timer.Interval = TimeSpan.FromMilliseconds(10);
                    soundLibrary_size = 0;
                    break;
                case MED:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-med.png", UriKind.Relative));
                    timer.Interval = TimeSpan.FromMilliseconds(10);
                    soundLibrary_size = 1;
                    break;

                case LARGE:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-large.png", UriKind.Relative));
                    timer.Interval = TimeSpan.FromMilliseconds(10);
                    soundLibrary_size = 2;
                    break;

                default:
                    Debug.WriteLine("This shouldn't have happened.");
                    break;
            }
            
            //set radius and rate of change in degrees according to orbit distance
            switch (orbitType)
            {
                case 0:
                    Radius = 130;
                    degreesDelta = 3;
                    break;
                case 1:
                    Radius = 190;
                    degreesDelta = 2;
                    break;
                case 2:
                    Radius = 275;
                    degreesDelta = 1;
                    break;

            }

            // make sure degrees and initial position match
            switch (direction)
            {
                case 0:
                    Degrees = 270;
                    break;
                case 1:
                    Degrees = 0;
                    break;
                case 2:
                    Degrees = 90;
                    break;
                case 3:
                    Degrees = 180;
                    break;
            }

            //choose sounds to use
            sound1 = new MediaPlayer();
            sound2 = new MediaPlayer();
            sound3 = new MediaPlayer();
            sound4 = new MediaPlayer();

            sound1.MediaEnded += Sound1_MediaEnded;
            sound2.MediaEnded += Sound2_MediaEnded;
            sound3.MediaEnded += Sound3_MediaEnded;
            sound4.MediaEnded += Sound4_MediaEnded;

            soundLibrary_direction = direction;
            
            if (soundLibrary_size == 0)
            {
                if (soundLibrary_direction == 0)
                {
                    sound1.Open(new Uri(@"../../sounds/small/I.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/small/III.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/small/V.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/small/VII.mp3", UriKind.RelativeOrAbsolute));

                }
                else if (soundLibrary_direction == 1)
                {
                    sound1.Open(new Uri(@"../../sounds/small/II.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/small/IV.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/small/VI.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/small/I.mp3", UriKind.RelativeOrAbsolute));

                }
                else if (soundLibrary_direction == 2)
                {
                    sound1.Open(new Uri(@"../../sounds/small/IV.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/small/III.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/small/II.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/small/III.mp3", UriKind.RelativeOrAbsolute));
                }
                else if (soundLibrary_direction == 3)
                {
                    sound1.Open(new Uri(@"../../sounds/small/I.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/small/VI.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/small/II.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/small/VII.mp3", UriKind.RelativeOrAbsolute));
                }
            }
            else if (soundLibrary_size == 1)
            {
                if (soundLibrary_direction == 0)
                {
                    sound1.Open(new Uri(@"../../sounds/med/III.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/med/IV.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/med/I.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/med/II.mp3", UriKind.RelativeOrAbsolute));

                }
                else if (soundLibrary_direction == 1)
                {
                    sound1.Open(new Uri(@"../../sounds/med/VI.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/med/V.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/med/III.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/med/II.mp3", UriKind.RelativeOrAbsolute));

                }
                else if (soundLibrary_direction == 2)
                {
                    sound1.Open(new Uri(@"../../sounds/med/V.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/med/II.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/med/VII.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/med/II.mp3", UriKind.RelativeOrAbsolute));
                }
                else if (soundLibrary_direction == 3)
                {
                    sound1.Open(new Uri(@"../../sounds/med/VII.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/med/VI.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/med/V.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/med/IV.mp3", UriKind.RelativeOrAbsolute));
                }
            }
            else if(soundLibrary_size == 2)
            {
                if (soundLibrary_direction == 0)
                {
                    sound1.Open(new Uri(@"../../sounds/large/I.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/large/VI.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/large/V.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/large/II.mp3", UriKind.RelativeOrAbsolute));

                }
                else if (soundLibrary_direction == 1)
                {
                    sound1.Open(new Uri(@"../../sounds/large/III.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/large/IV.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/large/V.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/large/II.mp3", UriKind.RelativeOrAbsolute));

                }
                else if (soundLibrary_direction == 2)
                {
                    sound1.Open(new Uri(@"../../sounds/large/VII.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/large/I.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/large/V.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/large/I.mp3", UriKind.RelativeOrAbsolute));
                }
                else if (soundLibrary_direction == 3)
                {
                    sound1.Open(new Uri(@"../../sounds/large/VI.mp3", UriKind.RelativeOrAbsolute));
                    sound2.Open(new Uri(@"../../sounds/large/III.mp3", UriKind.RelativeOrAbsolute));
                    sound3.Open(new Uri(@"../../sounds/large/I.mp3", UriKind.RelativeOrAbsolute));
                    sound4.Open(new Uri(@"../../sounds/large/V.mp3", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void Sound4_MediaEnded(object sender, EventArgs e)
        {
            ((MediaPlayer)sender).Stop();
            ((MediaPlayer)sender).Position = TimeSpan.Zero;
        }

        private void Sound3_MediaEnded(object sender, EventArgs e)
        {
            ((MediaPlayer)sender).Stop();
            ((MediaPlayer)sender).Position = TimeSpan.Zero;
        }

        private void Sound2_MediaEnded(object sender, EventArgs e)
        {
            ((MediaPlayer)sender).Stop();
            ((MediaPlayer)sender).Position = TimeSpan.Zero;
        }

        private void Sound1_MediaEnded(object sender, EventArgs e)
        {
            ((MediaPlayer)sender).Stop();
            ((MediaPlayer)sender).Position = TimeSpan.Zero;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            ((MainWindow)App.Current.MainWindow).UpdatePlanetPosition(this);

            Degrees += degreesDelta;
            if (Degrees == 360)
                Degrees = 0;

            // play a sound when reaching any of these four points
            if (Degrees == 0)
            {
                //Debug.WriteLine("Degree 0");
                sound1.Play();
                
            }
            else if (Degrees == 90)
            {
                //Debug.WriteLine("Degree 90");
                sound2.Play();
            }
            else if (Degrees == 180)
            {
                //Debug.WriteLine("Degree 180");
                sound3.Play();
            }
            else if (Degrees == 270)
            {
                //Debug.WriteLine("Degree 270");
                sound4.Play();
            }
        }

        public void BeginOrbit()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
