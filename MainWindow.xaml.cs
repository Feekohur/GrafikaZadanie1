using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace GrafikaZadanie1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point();
        string option = "";
        Line straightLine;
        TextBox textBox;
        Point textBoxPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (option == "linia")
            {
                straightLine = new Line();
                straightLine.Stroke = SystemColors.WindowFrameBrush;
                straightLine.X1 = e.GetPosition(this).X;
                straightLine.Y1 = e.GetPosition(this).Y;
            }
            else if(option == "rysowanie")
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    currentPoint = e.GetPosition(this);
            }
            else if(option == "tekst")
            {
                textBoxPoint.X = e.GetPosition(this).X;
                textBoxPoint.Y = e.GetPosition(this).Y;
                textBox = new TextBox();
                Canvas.SetLeft(textBox, textBoxPoint.X);
                Canvas.SetTop(textBox, textBoxPoint.Y);
                canvas.Children.Add(textBox);
            }
            
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(option == "rysowanie")
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();

                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.X1 = currentPoint.X;
                    line.Y1 = currentPoint.Y;
                    line.X2 = e.GetPosition(this).X;
                    line.Y2 = e.GetPosition(this).Y;

                    currentPoint = e.GetPosition(this);

                    canvas.Children.Add(line);
                }
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(option == "linia")
            {
                straightLine.X2 = e.GetPosition(this).X;
                straightLine.Y2 = e.GetPosition(this).Y;

                canvas.Children.Add(straightLine);
                //straightLine = new Line();
            }
        }

        private void Tekst(object sender, RoutedEventArgs e)
        {
            option = "tekst";
        }

        private void Linia(object sender, RoutedEventArgs e)
        {
            option = "linia";
        }

        private void Rysowanie(object sender, RoutedEventArgs e)
        {
            option = "rysowanie";
        }

        private void Zapisz(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image file (*.jpg)|*.jpg";
            if (saveFileDialog.ShowDialog() == true)
            {
                Rect rectangle = new Rect(canvas.RenderSize);
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)rectangle.Right,
                  (int)rectangle.Bottom, 96d, 96d, PixelFormats.Default);
                renderTargetBitmap.Render(canvas);
                BitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                MemoryStream memoryStream = new MemoryStream();

                bitmapEncoder.Save(memoryStream);
                memoryStream.Close();
                File.WriteAllBytes(saveFileDialog.FileName, memoryStream.ToArray());
            }
                
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(option == "tekst")
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = textBox.Text;
                    textBox.Text = "";
                    Canvas.SetLeft(textBlock, textBoxPoint.X);
                    Canvas.SetTop(textBlock, textBoxPoint.Y);
                    canvas.Children.Remove(textBox);
                    canvas.Children.Add(textBlock);
                }
            }
        }
    }
}

