﻿namespace Music_Club.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public bool  IsСonfirm { get; set; }
    }
}
