﻿using Garage2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
    public class VehicleSummaryViewModel
    {
        public string RegistrationNumber { get; set; }

        public VehicleType Type { get; set; }

        public string Colour { get; set; }

        // Currently just the date the vehicle was parked
        public DateTime ParkingTime { get; set; }

    }
}
