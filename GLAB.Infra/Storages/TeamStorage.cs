using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLab.Domains.Models.Laboratoires;
using Microsoft.Extensions.Configuration;


namespace GLAB.Infra.Storages
{ 
    internal class TeamStorage : ITeamStorage
    {
        private string connectionString;
        public TeamStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("GLabDB");
        }

        public async Task<List<Team>> SelectTeams()
        {
            List<Team> teams = new List<Team>();
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new("select * from dbo.Teams", connection);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                Team team = getTeamFromDataRow(row);
                teams.Add(team);
            }

            return teams;
        }


        public async Task<Team> SelectTeamById(string TeamId)
        {
            Team team = new Team();
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new("select * from dbo.Teams where TeamId = @aTeamId", connection);
            cmd.Parameters.AddWithValue("@aTeamId", TeamId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            if (ds.Rows.Count == 0)
                return null;

            return getTeamFromDataRow(ds.Rows[0]);
        }




        public async Task InserTeam(Team team)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new("Insert dbo.Teams(TeamId，Status,LaboratoryId) " +
                                 "VALUES(@aTeamId，@aStatus,@aLaboratoryId)", connection);

            cmd.Parameters.AddWithValue("@aTeamId",team.TeamId);
            cmd.Parameters.AddWithValue("@aStatus", team.Status);
            cmd.Parameters.AddWithValue("@aLaboratoryId", team.LaboratoryId);


            connection.Open();
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task DeleteTeam(String TeamId)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new("DELETE FROM dbo.Teams WHERE TeamId = @TeamId", connection);

            cmd.Parameters.AddWithValue("@TeamId", TeamId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateTeam(Team team)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE dbo.Teams SET LaboratoryId = @aLaboratoryId, Status = @Status WHERE TeamId = @TeamId", connection);


            cmd.Parameters.AddWithValue("@aStatus", team.Status);
            cmd.Parameters.AddWithValue("@aLaboratoryId", team.LaboratoryId);
            cmd.Parameters.AddWithValue("@aTeamId", team.TeamId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync(); 
        }

        private static Team getTeamFromDataRow(DataRow row)
        {
            return new()
            {
                TeamId = (string)row["TeamId"],
                LaboratoryId = (string)row["LaboratoryId"],
                Status = (string)row["Status"],
              
            };
        }


      
    }
}
