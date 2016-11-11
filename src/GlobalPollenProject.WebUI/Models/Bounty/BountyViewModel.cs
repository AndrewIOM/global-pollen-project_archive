using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Models
{
    public class ScoreViewModel
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }

    public class LeaderboardViewModel
    {
        public List<ScoreViewModel> TopOrgs { get; set; }
        public List<ScoreViewModel> TopIndividuals { get; set; }
    }
}
