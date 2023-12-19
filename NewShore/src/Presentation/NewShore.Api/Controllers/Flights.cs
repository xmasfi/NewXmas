using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asg.Services.ApplicationFramework.Presentation.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewShore.Api.Models;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Flights.Queries.GetFlightsWithReturn;
using NewShore.Domain.Entities;

namespace NewShore.Api.Controllers
{
    public class FlightsController : BaseController
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
        [HttpGet]
        public async Task<ActionResult<IList<Journey>>> Get([FromQuery] GetFlights getFlights)
        {
            var getflightQuery = _mapper.Map<GetFlightQuery>(getFlights);

            return Ok(await Mediator.Send(getflightQuery));            
        }

        /// <summary>
        ///     Get  flights with return (if possible)
        /// </summary>
        /// <returns>Detail of flights</returns>
        [HttpGet("GetWithReturn")]
        public async Task<ActionResult<IList<Journey>>> GetWithReturn([FromQuery] GetFlightsReturn getFlights)
        {
            var getflightReturnQuery = _mapper.Map<GetFlightReturnQuery>(getFlights);

            return Ok(await Mediator.Send(getflightReturnQuery));
        }

    }
}