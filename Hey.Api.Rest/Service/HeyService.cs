using System;
using System.Collections.Generic;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Response;
using Hey.Api.Rest.Schedules;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class HeyService : IHeyService
    {
        private readonly IJobRepository _repository;
        private readonly IHeyExceptionHandler _exceptionHandler;

        public HeyService(IJobRepository repository, IHeyExceptionHandler exceptionHandler = null)
        {
            _repository = repository;
            _exceptionHandler = exceptionHandler;
        }

        public IHeyResponse Create(HeyRememberDto heyRemember)
        {
            try
            {
                return new FindMethodService(heyRemember, new ResolveMethodByFireMeAttribute(_exceptionHandler))
                    .CreateNewResponse(_repository.MakeASchedulePrototype(heyRemember));
            }
            catch (Exception ex)
            {
                if (_exceptionHandler != null)
                {
                    _exceptionHandler.Handle(ex);
                    return new ErrorHeyResponse(ex, heyRemember);
                }
                else
                {
                    throw;
                }
            }
        }

        public List<HeyRememberResultDto> Find(string id)
        {
            try
            {
                return _repository.GetJobs(id);
            }
            catch (Exception ex)
            {
                _exceptionHandler.Handle(ex);
                throw;
            }
        }
    }
}