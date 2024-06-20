using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Отримуємо значення з textBox1 - textBox24
            int[] values = new int[24];
            for (int i = 0; i < 24; i++)
            {
                if (!int.TryParse(Controls["textBox" + (i + 1)].Text, out values[i]))
                {
                    MessageBox.Show("Введіть коректні числа у всі поля!");
                    return;
                }
            }

            // Розрахунки
            double averageAllPoints = values.Average();
            double averageUpperExtremities = (values[0] + values[1] + values[2] + values[3] + values[4] + values[5] + 
                values[6] + values[7] + values[8] + values[9] + values[10] + values[11]) / 12;
            double averageLowerExtremities = (values[12] + values[13] + values[14] + values[15] + values[16] + values[17] +
                values[18] + values[19] + values[20] + values[21] + values[22] + values[23]) / 12;
            double averageRightSide = (values[6] + values[7] + values[8] + values[9] + values[10] + values[11] +
                values[18] + values[19] + values[20] + values[21] + values[22] + values[23]) / 12;
            double averageLeftSide = (values[0] + values[1] + values[2] + values[3] + values[4] + values[5] +
                values[12] + values[13] + values[14] + values[15] + values[16] + values[17]) / 12;           
            double deviationAllPoints = Math.Sqrt(values.Select(x => Math.Pow(x - averageAllPoints, 2)).Average());
            double physiologicalCorridor = 2 * deviationAllPoints;
            double maxNormalValue = averageAllPoints + physiologicalCorridor;
            double minNormalValue = averageAllPoints - physiologicalCorridor;

            // Розрахунок кількості точок, які перевищують норму
            int countAboveNorm = values.Count(x => x >= averageAllPoints);

            // Розрахунок кількості точок, які менше норми
            int countBelowNorm = values.Count(x => x <= averageAllPoints);

            // Розрахунок коефіцієнта переважання
            double dominanceCoefficient;
            if (countAboveNorm >= countBelowNorm)
            {
                dominanceCoefficient = countAboveNorm / (double)countBelowNorm;
            }
            else
            {
                dominanceCoefficient = countBelowNorm / (double)countAboveNorm;
            }

            double uniformityDistribution = values.Max() - values.Min();
            double lateralAsymmetry = (averageRightSide / averageLeftSide) * 100;
            double transverseAsymmetry = (averageUpperExtremities / averageLowerExtremities) * 100;

            // Округлення результатів до двох знаків після коми
            averageAllPoints = Math.Round(averageAllPoints, 2);
            averageUpperExtremities = Math.Round(averageUpperExtremities, 2);
            averageLowerExtremities = Math.Round(averageLowerExtremities, 2);
            averageRightSide = Math.Round(averageRightSide, 2);
            averageLeftSide = Math.Round(averageLeftSide, 2);
            deviationAllPoints = Math.Round(deviationAllPoints, 2);
            physiologicalCorridor = Math.Round(physiologicalCorridor, 2);
            maxNormalValue = Math.Round(maxNormalValue, 2);
            minNormalValue = Math.Round(minNormalValue, 2);
            dominanceCoefficient = Math.Round(dominanceCoefficient, 2);
            uniformityDistribution = Math.Round(uniformityDistribution, 2);
            lateralAsymmetry = Math.Round(lateralAsymmetry, 2);
            transverseAsymmetry = Math.Round(transverseAsymmetry, 2);

            // Виведення результатів
            label35.Text = averageAllPoints.ToString();
            label36.Text = averageUpperExtremities.ToString();
            label37.Text = averageLowerExtremities.ToString();
            label38.Text = averageRightSide.ToString();
            label39.Text = averageLeftSide.ToString();
            label40.Text = string.Format("Від {0} до {1}", minNormalValue, maxNormalValue);
            label41.Text = dominanceCoefficient.ToString();
            label42.Text = uniformityDistribution.ToString();
            label43.Text = string.Format("{0}%", lateralAsymmetry);
            label44.Text = string.Format("{0}%", transverseAsymmetry);

            // Створити серії даних
            var seriesPoints = new Series("Точки");
            var seriesMaxNormalValue = new Series("Максимальне значення");
            var seriesMinNormalValue = new Series("Мінімальне значення");

            // Додати дані до серій
            for (int i = 0; i < 24; i++)
            {
                seriesPoints.Points.Add(new DataPoint(i + 1, values[i]));
                seriesMaxNormalValue.Points.Add(new DataPoint(i + 1, maxNormalValue));
                seriesMinNormalValue.Points.Add(new DataPoint(i + 1, minNormalValue));
            }

            // Додати серії до графіка
            chart1.Series.Clear();
            chart1.Series.Add(seriesPoints);
            chart1.Series.Add(seriesMaxNormalValue);
            chart1.Series.Add(seriesMinNormalValue);

            // Налаштувати тип діаграми для кожної серії
            seriesPoints.ChartType = SeriesChartType.Polar;
            seriesMaxNormalValue.ChartType = SeriesChartType.Polar;
            seriesMinNormalValue.ChartType = SeriesChartType.Polar;

            // Налаштувати параметри графіка
            chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            chart1.ChartAreas[0].AxisX.Maximum = 24;
            chart1.ChartAreas[0].AxisX.Minimum = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0}";

            // Розмір
            chart1.Width = 550;
            chart1.Height = 485;

            // Відобразити графік
            chart1.Show();

        }
    }
}
