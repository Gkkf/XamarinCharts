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
        private double[] values = { 3, 2, 4, 1 };
        Point[] points = new Point[4];
        public MainPage()
        {
            InitializeComponent();
            DrawBarChart();
            DrawPieChar();
            DrawPointChart();
            rd1.IsChecked = true;
        }

        private void DrawPointChart()
        {

            double points_position_counter = 0;
            double maximum = values.Max();
            int rectangleCount = 0;

            foreach (var element in LineChar.Children)
            {
                if (element is Ellipse)
                {
                    if (rectangleCount < values.Length)
                    {
                        double procent = (values[rectangleCount] * 100) / maximum;
                        double height = (300 * procent) / 100;
                        Ellipse rectangle = (Ellipse)element;
                        double newHeight = height;
                        Thickness t = new Thickness(0, 0, 0, height);
                        rectangle.Margin = t;
                        points[rectangleCount].X = points_position_counter;
                        points[rectangleCount].Y = 397.5 - height;
                        points_position_counter += 57.5;

                    }
                    rectangleCount++;
                }
            }
            Draw_Lines();

        }

        private void Draw_Lines()
        {
            Polyline polyline = LineChar.Children[4] as Polyline;
            polyline.Points.Clear();

            foreach (Point point in points)
            {
                polyline.Points.Add(point);
            }
        }

        private void DrawBarChart()
        {

            double max = values.Max();
            int BarCount = 0;
            double[] values_scale = new double[values.Length];
            foreach (var element in BarChar.Children)
            {
                if(element is BoxView)
                {
                    if(BarCount < values.Length)
                    {
                        double procent = (values[BarCount] * 100) / max;
                        double height = (300 * procent) / 100;
                        BoxView rect = (BoxView)element;
                        double newHeigh = height;
                        rect.HeightRequest = height;
                        values_scale[BarCount] = height;
                    }
                    BarCount++;
                }
            }
            Draw_Scale();
        }
        public void Draw_Scale()
        {
            double[] array = new double[values.Length];
            for(int i = 0; i<4;i++)
            {
                double precent = (values[i] * 100) / values.Max();
                double height = (300 * precent) / 100;
                array[i] = height;
            }
            Thickness a = new Thickness(0, 0, 0, array[0]);
            Thickness b = new Thickness(0, 0, 0, array[1]);
            Thickness c = new Thickness(0, 0, 0, array[2]);
            Thickness d = new Thickness(0, 0, 0, array[3]);
            ScaleLine1.Margin = a;
            ScaleLine2.Margin = b;
            ScaleLine3.Margin = c;
            ScaleLine4.Margin = d;
            a = new Thickness(60, 0, 0, array[0] - 5);
            b = new Thickness(60, 0, 0, array[1] - 5);
            c = new Thickness(60, 0, 0, array[2] - 5);
            d = new Thickness(60, 0, 0, array[3] - 5);


            b1.Text = values[0].ToString();
            b1.Margin = a;
            b2.Text = values[1].ToString();
            b2.Margin = b;
            b3.Text = values[2].ToString();
            b3.Margin = c;
            b4.Text = values[3].ToString();
            b4.Margin = d;
        }



        private void DrawPieChar()
        {
            double sum = values.Sum();
            double[] angles = new double[values.Length];
            double startAngle = 0;
            for (int i = 0; i < values.Length; i++)
            {
                angles[i] = 360 * values[i] / sum;

                Path path = new Path();
                path.Fill = new SolidColorBrush(Get_Color(i));

                var pathFigure = new PathFigure();
                pathFigure.StartPoint = new Point(150, 150);

                var lineSegment = new LineSegment();
                lineSegment.Point = new Point(150 + 100 * Math.Cos(startAngle * Math.PI / 180), 150 + 100 * Math.Sin(startAngle * Math.PI / 180));

                var arcSegment = new ArcSegment();
                arcSegment.Point = new Point(150 + 100 * Math.Cos((startAngle + angles[i]) * Math.PI / 180), 150 + 100 * Math.Sin((startAngle + angles[i]) * Math.PI / 180));
                arcSegment.Size = new Size(100, 100);
                arcSegment.IsLargeArc = angles[i] > 180;
                arcSegment.SweepDirection = SweepDirection.Clockwise;

                pathFigure.Segments.Add(lineSegment);
                pathFigure.Segments.Add(arcSegment);

                var pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(pathFigure);

                path.Data = new PathGeometry() { Figures = pathFigureCollection };
                GridPieChar1.Children.Add(path);
                startAngle+= angles[i];
            }

           /*//GridPieChar.Children.Clear();
           double sum = values.Sum();
           double[] angles = new double[values.Length];
           double centerX = 150;
           double centerY = 150;
           double radius = 100;
           double startAngle = 0;

           for (int i = 0; i < values.Length; i++)
           {
               angles[i] = 360 * values[i] / sum;
           }

           startAngle += angles[0];
           p1.StartPoint = new Point(centerX, centerY);
           l1.Point = new Point(centerX + radius * Math.Cos(startAngle * Math.PI / 180), centerY + radius * Math.Sin(startAngle * Math.PI / 180));
           as1.Point = new Point(centerX + radius * Math.Cos((startAngle + angles[0]) * Math.PI / 180), centerY + radius * Math.Sin((startAngle + angles[0]) * Math.PI / 180));
           as1.Size = new Size(radius, radius);
           as1.SweepDirection = SweepDirection.Clockwise;
           as1.IsLargeArc = angles[0] > 180;

           startAngle += angles[1];
           p2.StartPoint = new Point(centerX, centerY);
           l2.Point = new Point(centerX + radius * Math.Cos(startAngle * Math.PI / 180), centerY + radius * Math.Sin(startAngle * Math.PI / 180));
           as2.Point = new Point(centerX + radius * Math.Cos((startAngle + angles[1]) * Math.PI / 180), centerY + radius * Math.Sin((startAngle + angles[1]) * Math.PI / 180));
           as2.Size = new Size(radius, radius);
           as2.SweepDirection = SweepDirection.Clockwise;
           as2.IsLargeArc = angles[1] > 180;

           startAngle += angles[2];
           p3.StartPoint = new Point(centerX, centerY);
           l3.Point = new Point(centerX + radius * Math.Cos(startAngle * Math.PI / 180), centerY + radius * Math.Sin(startAngle * Math.PI / 180));
           as3.Point = new Point(centerX + radius * Math.Cos((startAngle + angles[2]) * Math.PI / 180), centerY + radius * Math.Sin((startAngle + angles[2]) * Math.PI / 180));
           as3.Size = new Size(radius, radius);
           as3.SweepDirection = SweepDirection.Clockwise;
           as3.IsLargeArc = angles[2] > 180;

           startAngle += angles[3];
           p4.StartPoint = new Point(centerX, centerY);
           l4.Point = new Point(centerX + radius * Math.Cos(startAngle * Math.PI / 180), centerY + radius * Math.Sin(startAngle * Math.PI / 180));
           as4.Point = new Point(centerX + radius * Math.Cos((startAngle + angles[3]) * Math.PI / 180), centerY + radius * Math.Sin((startAngle + angles[3]) * Math.PI / 180));
           as4.Size = new Size(radius, radius);
           as4.SweepDirection = SweepDirection.Clockwise;
           as4.IsLargeArc = angles[3] > 180;

           PathFigure[] pathFigures = { p1, p2, p3, p4 };
           LineSegment[] lineSegments = { l1, l2, l3, l4 };
           ArcSegment[] arcSegments = { as1, as2, as3, as4 };

           for (int i = 0; i < pathFigures.Length; i++)
           {
               pathFigures[i].StartPoint = new Point(centerX, centerY);
               lineSegments[i].Point = new Point(centerX + radius * Math.Cos(startAngle * Math.PI / 180), centerY + radius * Math.Sin(startAngle * Math.PI / 180));
               arcSegments[i].Point = new Point(centerX + radius * Math.Cos((startAngle + angles[i]) * Math.PI / 180), centerY + radius * Math.Sin((startAngle + angles[i]) * Math.PI / 180));
               arcSegments[i].Size = new Size(radius, radius);
               arcSegments[i].SweepDirection = SweepDirection.Clockwise;
               arcSegments[i].IsLargeArc = angles[i] > 180;

               startAngle += angles[i];
           }
           */
        }


        private Color Get_Color(int i)
        {
            switch(i)
            {
                case 0:
                    return Color.Red;   
                case 1:
                    return Color.Green;
                case 2:
                    return Color.Yellow;
                case 3:
                    return Color.Orange;
                default:
                    return Color.Red;
            }
        }


        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
            BarChar.IsVisible = false;
            LineChar.IsVisible = false;
            GridPieChar.IsVisible = false;

            BarLineX.IsVisible = false;
            BarLineY.IsVisible = false;
            ScaleLine1.IsVisible = true;
            ScaleLine2.IsVisible = true;
            ScaleLine3.IsVisible = true;
            ScaleLine4.IsVisible = true;


            if (rd1.IsChecked == true)
            {
                BarChar.IsVisible = true;
                BarLineX.IsVisible = true;
                BarLineY.IsVisible = true;
                DrawBarChart();
            }          
            else if (rd2.IsChecked == true)
            {
                LineChar.IsVisible = true;
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
            }
              
        }

        private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == NameEntry1)
                LabelName_1.Text = e.NewTextValue;
            else if (sender == NameEntry2)
                LabelName_2.Text = e.NewTextValue;
            else if (sender == NameEntry3)
                LabelName_3.Text = e.NewTextValue;
            else if (sender == NameEntry4)
                LabelName_4.Text = e.NewTextValue;
        }

        private void ValueEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry textBox = (Entry)sender;
            if (!String.IsNullOrEmpty(textBox.Text))
            {
                values[Convert.ToInt32(textBox.StyleId) - 1] = Convert.ToDouble(textBox.Text);
                DrawBarChart();
                DrawPointChart();
                DrawPieChar();
            }
            else
            {
                values[Convert.ToInt32(textBox.StyleId) - 1] = 1;
                DrawBarChart();
                DrawPointChart();
                DrawPieChar();
            }
        }
    }
}
