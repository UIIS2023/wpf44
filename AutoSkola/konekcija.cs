using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Konekcija
{
    public SqlConnection KreirajKonekciju()
    {
        SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder
        {
            DataSource = @"DESKTOP-01EVVU4\SQLEXPRESS",
            InitialCatalog = "AutoSkola",
            IntegratedSecurity = true
        };
        string con = ccnSb.ToString();
        SqlConnection konekcija = new SqlConnection(con);
        return konekcija;
    }
}

