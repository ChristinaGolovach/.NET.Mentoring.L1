using FluentValidation;
using Potestas.API.ViewModels;

namespace Potestas.API.Validators
{
    public class EnergyObservationModelValidator : AbstractValidator<EnergyObservationModel>
    {
        public EnergyObservationModelValidator()
        {
            RuleFor(obs => obs.EstimatedValue).GreaterThan(0);
            RuleFor(obs => obs.ObservationTime).NotEmpty().GreaterThan(new System.DateTime(1900, 1, 1));
            RuleFor(obs => obs.ObservationPoint.X).InclusiveBetween(Coordinates.xMinValue, Coordinates.xMaxValue);
            RuleFor(obs => obs.ObservationPoint.Y).InclusiveBetween(Coordinates.yMinValue, Coordinates.yMaxValue);
        }
    }

    //https://fluentvalidation.net/aspnet#asp-net-core
}
