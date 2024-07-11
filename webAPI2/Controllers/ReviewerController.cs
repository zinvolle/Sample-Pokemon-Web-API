using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webAPI2.DTO;
using webAPI2.Interface;
using webAPI2.Models;

namespace webAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        public IActionResult GetReviewer(int reviewerId) 
        {
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }

        [HttpGet("reviewer/reviews/{reviewerId}")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        public IActionResult CreateReviwer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var reviewer = _reviewerRepository.GetReviewers().Where(
                r=>r.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (reviewer != null)
            {
                ModelState.AddModelError("", "This Reviewer already exists");
                return StatusCode(422, ModelState);
            }
            var reviewMap = _mapper.Map<Reviewer>(reviewerCreate);
            if (!_reviewerRepository.CreateReviewer(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
    }
}
