using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

using System.Media;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Programa_1
{
    public partial class Form1 : Form
    {
        public List<Proceso> procesos = new List<Proceso>();
        public List<Lote> lotes = new List<Lote>();
        public bool error = false;
        bool balanced = false;
        bool mostrado = false;

        bool Quitado = false;
        int capture;
        int contador;
        int contadorres;
        int contadorcap;
        bool pausado = false;

        string str_restsegundos;
        string str_restminutos;
        string str_resthoras;

        int contlotes;

        int ticks;
        int ticks2;
        int ticks3;
        int segundos;
        int minutos;
        int horas;
        int errorTime;
        string strsegundos = "0";
        string strminutos = "0";
        string strhoras = "0";
        string lotsegundos = "0";
        string lotminutos = "0";
        string lothoras = "0";

        int resthoras;
        int restminutos;
        int restsegundos;

        int t2segundos;
        int t2minutos;
        int t2horas;
        string t2strsegundos = "0";
        string t2strminutos = "0";
        string t2strhoras = "0";

        int t3segundos;

        public bool process1 = true;
        public bool process2 = false;
        public bool process3 = false;

        public Form1()
        {
            InitializeComponent();
            contador = 0;
            contadorres = 0;
            contlotes = 0;
            segundos = 0;
            minutos = 0;
            horas = 0;
            ticks3 = 0;
            procesamiento.Enabled = false;
        }

        public int aleatorio(int min, int max)
        {
            Random rd = new Random();
            return rd.Next(min, max);
        }

        public void automatico()
        {
            int time = aleatorio(6, 16);
            int numero = aleatorio(1, 6);
            float num1 = aleatorio(0, 100);
            float num2 = aleatorio(0, 100);

            if (numero == 1)
            {
                procesos.Add(new Proceso(procesos.Count, time, num1, num2, "+"));
            }
            if (numero == 2)
            {
                procesos.Add(new Proceso(procesos.Count, time, num1, num2, "-"));
            }
            if (numero == 3)
            {
                procesos.Add(new Proceso(procesos.Count, time, num1, num2, "*"));
            }
            if (numero == 4)
            {
                do
                {
                    num1 = aleatorio(1, 100);
                    num2 = aleatorio(1, 100);
                } while (!validacion_operacional(num1, num2, "/"));
                procesos.Add(new Proceso(procesos.Count, time, num1, num2, "/"));
            }
            if (numero == 5)
            {
                do
                {
                    num1 = aleatorio(1, 100);
                    num2 = aleatorio(1, 100);
                } while (!validacion_operacional(num1, num2, "%"));
                procesos.Add(new Proceso(procesos.Count, time, num1, num2, "%"));
            }
            if (numero == 6)
            {
                procesos.Add(new Proceso(procesos.Count, time, num1, num2, "^"));
            }
        }

        private void Capturar_Click(object sender, EventArgs e)
        {
            Capturar.Enabled = false;
            int numx = int.Parse(textpro.Text);
            int cont = 0;
            while (cont < numx)
            {
                automatico();
                string temp = procesos[cont].op1 + procesos[cont].opera + procesos[cont].op2;
                string[] row = { procesos[cont].ID.ToString(), procesos[cont].time.ToString(), temp };
                var listView = new ListViewItem(row);
                listView1.Items.Add(listView);
                cont++;
            }
            procesamiento.Enabled = true;
        }

        private void tiempo_restante(int tiempo)
        {
            int horas = tiempo / 3600;
            int minutos = (tiempo / 60) - horas;
            int segundos = tiempo - minutos * 60;

            lotsegundos = segundos.ToString();
            lotminutos = minutos.ToString();
            lothoras = horas.ToString();

            if (lotsegundos.Length == 1)
            {
                lotsegundos = "0" + lotsegundos;
            }
            if (lotminutos.Length == 1)
            {
                lotminutos = "0" + lotminutos;
            }
            if (lothoras.Length == 1)
            {
                lothoras = "0" + lothoras;
            }
            if (lotsegundos == "60")
            {
                lotsegundos = "00";
            }
            if (lotminutos == "60")
            {
                lotminutos = "00";
            }
            label9.Text = lothoras + " : " + lotminutos + " : " + lotsegundos;
        }

        public string time_proceso(Proceso proceso)
        {
            
            int horas = proceso.time / 3600;
            int minutos = (proceso.time / 60) - horas;
            int segundos = proceso.time - minutos * 60;

            string lotsegundos = segundos.ToString();
            string lotminutos = minutos.ToString();
            string lothoras = horas.ToString();

            if (lotsegundos.Length == 1)
            {
                lotsegundos = "0" + lotsegundos;
            }
            if (lotminutos.Length == 1)
            {
                lotminutos = "0" + lotminutos;
            }
            if (lothoras.Length == 1)
            {
                lothoras = "0" + lothoras;
            }
            if (lotsegundos == "60")
            {
                lotsegundos = "00";
            }
            if (lotminutos == "60")
            {
                lotminutos = "00";
            }
            return lothoras + " : " + lotminutos + " : " + lotsegundos;
        }

        private void procesamiento_Click(object sender, EventArgs e)
        {
            timer1.Start();
            Capturar.Enabled = false;
            textpro.Enabled = false;
            procesamiento.Enabled = false;
            // Set KeyPreview object to true to allow the form to process 
            // the key before the control with focus processes it.
            this.KeyPreview = true;
            // Associate the event-handling method with the
            // KeyDown event.
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            int count = 0;
            Proceso proceso = new Proceso(0,0,0,0,"--");
            Proceso proceso1 = new Proceso(0, 0, 0, 0, "--");
            int timeToArmageddon = 0;

            foreach (Proceso element in procesos)
            {
                timeToArmageddon += element.time;
            }

            tiempo_restante(timeToArmageddon);

            if(procesos.Count == 1)
            {
                Lote temp = new Lote(procesos[0], proceso1, proceso);
                lotes.Add(temp);
            }
            else
            {
                for (int i = 1; i < procesos.Count + 1; i++)
                {
                    if (i % 3 == 0)
                    {
                        Lote temp = new Lote(procesos[i - 3], procesos[i - 2], procesos[i - 1]);
                        lotes.Add(temp);
                        count += 3;
                    }
                }
                if (procesos.Count - count == 1)
                {
                    Lote temp = new Lote(procesos[procesos.Count - 1], proceso, proceso1);
                    lotes.Add(temp);
                }
                if (procesos.Count - count == 2)
                {
                    Lote temp = new Lote(procesos[procesos.Count - 2], procesos[procesos.Count - 1], proceso);
                    lotes.Add(temp);
                }
            }
            restsegundos = int.Parse(lotsegundos);
            restminutos = int.Parse(lotminutos);
            resthoras = int.Parse(lothoras);

            str_resthoras = resthoras.ToString();
            str_restminutos = restminutos.ToString();
            str_restsegundos = restsegundos.ToString();

            timer2.Start();
            timer3.Start();
            proceso_lotes(contlotes);
            textBox1.Text = lotes[contlotes].proceso1.ID.ToString();
            textBox3.Text = lotes[contlotes].proceso1.time.ToString();
            textBox4.Text = lotes[contlotes].proceso1.op1.ToString();
            textBox5.Text = lotes[contlotes].proceso1.opera.ToString();
            textBox6.Text = lotes[contlotes].proceso1.op2.ToString();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(pausado == false)
            {
                ticks++;
                if (ticks % 10 == 0)
                {
                    segundos++;
                    strsegundos = segundos.ToString();
                }
                if (minutos == 59 && segundos == 60)
                {
                    horas++;
                    strhoras = horas.ToString();
                    minutos = 0;
                }
                if (segundos == 60)
                {
                    minutos++;
                    strminutos = minutos.ToString();
                    segundos = 0;
                }
                if (strsegundos.Length == 1)
                {
                    strsegundos = "0" + strsegundos;
                }
                if (strminutos.Length == 1)
                {
                    strminutos = "0" + strminutos;
                }
                if (strhoras.Length == 1)
                {
                    strhoras = "0" + strhoras;
                }
                if (strsegundos == "60")
                {
                    strsegundos = "00";
                }
                if (strminutos == "60")
                {
                    strminutos = "00";
                }
                time.Text = strhoras + " : " + strminutos + " : " + strsegundos;
            }
            
        }

        public void CalcularRestante()
        {
            restsegundos -= t3segundos;
            str_resthoras = resthoras.ToString();
            str_restminutos = restminutos.ToString();
            str_restsegundos = restsegundos.ToString();

            if(errorTime != 0)
            {
                restsegundos -= errorTime;
                errorTime = 0;
            }
            if (str_resthoras.Length == 1)
            {
                str_resthoras = "0" + str_resthoras;
            }
            if (str_restminutos.Length == 1)
            {
                str_restminutos = "0" + str_restminutos;
            }
            if (str_restsegundos.Length == 1)
            {
                str_restsegundos = "0" + str_restsegundos;
            }
            if (int.Parse(str_restsegundos) < 0 && int.Parse(str_restminutos) > 0)
            {
                ticks3 = 0;
                ticks2 = 0;
                str_restminutos = (int.Parse(str_restminutos) - 1).ToString();
                str_restsegundos = "60";
                restsegundos += 60;
                restminutos -= 1;
            }
            if(int.Parse(str_restminutos) < 0 && int.Parse(str_resthoras) > 0)
            {
                ticks3 = 0;
                ticks2 = 0;
                str_resthoras = (int.Parse(str_resthoras) - 1).ToString();
                str_restminutos = "59";
                restminutos += 60;

            }
            if(int.Parse(str_restsegundos) < 0)
            {
                str_restsegundos = "-1";
            }
            if((str_resthoras + " : " + str_restminutos + " : " + str_restsegundos) == "00 : 00 : -1")
            {
                restsegundos = 0;
                restminutos = 0;
                resthoras = 0;
                ticks3 = 0;
                ticks2 = 0;
                timer3.Stop();
                timer2.Stop();
                timer1.Stop();
                label9.Text =  "00 : 00 : 00";
                label18.Text = "0";

                if(!mostrado)
                {
                    mostrado = true;
                    timer1.Stop();
                    MessageBox.Show("Procesos Terminados...hasta pronto");
                }

                if (process2 == true)
                {
                    if (time_proceso(lotes[contlotes].proceso2) == label16.Text || error == true)
                    {

                        process2 = false;
                        process3 = true;
                        string temp = lotes[contlotes].proceso2.op1.ToString() + lotes[contlotes].proceso2.opera.ToString() + lotes[contlotes].proceso2.op2.ToString();
                        string[] row;
                        if (error == true)
                        {
                            error = false;
                            row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso2.ID.ToString(), temp, "Error", lotes[contlotes].proceso2.time.ToString() };


                            string actual = (restsegundos - lotes[contlotes].proceso2.time).ToString();
                            errorTime = int.Parse(str_restsegundos) - int.Parse(actual);

                        }
                        else
                        {
                            row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso2.ID.ToString(), temp, calcular_operación(lotes[contlotes].proceso2.op1, lotes[contlotes].proceso2.op2, lotes[contlotes].proceso2.opera).ToString(), lotes[contlotes].proceso2.time.ToString() };
                        }

                        var listView = new ListViewItem(row);
                        listView3.Items.Add(listView);
                        listView2.Items[0].Remove();
                        t2segundos = 0;
                        t2minutos = 0;
                        t2horas = 0;
                        label16.Text = "00 : 00 : 00";
                        t2strsegundos = "0";
                        t2strminutos = "0";
                        t2strhoras = "0";
                        timer2.Stop();
                        timer2.Dispose();
                        ticks2 = 0;
                        textBox1.Text = lotes[contlotes].proceso3.ID.ToString();
                        textBox3.Text = lotes[contlotes].proceso3.time.ToString();
                        textBox4.Text = lotes[contlotes].proceso3.op1.ToString();
                        textBox5.Text = lotes[contlotes].proceso3.opera.ToString();
                        textBox6.Text = lotes[contlotes].proceso3.op2.ToString();
                    }
                }
                else if (process3 == true)
                {
                    if (time_proceso(lotes[contlotes].proceso3) == label16.Text || error == true)
                    {
                        process3 = false;
                        process1 = true;
                        string temp = lotes[contlotes].proceso3.op1.ToString() + lotes[contlotes].proceso3.opera.ToString() + lotes[contlotes].proceso3.op2.ToString();
                        string[] row;
                        if (error == true)
                        {
                            error = false;
                            row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso3.ID.ToString(), temp, "Error", lotes[contlotes].proceso3.time.ToString() };


                            string actual = (restsegundos - lotes[contlotes].proceso3.time).ToString();
                            errorTime = int.Parse(str_restsegundos) - int.Parse(actual);

                        }
                        else
                        {
                            row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso3.ID.ToString(), temp, calcular_operación(lotes[contlotes].proceso3.op1, lotes[contlotes].proceso3.op2, lotes[contlotes].proceso3.opera).ToString(), lotes[contlotes].proceso3.time.ToString() };
                        }

                        var listView = new ListViewItem(row);
                        listView3.Items.Add(listView);
                        listView2.Items[0].Remove();
                        t2segundos = 0;
                        t2minutos = 0;
                        t2horas = 0;
                        label16.Text = "00 : 00 : 00";
                        t2strsegundos = "0";
                        t2strminutos = "0";
                        t2strhoras = "0";
                        timer2.Stop();
                        timer2.Dispose();
                        ticks2 = 0;

                        contlotes++;
                        if (lotes.Count > contlotes)
                        {
                            Quitado = false;
                            textBox1.Text = lotes[contlotes].proceso1.ID.ToString();
                            textBox3.Text = lotes[contlotes].proceso1.time.ToString();
                            textBox4.Text = lotes[contlotes].proceso1.op1.ToString();
                            textBox5.Text = lotes[contlotes].proceso1.opera.ToString();
                            textBox6.Text = lotes[contlotes].proceso1.op2.ToString();
                            proceso_lotes(contlotes);
                        }
                    }
                }
            }
            else
            {
                label9.Text = str_resthoras + " : " + str_restminutos + " : " + str_restsegundos;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(pausado == false)
            {
                ticks2++;
                if (ticks2 % 10 == 0)
                {
                    t2segundos++;
                    t2strsegundos = t2segundos.ToString();
                }
                if (t2minutos == 59 && t2segundos == 60)
                {
                    t2horas++;
                    t2strhoras = t2horas.ToString();
                    t2minutos = 0;
                }
                if (t2segundos == 60)
                {
                    t2minutos++;
                    t2strminutos = t2minutos.ToString();
                    t2segundos = 0;
                }
                if (t2strsegundos.Length == 1)
                {
                    t2strsegundos = "0" + t2strsegundos;
                }
                if (t2strminutos.Length == 1)
                {
                    t2strminutos = "0" + t2strminutos;
                }
                if (t2strhoras.Length == 1)
                {
                    t2strhoras = "0" + t2strhoras;
                }
                if (t2strsegundos == "60")
                {
                    t2strsegundos = "00";
                }
                if (t2strminutos == "60")
                {
                    t2strminutos = "00";
                }
                label16.Text = t2strhoras + " : " + t2strminutos + " : " + t2strsegundos;
                CalcularRestante();
                if (lotes.Count > contlotes)
                {
                    if(process1 == true && Quitado == false)
                    {
                        Quitado = true;
                        if(listView1.Items.Count > 2)
                        {
                            listView1.Items[2].Remove();
                        }
                        if (listView1.Items.Count > 1)
                        {
                            listView1.Items[1].Remove();
                        }
                        listView1.Items[0].Remove();
                    }
                    
                    if (process1 == true)
                    {
                        if (time_proceso(lotes[contlotes].proceso1) == label16.Text || error == true)
                        {
                            process1 = false;
                            process2 = true;
                            string temp = lotes[contlotes].proceso1.op1.ToString() + lotes[contlotes].proceso1.opera.ToString() + lotes[contlotes].proceso1.op2.ToString();
                            string[] row;
                            if(error == true)
                            {
                                ticks3 = 0;
                                ticks2 = 0;
                                error = false;
                                row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso1.ID.ToString(), temp, "Error", lotes[contlotes].proceso1.time.ToString() };
                                
                                
                                string actual = (restsegundos - lotes[contlotes].proceso1.time).ToString();
                                errorTime = int.Parse(str_restsegundos) - int.Parse(actual);
                                
                                
                            }
                            else
                            {
                                row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso1.ID.ToString(), temp, calcular_operación(lotes[contlotes].proceso1.op1, lotes[contlotes].proceso1.op2, lotes[contlotes].proceso1.opera).ToString(), lotes[contlotes].proceso1.time.ToString() };
                            }
                            var listView = new ListViewItem(row);
                            listView3.Items.Add(listView);
                            listView2.Items[0].Remove();
                            t2segundos = 0;
                            t2minutos = 0;
                            t2horas = 0;
                            label16.Text = "00 : 00 : 00";
                            t2strsegundos = "0";
                            t2strminutos = "0";
                            t2strhoras = "0";
                            timer2.Stop();
                            timer2.Dispose();
                            ticks2 = 0;
                            timer2.Start();
                            textBox1.Text = lotes[contlotes].proceso2.ID.ToString();
                            textBox3.Text = lotes[contlotes].proceso2.time.ToString();
                            textBox4.Text = lotes[contlotes].proceso2.op1.ToString();
                            textBox5.Text = lotes[contlotes].proceso2.opera.ToString();
                            textBox6.Text = lotes[contlotes].proceso2.op2.ToString();
                            
                        }
                    }
                    else if (process2 == true)
                    {
                        if (time_proceso(lotes[contlotes].proceso2) == label16.Text || error == true)
                        {
                            
                            process2 = false;
                            process3 = true;
                            string temp = lotes[contlotes].proceso2.op1.ToString() + lotes[contlotes].proceso2.opera.ToString() + lotes[contlotes].proceso2.op2.ToString();
                            string[] row;
                            if(error == true)
                            {
                                ticks3 = 0;
                                ticks2 = 0;
                                error = false;
                                row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso2.ID.ToString(), temp,"Error", lotes[contlotes].proceso2.time.ToString() };

                                
                                string actual = (restsegundos - lotes[contlotes].proceso2.time).ToString();
                                errorTime = int.Parse(str_restsegundos) - int.Parse(actual);
                                
                            }
                            else
                            {
                                row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso2.ID.ToString(), temp, calcular_operación(lotes[contlotes].proceso2.op1, lotes[contlotes].proceso2.op2, lotes[contlotes].proceso2.opera).ToString(), lotes[contlotes].proceso2.time.ToString() };
                            }
                            
                            var listView = new ListViewItem(row);
                            listView3.Items.Add(listView);
                            listView2.Items[0].Remove();
                            t2segundos = 0;
                            t2minutos = 0;
                            t2horas = 0;
                            label16.Text = "00 : 00 : 00";
                            t2strsegundos = "0";
                            t2strminutos = "0";
                            t2strhoras = "0";
                            timer2.Stop();
                            timer2.Dispose();
                            ticks2 = 0;
                            timer2.Start();
                            textBox1.Text = lotes[contlotes].proceso3.ID.ToString();
                            textBox3.Text = lotes[contlotes].proceso3.time.ToString();
                            textBox4.Text = lotes[contlotes].proceso3.op1.ToString();
                            textBox5.Text = lotes[contlotes].proceso3.opera.ToString();
                            textBox6.Text = lotes[contlotes].proceso3.op2.ToString();
                        }
                    }
                    else if (process3 == true)
                    {
                        if (time_proceso(lotes[contlotes].proceso3) == label16.Text || error == true)
                        {
                           
                            process3 = false;
                            process1 = true;
                            string temp = lotes[contlotes].proceso3.op1.ToString() + lotes[contlotes].proceso3.opera.ToString() + lotes[contlotes].proceso3.op2.ToString();
                            string[] row;
                            if (error == true)
                            {
                                ticks3 = 0;
                                ticks2 = 0;
                                error = false;
                                row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso3.ID.ToString(), temp,"Error", lotes[contlotes].proceso3.time.ToString() };

                                
                                string actual = (restsegundos - lotes[contlotes].proceso3.time).ToString();
                                errorTime = int.Parse(str_restsegundos) - int.Parse(actual);
                                
                            }
                            else
                            {
                                row = new string[] { (contlotes + 1).ToString(), lotes[contlotes].proceso3.ID.ToString(), temp, calcular_operación(lotes[contlotes].proceso3.op1, lotes[contlotes].proceso3.op2, lotes[contlotes].proceso3.opera).ToString(), lotes[contlotes].proceso3.time.ToString() };
                            }

                            var listView = new ListViewItem(row);
                            listView3.Items.Add(listView);
                            listView2.Items[0].Remove();
                            t2segundos = 0;
                            t2minutos = 0;
                            t2horas = 0;
                            label16.Text = "00 : 00 : 00";
                            t2strsegundos = "0";
                            t2strminutos = "0";
                            t2strhoras = "0";
                            timer2.Stop();
                            timer2.Dispose();
                            ticks2 = 0;
                            timer2.Start();
                            contlotes++;
                            
                            if (lotes.Count > contlotes)
                            {
                                Quitado = false;
                                textBox1.Text = lotes[contlotes].proceso1.ID.ToString();
                                textBox3.Text = lotes[contlotes].proceso1.time.ToString();
                                textBox4.Text = lotes[contlotes].proceso1.op1.ToString();
                                textBox5.Text = lotes[contlotes].proceso1.opera.ToString();
                                textBox6.Text = lotes[contlotes].proceso1.op2.ToString();
                                proceso_lotes(contlotes);
                            }
                        }
                    }
                }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (pausado == false)
            {
                ticks3++;
                if (ticks3 % 10 == 0)
                {
                    t3segundos = 1;
                }
                else
                {
                    t3segundos = 0;
                }
            }
            
        }

        public void proceso_lotes(int loteactual)
        {
            label17.Text = (contlotes+1).ToString();
            label18.Text = (lotes.Count - contlotes).ToString();
            string[] row = { lotes[loteactual].proceso1.ID.ToString(), lotes[loteactual].proceso1.time.ToString() };
            string[] row1 = { lotes[loteactual].proceso2.ID.ToString(), lotes[loteactual].proceso2.time.ToString() };
            string[] row2 = { lotes[loteactual].proceso3.ID.ToString(), lotes[loteactual].proceso3.time.ToString() };
            var listView = new ListViewItem(row);
            listView2.Items.Add(listView);
            listView = new ListViewItem(row1);
            listView2.Items.Add(listView);
            listView = new ListViewItem(row2);
            listView2.Items.Add(listView);

        }

        public float calcular_operación(float a, float b, string op)
        {
            if(op == "+")
            {
                return (float)(a + b);
            }
            if (op == "-")
            {
                return (float)(a - b);
            }
            if (op == "*")
            {
                return (float)(a * b);
            }
            if (op == "/")
            {
                return (float)(a / b);
            }
            if (op == "%")
            {
                return (float)(a % b);
            }
            if (op == "^")
            {
                return (float)Math.Pow(a, b);
            }
            return 0;
        }

        public bool validacion_operacional(float a, float b, string op)
        {
            if (op == "/")
            {
                if(b == 0)
                {
                    return false;
                }
                return true;
            }
            if (op == "%")
            {
               if(a == 0 || b == 0)
               {
                    return false;
               }
                return true;
            }
            return true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.E)
            {
                /*SoundPlayer ugh = new SoundPlayer(" C:/Users/Lenovo VIIL/Desktop/Semestre 6/Seminario de Sistemas Operativos/Act.02/Programa 1/bin/Debug/net6.0-windows/ugh.wav");
                ugh.Play();*/
                Proceso temp1 = new Proceso();
                
                

                if(process1 == true)
                {
                    temp1 = lotes[contlotes].proceso1;
                    lotes[contlotes].proceso1 = lotes[contlotes].proceso2;
                    lotes[contlotes].proceso2 = lotes[contlotes].proceso3;
                    lotes[contlotes].proceso3 = temp1;

                    listView2.Items.Clear();
                    label17.Text = (contlotes + 1).ToString();
                    label18.Text = (lotes.Count - contlotes).ToString();
                    string[] row = { lotes[contlotes].proceso1.ID.ToString(), lotes[contlotes].proceso1.time.ToString() };
                    string[] row1 = { lotes[contlotes].proceso2.ID.ToString(), lotes[contlotes].proceso2.time.ToString() };
                    string[] row2 = { lotes[contlotes].proceso3.ID.ToString(), lotes[contlotes].proceso3.time.ToString() };
                    var listView = new ListViewItem(row);
                    listView2.Items.Add(listView);
                    listView = new ListViewItem(row1);
                    listView2.Items.Add(listView);
                    listView = new ListViewItem(row2);
                    listView2.Items.Add(listView);
                }
                else if(process2 == true)
                {
                    temp1 = lotes[contlotes].proceso2;
                    lotes[contlotes].proceso2 = lotes[contlotes].proceso3;
                    lotes[contlotes].proceso3 = temp1;

                    listView2.Items.Clear();
                    label17.Text = (contlotes + 1).ToString();
                    label18.Text = (lotes.Count - contlotes).ToString();
                    string[] row1 = { lotes[contlotes].proceso2.ID.ToString(), lotes[contlotes].proceso2.time.ToString() };
                    string[] row2 = { lotes[contlotes].proceso3.ID.ToString(), lotes[contlotes].proceso3.time.ToString() };
                    var listView = new ListViewItem();
                    listView = new ListViewItem(row1);
                    listView2.Items.Add(listView);
                    listView = new ListViewItem(row2);
                    listView2.Items.Add(listView);
                }
                
                

            }
            if (e.KeyCode == Keys.W)
            {
                error = true;
                
            }
            if (e.KeyCode == Keys.P)
            {
                if (pausado == false)
                {
                    pausado = true;
                }
            }
            if (e.KeyCode == Keys.C)
            {
                if (pausado == true)
                {

                    pausado = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}