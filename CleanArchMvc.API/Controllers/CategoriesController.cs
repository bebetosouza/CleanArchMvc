using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var categories = await _categoryService.GetCategories();
            if (categories == null)
            {
                return NotFound("Categories Not Found");
            }
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound("Categories Not Found");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Invalid Data");

            await _categoryService.Add(categoryDto);

            return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest();
            
            if (categoryDto == null)
                return BadRequest();

            await _categoryService.Update(categoryDto);

            return Ok(categoryDto);
        } 

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Delete(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            await _categoryService.Remove(id);

            return Ok(category);
        }

    }
}
