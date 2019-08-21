﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class Telemetries
    {
        public Telemetries()
        {
        }

        public Telemetries(string id, string name, string unit)
        {
            Id = id;
            Name = name;
            Unit = unit;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

        public static implicit operator List<object>(Telemetries v)
        {
            throw new NotImplementedException();
        }
    }
}
