using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CifruCompus
{
    /*
       clasa folosita pentru implementarea ferestrelor de vizualizare a continutului
       fisierelor de intrare si iesire
    */
    public partial class FisierIncarcat : Form
    {
        
        public FisierIncarcat(string NumeFisier, string continut)
        {
            InitializeComponent();
            this.Text = NumeFisier;
            this.richTextBox1.Text = continut;
        }
    }
}
