using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asg.Services.ApplicationFramework.Presentation.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewShore.Api.Models;
using NewShore.Application.Flights.Queries.GetFlightsWithReturn;
using NewShore.Domain.Entities;

namespace NewShore.Api.Controllers
{
    [ApiVersion("2.0")]
    public class FlightsController : BaseVersioningController
    {
        private readonly IMapper _mapper;

        public FlightsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Get  flights with return (if possible)
        /// </summary>
        /// <returns>Detail of flights</returns>
        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<ActionResult<IList<Journey>>> Get([FromQuery] GetFlightsReturn getFlights)
        {
            var getflightReturnQuery = _mapper.Map<GetFlightReturnQuery>(getFlights);

            return Ok(await Mediator.Send(getflightReturnQuery));
        }

    }
}