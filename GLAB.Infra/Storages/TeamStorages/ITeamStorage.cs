using System.Collections.Generic;
using System.Threading.Tasks;
using GLab.Domains.Models.Laboratoires;

namespace GLAB.Infra.Storages.TeamStorages
{
    public interface ITeamStorage
    {
        Task<List<Team>> SelectTeams();
        Task<Team> SelectTeamById(string TeamId);
        Task<Team> SelectTeamByName(string TeamName);
        Task InsertTeam(Team team);
        Task DeleteTeam(string TeamId);
        Task UpdateTeam(Team team);
        Task<bool> ExistId(string TeamId);
        Task<bool> ExistName(string TeamName);
    }
}
