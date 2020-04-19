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

namespace CifruCompus
{
    public partial class Form1 : Form
    {
        string continutFisierIncarcat = "";
        string continutFisierRezultat = "";

        public Form1()
        {
            InitializeComponent();
        }

        readonly OpenFileDialog dialogFisierIncarcat = new OpenFileDialog
        {
            Filter = "TXT|*.txt"
        };

        static class Criptare
        {
            static readonly string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            public static string CifrulCaesar(string textClar, int cheie)
            {
                string textCriptat = "";

                if (cheie < 0)
                {
                    return textCriptat;
                }

                foreach (var litera in textClar)
                {
                    int pozitie = -1;
                    for (int i = 0; i < alfabet.Length; i++)
                    {
                        if (alfabet[i] == litera)
                        {
                            pozitie = i;
                        }
                    }

                    if (pozitie == -1)
                    {
                        return "";
                    }

                    textCriptat += alfabet[(pozitie + cheie) % 26];
                }

                return textCriptat;
            }

            public static string CifrulSubstitutie(string textClar, string cheie)
            {
                string alfabetCheie = "";
                string textCriptat = "";

                foreach (var litera in cheie)
                {
                    if (!alfabetCheie.Contains(litera))
                    {
                        alfabetCheie += litera;
                    }
                }

                foreach (var litera in alfabet)
                {
                    if (!alfabetCheie.Contains(litera))
                    {
                        alfabetCheie += litera;
                    }
                }

                for (int i = 0; i < textClar.Length; i++)
                {
                    int pozitieAlfabet = 0;
                    for (int j = 0; j < alfabet.Length; j++)
                    {
                        if (alfabet[j] == textClar[i])
                        {
                            pozitieAlfabet = j;
                            break;
                        }
                    }

                    textCriptat += alfabetCheie[pozitieAlfabet];
                }

                return textCriptat;
            }

        }

        class Decriptare
        {
            static readonly string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            public static string CifrulCaesar(string textCriptat, int cheie)
            {
                string textDecriptat = "";

                if (cheie < 0)
                {
                    return textDecriptat;
                }

                foreach (var litera in textCriptat)
                {
                    int pozitie = -1;
                    for (int i = 0; i < alfabet.Length; i++)
                    {
                        if (alfabet[i] == litera)
                        {
                            pozitie = i;
                        }
                    }

                    if (pozitie == -1)
                    {
                        return "";
                    }

                    textDecriptat += alfabet[((pozitie - cheie) < 0 ? (26 + pozitie - cheie) : (pozitie - cheie)) % 26];
                }

                return textDecriptat;
            }

            public static string CifrulSubstitutie(string textCriptat, string cheie)
            {
                string alfabetCheie = "";
                string textClar = "";

                foreach (var litera in cheie)
                {
                    if (!alfabetCheie.Contains(litera))
                    {
                        alfabetCheie += litera;
                    }
                }

                foreach (var litera in alfabet)
                {
                    if (!alfabetCheie.Contains(litera))
                    {
                        alfabetCheie += litera;
                    }
                }

                for (int i = 0; i < textCriptat.Length; i++)
                {
                    int pozitieAlfabet = 0;
                    for (int j = 0; j < alfabetCheie.Length; j++)
                    {
                        if (alfabetCheie[j] == textCriptat[i])
                        {
                            pozitieAlfabet = j;
                            break;
                        }
                    }

                    textClar += alfabet[pozitieAlfabet];
                }

                return textClar;
            }
        }

        private void IncarcaFisier_Click(object sender, EventArgs e)
        { 
            if(dialogFisierIncarcat.ShowDialog() == DialogResult.OK)
            {
                denumireFisierIncarcat.ForeColor = Color.Black;
                denumireFisierIncarcat.Text = dialogFisierIncarcat.SafeFileName;

                StreamReader streamReader = new StreamReader(dialogFisierIncarcat.FileName);
                continutFisierIncarcat = streamReader.ReadToEnd();
                for(int i = 0; i < continutFisierIncarcat.Length; i ++)
                {
                    if(continutFisierIncarcat[i] < 'A' || continutFisierIncarcat[i] > 'Z')
                    {
                        continutFisierIncarcat = "";
                        denumireFisierIncarcat.ForeColor = Color.Red;
                        denumireFisierIncarcat.Text = "Eroare! Textul trebuie sa contina doar litere scrise cu majuscula";
                    }
                }
            }
        }

        private void vizualizeazaFisierIncarcat_Click(object sender, EventArgs e)
        {
            FisierIncarcat formFisierIncarcat = new FisierIncarcat(denumireFisierIncarcat.Text, continutFisierIncarcat);
            formFisierIncarcat.Show();
        }

        private void vizualizeazaTextCriptat_Click(object sender, EventArgs e)
        {
            FisierIncarcat formFisierIncarcat = new FisierIncarcat("Rezultat", continutFisierRezultat);
            formFisierIncarcat.Show();
        }

        private void cripteaza_Click(object sender, EventArgs e)
        {
            int cheie1 = (int)cheieCaesar.Value;
            string rezultatIntermediar = Criptare.CifrulCaesar(continutFisierIncarcat, cheie1);
            string cheie2 = cheieSubstitutie.Text;
            continutFisierRezultat = Criptare.CifrulSubstitutie(rezultatIntermediar, cheie2);

            StreamWriter fisierTextDecriptat = new StreamWriter("textCriptat.txt");
            fisierTextDecriptat.WriteLine("Text clar:");
            fisierTextDecriptat.WriteLine(continutFisierIncarcat);
            fisierTextDecriptat.WriteLine();
            fisierTextDecriptat.WriteLine("Cheie Caesar:");
            fisierTextDecriptat.WriteLine(cheie1);
            fisierTextDecriptat.WriteLine();
            fisierTextDecriptat.WriteLine("Cheie Substitutie simpla:");
            fisierTextDecriptat.WriteLine(cheie2);
            fisierTextDecriptat.WriteLine();
            fisierTextDecriptat.WriteLine("Text rezultat:");
            fisierTextDecriptat.WriteLine(continutFisierRezultat);
            fisierTextDecriptat.Close();
        }

        private void decripteaza_Click(object sender, EventArgs e)
        {
            string cheie2 = cheieSubstitutie.Text;
            string rezultatIntermediar = Decriptare.CifrulSubstitutie(continutFisierIncarcat, cheie2);
            int cheie1 = (int)cheieCaesar.Value;
            continutFisierRezultat = Decriptare.CifrulCaesar(rezultatIntermediar, cheie1);

            StreamWriter fisierTextDecriptat = new StreamWriter("textDecriptat.txt");
            fisierTextDecriptat.WriteLine("Text criptat:");
            fisierTextDecriptat.WriteLine(continutFisierIncarcat);
            fisierTextDecriptat.WriteLine();
            fisierTextDecriptat.WriteLine("Cheie Caesar:");
            fisierTextDecriptat.WriteLine(cheie1);
            fisierTextDecriptat.WriteLine();
            fisierTextDecriptat.WriteLine("Cheie Substitutie simpla:");
            fisierTextDecriptat.WriteLine(cheie2);
            fisierTextDecriptat.WriteLine();
            fisierTextDecriptat.WriteLine("Text rezultat:");
            fisierTextDecriptat.WriteLine(continutFisierRezultat);
            fisierTextDecriptat.Close();
        }

        private void cheieSubstitutie_TextChanged(object sender, EventArgs e)
        {
            eroareCheieSubstitutie.Text = "";
            string textCheieSubstitutie = cheieSubstitutie.Text;
            for(int i = 0; i < textCheieSubstitutie.Length; i ++)
            {
                if(textCheieSubstitutie[i] < 'A' || textCheieSubstitutie[i] > 'Z')
                {
                    eroareCheieSubstitutie.Text = "Eroare! Textul trebuie sa contina doar litere scrise cu majuscula";
                }
            }
        }
    }
}
