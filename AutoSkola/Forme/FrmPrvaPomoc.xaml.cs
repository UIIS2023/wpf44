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
    /// Interaction logic for FrmPrvaPomoc.xaml
    /// </summary>
    public partial class FrmPrvaPomoc : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmPrvaPomoc()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            cbKandidat.Focus();

        }
        public FrmPrvaPomoc(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            cbKandidat.Focus();
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
                cmd.Parameters.Add("@odslusano", SqlDbType.Text).Value = odslusan.Text;
                cmd.Parameters.Add("@polozen", SqlDbType.Text).Value = polozen.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["id"];
                    cmd.CommandText = @"Update prvaPomoc set odslusanaObuka = @odslusano, polozenIspitPrveP = @polozen, kandidatID = @kandidatID where letID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into prvaPomoc(odslusanaObuka,polozenIspitPrveP,kandidatID) values(@odslusano,@polozen, @kandidatID)";
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
