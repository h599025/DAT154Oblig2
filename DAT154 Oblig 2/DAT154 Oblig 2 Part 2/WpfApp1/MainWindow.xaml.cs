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
using SpaceSim;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool ShowTextBool;
        private List<SpaceObject> solarSystem;
        private List<SpaceObject> currentList;
        private SpaceObject parent;
        private double distanceScale;
        private double spaceObjDrawScale;
        private bool isZoomed;
        private TimeController timeController;

        public MainWindow()
        {
            InitializeComponent();
            timeController = new TimeController();
			Star sun = new Star("Sun", "Yellow", 1390000, 0, 0, (27 * 24), null);
			sun.position[0] = 0;
			sun.position[1] = 0;
            SolarSystem solar_system = new SolarSystem();
            this.solarSystem = solar_system.getSolarSystem();
            currentList = solarSystem;
            parent = sun;
            foreach(SpaceObject v in solarSystem)
            {
                if((v is Planet))
                {
                    timeController.MYTICK += (s,o) => v.updatePosition(o.Time, parent);
                }
                
            }

            this.distanceScale = 5000;
            this.spaceObjDrawScale = 1000; 
            this.isZoomed = false;

            //EVENTS:
            IncreaseRateButton.Click += (s, o) => timeController.increaseRate();
            DecreaseRateButton.Click += (s, o) => timeController.decreaseRate();
            StartStopButton.Click += (s, o) => timeController.StartStopTimer();
            this.Loaded += (s,o) => drawSolarSystem(solarSystem, sun);
            SizeChanged += (s, o) => drawSolarSystem(currentList, parent);
            timeController.MYTICK += (s, o) => drawSolarSystem(currentList, parent);
            MouseRightButtonDown += (s, o) =>
            {
                if (isZoomed)
                {
                    this.distanceScale = 5000;
                    this.spaceObjDrawScale = 1000;
                    this.parent = sun;
                    this.currentList = solarSystem;
                    drawSolarSystem(solarSystem, sun);
                }
            };

        }


        public void drawSolarSystem(List<SpaceObject> solar_system, SpaceObject center_planet)
        {
            ClearCanvasSpaceObj();

            //TODO: DRAW all sub-objects in solar_system
            center_planet.position = new double[] { 0, 0 }; 

            double screenOffsetX = myCanvas.RenderSize.Width / 2;
            double screenOffsetY = myCanvas.RenderSize.Height / 2;
            //TODO: DRAW SUN:
            double[] screenPos1 = transformSpacePosToScreenPos(center_planet.position, screenOffsetX, screenOffsetY);

            drawText(center_planet, screenPos1);
            
            Ellipse ellipse1 = new Ellipse();
            SolidColorBrush solidColorBrush1 = new SolidColorBrush();
            solidColorBrush1.Color = Color.FromArgb(255, 255, 255, 1);
            ellipse1.Fill = solidColorBrush1;
            ellipse1.StrokeThickness = 2;
            ellipse1.Stroke = Brushes.Black;
            double centerPlanetDimension;

            //TODO: WHY VIRKER IKKE DETTE? 
            if (center_planet is Star)
            {
                centerPlanetDimension = center_planet.radius / (this.spaceObjDrawScale* 10);
            } else
            {
                centerPlanetDimension = center_planet.radius / (this.spaceObjDrawScale * 5);
            }
            ellipse1.Width = (int)centerPlanetDimension;
            ellipse1.Height = (int)centerPlanetDimension;

            /// 
            ellipse1.MouseUp += zoomInOnObject;
            Canvas.SetLeft(ellipse1, screenPos1[0] - (ellipse1.Width / 2));
            Canvas.SetTop(ellipse1, screenPos1[1] - (ellipse1.Height / 2));
            myCanvas.Children.Add(ellipse1);
            
            foreach (SpaceObject obj in solar_system) {

                
                //Getting information from obj
                //string type = obj.GetType().Name;

                //Transforming to screenSpacePositon
                double[] screenPos = transformSpacePosToScreenPos(obj.position, screenOffsetX, screenOffsetY);

                //Rendering information about object to screen

                //Draw orbit
                Ellipse orbit = new Ellipse();
                orbit.Stroke = Brushes.Black;
                double radius = distance(new double[] { screenOffsetX, screenOffsetY }, screenPos);
                radius = Math.Abs(radius);
                orbit.Width = radius * 2;
                orbit.Height = radius * 2;
                myCanvas.Children.Add(orbit);
                Canvas.SetLeft(orbit, screenOffsetX - radius);
                Canvas.SetTop(orbit, screenOffsetY - radius);

                drawText(obj, screenPos);

                Ellipse ellipse = new Ellipse();
                ellipse.Tag = obj.name;
                SolidColorBrush solidColorBrush = new SolidColorBrush();
                solidColorBrush.Color = Color.FromArgb(255, 255, 0, 1);
                ellipse.Fill = solidColorBrush;
                ellipse.StrokeThickness = 2;
                ellipse.Stroke = Brushes.Black;
                ellipse.Width = obj.radius / this.spaceObjDrawScale;
                ellipse.Height = obj.radius / this.spaceObjDrawScale;
                ellipse.MouseDown += zoomInOnObject;
                myCanvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, screenPos[0] - (ellipse.Width / 2));
                Canvas.SetTop(ellipse, screenPos[1] - (ellipse.Height / 2));
            }
        }
        private double distance(double[] a, double[] b)
        {
            double dx = a[0] - b[0];
            double dy = a[1] - b[1];
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void drawText(SpaceObject obj, double[] screenPos)
        {
            if (ShowTextBool)
            {
                TextBox textBox = new TextBox();
                textBox.Text = obj.GetType().Name + ": " + obj.name;
                Panel.SetZIndex(textBox, 1);
                myCanvas.Children.Add(textBox);
                Canvas.SetLeft(textBox, screenPos[0]);
                Canvas.SetTop(textBox, screenPos[1]);
            }
        }

        private void ClearCanvasSpaceObj()
        {
            for (int i = myCanvas.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = myCanvas.Children[i];
                if (Child is TextBox || Child is Ellipse)
                    myCanvas.Children.Remove(Child);
            }
        }

        private double[] transformSpacePosToScreenPos(double[] position, double sOX, double sOY)
        { 
            double[] screenPos = { sOX + position[0] / this.distanceScale, sOY + position[1] / this.distanceScale };
            return screenPos;
        }
        private void HandleTextCheck(object sender, RoutedEventArgs e)
        {
            ShowTextBool = true;
            if(this.parent != null)
            {
                drawSolarSystem(this.currentList, this.parent);
            }
            
        }

        private void HandleTextUnchecked(object sender, RoutedEventArgs e)
        {
            ShowTextBool = false;
            drawSolarSystem(this.currentList, this.parent);
        }


        void zoomInOnObject(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                SpaceObject current_object = null;
                foreach(SpaceObject so in solarSystem)
                {
                    if(so.name == (String)ellipse.Tag)
                    {
                        current_object = so;
                    }
                }

                if (current_object != null)
                {
                    if (current_object.moonList != null)
                    {
                        PlanetInfoBox.Text = current_object.toString();
                        ClearCanvasSpaceObj();
                        //current_object.moonList.Insert(0, current_object);
                        this.parent = current_object;
                        this.currentList = current_object.moonList;
                        this.distanceScale = 5000;
                        this.spaceObjDrawScale = 100;
                        this.isZoomed = true;
                        drawSolarSystem(currentList, parent);
                    }
                }
            }
        }
    }
}
