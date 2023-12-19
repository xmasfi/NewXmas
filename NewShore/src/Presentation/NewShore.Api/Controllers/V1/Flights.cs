using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asg.Services.ApplicationFramework.Presentation.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewShore.Api.Models;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Domain.Entities;

namespace NewShore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class FlightsController : BaseVersioningController
    {
        private readonly IMapper _mapper;

        public FlightsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Get flights 
        /// </summary>
        /// <returns>Detail of an flights</returns>
        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<ActionResult<IList<Journey>>> Get([FromQuery] GetFlights getFlights)
        {
            var getflightQuery = _mapper.Map<GetFlightQuery>(getFlights);

            return Ok(await Mediator.Send(getflightQuery));
        }
    }
}