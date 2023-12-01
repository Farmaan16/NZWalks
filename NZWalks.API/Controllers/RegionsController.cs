using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.DTO;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using System.Collections.Generic;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        
        // Get all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from database - Domain models
           var regionsDomain = await regionRepository.GetAllAsync();

            //map domain models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //Return DTOs
            return Ok(regionsDto);
        }

        //Get single region (Get region by Id)
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //var region = dbContext.regions.Find(id);
            //Get region domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map or convert region domain model to region DTo
            var regionDto = mapper.Map<RegionDto>(regionDomain);
                          
            //Return DTO back to client
            return Ok(regionDto);
        }

        //Post to create new region
        [HttpPost]
         
        public async Task <IActionResult> Create([FromBody]AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert the DTo to domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //use Domain model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);


            //Map domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id},regionDto);
        }

        //Update region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //check if region exists
            regionDomainModel =  await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            

            // Convert domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);   

        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id); 

            if (regionDomainModel == null)
            {
                return NotFound();
            }


            //Return deleted region back
            //map domain model to dto
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);

        }


    }

}
