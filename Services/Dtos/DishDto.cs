﻿using System;

namespace Services.Dtos
{
    public class DishDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Calories { get; set; }

        public int Weight { get; set; }
    }
}