using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MARSH_App
{
    public partial class Form1 : Form
    {
        private ListBox listBox;
        private Button generateButton;
        private Button readButton;
        private Button searchButton;
        private TextBox routeTextBox;
        private Label routeLabel;

        public Form1()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            // Инициализация элементов управления
            listBox = new ListBox { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(760, 364) };

            generateButton = new Button
            {
                Location = new System.Drawing.Point(10, 380),
                Size = new System.Drawing.Size(180, 30),
                Text = "Generate and Save Routes"
            };

            readButton = new Button
            {
                Location = new System.Drawing.Point(200, 380),
                Size = new System.Drawing.Size(180, 30),
                Text = "Read and Display Routes"
            };

            searchButton = new Button
            {
                Location = new System.Drawing.Point(460, 380),
                Size = new System.Drawing.Size(180, 30),
                Text = "Search by Route Number"
            };

            routeTextBox = new TextBox
            {
                Location = new System.Drawing.Point(650, 380),
                Size = new System.Drawing.Size(60, 27)
            };

            routeLabel = new Label
            {
                Location = new System.Drawing.Point(560, 383),
                Text = "Route №:",
                AutoSize = true
            };

            // Настройка формы
            Controls.AddRange(new Control[] { listBox, generateButton, readButton, searchButton, routeTextBox, routeLabel });
            Text = "Route Manager";
            ClientSize = new System.Drawing.Size(780, 420);

          
            generateButton.Click += GenerateButton_Click;
            readButton.Click += ReadButton_Click;
            searchButton.Click += SearchButton_Click;
        }

        private class MARSH
        {
            public string StartPoint { get; set; }
            public string EndPoint { get; set; }
            public int RouteNumber { get; set; }
        }

        private List<MARSH> GenerateMarshes()
        {
            Random rand = new Random();
            string[] cities = { "Moscow", "St.Petersburg", "Novosibirsk", "Yekaterinburg", "Kazan", "Nizhny Novgorod", "Chelyabinsk", "Samara", "Omsk", "Rostov" };

            return Enumerable.Range(1, 10)
                .Select(i => new MARSH
                {
                    StartPoint = cities[rand.Next(cities.Length)],
                    EndPoint = cities[rand.Next(cities.Length)],
                    RouteNumber = rand.Next(1, 10)
                })
                .OrderBy(m => m.RouteNumber)
                .ToList();
        }

        private void SaveMarshesToFile(List<MARSH> marshes)
        {
            File.WriteAllLines("routes.txt",
                marshes.Select(m => $"{m.StartPoint};{m.EndPoint};{m.RouteNumber}"));
        }

        private List<MARSH> ReadMarshesFromFile()
        {
            if (!File.Exists("routes.txt")) return new List<MARSH>();

            return File.ReadAllLines("routes.txt")
                .Select(line => line.Split(';'))
                .Where(parts => parts.Length == 3)
                .Select(parts => new MARSH
                {
                    StartPoint = parts[0],
                    EndPoint = parts[1],
                    RouteNumber = int.Parse(parts[2])
                })
                .OrderBy(m => m.RouteNumber)
                .ToList();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            var marshes = GenerateMarshes();
            SaveMarshesToFile(marshes);
            MessageBox.Show("10 routes generated and saved!");
        }

артем, [21.04.2025 17:21]
private void ReadButton_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            var marshes = ReadMarshesFromFile();
            foreach (var m in marshes)
            {
                listBox.Items.Add($"{m.RouteNumber}: {m.StartPoint} -> {m.EndPoint}");
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(routeTextBox.Text, out int routeNumber))
            {
                MessageBox.Show("Invalid route number!");
                return;
            }

            var marshes = ReadMarshesFromFile();
            var found = marshes.FirstOrDefault(m => m.RouteNumber == routeNumber);

            listBox.Items.Clear();
            if (found != null)
            {
                listBox.Items.Add($"Found route: {found.StartPoint} -> {found.EndPoint}, №{found.RouteNumber}");
            }
            else
            {
                MessageBox.Show("Route not found!");
            }
        }

        [STAThread]
        static void Base()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
