using AutoMapper;
using ErrorLog.Data;
using ErrorLog.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ErrorLog.Controllers
{
    [RoutePrefix("api/logs")]
    public class LogsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly ILogRepository _repository;

        public LogsController(ILogRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [Route()]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await _repository.GetLogsAsync(true);
                var model = _mapper.Map<IEnumerable<LogModel>>(result);
                return Ok(model);
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }
        }

        [Route("{moniker}/{id}")]
        public async Task<IHttpActionResult> Get(string moniker, int id)
        {
            try
            {
                var result = await _repository.GetLogByAppAsync(moniker, id, true);

                if (result == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<LogModel>(result);
                return Ok(model);
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {
                var result = await _repository.GetLogsByAppAsync(moniker, true);

                if (result == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<IEnumerable<LogModel>>(result);
                return Ok(model);
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }
        }

        [Route("searchByDate/{date:DateTime}")]
        [HttpGet] // Needed since not using Get as the action name
        public async Task<IHttpActionResult> SearchByDate(DateTime date)
        {
            try
            {
                var result = await _repository.GetLogsByDateAsync(date);

                if (result == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<IEnumerable<LogModel>>(result);
                return Ok(model);
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }
        }
    }
}
