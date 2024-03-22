﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace chess_tournament_manager_backend.Models
{
    public class Tournament
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsFinished { get; set; }
        public bool IsStarted { get; set; }
        public int PlayersNumber { get; set; }
        public int RoundsNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string? OwnerUserId { get; set; }
        public IdentityUser? OwnerUser { get; set; }
        public List<Player>? Players { get; set; }
    }
}
