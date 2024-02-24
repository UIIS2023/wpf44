using AutoSkola.Forme;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoSkola
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        string ucitanaTabela;
        bool azuriraj;
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();



        #region Select upiti
        #region Select sa uslovom
        string selectUslovKandidat = @"select * from kandidat where kandidatID=";
        string selectUslovInstruktor = @"select * from instruktor where instruktorID=";
        string selectUslovLekarski = @"select * from lekarski where lekarskiID=";
        string selectUslovPrvaPomoc= @"select * from prvaPomoc where prvaPomocID=";
        string selectUslovPotvrda = @"select * from potvrda where potvrdaID=";
        string selectUslovIspiti = @"select * from ispiti where ispitiID=";
        string selectUslovZaposleni = @"select * from zaposleni where zaposleniID=";
        string selectUslovObuka = @"select * from obuka where obukaID=";
        #endregion


        static string kandidatSelect = @"select kandidatID as ID, Ime, Prezime, datumRodjenja, JMBG, adresaStanovanja, mestoStanovanja,kontakt from kandidat";
                                        
        static string instruktorSelect = @"select instruktorID as ID, imeI, prezime, JMBG, adresaStanovanja, mestoStanovanja , kontakt from instruktor";
        static string zaposleniSelect = @"select zaposleniID as ID, Ime, Prezime,JMBG, kontakt, adresaStanovanja,mestoStanovanja from zaposleni";
        static string potvrdaSelect = @"select  polozen, isplacenaObuks , zaposleni.ime + ' ' + zaposleni.prezime as Zaposleni, kandidat.ime + ' ' + kandidat.prezime as Kandidat  from potvrda
                                      join zaposleni on potvrda.zaposleniID = zaposleni.zaposleniID
                                      join kandidat on potvrda.kandidatID = kandidat.kandidatID";
        static string lekarskiSelect = @"select  testVidaUspesan , testMotoSposUspesan , opstiPregledUspesan , kandidat.ime + ' ' + kandidat.prezime as Kandidat from lekarski
                                        join kandidat on lekarski.kandidatID = kandidat.kandidatID";
        static string prvaPomocSelect = @"select odslusanaObuka, polozeniIspitPrveP, kandidat.ime + ' ' + kandidat.prezime as kandidat from prvaPomoc
                                          join kandidat on prvaPomoc.kandidatID = kandidat.kandidatID";
        static string ispitiSelect = @"select kandidat.ime + ' ' + kandidat.prezime as kandidat, teorijskiIspit  , polozen   from Ispiti
                                       join kandidat on Ispiti.kandidatID = kandidat.kandidatID";

        static string obukaSelect = @"select  zavrsenaTObuka,zavrsenaPObuka, kandidat.ime + ' ' + kandidat.prezime as Kandidat  from obuka
                                       join kandidat on obuka.kandidatID = kandidat.kandidatID ";
        #endregion

        #region Delete upiti
        string kandidatDelete = @"Delete from kandidat where kandidatID=";
        string instruktorDelete = @"Delete from instruktor where instruktorID=";
        string lekarskiDelete = @"Delete from from lekarski where lekarskiID=";
        string prvaPomocDelete = @"Delete from prvaPomoc where prvaPomocID=";
        string potvrdaDelete = @"Delete from potvrda where potvrdaID=";
        string ispitiDelete = @"Delete from ispiti where ispitiID=";
        string zaposleniDelete = @"Delete from zaposleni where zaposleniID=";
        string obukaDelete = @"Delete from obuka where obukaID=";
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(dataGridCentralni, kandidatSelect);
        }


        private void UcitajPodatke(DataGrid grid, string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (grid != null)
                {
                    grid.ItemsSource = dt.DefaultView;
                }
                ucitanaTabela = selectUpit;
                dt.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Neuspesno uneti podaci", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(kandidatSelect))
                    {
                        FrmKandidat prozorKandidat = new FrmKandidat(azuriraj, red);
                        prozorKandidat.txtIme.Text = citac["Ime"].ToString();
                        prozorKandidat.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorKandidat.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorKandidat.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorKandidat.txtGrad.Text = citac["Grad"].ToString();
                        prozorKandidat.datum1.SelectedDate = (DateTime)citac["DatumRodjenja"];
                        prozorKandidat.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorKandidat.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(potvrdaSelect))
                    {
                        FrmPotvrda prozorPotvrda = new FrmPotvrda(azuriraj, red);
                        prozorPotvrda.polozenTeorijski.Text = citac["polozenTeorijski"].ToString();
                        prozorPotvrda.polozenPrakticni.Text = citac["polozenPrakticni"].ToString();
                        prozorPotvrda.isplaceno.Text = citac["IsplacenaObuks"].ToString();
                        prozorPotvrda.cbKandidat.SelectedValue = citac["kandidatID"].ToString();
                        prozorPotvrda.cbZaposleni.SelectedValue = citac["zaposleniID"].ToString();
                        prozorPotvrda.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(lekarskiSelect))
                    {
                        FrmLekarski prozorLekarski = new FrmLekarski(azuriraj, red);
                        prozorLekarski.cbKandidat.SelectedValue = citac["kandidatID"].ToString();
                        prozorLekarski.sposobnost.Text = citac["testMotoSposUspesan"].ToString();
                        prozorLekarski.vid.Text = citac["testVidaUspesan"].ToString();
                        prozorLekarski.opsti.Text = citac["opstiPregledUspesan"].ToString();
                        prozorLekarski.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(ispitiSelect))
                    {
                        Ispiti prozorIspiti = new Ispiti(azuriraj, red);
                        prozorIspiti.cbKandidat.SelectedValue = citac["kandidatID"].ToString();
                        prozorIspiti.teorija.Text = citac["teorijskiIspit"].ToString();
                        prozorIspiti.praksa.Text = citac["polozen"].ToString();
                        prozorIspiti.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(prvaPomocSelect))
                    {
                        FrmPrvaPomoc prozorPrvaPomoc = new FrmPrvaPomoc(azuriraj, red);
                        prozorPrvaPomoc.cbKandidat.SelectedValue = citac["kandidatID"].ToString();
                        prozorPrvaPomoc.odslusan.Text = citac["odslusanaObuka"].ToString();
                        prozorPrvaPomoc.polozen.Text = citac["polozenIspitPrveP"].ToString();
                        prozorPrvaPomoc.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(instruktorSelect))
                    {
                        FrmInstruktor prozorInstruktor = new FrmInstruktor(azuriraj, red);
                        prozorInstruktor.txtIme.Text = citac["imeI"].ToString();
                        prozorInstruktor.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorInstruktor.txtAdresa.Text = citac["adresaStanovanja"].ToString();
                        prozorInstruktor.txtGrad.Text = citac["mestoStanovanja"].ToString();
                        prozorInstruktor.txtKontakt.Text = citac["kontakt"].ToString();
                        prozorInstruktor.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorInstruktor.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(zaposleniSelect))
                    {
                        Zaposleni prozorZaposleni = new Zaposleni (azuriraj, red);
                        prozorZaposleni.txtIme.Text = citac["Ime"].ToString();
                        prozorZaposleni.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorZaposleni.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorZaposleni.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorZaposleni.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorZaposleni.txtGrad.Text = citac["Grad"].ToString();
                        prozorZaposleni.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(obukaSelect))
                    {
                        Obuka prozorObuka = new Obuka(azuriraj, red);
                        prozorObuka.teorija.Text = citac["Teorija"].ToString();
                        prozorObuka.praksa.Text = citac["Praksa"].ToString();
                        prozorObuka.cbIspiti.SelectedValue = citac["ispitiID"].ToString();
                        prozorObuka.cbLekarski.SelectedValue = citac["lekarskiID"].ToString();
                        prozorObuka.cbPrvaPomoc.SelectedValue = citac["PrvaPomocID"].ToString();
                        prozorObuka.cbInstruktor.SelectedValue = citac["instruktorID"].ToString();
                        prozorObuka.ShowDialog();
                    }
                }

            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
                azuriraj = false;
            }
        }

        void ObrisiZapis(DataGrid grid, string deleteUpit)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteUpit + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u nekim drugim tabelama", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }


        private void btnIspiti_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, ispitiSelect);
        }

        private void btnLekarskiPregled_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, lekarskiSelect);
        }

        private void btnPrvaPomoc_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, prvaPomocSelect);
        }

        private void btnInstruktor_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, instruktorSelect);
        }

        private void btnZaposleni_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, zaposleniSelect);
        }

        private void btnPotvrda_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, potvrdaSelect);
        }
        private void btnObuka_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, kandidatSelect);
        }


        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(kandidatSelect))
            {
                ObrisiZapis(dataGridCentralni, kandidatDelete);
                UcitajPodatke(dataGridCentralni, kandidatSelect);
            }
            else if (ucitanaTabela.Equals(potvrdaSelect))
            {
                ObrisiZapis(dataGridCentralni, potvrdaDelete);
                UcitajPodatke(dataGridCentralni, potvrdaSelect);
            }
            else if (ucitanaTabela.Equals(lekarskiSelect))
            {
                ObrisiZapis(dataGridCentralni, lekarskiDelete);
                UcitajPodatke(dataGridCentralni, lekarskiSelect);
            }
            else if (ucitanaTabela.Equals(prvaPomocSelect))
            {
                ObrisiZapis(dataGridCentralni, prvaPomocDelete);
                UcitajPodatke(dataGridCentralni, prvaPomocSelect);
            }
            else if (ucitanaTabela.Equals(ispitiSelect))
            {
                ObrisiZapis(dataGridCentralni, ispitiDelete);
                UcitajPodatke(dataGridCentralni, ispitiSelect);
            }
            else if (ucitanaTabela.Equals(instruktorSelect))
            {
                ObrisiZapis(dataGridCentralni, instruktorDelete);
                UcitajPodatke(dataGridCentralni, instruktorSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                ObrisiZapis(dataGridCentralni, zaposleniDelete);
                UcitajPodatke(dataGridCentralni, zaposleniSelect);
            }
            else if (ucitanaTabela.Equals(obukaSelect))
            {
                ObrisiZapis(dataGridCentralni, obukaDelete);
                UcitajPodatke(dataGridCentralni, obukaSelect);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {

            if (ucitanaTabela.Equals(kandidatSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovKandidat);
                UcitajPodatke(dataGridCentralni, kandidatSelect);
            }
            else if (ucitanaTabela.Equals(potvrdaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovPotvrda);
                UcitajPodatke(dataGridCentralni, potvrdaSelect);
            }
            else if (ucitanaTabela.Equals(lekarskiSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovLekarski);
                UcitajPodatke(dataGridCentralni, lekarskiSelect);
            }
            else if (ucitanaTabela.Equals(ispitiSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovIspiti);
                UcitajPodatke(dataGridCentralni, ispitiSelect);
            }
            else if (ucitanaTabela.Equals(prvaPomocSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovPrvaPomoc);
                UcitajPodatke(dataGridCentralni, prvaPomocSelect);
            }
            else if (ucitanaTabela.Equals(instruktorSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovInstruktor);
                UcitajPodatke(dataGridCentralni, instruktorSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovZaposleni);
                UcitajPodatke(dataGridCentralni, zaposleniSelect);
            }
            else if (ucitanaTabela.Equals(obukaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovObuka);
                UcitajPodatke(dataGridCentralni, obukaSelect);
            }

        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela.Equals(kandidatSelect))
            {
                prozor = new FrmKandidat();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, kandidatSelect);
            }
            else if (ucitanaTabela.Equals(instruktorSelect))
            {
                prozor = new FrmInstruktor();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, instruktorSelect);
            }
            else if (ucitanaTabela.Equals(lekarskiSelect))
            {
                prozor = new FrmLekarski();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, lekarskiSelect);
            }
            else if (ucitanaTabela.Equals(potvrdaSelect))
            {
                prozor = new FrmPotvrda();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, potvrdaSelect);
            }
            else if (ucitanaTabela.Equals(prvaPomocSelect))
            {
                prozor = new FrmPrvaPomoc();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, prvaPomocSelect);
            }
            else if (ucitanaTabela.Equals(ispitiSelect))
            {
                prozor = new Ispiti();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, ispitiSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                prozor = new Zaposleni();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, zaposleniSelect);
            }
            else if (ucitanaTabela.Equals(obukaSelect))
            {
                prozor = new Obuka();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, obukaSelect);
            }
        }

        private void btnObuka_Click_1(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, obukaSelect);
        }
    }
}
