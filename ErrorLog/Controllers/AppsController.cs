using AutoMapper;
using ErrorLog.Data;
using ErrorLog.Models;
using System;
using System.Collections.Generic;
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
            return await Try<App[], IEnumerable<AppModel>>(async () =>
            {
                return await _repository.GetAllAppsAsync(includeLogs);
            });
        }

        [Route("{moniker}", Name = "GetApp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeLogs = false)
        {
            return await Try<App, AppModel>(async () =>
            {
                return await _repository.GetAppAsync(moniker, includeLogs);
            });
        }

        [Route()]
        public async Task<IHttpActionResult> Post(AppModel model)
        {
            try
            {
                if (await _repository.GetAppAsync(model.Moniker) != null)
                {
                    ModelState.AddModelError("Moniker", "Moniker in use");
                }

                if (ModelState.IsValid)
                {
                    var app = _mapper.Map<App>(model);
                    _repository.AddApp(app);
                    if (await _repository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<AppModel>(app);
                        return CreatedAtRoute("GetApp", new { moniker = newModel.Moniker }, newModel);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, AppModel model)
        {
            try
            {
                if (model.Moniker != moniker)
                {
                    ModelState.AddModelError("Moniker", "Moniker cannot be updated");
                }

                if (ModelState.IsValid)
                {
                    var app = await _repository.GetAppAsync(moniker);
                    if (app == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(model, app);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Ok(_mapper.Map<AppModel>(app));
                    }
                    else
                    {
                        return InternalServerError();
                    }

                }
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var app = await _repository.GetAppAsync(moniker);
                if (app == null)
                {
                    return NotFound();
                }

                _repository.DeleteApp(app);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                // TODO: Log locally, in production don't return exception
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        private async Task<IHttpActionResult> Try<T1,T2>(Func<Task<T1>> method)
        {
            try
            {
                var result = await method();

                if (result == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<T2>(result);
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
