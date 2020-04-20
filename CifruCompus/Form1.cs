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

        //fisierul de intrare trebuie sa aiba extensia .txt
        readonly OpenFileDialog dialogFisierIncarcat = new OpenFileDialog
        {
            Filter = "TXT|*.txt"
        };

        //clasa care contine cele 2 metode de criptare folosite
        static class Criptare
        {
            static readonly string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //metoda care cripteaza un text clar, folosind cifrul Caesar
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

                    /*
                       se inlocuieste litera din cuvant cu litera care urmeaza in 
                       alfabet dupa un numar de pozitii egal cu valoarea cheii
                    */
                    textCriptat += alfabet[(pozitie + cheie) % 26];
                }

                return textCriptat;
            }

            //metoda care cripteaza un text clar, folosind cifrul de substitutie simpla
            public static string CifrulSubstitutie(string textClar, string cheie)
            {
                string alfabetCheie = "";
                string textCriptat = "";

                /*
                    se formeaza noul alfabet, in functie de cheie
                */
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

                /*
                   se cauta pozitia unei litere din textul clar in alfabetul original
                   si se inlocuieste litera respectiva cu litera de pe aceeasi pozitie
                   din alfabetul nou
                */
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

            //metoda care decripteaza un text, folosind cifrul Caesar
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

                    /*
                       se inlocuieste litera din cuvant cu litera aflata in 
                       alfabet inaintea acesteia cu un numar de pozitii egal cu valoarea cheii
                    */
                    textDecriptat += alfabet[((pozitie - cheie) < 0 ? (26 + pozitie - cheie) : (pozitie - cheie)) % 26];
                }

                return textDecriptat;
            }

            //metoda care decripteaza un text, folosind cifrul de substitutie simpla
            public static string CifrulSubstitutie(string textCriptat, string cheie)
            {
                string alfabetCheie = "";
                string textClar = "";

                /*
                    se formeaza noul alfabet, in functie de cheie
                 */
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

                /*
                   se cauta pozitia unei litere din textul criptat in alfabetul nou
                   si se inlocuieste litera respectiva cu litera de pe aceeasi pozitie
                   din alfabetul vechi
                */
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

                //se afiseaza numele fisierului incarcat
                denumireFisierIncarcat.Text = dialogFisierIncarcat.SafeFileName;

                //se citeste continutul fisierului incarcat
                StreamReader streamReader = new StreamReader(dialogFisierIncarcat.FileName);
                continutFisierIncarcat = streamReader.ReadToEnd();
                for(int i = 0; i < continutFisierIncarcat.Length; i ++)
                {
                    //fisierul trebuie sa contina doar litere scrise cu majuscula
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
            formFisierIncarcat.StartPosition = FormStartPosition.CenterScreen;
            formFisierIncarcat.Show();
        }

        private void vizualizeazaTextCriptat_Click(object sender, EventArgs e)
        {
            FisierIncarcat formFisierIncarcat = new FisierIncarcat("Rezultat", continutFisierRezultat);
            formFisierIncarcat.StartPosition = FormStartPosition.CenterScreen;
            formFisierIncarcat.Show();
        }

        private void cripteaza_Click(object sender, EventArgs e)
        {
            //se cripteaza continutul fisierului incarcat cu cifrul Caesar
            int cheie1 = (int)cheieCaesar.Value;
            string rezultatIntermediar = Criptare.CifrulCaesar(continutFisierIncarcat, cheie1);

            //se cripteaza continutul fisierului incarcat cu cifrul substitutie simpla
            string cheie2 = cheieSubstitutie.Text;
            continutFisierRezultat = Criptare.CifrulSubstitutie(rezultatIntermediar, cheie2);

            //se creeaza un fisier in care se scrie textul criptat
            StreamWriter fisierTextDecriptat = new StreamWriter("textCriptat.txt");
            fisierTextDecriptat.WriteLine(continutFisierRezultat);
            fisierTextDecriptat.Close();
        }

        private void decripteaza_Click(object sender, EventArgs e)
        {
            //se decripteaza continutul fisierului incarcat cu cifrul Caesar
            string cheie2 = cheieSubstitutie.Text;
            string rezultatIntermediar = Decriptare.CifrulSubstitutie(continutFisierIncarcat, cheie2);

            //se decripteaza continutul fisierului incarcat cu cifrul substitutie simpla
            int cheie1 = (int)cheieCaesar.Value;
            continutFisierRezultat = Decriptare.CifrulCaesar(rezultatIntermediar, cheie1);

            //se creeaza un fisier in care se scrie textul decriptat
            StreamWriter fisierTextDecriptat = new StreamWriter("textDecriptat.txt");
            fisierTextDecriptat.WriteLine(continutFisierRezultat);
            fisierTextDecriptat.Close();
        }

        private void cheieSubstitutie_TextChanged(object sender, EventArgs e)
        {
            //se testeaza daca cheia pentru cifrul de substitutie simpla este formata doar din litere scrise cu majuscula
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
