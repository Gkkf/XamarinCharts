using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace WykresySVGMobile
{
    public partial class MainPage : ContentPage
    {
        //values to show on charts
        private double[] values = { 1, 2, 3, 4 };
        //points for lineChart
        Point[] points = new Point[4];
        public MainPage()
        {
            InitializeComponent();
            //initialize charts
            DrawBarChart();
            DrawPieChar();
            DrawPointChart();
            //set default radiobutton
            rd1.IsChecked = true;
        }

        //function for barChart
        private void DrawBarChart()
        {
            double max = values.Max();
            int BarCount = 0;
            double[] values_scale = new double[values.Length];

            foreach (var element in BarChar.Children)
            {
                //checks if element is ellipse
                if (element is BoxView)
                {
                    //checks for all elements in chart
                    if (BarCount < values.Length)
                    {
                        //sets element procent based on users values
                        double procent = (values[BarCount] * 100) / max;
                        //creates height based on procent
                        double height = (300 * procent) / 100;
                        //making rectangle based on element from grid
                        BoxView box = (BoxView)element;
                        //sets new height for element
                        double newHeigh = height;
                        box.HeightRequest = height;
                        //saves value to array
                        values_scale[BarCount] = height;
                    }
                    BarCount++;
                }
            }
            Draw_Scale();
        }

        //draws pointChart
        private void DrawPointChart()
        {
            double points_position_counter = 0;
            double maximum = values.Max();
            int rectangleCount = 0;

            foreach (var element in LineChar.Children)
            {
                //checks if element is ellipse
                if (element is Ellipse)
                {
                    //checks for all elements in chart
                    if (rectangleCount < values.Length)
                    {
                        //sets element procent from users values
                        double procent = (values[rectangleCount] * 100) / maximum;
                        //sets height based on procent
                        double height = (300 * procent) / 100;
                        //makes new ellipse based on element
                        Ellipse ellipse = (Ellipse)element;
                        double newHeight = height;
                        Thickness t = new Thickness(0, 0, 0, height);
                        //sets margin for new point
                        ellipse.Margin = t;
                        //saves point with specific parameters to points array
                        points[rectangleCount].X = points_position_counter;
                        points[rectangleCount].Y = 397.5 - height;
                        points_position_counter += 57.5;

                    }
                    rectangleCount++;
                }
            }
            Draw_Lines();
        }

        //draws line for pointChart
        private void Draw_Lines()
        {
            Polyline polyline = LineChar.Children[4] as Polyline;
            polyline.Points.Clear();

            //adding user points to polyline
            foreach (Point point in points)
            {
                polyline.Points.Add(point);
            }
        }

        //function to pieChar
        private void DrawPieChar()
        {
            double sum = values.Sum();
            //array for angles
            double[] angles = new double[values.Length];
            double startAngle = 0;

            for (int i = 0; i < values.Length; i++)
            {
                //sets starting angle based on user actual value
                angles[i] = 360 * values[i] / sum;

                //creating path to fill with unique color
                Path path = new Path();
                path.Fill = new SolidColorBrush(Get_Color(i));

                //creates path figure
                var pathFigure = new PathFigure();
                pathFigure.StartPoint = new Point(150, 150);

                //path figure parameters (odcinek linia z początkowego punktu -> łuk)
                var lineSegment = new LineSegment();
                //adds to set real X Y coordinates   X start line using cosinus, and changes from degrees to radians  Y start line using sinus, and changes from degrees to radians
                lineSegment.Point = new Point(150 + 100 * Math.Cos(startAngle * Math.PI / 180), 150 + 100 * Math.Sin(startAngle * Math.PI / 180));

                //path figure parameters (dodawanie łuku jako segmentu)
                var arcSegment = new ArcSegment();
                //adds to set real X Y coordinates   X end angle using cosinus, and changes from degrees to radians  Y end angle using sinus, and changes from degrees to radians
                arcSegment.Point = new Point(150 + 100 * Math.Cos((startAngle + angles[i]) * Math.PI / 180), 150 + 100 * Math.Sin((startAngle + angles[i]) * Math.PI / 180));
                arcSegment.Size = new Size(100, 100);
                arcSegment.IsLargeArc = angles[i] > 180;
                arcSegment.SweepDirection = SweepDirection.Clockwise;

                //adds parameters to pathFigure
                pathFigure.Segments.Add(lineSegment);
                pathFigure.Segments.Add(arcSegment);

                //creates figure colletion
                var pathFigureCollection = new PathFigureCollection
                {
                    pathFigure
                };

                //sets parameters for path element
                path.Data = new PathGeometry() { Figures = pathFigureCollection };
                //adds path to grid
                GridPieChar1.Children.Add(path);
                //update start angle for next element
                startAngle+= angles[i];
            }
        }

        //function to get selected color
        private Color Get_Color(int i)
        {
            switch(i)
            {
                case 0:
                    return Color.DarkSalmon;   
                case 1:
                    return Color.LightGreen;
                case 2:
                    return Color.DarkOrange;
                case 3:
                    return Color.LightBlue;
                default:
                    return Color.DarkSalmon;
            }
        }

        //function to create scale
        public void Draw_Scale()
        {
            double[] array = new double[values.Length];

            for (int i = 0; i < 4; i++)
            {
                //sets the height for scale based on user values
                double procent = (values[i] * 100) / values.Max();
                double height = (300 * procent) / 100;
                array[i] = height;
            }

            Thickness a = new Thickness(0, 0, 0, array[0]);
            Thickness b = new Thickness(0, 0, 0, array[1]);
            Thickness c = new Thickness(0, 0, 0, array[2]);
            Thickness d = new Thickness(0, 0, 0, array[3]);

            //sets margins for scale lines
            ScaleLine1.Margin = a;
            ScaleLine2.Margin = b;
            ScaleLine3.Margin = c;
            ScaleLine4.Margin = d;

            a = new Thickness(60, 0, 0, array[0] - 5);
            b = new Thickness(60, 0, 0, array[1] - 5);
            c = new Thickness(60, 0, 0, array[2] - 5);
            d = new Thickness(60, 0, 0, array[3] - 5);

            //sets scale text based on values
            //sets margins for text based on scale line values
            b1.Text = values[0].ToString();
            b1.Margin = a;
            b2.Text = values[1].ToString();
            b2.Margin = b;
            b3.Text = values[2].ToString();
            b3.Margin = c;
            b4.Text = values[3].ToString();
            b4.Margin = d;
        }

        //function to radiobuttons
        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //clear everything
            BarChar.IsVisible = false;
            LineChar.IsVisible = false;
            GridPieChar.IsVisible = false;

            BarLineX.IsVisible = false;
            BarLineY.IsVisible = false;
            ScaleLine1.IsVisible = true;
            ScaleLine2.IsVisible = true;
            ScaleLine3.IsVisible = true;
            ScaleLine4.IsVisible = true;

            //shows selected chart
            if (rd1.IsChecked == true)
            {
                BarChar.IsVisible = true;
                BarLineX.IsVisible = true;
                BarLineY.IsVisible = true;
                b1.IsVisible = true;
                b2.IsVisible = true;
                b3.IsVisible = true;
                b4.IsVisible = true;
                DrawBarChart();
            }          
            else if (rd2.IsChecked == true)
            {
                LineChar.IsVisible = true;
                b1.IsVisible = true;
                b2.IsVisible = true;
                b3.IsVisible = true;
                b4.IsVisible = true;
                DrawPointChart();
            }           
            else if (rd3.IsChecked == true)
            {
                GridPieChar.IsVisible = true;
                DrawPieChar();
                ScaleLine1.IsVisible = false;
                ScaleLine2.IsVisible = false;
                ScaleLine3.IsVisible = false;
                ScaleLine4.IsVisible = false;
                b1.IsVisible = false;
                b2.IsVisible = false;
                b3.IsVisible = false;
                b4.IsVisible = false;
            }
              
        }

        //function for user chart names
        private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //changes labels text for names
            if (sender == NameEntry1)
            {
                LabelName_1.Text = e.NewTextValue;
            } 
            else if (sender == NameEntry2)
            {
                LabelName_2.Text = e.NewTextValue;
            }
            else if (sender == NameEntry3)
            {
                LabelName_3.Text = e.NewTextValue;
            }
            else if (sender == NameEntry4)
            {
                LabelName_4.Text = e.NewTextValue;
            }
        }

        //function for user chart values
        private void ValueEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry textBox = (Entry)sender;

            //sets values to array if not null and calls functions
            if (!String.IsNullOrEmpty(textBox.Text))
            {
                values[Convert.ToInt32(textBox.StyleId) - 1] = Convert.ToDouble(textBox.Text);
                DrawBarChart();
                DrawPointChart();
                DrawPieChar();
            }
            else
            {
                //sets value in array to 1 for selected index and calls functions
                values[Convert.ToInt32(textBox.StyleId) - 1] = 1;
                DrawBarChart();
                DrawPointChart();
                DrawPieChar();
            }
        }
    }
}
