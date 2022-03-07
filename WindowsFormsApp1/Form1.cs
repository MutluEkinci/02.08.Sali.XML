using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        XmlDocument xdoc = new XmlDocument();
        
        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("../../Kitaplar.xml");//bin içindeki exeden 2 kez geri çıkılıp xml dosyaya ulaşıldığından 2 kez ...
            //xdoc.Save()
            //MessageBox.Show(xdoc.InnerXml);
            //MessageBox.Show(xdoc.DocumentElement.InnerXml);//root=doxument element
            TreeNode root = new TreeNode(xdoc.DocumentElement.Name);//root name
            foreach (XmlElement child in xdoc.DocumentElement.ChildNodes)
            {
                TreeNode childNode = new TreeNode(child.Name + ":" + child.Attributes[0].Value);
                //root.Nodes.Add(child.Name + ":" + child.Attributes[0].Value);
                root.Nodes.Add(childNode);
                foreach (XmlElement childChild in child.ChildNodes)
                {
                    childNode.Nodes.Add(childChild.Name + "=" + childChild.InnerText);
                }
            }
            treeView1.Nodes.Add(root);
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Gerekli element yada attribute'leri oluştur.
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("../../Kitaplar.xml");
            XmlElement kitap = xdoc.CreateElement("Kitap");
            XmlElement ad = xdoc.CreateElement("Ad");
            XmlElement yazar = xdoc.CreateElement("Yazar");
            XmlElement fiyat = xdoc.CreateElement("Fiyat");
            XmlAttribute id = xdoc.CreateAttribute("KitapID");
            //Değerleri Ata.
            ad.InnerText = txtKitapAdi.Text;
            yazar.InnerText = txtYazar.Text;
            fiyat.InnerText = txtFiyat.Text;
            id.Value = txtKitapID.Text;
            //Hiyerarşik yapıyı olluştur...
            kitap.Attributes.Append(id);
            kitap.AppendChild(ad);
            kitap.AppendChild(yazar);
            kitap.AppendChild(fiyat);
            
            xdoc.DocumentElement.AppendChild(kitap);

            xdoc.Save("../../Kitaplar.xml");
        }
        private void VeriGoster(int indis)
        {
            txtKitapAdi.Text = xdoc.DocumentElement.ChildNodes[indis].ChildNodes[0].InnerText;
            txtYazar.Text = xdoc.DocumentElement.ChildNodes[indis].ChildNodes[1].InnerText;
            txtFiyat.Text = xdoc.DocumentElement.ChildNodes[indis].ChildNodes[2].InnerText;
            txtKitapID.Text = xdoc.DocumentElement.ChildNodes[indis].Attributes[0].Value;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            sayac = 0;
            VeriGoster(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xdoc.Load("../../Kitaplar.xml");
        }
        int sayac;
        private void button4_Click(object sender, EventArgs e)
        {
            if(sayac>0)
            VeriGoster(--sayac);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(sayac < xdoc.DocumentElement.ChildNodes.Count-1)
            VeriGoster(++sayac);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sayac = xdoc.DocumentElement.ChildNodes.Count - 1;
            VeriGoster(sayac);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string strConn = @"Data source=DESK1\MS_SQL_2019; initial Catalog=Calisma;Integrated Security=True";

            SqlDataAdapter da = new SqlDataAdapter("select * from tbl_Urunler", strConn);

            DataSet ds = new DataSet();

            da.Fill(ds);
            //doğrudan table a yazılır.
            ds.WriteXml("products.xml");

            DataSet newDs = new DataSet();
            newDs.ReadXml("products.xml");
            dataGridView1.DataSource = ds.Tables[0];//okumak için şemaya gerek duymaz. birden çok tablo olabilir.

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string strConn = @"Data source=DESK1\MS_SQL_2019; initial Catalog=Calisma;Integrated Security=True";

            SqlDataAdapter da = new SqlDataAdapter("select * from tbl_Urunler", strConn);

            DataTable dt = new DataTable("Urunler");
            da.Fill(dt);

            dt.WriteXml("urunler.xml");
            dt.WriteXmlSchema("urunler.xsd");

            DataTable yeni = new DataTable();
            //okurken Önce şemadan okunmalı...
            yeni.ReadXmlSchema("urunler.xsd");
            yeni.ReadXml("urunler.xml");
            dataGridView1.DataSource = yeni;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Ramdeki objeyi ddosya ya yazma
            Personel p = new Personel() { PerID = 1, PerAd = "ali", Adres = "ist" };
            StreamWriter sw = new StreamWriter("pers.dat");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(sw.BaseStream, p);
            sw.Close();
            StreamReader sr = new StreamReader("pers.dat");
            Personel per = (Personel)bf.Deserialize(sr.BaseStream);//obj geldiğinden cast
            MessageBox.Show(per.PerID + " " + per.PerAd);
            sr.Close();
        }
    }
}
