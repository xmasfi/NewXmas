using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asg.Services.ApplicationFramework.Presentation.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewShore.Api.Models;
using NewShore.Application.flights.Queries.Getflights;
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
        ///     Get an flights 
        /// </summary>
        /// <returns>Detail of an flights</returns>
        [HttpGet]
        public async Task<ActionResult<IList<Journey>>> Get([FromQuery] GetFlights getFlights)
        {
            var getflightQuery = _mapper.Map<GetFlightQuery>(getFlights);

            return Ok(await Mediator.Send(getflightQuery));            
        }

    }
}