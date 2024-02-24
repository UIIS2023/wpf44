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
    /// Interaction logic for FrmLekarski.xaml
    /// </summary>
    public partial class FrmLekarski : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmLekarski()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            cbKandidat.Focus();

        }
        public FrmLekarski(bool azuriraj, DataRowView pomocniRed)
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
                string vratiKandidat = @"select  ime + ' ' + prezime as Kandidat from kandidat";
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
                cmd.Parameters.Add("@sposobnost", SqlDbType.Text).Value = sposobnost.Text;
                cmd.Parameters.Add("@vid", SqlDbType.Text).Value = vid.Text;
                cmd.Parameters.Add("@opsti", SqlDbType.Text).Value = opsti.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["id"];
                    cmd.CommandText = @"Update lekarski set testVidaUspesan = @vid, testMotoSposUspesan = @sposobnost, opstiPregledUspesan = @opsti, kandidatID = @kandidatID where letID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into lekarski(testVidaUspesan,testMotoSposUspesan,opstiPregledUspesan,kandidatID) values(@vid,@sposobnost,@opsti, @kandidatID)";
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
