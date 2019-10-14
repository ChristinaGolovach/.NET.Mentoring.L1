using System.Collections.Generic;

namespace Potestas
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Usually repositories are implemented by another way.
    /// </remarks>
    public interface IEnergyObservationStorage<T> : ICollection<T> where T: IEnergyObservation
    {
        string Description { get; }
    }
}
