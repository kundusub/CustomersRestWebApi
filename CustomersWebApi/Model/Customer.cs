﻿using Microsoft.AspNetCore.Mvc;

namespace RestServerProgram.Model
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Id { get; set; }
    }
}