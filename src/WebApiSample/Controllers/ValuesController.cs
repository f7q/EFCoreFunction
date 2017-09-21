using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private SampleDbContext _dbContext { get; set; }
        private ILogger _logger { get; set; }
        
        public ValuesController(SampleDbContext dbContext, ILogger<ValuesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var result = _dbContext.Values.OrderBy(i => i.Name);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        // GET api/values/km
        [HttpGet("{name}")]
        public IActionResult Get([FromRoute]string name)
        {
            
            var sql = _dbContext.Values.Where(i => i.Name == name).ToSql();
            _logger.LogInformation("before sql{@}", sql);
            sql = sql.Replace("=", "&@~"); // AND ORは大文字
            _logger.LogInformation("after sql{@}", sql);
            var result = _dbContext.Values.FromSql(sql).ToList();
            
            //var result = _dbContext.Values.Where(i => EF.Functions.FTS(i.Name, name)).ToList();
            //var result = _dbContext.Values.Where(i => i.Name == name).ToList();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Value value)
        {
            try
            {
                _dbContext.Values.Add(value);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return new NotFoundResult();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Value value)
        {
            try
            {
                var result = _dbContext.Values.Where(i => i.Id == id).First();
                if(result != null) {
                    _dbContext.Values.Update(value);
                    _dbContext.SaveChanges();
                }
                else
                {
                    _dbContext.Values.Add(value);
                    _dbContext.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return new NotFoundResult();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try { 
                var value = _dbContext.Values.Where(i => i.Id == id).First();
                _dbContext.Values.Remove(value);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return new NotFoundResult();

            }
        }
    }
}
