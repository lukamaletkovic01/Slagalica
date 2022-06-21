using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slagalica_91_2018
{
    public partial class Slagalica : Form
    {
        SlagalicaEngine engine;
        Stopwatch stopwatch;
        SlagalicaDB db;
        int brojPoteza;
        Image herc;
        Image karo;
        Image pik;
        Image tref;
        Image prazno;
        Image newgame;
        Image theend;

        List<Button> buttons = new List<Button>();
        List<Label> labels = new List<Label>();
        string filePath;
        public Slagalica()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filePath = "slagalicaData.txt";
            stopwatch = new Stopwatch();
            brojPoteza = 0;
            if (File.Exists(filePath))
            {
                DialogResult res = MessageBox.Show("Zelite li da nastavite prethodno sacuvanu igru?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    DataSerializer dataSerializer = new DataSerializer();
                    engine = dataSerializer.BinaryDeserialize(filePath) as SlagalicaEngine;

                    brojPoteza = engine.brojPoteza;
                }
                else
                {
                    engine = new SlagalicaEngine();
                }
                
            }
            else
                engine = new SlagalicaEngine();
            
            db = new SlagalicaDB();

            herc = Image.FromFile(@"hearts.bmp");
            karo = Image.FromFile(@"karo.bmp");
            pik = Image.FromFile(@"pik.bmp");
            tref = Image.FromFile(@"tref.bmp");
            prazno = Image.FromFile(@"prazno.bmp");
            newgame = Image.FromFile(@"newgame.bmp");
            theend = Image.FromFile(@"theend.bmp");

            buttons = new List<Button>();
            this.ControlBox = false;
            btn_newgame.BackgroundImage = newgame;
            btn_end.BackgroundImage = theend;

            foreach (var button in this.flp_mreza.Controls.OfType<Button>())
            {               
                buttons.Add(button);
            }
            
            foreach (var label in this.pnl_rezultati.Controls.OfType<Label>())
            {
                labels.Add(label);
            }

            osvezi();
            
        }

        private void osvezi()
        {
            
            int[,] matrica = engine.Matrica;

            for (int i=0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (matrica[i, j] == 1)
                    {
                        buttons[i * 4 + j].BackgroundImage = tref;
                    }
                    else if (matrica[i, j] == 2)
                    {
                        buttons[i * 4 + j].BackgroundImage = karo;
                    }
                    else if (matrica[i, j] == 3)
                    {
                        buttons[i * 4 + j].BackgroundImage = pik;
                    }
                    else if (matrica[i, j] == 4)
                    {
                        buttons[i * 4 + j].BackgroundImage = herc;
                    }
                    else
                    {
                        buttons[i * 4 + j].BackgroundImage = prazno;
                    }
                }
            }

            l_brojpoteza.Text = brojPoteza.ToString();

            List<string> rezultati = db.rezultati();
            for (int i = 0; i < rezultati.Count; i++)
            {
                if (i > 9)
                    break;
                labels[i].Text = (i+1).ToString()+". "+rezultati[i];
            }

        }

       

        private void btn_newgame_Click(object sender, EventArgs e)
        {
            engine.promesaj();
            brojPoteza = 0;
            osvezi();
            stopwatch.Reset();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }
            
            Button button = (Button)sender;
            int redni = int.Parse(button.Tag.ToString());

            int vrsta = redni / 4;
            int kolona = redni - vrsta * 4;
            
            int f = engine.pomeri(vrsta, kolona);
            if (f != 0)
                brojPoteza++;

            osvezi();
            if (engine.proveriKraj())
            {
                stopwatch.Stop();
                int min = ((int)stopwatch.Elapsed.TotalSeconds + engine.vreme) / 60;
                int sec = (int)stopwatch.Elapsed.TotalSeconds+engine.vreme - min*60;
                string igrac = Interaction.InputBox("Uspesno ste zavrsili igru!\nBroj poteza: "+brojPoteza+"\nVreme: " + min + "min " + sec + "sec\n\n\nUnesite ime:", "Kraj", "igrac1");
                int vreme = (int)stopwatch.Elapsed.TotalSeconds + engine.vreme;
                
                if (igrac != null && igrac != ""){
                    db.unesiRezultat(igrac, vreme, brojPoteza);
                }
                engine.promesaj();
                stopwatch.Reset();
                brojPoteza = 0;
                osvezi();
            }

        }

        private void btn_end_Click(object sender, EventArgs e)
        {
            if (brojPoteza > 0)
            {
                DialogResult res = MessageBox.Show("Zelite li da sacuvate stanje igre?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    engine.brojPoteza = brojPoteza;
                    engine.vreme = (int)stopwatch.Elapsed.TotalSeconds + engine.vreme;
                    DataSerializer dataSerializer = new DataSerializer();
                    dataSerializer.BinarySerialize(engine, filePath);
                }
                if (res == DialogResult.No)
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                
            }
            else
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            this.Close();
        }

        
    }
}
