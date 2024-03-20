using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        private double _offsetX;
        private double _offsetY;
        private Image _image1;
        private Image _image2;

        public MainWindow()
        {
            InitializeComponent();
            _image1 = this.FindControl<Image>("logo1");
            _image2 = this.FindControl<Image>("logo2");
            _image1.PointerPressed += Image_PointerPressed;
            _image2.PointerPressed += Image_PointerPressed;
            _image1.PointerWheelChanged += Image_PointerWheelChanged;
            _image2.PointerWheelChanged += Image_PointerWheelChanged;
            Trace.WriteLine("Initialization ended!");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

        }

        private void Image_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            var image = (Image)sender;
            var clickPosition = e.GetPosition(image);
            _offsetX = clickPosition.X;
            _offsetY = clickPosition.Y;

            if (e.ClickCount == 2) //Stupido?
            {
                Image_DoubleClicked(image);
            }
            else
            {
                image.PointerMoved += Image_PointerMoved;
                image.PointerReleased += Image_PointerReleased;
            }
        }

        private void Image_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            var image = (Image)sender;
            image.PointerMoved -= Image_PointerMoved;
            image.PointerReleased -= Image_PointerReleased;
        }

        private void Image_PointerMoved(object sender, PointerEventArgs e)
        {
            var image = (Image)sender;
            var position = e.GetPosition(this);
            //Trace.WriteLine($"X: {position.X}, Y: {position.Y}");
            Canvas.SetLeft(image, position.X - _offsetX);
            Canvas.SetTop(image, position.Y - _offsetY);
        }

        private void Image_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            var image = (Image)sender;
            var scale = e.Delta.Y > 0 ? 1.1 : 0.9;
            image.Width *= scale;
            image.Height *= scale;
        }

        private void Image_DoubleClicked(Image image)
        {
            var newImage = new Image
            {
                Source = image.Source,
                Width = image.Width,
                Height = image.Height,
        };
            newImage.PointerPressed += Image_PointerPressed;
            newImage.PointerWheelChanged += Image_PointerWheelChanged;
            var content = this.Content as Panel;
            content.Children.Add(newImage);
        }
    }
}
