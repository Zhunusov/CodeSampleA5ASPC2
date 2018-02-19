using System.Collections.Generic;
using System.Linq;

namespace Domain.Core
{
    public sealed class ServiceResult
    {
        public bool Succeeded { get; }

        public IReadOnlyList<string> Errors { get; } = new List<string>();

        public ServiceResult()
        {

        }

        public ServiceResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public ServiceResult(bool succeeded, IReadOnlyList<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public ServiceResult(bool succeeded, params string[] errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToList();
        }
    }
}
