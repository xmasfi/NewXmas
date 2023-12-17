using Asg.Services.ApplicationFramework.Application.AutoMapper.Interfaces;
using Asg.Services.ApplicationFramework.Application.Validators;
using NewShore.Application.flights.Queries.Getflights;
using System.ComponentModel.DataAnnotations;

namespace NewShore.Api.Models
{
    public class GetFlights : IMapTo<GetFlightQuery>
    {
        [Required]
        [StringLength(DefaultValidations.Maxlength, MinimumLength = DefaultValidations.MinLength)]
        public string Origin { get; set; }

        [Required]
        [StringLength(DefaultValidations.Maxlength, MinimumLength = DefaultValidations.MinLength)]
        public string Destination { get; set; }

        public int MaxScales { get; set; }
    }
}