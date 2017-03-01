using System;
using System.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.IO.Ports;

namespace Temperature
{
    internal class SerialReader
    {
        private ComboBox ports;
        private Ellipse status;
        private Label temperature;
        private SerialPort port = null;
        string newPort = null;
        private SolidColorBrush OK_BRUSH = new SolidColorBrush(Color.FromRgb(89, 241, 18));
        private SolidColorBrush BAD_BRUSH = new SolidColorBrush(Color.FromRgb(243, 70, 70));

        public SerialReader(ComboBox ports, Label temperature, Ellipse status)
        {
            this.ports = ports;
            this.temperature = temperature;
            this.status = status;
            var thread = new Thread(this.run);
            thread.IsBackground = true;
            thread.Start();
        }

        private void run()
        {
            while (true)
            {
                ports.Dispatcher.BeginInvoke((Action)(() =>
                {
                    newPort = (string)ports.SelectedItem;
                }));

                if (checkPort())
                {
                    var tmp = port.ReadLine();
                    if (tmp.StartsWith("Temperature = "))
                        temperature.Dispatcher.BeginInvoke((Action)(() => { temperature.Content = tmp.TrimEnd().Replace("Temperature = ", "") + " °C";  }));
                }

                Thread.Sleep(1000);
            }
        }

        private bool checkPort()
        {
            // new port
            if (port == null && newPort != null)
            {
                port = new SerialPort(newPort);
                port.BaudRate = 9600;
            }

            if (port != null)
            {
                if (!port.IsOpen)
                    try { port.Open(); }
                    catch (System.IO.IOException e) { }

                status.Dispatcher.BeginInvoke((Action)(() => { status.Fill = (port.IsOpen) ? OK_BRUSH : BAD_BRUSH; }));
            }

            return port != null && port.IsOpen;
        }
    }
}