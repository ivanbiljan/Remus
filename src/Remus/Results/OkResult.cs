using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remus.Results {
    /// <summary>
    /// Represents an "OK" result. This result carries no data and simply indicates that an action has completed successfully.
    /// </summary>
    public sealed class OkResult : IResult
    {
        /// <inheritdoc />
        public bool IsSuccess { get; }
    }
}
