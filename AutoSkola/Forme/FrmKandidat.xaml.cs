using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoSkola.Forme
{
    /// <summary>
    /// Interaction logic for FrmKandidat.xaml
    /// </summary>
    public partial class FrmKandidat : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmKandidat()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            datum1.Focus();

        }
        public FrmKandidat(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            datum1.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                DateTime date = (DateTime)datum1.SelectedDate;
                string datum = date.ToString("dd-MM-yyyy");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@ime", SqlDbType.Text).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.Text).Value = txtPrezime.Text;
                cmd.Parameters.Add("@JMBG", SqlDbType.Text).Value = txtJMBG.Text;
                cmd.Parameters.Add("@Adresa", SqlDbType.Text).Value = txtAdresa.Text;
                cmd.Parameters.Add("@Grad", SqlDbType.Text).Value = txtGrad.Text;
                cmd.Parameters.Add("@Datum", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@Kontakt", SqlDbType.Text).Value = txtKontakt.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["id"];
                    cmd.CommandText = @"Update kandidat set ime = @ime, prezime = @prezime,datumRodjenja = @Datum,JMBG = @JMBG,adresaStanovanja = @Adresa, mestoStanovanja = @Grad, kontakt = @Kontakt,";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into kandidat(ime, prezime, datumRodnjenja, JMBG, adresaStanovanja,mestoStanovanja,kontakt)";
                }


                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
