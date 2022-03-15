using System;
using System.IO;
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
    public partial class Form1 : Form
    {
        public static int grupy = 0;//ilość grup
        public static int kolor = 0;//zapamiętany kolor wykresu
        public static int najwyzsza_o;//najwyższa grupa ocen
        public static int najnizsza_o;//najniższa grupa ocen
        public static bool legenda = true;//zapamiętanie odpalonej legendy
        public static bool serie = true;//zapamiętanie widocznych słupków wykresu
        public static string string_p = "";//wypisanie danych w okienkach
        public Form1()
        {
            InitializeComponent();
        }
        OpenFileDialog ofd = new OpenFileDialog();// kod potrzebny do wywołania akcji otwierania i wyszukania pliku
        private void button1_Click_1(object sender, EventArgs e)
        {
            ofd.Filter = "TXT|*.txt";//filtr który pokazuje tylko rozszerzenia txt
            if (ofd.ShowDialog() == DialogResult.OK)//po kliknięciu "OK" program urochumi poniższy kod
            {
                textBox1.Text = ofd.FileName;//pokazanie ścieżki do pliku
                string sciezka = ofd.FileName;//przypisanie ścieżki pliku do zmiennej
                string zawartosc_pliku = File.ReadAllText(@sciezka);//wpisanie od zmiennej całej zawartości pliku
                textBox2.Text = zawartosc_pliku; //wyświetla zawartość plików textBoxie
                //umożliwia wciskanie przycisków wcześniej zablokowanych dla użytkownika oraz czyści zawartość pól po zmianie pliku 
                button2.Enabled = true;
                button3.Enabled = true;
                richTextBox1.Clear();
                textBox3.Clear();
                string_p = "";
                string[] wyniki_s = zawartosc_pliku.Split(';');//tworzy tablice typu string z danymi z pliku rozdzielonymi znakiem ";"
                wyniki_s = wyniki_s.Skip(1).ToArray();//Pomija pierwszą linijkę
                wyniki_s = wyniki_s.Take(wyniki_s.Count() - 1).ToArray();//Usuwa słowo "Oceny"
                //Usuwa znak nowej lini i powrotu z tablicy
                for (int i = 0; i < wyniki_s.Length; i++)
                {
                    wyniki_s[i] = wyniki_s[i].Replace(Environment.NewLine, string.Empty);
                }
                double[] wyniki_d = Array.ConvertAll(wyniki_s, new Converter<string, double>(Double.Parse));//Tworzenie tablicy typu double i zmiana tablicy typu string na double
                //Sprawdza czy dane sa w dobrym formacie 
                bool prawdziwe = true;
                try
                {
                    najwyzsza_o = (int)Math.Ceiling(wyniki_d[0]);
                    najnizsza_o = (int)Math.Floor(wyniki_d[0]);
                }
                catch
                {
                    prawdziwe = false;
                }
                if (prawdziwe==true)
                {
                    //Wyszukiwanie najwyższej i najniższej oceny
                    for (int i = 0; i < wyniki_d.Length; i++)
                    {
                        if (najwyzsza_o < (int)Math.Ceiling(wyniki_d[i]))
                        {
                            najwyzsza_o = (int)Math.Ceiling(wyniki_d[i]);
                        }
                        if (najnizsza_o > (int)Math.Floor(wyniki_d[i]))
                        {
                            najnizsza_o = (int)Math.Floor(wyniki_d[i]);
                        }
                    }
                    grupy = najwyzsza_o - najnizsza_o;//liczy ilość potrzebnych grup i tworzy tablice
                    int[] ile_w = new int[grupy];
                    textBox3.Text = grupy.ToString(); //pokazuje ilość wyliczonych grup
                    int j = 0;
                    //tworzy przedziały ocen i liczy ile jest w danym przedziale ocen
                    foreach (var wynik in wyniki_d)
                    {
                        j = 0;
                        for (int i = najnizsza_o + 1; i <= najwyzsza_o; i++)
                        {
                            if (wynik > i - 1 && wynik <= i)
                            {
                                ile_w[j]++;
                            }
                            else if (wynik == 0) ile_w[0]++;
                            j++;
                        }
                    }
                    j = najnizsza_o + 1;
                    string_p = "";
                    //Tworzenie rozkładu wyniku, który zostanie przekazany do wykresu
                    foreach (var wynik in ile_w)
                    {
                        richTextBox1.AppendText(j - 1 + "-" + j + ": " + wynik + Environment.NewLine);
                        string_p += j - 1 + "-" + j + ";" + wynik + ";";
                        j++;
                    }
                    MessageBox.Show("Wczytano dane!", "Komunikat programu");//Komunikat
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Sprawdzam czy pole tekstowe zawierające liczbę grup nie jest puste, następnie otwiera panel z histogram
            if (string.IsNullOrWhiteSpace(textBox3.Text)) MessageBox.Show("Program wymaga wprowadzenia poprawny dany", "Komunikat programu");
            else
            {
                Form2 otworzHistogram = new Form2();
                otworzHistogram.ShowDialog();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //Sprawdzam czy pole tekstowe zawierające liczbę grup nie jest puste, następnie tworzy plik w którym zapisywany jest wynik 
            if (string.IsNullOrWhiteSpace(textBox3.Text)) MessageBox.Show("Program wymaga wprowadzenia poprawny dany", "Komunikat programu");
            else
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "eksport_wykresu.txt")))
                {
                    string wynik = richTextBox1.Text;
                    outputFile.WriteLine("Liczba grup: " + textBox3.Text + Environment.NewLine + "Rozkład: " + Environment.NewLine + wynik);
                }
                MessageBox.Show("Poprawny eksport", "Komunikat programu");//Komunikat
            }
        }
    }
}
