using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Praktikum_AD_Week_10
{
    public partial class HasilPertandingan : Form
    {
        public HasilPertandingan()
        {
            InitializeComponent();
        }

        public static string sqlConnection = "server = localhost;uid=root;pwd=;database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection); //Sebagai data koneksi ke DBMS
        public MySqlCommand sqlCommand; //Sebagai perintah SQL (select, insert, update, delete)
        public MySqlDataAdapter sqlAdapter; //Sebagai menampung hasil query
        string sqlQuery;

        private void HasilPertandingan_Load(object sender, EventArgs e)
        {
            DataTable teamName = new DataTable();
            sqlQuery = "SELECT team_name as `Team Name`, team_id as `Team ID` FROM team";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(teamName);
            cbBoxLeft.ValueMember = "Team ID";
            cbBoxLeft.DisplayMember = "Team Name";
            cbBoxLeft.DataSource = teamName;
            DataTable teamName2 = new DataTable();
            sqlQuery = "SELECT team_name as `Team Name`, team_id as `Team ID` FROM team";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(teamName2);
            cbBoxRight.ValueMember = "Team ID";
            cbBoxRight.DisplayMember = "Team Name";
            cbBoxRight.DataSource = teamName2;
        }

        private void cbBoxLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable managerAndCaptainLeft = new DataTable();
            sqlQuery = "select m.manager_name as `Manager Name`, p.player_name as `Captain` from manager m, player p, team t where m.manager_id = t.manager_id and t.captain_id = p.player_id and t.team_id = '" + cbBoxLeft.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(managerAndCaptainLeft);
            lblOutputManagerLeft.Text = managerAndCaptainLeft.Rows[0]["Manager Name"].ToString();
            lblOutputCaptainLeft.Text = managerAndCaptainLeft.Rows[0]["Captain"].ToString();
        }

        private void cbBoxRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable managerAndCaptainRight = new DataTable();
            sqlQuery = "select m.manager_name as `Manager Name`, p.player_name as `Captain` from manager m, player p, team t where m.manager_id = t.manager_id and t.captain_id = p.player_id and t.team_id = '" + cbBoxRight.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(managerAndCaptainRight);
            lblOutputManagerRight.Text = managerAndCaptainRight.Rows[0]["Manager Name"].ToString();
            lblOutputCaptainRight.Text = managerAndCaptainRight.Rows[0]["Captain"].ToString();
            DataTable stadiumCapacity = new DataTable();
            sqlQuery = "select concat(t.home_stadium, ', ', t.city) as `Stadium`, t.capacity as `Capacity` from team t where t.team_id = '" + cbBoxLeft.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(stadiumCapacity);
            lblOutputStadium.Text = stadiumCapacity.Rows[0]["Stadium"].ToString();
            lblOutputCapacity.Text = stadiumCapacity.Rows[0]["Capacity"].ToString();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            DataTable tanggalSkor = new DataTable();
            sqlQuery = "select date_format(m.match_date, \"%e %M %Y\") as Tanggal, concat(m.goal_home, ' - ', m.goal_away) as Skor from `match` m where m.team_home = '" + cbBoxLeft.SelectedValue.ToString() + "' and m.team_away = '" + cbBoxRight.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(tanggalSkor);
            lblTanggal.Text = tanggalSkor.Rows[0]["Tanggal"].ToString();
            lblHasilScore.Text = tanggalSkor.Rows[0]["Skor"].ToString();
            DataTable detailMatch = new DataTable();
            sqlQuery = "select dm1.minute as Minute, if(p.team_id != m.team_home, '', p.player_name) as 'Player Name 1', if(p.team_id != m.team_home, '', if(dm1.type = 'CY', 'Yellow Card', if(dm1.type = 'CR', 'Red Card', if(dm1.type = 'GO', 'Goal', if(dm1.type = 'GP', 'Goal Penalty', if(dm1.type = 'GW', 'Own Goal', if(dm1.type = 'PM', 'Penalty Miss', ''))))))) as 'Tipe 1', if(p.team_id != m.team_away, '', p.player_name) as 'Player Name 2', if(p.team_id != m.team_away, '', if(dm1.type = 'CY', 'Yellow Card', if(dm1.type = 'CR', 'Red Card', if(dm1.type = 'GO', 'Goal', if(dm1.type = 'GP', 'Goal Penalty', if(dm1.type = 'GW', 'Own Goal', if(dm1.type = 'PM', 'Penalty Miss', ''))))))) as 'Tipe 2'  from dmatch dm1, player p, `match` m where dm1.match_id = m.match_id and p.player_id = dm1.player_id and m.team_home = '" + cbBoxLeft.SelectedValue.ToString() + "' and m.team_away = '" + cbBoxRight.SelectedValue.ToString() + "' order by 1";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(detailMatch);
            dgvDetail.DataSource = detailMatch;
        }
    }
}
