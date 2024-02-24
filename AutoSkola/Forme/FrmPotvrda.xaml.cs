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
using System.Security.Cryptography;

namespace AutoSkola.Forme
{
    /// <summary>
    /// Interaction logic for FrmPotvrda.xaml
    /// </summary>
    public partial class FrmPotvrda : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmPotvrda()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            polozenTeorijski.Focus();

        }
        public FrmPotvrda(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            polozenTeorijski.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKandidat = @"select kandidatID, ime + ' ' + prezime as Kandidat from kandidat";
                DataTable dtKandidat = new DataTable();
                SqlDataAdapter daKandidat = new SqlDataAdapter(vratiKandidat, konekcija);
                daKandidat.Fill(dtKandidat);
                cbKandidat.ItemsSource = dtKandidat.DefaultView;
                dtKandidat.Dispose();
                dtKandidat.Dispose();

                konekcija.Open();
                string vratiZaposleni = @"select zaposleniID, ime + ' ' + prezime as Zapsoleni from Zaposleni";
                DataTable dtZaposleni = new DataTable();
                SqlDataAdapter daZaposleni = new SqlDataAdapter(vratiZaposleni, konekcija);
                daZaposleni.Fill(dtZaposleni);
                cbZaposleni.ItemsSource = dtZaposleni.DefaultView;
                dtZaposleni.Dispose();
                dtZaposleni.Dispose();
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
                cmd.Parameters.Add("@kandidatID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbKandidat.SelectedItem).Row["kandidatID"].ToString());
                cmd.Parameters.Add("@zaposleniID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbKandidat.SelectedItem).Row["zaposleniID"].ToString());
                cmd.Parameters.Add("@polozenTeorijski", SqlDbType.Text).Value = polozenTeorijski.Text;
                cmd.Parameters.Add("@polozenPrakticni", SqlDbType.Text).Value = polozenPrakticni.Text;
                cmd.Parameters.Add("@isplacen", SqlDbType.Text).Value = isplaceno.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["id"];
                    cmd.CommandText = @"Update potvrda set polozenTeorijski = @polozenTeorijski,polozenPrakticni = @polozenPrakticni isplacenaObuks = @isplacen, kandidatID = @kandidatID, zaposleniID = @zaposleniID where letID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into potvrda(polozenTeorijski,polozenPrakticni,isplacenaObuks,kandidatID,zaposleniID) values(@polozenTeorijski,@polozenPrakticni,@isplacen, @kandidatID, @zaposleniID)";
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
