using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Models
{
    public class UserScoreViewModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }
    }

    public class ClubScoreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }
    }

    public class LeaderboardViewModel
    {
        public List<ClubScoreViewModel> TopOrgs { get; set; }
        public List<UserScoreViewModel> TopIndividuals { get; set; }
    }
}
