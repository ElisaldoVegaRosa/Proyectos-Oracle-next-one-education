using System;
using System.Windows.Forms;

namespace Calculadora
{
    public class CalculatorForm : Form
    {
        private TextBox display;
        private double currentValue = 0;
        private string currentOp = "";
        private bool newEntry = true;

        public CalculatorForm()
        {
            this.Text = "Calculadora";
            this.Width = 250;
            this.Height = 300;
            InitComponents();
        }

        private void InitComponents()
        {
            display = new TextBox();
            display.Dock = DockStyle.Top;
            display.ReadOnly = true;
            display.TextAlign = HorizontalAlignment.Right;
            display.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.Controls.Add(display);

            var panel = new TableLayoutPanel();
            panel.RowCount = 5;
            panel.ColumnCount = 4;
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);

            string[] buttons = new string[]
            {
                "7","8","9","/",
                "4","5","6","*",
                "1","2","3","-",
                "0",".","=","+",
                "C"
            };

            int index = 0;
            foreach (var text in buttons)
            {
                var btn = new Button();
                btn.Text = text;
                btn.Dock = DockStyle.Fill;
                btn.Font = new System.Drawing.Font("Segoe UI", 14F);
                btn.Click += ButtonClick;
                if(text == "C")
                {
                    panel.Controls.Add(btn, 0, 4);
                    panel.SetColumnSpan(btn, 4);
                }
                else
                {
                    panel.Controls.Add(btn, index % 4, index / 4);
                    index++;
                }
            }
        }

        private void ButtonClick(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if(btn == null) return;

            string text = btn.Text;

            if (double.TryParse(text, out _) || text == ".")
            {
                if (newEntry) display.Text = "";
                display.Text += text;
                newEntry = false;
            }
            else if (text == "C")
            {
                currentValue = 0;
                currentOp = "";
                display.Text = "";
            }
            else if (text == "=")
            {
                double value = 0;
                double.TryParse(display.Text, out value);
                Calculate(value);
                currentOp = "";
                display.Text = currentValue.ToString();
                newEntry = true;
            }
            else // operators
            {
                double value = 0;
                double.TryParse(display.Text, out value);
                if (!string.IsNullOrEmpty(currentOp))
                {
                    Calculate(value);
                    display.Text = currentValue.ToString();
                }
                else
                {
                    currentValue = value;
                }
                currentOp = text;
                newEntry = true;
            }
        }

        private void Calculate(double value)
        {
            switch (currentOp)
            {
                case "+": currentValue += value; break;
                case "-": currentValue -= value; break;
                case "*": currentValue *= value; break;
                case "/": currentValue = value != 0 ? currentValue / value : 0; break;
                default: currentValue = value; break;
            }
        }
    }
}
