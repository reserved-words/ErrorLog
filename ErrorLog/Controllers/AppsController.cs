using AutoMapper;
using ErrorLog.Data;
using ErrorLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ErrorLog.Controllers
{
    [RoutePrefix("api/apps")]
    public class AppsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly ILogRepository _repository;

        public AppsController(ILogRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeLogs = false)
        {
            try
            {
                var result = await _repository.GetAllAppsAsync(includeLogs);
                var model = _mapper.Map<IEnumerable<AppModel>>(result);
                return Ok(model);
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeLogs = false)
        {
            try
            {
                var result = await _repository.GetAppAsync(moniker, includeLogs);

                if (result == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<AppModel>(result);
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
