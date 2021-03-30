using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GpsGate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            int highest = 0;

            foreach (FrameworkElement x in mainCanvas.Children.OfType<Button>())
            {
                int.TryParse(x.GetValue(Canvas.TopProperty).ToString(), out int result);

                highest = result > highest ? result : highest;
            }

            mainCanvas.Height = highest + 50 >= mainCanvas.Height ? mainCanvas.Height + 50 : mainCanvas.Height;

            Button buttonLeft = new Button { Content = "o", Height = 25, Width = 25 };
            Button buttonRight = new Button { Content = "x", Height = 25, Width = 25 };

            buttonLeft.Click += new RoutedEventHandler(LeftButtonsLogic);
            buttonRight.Click += new RoutedEventHandler(RightButtonsLogic);

            mainCanvas.Children.Add(buttonLeft);
            mainCanvas.Children.Add(buttonRight);
            Canvas.SetLeft(buttonLeft, 10); //X
            Canvas.SetTop(buttonLeft, highest + 50); //Y
            Canvas.SetRight(buttonRight, 40); //X
            Canvas.SetTop(buttonRight, highest + 50); //Y

            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "x").ToList().ForEach(x => x.IsEnabled = false);
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            var lines = mainCanvas.Children.OfType<Line>().ToList();
            foreach (var line in lines)
            {
                mainCanvas.Children.Remove(line);
            }

            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "o").ToList().ForEach(x => x.IsEnabled = true);
            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "x").ToList().ForEach(x => x.IsEnabled = false);
        }

        private void LeftButtonsLogic(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "o").ToList().ForEach(x => x.IsEnabled = false);
            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "x").ToList().ForEach(x => x.IsEnabled = true);

            var btn = sender as Button;
            btn.IsEnabled = true;
            int.TryParse(btn.GetValue(Canvas.TopProperty).ToString(), out int position);
        }

        private void RightButtonsLogic(object sender, RoutedEventArgs e)
        {
            var fromBtn = mainCanvas.Children.OfType<Button>().FirstOrDefault(x => x.Content.ToString() == "o" && x.IsEnabled);
            int.TryParse(fromBtn.GetValue(Canvas.TopProperty).ToString(), out int fromPosition);

            var toBtn = sender as Button;
            int.TryParse(toBtn.GetValue(Canvas.TopProperty).ToString(), out int toPosition);

            SolidColorBrush redBrush = new SolidColorBrush
            {
                Color = Colors.Red
            };

            var allLines = mainCanvas.Children.OfType<Line>().ToList();

            if (allLines.Count > 0)
            {
                bool isOverlapping = false;

                foreach (var line in allLines)
                {
                    var a = line.Y1;
                    var b = line.Y2;

                    if (fromPosition <= line.Y1 && toPosition >= line.Y2)
                    {
                        isOverlapping = true;
                    }

                    if (fromPosition >= line.Y1 && toPosition <= line.Y2)
                    {
                        isOverlapping = true;
                    }
                }

                if (isOverlapping)
                {
                    MessageBoxResult result = MessageBox.Show("Can not overlap", "Warning");
                    mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "o").ToList().ForEach(x => x.IsEnabled = true);
                    mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "x").ToList().ForEach(x => x.IsEnabled = false);
                    return;
                }
            }

            Line newLine = new Line { X1 = 35, X2 = 735, Y1 = fromPosition + 15, Y2 = toPosition + 15, StrokeThickness = 1, Stroke = redBrush };
            mainCanvas.Children.Add(newLine);

            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "o").ToList().ForEach(x => x.IsEnabled = true);
            mainCanvas.Children.OfType<Button>().Where(x => x.Content.ToString() == "x").ToList().ForEach(x => x.IsEnabled = false);
        }
    }
}
