using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Input;
using System;
using System.Collections.Generic;
using Windows.System;
using Microsoft.UI;

namespace TravelAgency
{
    public sealed partial class MainPage : Page
    {
        // --- DATA ---
        private int _currentTestimonialIndex = 0;
        private List<Testimonial> _testimonials = new List<Testimonial>
        {
            new Testimonial { Name = "Mike Taylor", Role = "CEO of Mike Studios", Quote = "“On the Windows talking painted pasture yet its express parties use. Sure last upon he same as knew next. Of believed or diverted no.”", ImagePath = "ms-appx:///Assets/Images/Customer1.png" },
            new Testimonial { Name = "Chris Thomas", Role = "CEO of Red Button", Quote = "“Vodoo provided the best travel experience I have ever had. Seamless booking and amazing support throughout the trip.”", ImagePath = "ms-appx:///Assets/Images/Customer2.png" },
            new Testimonial { Name = "Sarah Jenkins", Role = "Travel Blogger", Quote = "“Finding hidden gems was so easy with Vodoo. Highly recommended for anyone looking for a unique adventure.”", ImagePath = "ms-appx:///Assets/Images/Customer3.png" }
        };

        public MainPage()
        {
            this.InitializeComponent();
        }

        // --- NAVIGATION & SCROLLING ---
        private void OnNavDestinationsClick(object sender, RoutedEventArgs e) => ScrollToSection(DestinationsSection);
        private void OnNavServicesClick(object sender, RoutedEventArgs e) => ScrollToSection(ServicesSection);
        private void OnNavBookingsClick(object sender, RoutedEventArgs e) => ScrollToSection(BookingSection);
        private void OnNavTestimonialsClick(object sender, RoutedEventArgs e) => ScrollToSection(TestimonialsSection);

        private void OnLogoClick(object sender, TappedRoutedEventArgs e)
        {
            MainScrollViewer.ChangeView(null, 0, null, false);
        }

        private void OnFindOutMoreClick(object sender, RoutedEventArgs e) => ScrollToSection(BookingSection);

        private void ScrollToSection(UIElement element)
        {
            try
            {
                var transform = element.TransformToVisual(MainScrollViewer.Content as UIElement);
                var position = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

                // 2. Define a fixed offset.
                // The Sticky Navbar is roughly 60-70px tall. 
                // We use 100px to ensure it clears the navbar AND gives 30px-40px of nice breathing room.
                double safeOffset = 100;

                // 3. Scroll to the calculated position
                // We subtract the offset so the scrolling stops BEFORE the element hits the top of the screen
                MainScrollViewer.ChangeView(null, position.Y - safeOffset, null, false);
            }
            catch { }
        }

        private void MainScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            // Sticky Navbar Logic: Show after scrolling 600px
            if (MainScrollViewer.VerticalOffset > 600)
            {
                if (StickyNavbar.Visibility == Visibility.Collapsed)
                    StickyNavbar.Visibility = Visibility.Visible;
            }
            else
            {
                if (StickyNavbar.Visibility == Visibility.Visible)
                    StickyNavbar.Visibility = Visibility.Collapsed;
            }
        }

        // --- CURSOR LOGIC ---
        private void OnElementPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void OnElementPointerExited(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = null;
        }

        // --- SOCIAL LINKS ---
        private async void OnFacebookClick(object sender, TappedRoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com"));
        private async void OnInstagramClick(object sender, TappedRoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://www.instagram.com"));
        private async void OnTwitterClick(object sender, TappedRoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://www.twitter.com"));

        // --- TESTIMONIALS ---
        private void OnTestimonialUpClick(object sender, TappedRoutedEventArgs e)
        {
            _currentTestimonialIndex--;
            if (_currentTestimonialIndex < 0) _currentTestimonialIndex = _testimonials.Count - 1;
            UpdateTestimonialUI();
        }

        private void OnTestimonialDownClick(object sender, TappedRoutedEventArgs e)
        {
            _currentTestimonialIndex++;
            if (_currentTestimonialIndex >= _testimonials.Count) _currentTestimonialIndex = 0;
            UpdateTestimonialUI();
        }

        private void UpdateTestimonialUI()
        {
            var data = _testimonials[_currentTestimonialIndex];
            TestimonialName.Text = data.Name;
            TestimonialRole.Text = data.Role;
            TestimonialQuote.Text = data.Quote;
            TestimonialImage.ImageSource = new BitmapImage(new Uri(data.ImagePath)); 

            // Update Dots
            Dot1.Fill = _currentTestimonialIndex == 0 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(ColorHelper.FromArgb(255, 229, 229, 229));
            Dot2.Fill = _currentTestimonialIndex == 1 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(ColorHelper.FromArgb(255, 229, 229, 229));
            Dot3.Fill = _currentTestimonialIndex == 2 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(ColorHelper.FromArgb(255, 229, 229, 229));
        }

        // --- CONFETTI ANIMATION ---
        private void OnSubscribeClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EmailInput.Text))
            {
                EmailInput.Text = ""; // Clear input
                SpawnConfetti();
            }
        }

        private void SpawnConfetti()
        {
            Random rand = new Random();

            // UPDATED COLORS: Matches website theme (Light, Soft)
            var colors = new SolidColorBrush[] {
                new SolidColorBrush(ColorHelper.FromArgb(255, 223, 105, 81)),  // Theme Orange (#DF6951)
                new SolidColorBrush(ColorHelper.FromArgb(255, 241, 165, 1)),   // Theme Yellow (#F1A501)
                new SolidColorBrush(ColorHelper.FromArgb(255, 223, 215, 249)), // Light Purple (#DFD7F9)
                new SolidColorBrush(ColorHelper.FromArgb(255, 137, 207, 240)), // Light Blue (Baby Blue)
                new SolidColorBrush(ColorHelper.FromArgb(255, 255, 241, 218))  // Light Cream
            };

            // Create 80 confetti particles
            for (int i = 0; i < 180; i++)
            {
                // SHAPES: Rectangles of varying aspect ratios (Squares, Strips)
                Rectangle particle = new Rectangle
                {
                    Width = rand.Next(8, 16),  // Random width
                    Height = rand.Next(5, 12), // Random height
                    Fill = colors[rand.Next(0, colors.Length)]
                };

                // Set Transform Group for moving and rotating
                particle.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                var group = new TransformGroup();
                var translate = new TranslateTransform();
                var rotate = new RotateTransform();
                group.Children.Add(rotate);
                group.Children.Add(translate);
                particle.RenderTransform = group;

                // Start position: Top of the screen (Y = -20), Random X across the screen width
                double startX = rand.NextDouble() * this.ActualWidth;
                double startY = -20; 

                Canvas.SetLeft(particle, startX);
                Canvas.SetTop(particle, startY);

                ConfettiCanvas.Children.Add(particle);

                // Animation 1: Fall Down
                DoubleAnimation fallAnim = new DoubleAnimation
                {
                    From = startY,
                    To = this.ActualHeight + 50, // Fall off the bottom of screen
                    Duration = TimeSpan.FromSeconds(2 + rand.NextDouble() * 2.7), // Random speed between 2s and 4.5s
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(fallAnim, translate);
                Storyboard.SetTargetProperty(fallAnim, "Y");

                // Animation 2: Rotate (Spin)
                DoubleAnimation rotateAnim = new DoubleAnimation
                {
                    From = 0,
                    To = rand.Next(180, 720) * (rand.Next(0, 2) == 0 ? 1 : -1), // Spin random amount, left or right
                    Duration = fallAnim.Duration
                };
                Storyboard.SetTarget(rotateAnim, rotate);
                Storyboard.SetTargetProperty(rotateAnim, "Angle");

                // Run Storyboard
                Storyboard sb = new Storyboard();
                sb.Children.Add(fallAnim);
                sb.Children.Add(rotateAnim);
                
                // Cleanup when done
                sb.Completed += (s, ev) => ConfettiCanvas.Children.Remove(particle);
                sb.Begin();
            }
        }
    }

    public class Testimonial
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Quote { get; set; }
        public string ImagePath { get; set; }
    }
}
