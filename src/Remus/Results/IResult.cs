using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remus.Results {
    /// <summary>
    /// Defines a contract that describes the result of an action.
    /// </summary>
    public interface IResult
    {
        bool IsSuccess { get; }
    }
}
