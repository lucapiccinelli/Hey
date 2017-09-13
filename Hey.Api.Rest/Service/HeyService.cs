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

        public IHeyResponse Create(HeyRememberDto heyRemember, bool update = false)
        {
            try
            {
                FindMethodService findService = new FindMethodService(heyRemember, new ResolveMethodByFireMeAttribute(_exceptionHandler));
                return update 
                    ? findService.UpdateResponse(_repository.MakeASchedulePrototype(heyRemember))
                    : findService.CreateNewResponse(_repository.MakeASchedulePrototype(heyRemember));
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

        public IHeyResponse Delete(string id)
        {
            return DeleteAnd(id, funid => new DeletedHeyResponse(funid));
        }

        public IHeyResponse Update(string id, HeyRememberDto heyRemember)
        {
            return DeleteAnd(id, funid => Create(heyRemember, update: true));
        }

        private IHeyResponse DeleteAnd(string id, Func<string, IHeyResponse> actionFunction)
        {
            List<HeyRememberResultDto> heyRemembers = Find(id);
            if (heyRemembers.Count == 0)
            {
                return new NotFoundHeyResponse(id);
            }
            else
            {
                try
                {
                    _repository.DeleteJobs(heyRemembers);
                    return actionFunction.Invoke(id);
                }
                catch (Exception ex)
                {
                    _exceptionHandler.Handle(ex);
                    throw;
                }
            }
        }
    }
}