using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProsjekOcjenaForms.ocjena;
using static ProsjekOcjenaForms.prosjek;

namespace ProsjekOcjenaForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<Ocjena> ocjene = new List<Ocjena>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int vrijednostOcjene = Convert.ToInt32(textBox1.Text);
                if (vrijednostOcjene < 1 || vrijednostOcjene > 5)
                {
                    MessageBox.Show("Ocjena mora biti između 1 i 5.");
                    return;
                }

                Predmet predmet = (Predmet)comboBox1.SelectedItem;

                DateTime datum = dateTimePicker1.Value;

                Ocjena novaOcjena = new Ocjena(predmet, vrijednostOcjene, datum);

                ocjene.Add(novaOcjena);

                MessageBox.Show("Ocjena uspješno unesena!");

                IzracunajProsjek();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri unosu: " + ex.Message);
            }
        }

        private void IzracunajProsjek()
        {
            if (ocjene.Count == 0)
            {
                MessageBox.Show("Nema unesenih ocjena.");
                return;
            }

            double suma = 0;
            foreach (Ocjena ocjena in ocjene)
            {
                suma += ocjena.Vrijednost;
            }

            double prosjek = suma / ocjene.Count;
            label1.Text = "Prosjek: " + prosjek.ToString("F2");
        }

        private void SpremiOcjene()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("ocjene.txt"))
                {
                    foreach (Ocjena ocjena in ocjene)
                    {
                        writer.WriteLine($"{ocjena.Predmet.Naziv_predmeta},{ocjena.Vrijednost},{ocjena.Datum.ToShortDateString()}");
                    }
                }
                MessageBox.Show("Ocjene su uspješno spremljene.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri spremanju: " + ex.Message);
            }
        }

        private void UcitajOcjene()
        {
            try
            {
                ocjene.Clear();
                using (StreamReader reader = new StreamReader("ocjene.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(',');
                        string nazivPredmeta = parts[0];
                        int vrijednost = int.Parse(parts[1]);
                        DateTime datum = DateTime.Parse(parts[2]);

                        Predmet predmet = new Predmet(nazivPredmeta);
                        Ocjena ocjena = new Ocjena(predmet, vrijednost, datum);
                        ocjene.Add(ocjena);
                    }
                }
                MessageBox.Show("Ocjene su uspješno učitane.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBox1.Items.Add(new Predmet("Matematika"));
            comboBox1.Items.Add(new Predmet("Povijest"));
            comboBox1.Items.Add(new Predmet("Fizika"));
            comboBox1.Items.Add(new Predmet("Hrvatski"));
            comboBox1.Items.Add(new Predmet("Engleski"));
            comboBox1.Items.Add(new Predmet("Talijanski"));
            comboBox1.SelectedIndex = 0;
        }

    }

}
