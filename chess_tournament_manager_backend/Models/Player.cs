using chess_tournament_manager_backend.Models;

namespace chess_tournament_manager_backend.Models
{
    public class Player
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public long? TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
