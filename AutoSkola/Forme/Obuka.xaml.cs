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
    /// Interaction logic for Obuka.xaml
    /// </summary>
    public partial class Obuka : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Obuka()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            teorija.Focus();

        }
        public Obuka(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            teorija.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiIspiti = @"select ispitiId, Polozen from Ispiti";
                DataTable dtIspiti = new DataTable();
                SqlDataAdapter daIspiti = new SqlDataAdapter(vratiIspiti, konekcija);
                daIspiti.Fill(dtIspiti);
                cbIspiti.ItemsSource = dtIspiti.DefaultView;
                dtIspiti.Dispose();
                dtIspiti.Dispose();

                string vratiLekarski = @"select leksarskiPregledID, opstiPregledUspesan from lekarski";
                DataTable dtLekarski = new DataTable();
                SqlDataAdapter daLekarski = new SqlDataAdapter(vratiLekarski, konekcija);
                daLekarski.Fill(dtLekarski);
                cbLekarski.ItemsSource = dtLekarski.DefaultView;
                dtLekarski.Dispose();
                dtLekarski.Dispose();

                string vratiPrvaPomoc = @"select prvaPomocID, polozenIspitPrveP  from prvaPomoc";
                DataTable dtPrvaPomoc = new DataTable();
                SqlDataAdapter daPrvaPomoc = new SqlDataAdapter(vratiPrvaPomoc, konekcija);
                daPrvaPomoc.Fill(dtPrvaPomoc);
                cbPrvaPomoc.ItemsSource = dtPrvaPomoc.DefaultView;
                dtPrvaPomoc.Dispose();
                dtPrvaPomoc.Dispose();

                string vratiInstruktor = @"select instruktorID, ime + ' ' + prezime  as Instruktor  from Instruktor";
                DataTable dtInstruktor = new DataTable();
                SqlDataAdapter daInstruktor = new SqlDataAdapter(vratiInstruktor, konekcija);
                daInstruktor.Fill(dtPrvaPomoc);
                cbInstruktor.ItemsSource = dtPrvaPomoc.DefaultView;
                dtInstruktor.Dispose();
                dtInstruktor.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@teorija", SqlDbType.Text).Value = teorija.Text;
                cmd.Parameters.Add("@praksa", SqlDbType.Text).Value = praksa.Text;
                cmd.Parameters.Add("@ispitiID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbIspiti.SelectedItem).Row["ispitiID"].ToString());
                cmd.Parameters.Add("@lekarskiID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbLekarski.SelectedItem).Row["lekarskiID"].ToString());
                cmd.Parameters.Add("@prvaPomocID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbPrvaPomoc.SelectedItem).Row["prvaPomocID"].ToString());
                cmd.Parameters.Add("@instruktorID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbInstruktor.SelectedItem).Row["instruktorID"].ToString());
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["id"];
                    cmd.CommandText = @"Update kandidat set ime = @ime, prezime = @prezime,datumRodjenja = @Datum,JMBG = @JMBG,adresaStanovanja = @Adresa, mestoStanovanja = @Grad, kontakt = @Kontakt, ispitiID = @ispitiID , lekarskiPregledID = @lekarskiID, instruktorID = @instruktorID where letID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into kandidat(ime, prezime, datumRodnjenja, JMBG, adresaStanovanja,mestoStanovanja,kontakt,ispitiID,lekarskiPregledID,instruktorID) values(@ime, @prezime, @Datum,@JMBG, @Adresa, @Grad,@Kontakt, @ispitiID, @lekarskiPregledID,@instruktorID)";
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
    }
}
