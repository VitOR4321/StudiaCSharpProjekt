using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wiktor_Górski_projekt_sem2
{
    public partial class Form2 : Form
    {
        int ile_grup;//zmienna do przechowania ilości grup
        string zawartosc;//zmienna do przechowania danych do wykresu
        public Form2()
        {
            InitializeComponent();
        }
        private void chart_w()
        {
            ile_grup = Form1.grupy;//pobranie danych
            zawartosc = Form1.string_p;//pobranie danych
            string[] wynik_s = zawartosc.Split(';');
            wynik_s = wynik_s.Take(wynik_s.Count() - 1).ToArray();
            //ustawienie danych na wykresie
            for (int i = 0; i < ile_grup * 2; i += 2)
            {
                chart1.Series["Ocena"].Points.AddXY(wynik_s[i], wynik_s[i + 1]);
            }
            chart1.Titles.Add("Histogram ocen");//nadanie tytułu
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //ustawienie kolorów i uruchomienie ustawien wykresu
            chart_w();
            comboBox1.SelectedIndex = Form1.kolor;
            checkBox1.Checked = Form1.serie;
            checkBox2.Checked = Form1.legenda;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //wyświetlanie kolumn na wykresie i pobieranie kolorów 
            if (chart1.Series["Ocena"].Color != Color.Transparent)
            {
                chart1.Series["Ocena"].Color = Color.Transparent;
                comboBox1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                comboBox1.Enabled = true;
                chart1.Series["Ocena"].Color = Color.FromArgb(255, 0, 128, 255);
                Form1.kolor = 0;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                comboBox1.Enabled = true;
                chart1.Series["Ocena"].Color = Color.FromArgb(255, 0, 255, 0);
                Form1.kolor = 1;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                comboBox1.Enabled = true;
                chart1.Series["Ocena"].Color = Color.FromArgb(255, 255, 0, 0);
                Form1.kolor = 2;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //opdalanie legendy
            {
                if (chart1.Legends["Legend1"].Enabled == true) chart1.Legends["Legend1"].Enabled = false;
                else chart1.Legends["Legend1"].Enabled = true;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //zmiana koloru słupków na wykresie
            if (comboBox1.SelectedIndex == 0) chart1.Series["Ocena"].Color = Color.FromArgb(255, 0, 128, 255);//niebieski
            if (comboBox1.SelectedIndex == 1) chart1.Series["Ocena"].Color = Color.FromArgb(255, 0, 255, 0);//zielony
            if (comboBox1.SelectedIndex == 2) chart1.Series["Ocena"].Color = Color.FromArgb(255, 255, 0, 0);//czerwony
        }
        private void Form2_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            //zapisanie opcji histogramu 
            Form1.kolor = comboBox1.SelectedIndex;
            Form1.serie = checkBox1.Checked;
            Form1.legenda = checkBox2.Checked;
        }
    }
}