using FluentValidation;
using Potestas.API.ViewModels;

namespace Potestas.API.Validators
{
    public class CoordinatesModelValidator: AbstractValidator<CoordinatesModel>
    {
        public CoordinatesModelValidator()
        {
            RuleFor(c => c.X).InclusiveBetween(Coordinates.xMinValue, Coordinates.xMaxValue);
            RuleFor(c => c.Y).InclusiveBetween(Coordinates.yMinValue, Coordinates.yMaxValue);
        }
    }
}
