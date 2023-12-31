﻿using Asg.Services.ApplicationFramework.Application.Validators;
using FluentValidation;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Common.Constants;
using System;


namespace NewShore.Application.Flights.Queries.GetFlights
{
    public class GetFlightQueryHandlerValidation : AbstractValidator<GetFlightQuery>
    {
        public GetFlightQueryHandlerValidation() 
        {

            RuleFor(x => x.Destination)
                .NotEmpty()
                .WithErrorCode($"Flight_{ErrorCodes.NotEmpty}")

                .Length(DefaultValidations.MinLength, DefaultValidations.Maxlength)
                .WithErrorCode($"Flight_{ErrorCodes.InvalidLength}");

            RuleFor(x => x.Origin)
                .NotEmpty()
                .WithErrorCode($"Flight_{ErrorCodes.NotEmpty}")

                .Length(DefaultValidations.MinLength, DefaultValidations.Maxlength)
                .WithErrorCode($"Flight_{ErrorCodes.InvalidLength}");

            RuleFor(x => x.MaxScales)
                .GreaterThanOrEqualTo(Constants.MinScales)
                .WithErrorCode($"Flight_{ErrorCodes.InvalidValue}")
                .WithMessage("Number of scales must be greater than" + Constants.MinScales)

                .LessThanOrEqualTo(Constants.MaxScales)
                .WithErrorCode($"Flight_{ErrorCodes.InvalidValue}")
                .WithMessage("Number of scales must be less than" + Constants.MaxScales);


            RuleFor(x => x)
                .Must(DifferentOriginDestination)
                .WithErrorCode($"Flight_{ErrorCodes.SameDestination}")
                .WithMessage("Origin and Destination are the same");
        }

        private bool DifferentOriginDestination(GetFlightQuery getflightQuery)
        {

            return String.Compare(getflightQuery.Destination, getflightQuery.Origin, comparisonType: StringComparison.OrdinalIgnoreCase) != 0;
        }
    }
}
