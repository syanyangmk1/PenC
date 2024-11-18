using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI;

namespace App6
{

    public sealed partial class MainWindow : Window
    {
        private List<float> vx = new List<float>();
        private List<float> vy = new List<float>();
        private List<Color> col = new List<Color>();
        private List<float> size = new List<float>();

        // Variables to manage pointer events and drawing state
        private bool flag = false;
        private float px = 100;
        private float py = 100;
        private float mySize = 16;
        private Color myCol = Colors.Green;
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            vx.Clear();
            vy.Clear();
            col.Clear();
            size.Clear();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Save to a fixed file path (you can modify this to use a file picker as well)
            try
            {
                string path = @"C:\Users\user\source\repos\App6.txt";
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < vx.Count; i++)
                    {
                        writer.WriteLine($"{vx[i]} {vy[i]} {col[i].R} {col[i].G} {col[i].B} {col[i].A} {size[i]}");
                    }
                }
                //MessageBox.Show("The drawing was saved successfully.", "Save Successful", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = @"C:\Users\user\source\repos\App6.txt";
                using (StreamReader reader = new StreamReader(path))
                {
                    vx.Clear();
                    vy.Clear();
                    col.Clear();
                    size.Clear();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(' ');
                        float x = float.Parse(parts[0]);
                        float y = float.Parse(parts[1]);
                        byte r = byte.Parse(parts[2]);
                        byte g = byte.Parse(parts[3]);
                        byte b = byte.Parse(parts[4]);
                        byte a = byte.Parse(parts[5]);
                        float s = float.Parse(parts[6]);

                        vx.Add(x);
                        vy.Add(y);
                        col.Add(Color.FromArgb(a, r, g, b));
                        size.Add(s);
                    }
                    CanvasControl_PointerReleased(null, null); // Redraw
                }
               // MessageBox.Show("The drawing was loaded successfully.", "Load Successful", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mySize = (float)e.NewValue;
        }

        private async void myWrite_Click(object sender, RoutedEventArgs e)
        {
            // Open file for writing
            var picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.SuggestedFileName = "drawing";
            picker.FileTypeChoices.Add("Text Files", new List<string>() { ".txt" });

            var file =  await picker.PickSaveFileAsync();
            if (file != null)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
                    {
                        writer.WriteLine(vx.Count);
                        for (int i = 0; i < vx.Count; i++)
                        {
                            writer.WriteLine($"{vx[i]} {vy[i]} {col[i].A} {col[i].B} {col[i].G} {col[i].R} {size[i]}");
                        }
                    }
                    //MessageBox.Show("The file was saved successfully.", "Save Successful", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
            else
            {
              //  MessageBox.Show("The file was not saved.", "Save Cancelled", MessageBoxButton.OK);
            }
        }

        private async void myRead_Click(object sender, RoutedEventArgs e)
        {
            // Open file for reading
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".txt");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
                    {
                        vx.Clear();
                        vy.Clear();
                        col.Clear();
                        size.Clear();

                        int num = int.Parse(reader.ReadLine());
                        for (int i = 0; i < num; i++)
                        {
                            var line = reader.ReadLine();
                            var parts = line.Split(' ');
                            float x = float.Parse(parts[0]);
                            float y = float.Parse(parts[1]);
                            byte a = byte.Parse(parts[2]);
                            byte b = byte.Parse(parts[3]);
                            byte g = byte.Parse(parts[4]);
                            byte r = byte.Parse(parts[5]);
                            float s = float.Parse(parts[6]);

                            vx.Add(x);
                            vy.Add(y);
                            col.Add(Color.FromArgb(a, r, g, b));
                            size.Add(s);
                        }
                    }
                    CanvasControl_PointerReleased(null, null);  // Ensure the canvas is redrawn
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
            else
            {
              //  MessageBox.Show("The file was not opened.", "Open Cancelled", MessageBoxButton.OK);
            }
        }

        private void myClear_Click(object sender, RoutedEventArgs e)
        {
            vx.Clear();
            vy.Clear();
            col.Clear();
            size.Clear();
            flag = false;
            px = 100;
            py = 100;
            mySize = 16;

        }

        private void CanvasControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

        }

        private void CanvasControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            flag = true;
        }

        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            flag = false;
            px = py = 0.0f;
            vx.Add(px);
            vy.Add(py);
            col.Add(myCol);
            size.Add(mySize);
        }

        private void CanvasControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var canvas = sender as CanvasControl;
            var position = e.GetCurrentPoint(canvas).Position;
            px = (float)position.X;
            py = (float)position.Y;

            if (flag)
            {
                vx.Add(px);
                vy.Add(py);
                col.Add(myCol);
                size.Add(mySize);
                canvas.Invalidate();
            }
        }

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            int n = vx.Count;
            if (n <= 2) return;

            for (int i = 1; i < n; i++)
            {
                if (vx[i] == 0.0f && vy[i] == 0.0f)
                {
                    i++;
                    continue;
                }

                // Draw line and circles
                args.DrawingSession.DrawLine(vx[i - 1], vy[i - 1], vx[i], vy[i], new Windows.UI.Color() { R = col[i].R, G = col[i].G, B = col[i].B, A = col[i].A }, size[i]);
                args.DrawingSession.FillCircle(vx[i - 1], vy[i - 1], size[i] / 2, new Windows.UI.Color() { R = col[i].R, G = col[i].G, B = col[i].B, A = col[i].A });
                args.DrawingSession.FillCircle(vx[i], vy[i], size[i] / 2, new Windows.UI.Color() { R = col[i].R, G = col[i].G, B = col[i].B, A = col[i].A });
            }
        }

        private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            myCol = args.NewColor;
        }
    }
}
