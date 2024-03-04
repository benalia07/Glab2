using System.Collections.Generic;
using System.Threading.Tasks;
using GLab.Domains.Models.Laboratoires;

namespace GLAB.Infra.Storages
{
    public interface ITeamStorage
    {
        Task<List<Team>> SelectTeams();
        Task<Team> SelectTeamById(string TeamId);
        Task InserTeam(Team team);
        Task DeleteTeam(string TeamId);
        Task UpdateTeam(Team team);
    }
}